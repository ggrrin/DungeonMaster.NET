using DungeonMasterEngine.Helpers;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.EntitySupport;
using DungeonMasterEngine.DungeonContent.EntitySupport.Attacks;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DungeonMasterParser.Tiles;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    public class LegacyMapBuilder : BuilderBase
    {
        private readonly Dictionary<int, DungeonLevel> loadedLevels = new Dictionary<int, DungeonLevel>();
        private List<Tile> outputTiles;
        private Stack<TileInfo<TileData>> stack;
        private Point start;
        private BitMapMemory bitMap;
        private TileInfo<TileData> currentTile;
        private LegacyTileCreator legacyTileCreator;

        private Texture2D[] doorTextures;
        private Texture2D[] wallTextures;
        private Texture2D[] floorTextures;
        private List<Creature> creatures = new List<Creature>();

        public DungeonData Data { get; }
        public DungeonMap CurrentMap { get; private set; }
        public Dictionary<Point, Tile> TilesPositions { get; private set; }
        public WallActuatorCreator WallActuatorCreator { get; private set; }
        public FloorActuatorCreator FloorActuatorCreator { get; private set; }
        public LegacyItemCreator ItemCreator { get; private set; }
        public Texture2D defaultDoorTexture { get; private set; }
        public Texture2D defaultMapDoorTypeTexture { get; private set; }
        public IReadOnlyList<Texture2D> DoorTextures => doorTextures;
        public IReadOnlyList<Texture2D> WallTextures => wallTextures;
        public IReadOnlyList<Texture2D> FloorTextures => floorTextures;
        public int CurrentLevel { get; private set; }
        public RelationToken CreatureToken { get; } = new RelationToken(1); //TODO RelationTokenFactory.GetNextToken();
        public RelationToken ChampionToken { get; } = new RelationToken(0); //TODO RelationTokenFactory.GetNextToken();

        //item factories
        public IReadOnlyList<WeaponItemFactory> WeaponFactories { get; }
        public IReadOnlyList<ClothItemFactory> ClothFactories { get; }
        public IReadOnlyList<ContainerItemFactory> ContainerFactories { get; }
        public IReadOnlyList<ScrollItemFactory> ScrollFactories { get; }
        public IReadOnlyList<PotionItemFactory> PotionFactories { get; }

        public IReadOnlyList<MiscItemFactory> MiscFactories { get; }

        public LegacyMapBuilder()
        {
            var parser = new DungeonParser();
            parser.Parse();
            Data = parser.Data;


            WeaponFactories = Data.WeaponDescriptors
                .Select(wd =>
                {
                    var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Weapon, wd.Identifer);
                    return new WeaponItemFactory(wd.Name, wd.Weight, GetAttackFactories(itemDescriptor),
                        ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation),
                        wd.DeltaEnergy, (WeaponClass)wd.Class, wd.KineticEnergy, wd.ShootDamage, wd.Strength);
                })
                .ToArray();

            ClothFactories = Data.ArmorDescriptors
               .Select(wd =>
               {
                   var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Clothe, wd.Identifer);
                   return new ClothItemFactory(wd.Name, wd.Weight, GetAttackFactories(itemDescriptor)
                       , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation));
               })
               .ToArray(); ;

            ContainerFactories = Data.ContainerDescriptors
               .Select(wd =>
               {
                   var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Container, wd.Identifer);
                   return new ContainerItemFactory(wd.Name, wd.Weight, GetAttackFactories(itemDescriptor)
                       , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation));
               })
               .ToArray();

            ScrollFactories = Data.ScrollDescriptors
               .Select(wd =>
               {
                   var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Scroll, wd.Identifer);
                   return new ScrollItemFactory(wd.Name, wd.Weight, GetAttackFactories(itemDescriptor)
                       , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation));
               })
               .ToArray();


            MiscFactories = Data.MiscDescriptors
                .Select(wd =>
                {
                    var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Miscellenaous, wd.Identifer);
                    return new MiscItemFactory(wd.Name, wd.Weight, GetAttackFactories(itemDescriptor)
                        , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation));
                })
                .ToArray();

            PotionFactories = Data.PotionDescriptors
                .Select(p =>
                {
                    var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Potion, p.Identifer);
                    return new PotionItemFactory(p.Name, p.Weight, GetAttackFactories(itemDescriptor)
                        , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation));
                })
                .ToArray();

        }

        private IList<IAttackFactory> GetAttackFactories(ItemDescriptor itemsDescriptor)
        {
            // ReSharper disable once CoVariantArrayConversion
            return Data.FightCombos[itemsDescriptor.AttackCombo].Actions
               .Select(a =>
                   new HumanAttackFactory(a.ActionDescriptor.Name,
                   a.ActionDescriptor.ExperienceGain,
                   a.ActionDescriptor.DefenseModifier,
                   a.ActionDescriptor.HitProbability,
                   a.ActionDescriptor.Damage,
                   a.ActionDescriptor.Fatigue,
                   null,
                   a.ActionDescriptor.Stamina,
                   -1))//TODO
               .ToArray();
        }

        private void Initialize(int level, Point? startTile)
        {
            CurrentLevel = level;
            CurrentMap = Data.Maps[level];
            legacyTileCreator = new LegacyTileCreator(this);
            WallActuatorCreator = new WallActuatorCreator(this);
            FloorActuatorCreator = new FloorActuatorCreator(this);
            ItemCreator = new LegacyItemCreator(this);
            creatures = new List<Creature>();
            InitializeMapTextures();

            outputTiles = new List<Tile>();
            stack = new Stack<TileInfo<TileData>>();
            start = startTile ?? new Point(Data.StartPosition.Position.X, Data.StartPosition.Position.Y);
            TilesPositions = new Dictionary<Point, Tile>();
            bitMap = new BitMapMemory(CurrentMap.OffsetX, CurrentMap.OffsetY, CurrentMap.Width, CurrentMap.Height);
        }



        public override DungeonLevel GetLevel(int level, Dungeon dungeon, Point? startTile)
        {
            DungeonLevel dungeonLevel = null;
            if (Data.Maps.Count <= level)
                throw new ArgumentException("Level does not exist.");
            if (loadedLevels.TryGetValue(level, out dungeonLevel))
                return dungeonLevel;

            Initialize(level, startTile);
            stack.Push(new TileInfo<TileData>
            {
                Position = start,
                Tile = CurrentMap[start.X, start.Y]
            });
            bitMap[start.X, start.Y] = true;

            while (stack.Count > 0)
            {
                currentTile = stack.Pop();
                var nextTiles = CreateTile(currentTile).Where(t => t.Tile != null && t.Tile.GetType() != typeof(WallTileData) && !bitMap[t.Position.X, t.Position.Y]);

                foreach (var t in nextTiles) //recursion
                {
                    stack.Push(t);
                    bitMap[t.Position.X, t.Position.Y] = true;
                }
            }

            SetupNeighbours(TilesPositions, outputTiles);
            SetupItems();

            dungeonLevel = new DungeonLevel(dungeon, outputTiles, creatures, level, TilesPositions, TilesPositions[start], legacyTileCreator.MiniMap);
            loadedLevels.Add(level, dungeonLevel);
            return dungeonLevel;
        }

        private IEnumerable<TileInfo<TileData>> CreateTile(TileInfo<TileData> currentTile)
        {
            var newTile = legacyTileCreator.GetTile(currentTile);
            TilesPositions.Add(currentTile.Position, newTile); //remember Tiles-position association
            outputTiles.Add(newTile); //remember created tile

            return GetNeigbourTiles(currentTile.Position, CurrentMap).Concat(legacyTileCreator.Successors); //add nextTiles advised by CurrentTile
        }

        public IEnumerable<TileInfo<TileData>> GetNeigbourTiles(Point position, DungeonMap map)
        {
            return MapDirection.AllSides.Select(side =>
            {
                var p = position + side.RelativeShift;
                return new TileInfo<TileData>
                {
                    Position = p,
                    Tile = map[p.X, p.Y]
                };
            });
        }

        private void SetupItems()
        {
            foreach (var tile in outputTiles)
            {
                WallActuatorCreator.CreateSetupActuators(tile);
                SetupFloorItems(tile);
            }
        }

        private void SetupFloorItems(Tile tile)
        {
            FloorActuatorCreator.CreateSetupActuators(tile);

            var itemCreator = new LegacyItemCreator(this);
            var tileData = CurrentMap[tile.GridPosition.X, tile.GridPosition.Y];
            tileData.GrabableItems.ForEach(x => tile.SubItems.Add(itemCreator.CreateItem(x, tile)));

            //TODO creatures
            var creatureCreator = new CreatureCreator(this);
            foreach (var creatre in tileData.Creatures.Where(i => !i.Processed))
                creatures.AddRange(creatureCreator.AddCreature(creatre, tile));
        }

        private void InitializeMapTextures()
        {
            if (CurrentMap.DoorType0Index == 0)
                defaultDoorTexture = ResourceProvider.Instance.Content.Load<Texture2D>("Textures/DefaultDoor");
            else
                throw new NotSupportedException();

            defaultMapDoorTypeTexture = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{Enum.GetName(CurrentMap.DoorType.GetType(), CurrentMap.DoorType)}");

            doorTextures = new Texture2D[CurrentMap.DoorDecorationCount];
            for (int i = 0; i < doorTextures.Length; i++)
                doorTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{CurrentMap.DoorDecorations[i]}");

            wallTextures = new Texture2D[CurrentMap.WallGraphicsCount];
            for (int i = 0; i < wallTextures.Length; i++)
                wallTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{CurrentMap.WallDecorations[i]}");

            floorTextures = new Texture2D[CurrentMap.FloorGraphicsCount];
            for (int i = 0; i < floorTextures.Length; i++)
                floorTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{CurrentMap.FloorDecorations[i]}");
        }




        public Tile GetTargetTile(ActuatorItemData callingActuator)
        {
            var targetPos = ((RemoteTarget)callingActuator.ActionLocation).Position.Position.ToAbsolutePosition(CurrentMap);

            Tile targetTile = null;
            if (TilesPositions.TryGetValue(targetPos, out targetTile))
                return targetTile;
            else
            {
                //try find tile in raw data, and than actuator, add it to Tiles Positions
                var virtualTileData = CurrentMap[targetPos.X, targetPos.Y];
                if (virtualTileData.Actuators.Any()) //virtual tile will be proccessed at the and so any checking shouldnt be necessary
                {
                    var newTile = new LogicTile(targetPos.ToGridVector3(CurrentLevel));
                    newTile.Gates = virtualTileData.Actuators.Where(x => x.ActuatorType == 5).Select(y => InitLogicGates(y, newTile)).ToArray(); //recurse
                    newTile.Counters = virtualTileData.Actuators.Where(x => x.ActuatorType == 6).Select(y => InitCounters(y, newTile)).ToArray(); //recurse

                    TilesPositions.Add(targetPos, targetTile = newTile); //subitems will be processed 
                }
                else if (virtualTileData.TextTags.Any())
                {
                    var textTag = virtualTileData.TextTags.Single();
                    textTag.HasTargetingActuator = true;
                    targetTile = TilesPositions[textTag.GetParentPosition(targetPos)];

                    if (textTag.Processed)
                        targetTile.SubItems.Single(x => x is TextTag).AcceptMessages = true;
                }

                return targetTile; //TODO (if null)  think out what to do 
                //Acutor at the begining references wall near by with tag only ... what to do ? 
            }
        }

        private CounterActuator InitCounters(ActuatorItemData gateActuator, Tile gateActuatorTile)
        {
            //if nextTarget tile is current tile do not call recurese
            Tile nextTargetTile = gateActuatorTile.GridPosition == ((RemoteTarget)gateActuator.ActionLocation).Position.Position.ToAbsolutePosition(CurrentMap) ? gateActuatorTile : GetTargetTile(gateActuator);

            return new CounterActuator(nextTargetTile, gateActuator.GetActionStateX(), gateActuator.Data, gateActuatorTile.Position);
        }

        private LogicGate InitLogicGates(ActuatorItemData gateActuator, Tile gateActuatorTile)
        {
            //if nextTarget tile is current tile do not call recurese
            Tile nextTargetTile = gateActuatorTile.GridPosition == ((RemoteTarget)gateActuator.ActionLocation).Position.Position.ToAbsolutePosition(CurrentMap) ? gateActuatorTile : GetTargetTile(gateActuator);

            return new LogicGate(nextTargetTile, gateActuator.GetActionStateX(), gateActuatorTile.Position, (gateActuator.Data & 0x10) == 0x10, (gateActuator.Data & 0x20) == 0x20, (gateActuator.Data & 0x40) == 0x40, (gateActuator.Data & 0x80) == 0x80);
        }

        public Vector3 GetFloorPosition(TilePosition tilePosition, Tile currentTile)
        {
            Vector3 offset = Vector3.Zero;
            const float scalarOffset = 0.25f;

            switch (tilePosition)
            {
                case TilePosition.North_TopLeft:
                    offset = new Vector3(scalarOffset, 0, scalarOffset);
                    break;
                case TilePosition.East_TopRight:
                    offset = new Vector3(1 - scalarOffset, 0, scalarOffset);
                    break;
                case TilePosition.South_BottomLeft:
                    offset = new Vector3(scalarOffset, 0, 1 - scalarOffset);
                    break;
                case TilePosition.West_BottomRight:
                    offset = new Vector3(1 - scalarOffset, 0, 1 - scalarOffset);
                    break;
            }

            return currentTile.Position + offset;
        }

        public Vector3 GetWallPosition(TilePosition tilePosition, Tile currentTile)
        {
            Vector3 offset = Vector3.Zero;
            const float scalarOffset = 0.25f;
            switch (tilePosition)
            {
                case TilePosition.North_TopLeft:
                    offset = new Vector3(scalarOffset, scalarOffset, 1 - scalarOffset);
                    break;
                case TilePosition.East_TopRight:
                    offset = new Vector3(0, scalarOffset, 1 - scalarOffset);
                    break;
                case TilePosition.South_BottomLeft:
                    offset = new Vector3(scalarOffset, scalarOffset, 0);
                    break;
                case TilePosition.West_BottomRight:
                    offset = new Vector3(1 - scalarOffset, scalarOffset, scalarOffset);
                    break;
            }

            return currentTile.Position + offset;
        }

        public IGrabableItemFactoryBase GetItemFactory(int identifer)
        {
            var factoreis = new IReadOnlyList<IGrabableItemFactoryBase>[]
            {
                ScrollFactories,
                ContainerFactories,
                PotionFactories,
                WeaponFactories,
                ClothFactories,
                MiscFactories
            };

            int index = 0;
            int previousListCount = 0;
            foreach (var factory in factoreis)
            {
                if (index - previousListCount + factory.Count < 0)
                {
                    return factory[index - previousListCount];
                }
                previousListCount += factory.Count;
            }
            throw new IndexOutOfRangeException();
        }

        public bool PrepareActuatorData(ActuatorItemData i, out Tile targetTile, out IConstrain constrain, out Texture2D decoration, bool putOnWall)
        {
            targetTile = GetTargetTile(i);
            constrain = null;
            decoration = null;


            if (i.Data > 0)
                constrain = new GrabableItemConstrain(GetItemFactory(i.Data), i.IsRevertable);
            else
                constrain = new NoConstrain();

            if (i.IsLocal)
                throw new NotSupportedException("yet");
            decoration = GetTexture(i, putOnWall);

            return true;
        }

        public Texture2D GetTexture(ActuatorItemData i, bool putOnWall)
        {
            if (i.Decoration > 0)
                return putOnWall ? WallTextures[i.Decoration - 1] : FloorTextures[i.Decoration - 1];
            return null;
        }
    }
}
