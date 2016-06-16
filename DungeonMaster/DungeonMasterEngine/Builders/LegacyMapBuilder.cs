using DungeonMasterEngine.Helpers;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Attacks;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
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
        private static Random rand = new Random();
        private readonly Dictionary<int, DungeonLevel> loadedLevels = new Dictionary<int, DungeonLevel>();
        private Point start;
        private LegacyTileCreator legacyTileCreator;

        private Texture2D[] doorTextures;
        private Texture2D[] wallTextures;
        private Texture2D[] floorTextures;
        private Texture2D[] championTextures;
        private List<Creature> creatures = new List<Creature>();

        public DungeonData Data { get; }
        public DungeonMap CurrentMap { get; private set; }
        public Dictionary<Point, Tile> TilesPositions { get; private set; }
        public FloorActuatorCreator FloorActuatorCreator { get; private set; }
        public LegacyItemCreator ItemCreator { get; private set; }
        public Texture2D defaultDoorTexture { get; private set; }
        public Texture2D defaultMapDoorTypeTexture { get; private set; }
        public IReadOnlyList<Texture2D> DoorTextures => doorTextures;
        public IReadOnlyList<Texture2D> WallTextures => wallTextures;
        public IReadOnlyList<Texture2D> FloorTextures => floorTextures;
        public IReadOnlyList<Texture2D> ChampionTextures => championTextures;
        public Texture2D WallTexture { get; private set; }
        public int CurrentLevel { get; private set; }
        public RelationToken CreatureToken { get; } = new RelationToken(1); //TODO RelationTokenFactory.GetNextToken();
        public RelationToken ChampionToken { get; } = new RelationToken(0); //TODO RelationTokenFactory.GetNextToken();

        public List<TileInitializer> TileInitializers { get; private set; }
        private TaskCompletionSource<bool> tileInitialized;
        private List<Tile> tiles;
        public IWallGraphicSource RendererSource { get; }

        public IReadOnlyList<ISkillFactory> Skills { get; } = new ISkillFactory[]
        {
            SkillFactory<FighterSkill>.Instance,
            SkillFactory<NinjaSkill>.Instance,
            SkillFactory<PriestSkill>.Instance,
            SkillFactory<WizardSkill>.Instance,
            SkillFactory<SwingSkill>.Instance,
            SkillFactory<ThrustSkill>.Instance,
            SkillFactory<ClubSkill>.Instance,
            SkillFactory<ParrySkill>.Instance,
            SkillFactory<StealSkill>.Instance,
            SkillFactory<FightSkill>.Instance,
            SkillFactory<ThrowSkill>.Instance,
            SkillFactory<ShootSkill>.Instance,
            SkillFactory<IdentifySkill>.Instance,
            SkillFactory<HealSkill>.Instance,
            SkillFactory<InfluenceSkill>.Instance,
            SkillFactory<DeffendSkill>.Instance,
            SkillFactory<FireSkill>.Instance,
            SkillFactory<AirSkill>.Instance,
            SkillFactory<EarthSkill>.Instance,
            SkillFactory<WaterSkill>.Instance,
        };

        //item factories
        public IReadOnlyList<WeaponItemFactory> WeaponFactories { get; }
        public IReadOnlyList<ClothItemFactory> ClothFactories { get; }
        public IReadOnlyList<ContainerItemFactory> ContainerFactories { get; }
        public IReadOnlyList<ScrollItemFactory> ScrollFactories { get; }
        public IReadOnlyList<PotionItemFactory> PotionFactories { get; }

        public IReadOnlyList<MiscItemFactory> MiscFactories { get; }
        public Texture2D RandomWallDecoration
        {
            get
            {
                int val = rand.Next(29);
                return val < CurrentMap.WallDecorationGraphicsCount ? WallTextures[val] : null;
            }
        }

        public Texture2D RandomFloorDecoration
        {
            get
            {
                int val = rand.Next(29);
                return val < CurrentMap.FloorDecorationGraphicsCount ? FloorTextures[val] : null;
            }
        }

        public Texture2D DoorButtonTexture { get; private set; }
        public Texture2D TeleportTexture { get; private set; }

        public LegacyMapBuilder()
        {
            var parser = new DungeonParser();
            parser.Parse();
            Data = parser.Data;

            ItemCreator = new LegacyItemCreator(this);

            WeaponFactories = Data.WeaponDescriptors
                .Select(wd =>
                {
                    var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Weapon, wd.Identifer);
                    return new WeaponItemFactory(wd.Name, wd.Weight, GetAttackFactories(itemDescriptor),
                        ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation),
                        wd.DeltaEnergy, (WeaponClass)wd.Class, wd.KineticEnergy, wd.ShootDamage, wd.Strength, ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath));
                })
                .ToArray();

            ClothFactories = Data.ArmorDescriptors
               .Select(wd =>
               {
                   var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Clothe, wd.Identifer);
                   return new ClothItemFactory(wd.Name, wd.Weight, GetAttackFactories(itemDescriptor)
                       , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation), ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath));
               })
               .ToArray(); ;

            ContainerFactories = Data.ContainerDescriptors
               .Select(wd =>
               {
                   var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Container, wd.Identifer);
                   return new ContainerItemFactory(wd.Name, wd.Weight, GetAttackFactories(itemDescriptor)
                       , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation), ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath));
               })
               .ToArray();

            ScrollFactories = Data.ScrollDescriptors
               .Select(wd =>
               {
                   var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Scroll, wd.Identifer);
                   return new ScrollItemFactory(wd.Name, wd.Weight, GetAttackFactories(itemDescriptor)
                       , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation), ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath));
               })
               .ToArray();


            MiscFactories = Data.MiscDescriptors
                .Select(wd =>
                {
                    var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Miscellenaous, wd.Identifer);
                    return new MiscItemFactory(wd.Name, wd.Weight, GetAttackFactories(itemDescriptor)
                        , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation), ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath));
                })
                .ToArray();

            PotionFactories = Data.PotionDescriptors
                .Select(p =>
                {
                    var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Potion, p.Identifer);
                    return new PotionItemFactory(p.Name, p.Weight, GetAttackFactories(itemDescriptor)
                        , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation), ResourceProvider.Instance.Content.Load<Texture2D>(p.TexturePath));
                })
                .ToArray();



            RendererSource = new DefaultWallGrahicsSource();
        }

        private IList<IAttackFactory> GetAttackFactories(ItemDescriptor itemsDescriptor)
        {
            // ReSharper disable once CoVariantArrayConversion
            return Data.FightCombos[itemsDescriptor.AttackCombo].Actions
               .Select(a =>
                    new ComboAttackFactory(a.UseCharges == 1, a.MinimumSkillLevel,
                       new HumanAttackFactory(a.ActionDescriptor.Name,
                           a.ActionDescriptor.ExperienceGain,
                           a.ActionDescriptor.DefenseModifier,
                           a.ActionDescriptor.HitProbability,
                           a.ActionDescriptor.Damage,
                           a.ActionDescriptor.Fatigue,
                           Skills[a.ActionDescriptor.ImprovedSkill],
                           a.ActionDescriptor.Stamina,
                           -1)))
               .ToArray();
        }


        private void Initialize(int level, Point? startTile)
        {
            CurrentLevel = level;
            CurrentMap = Data.Maps[level];
            legacyTileCreator = new LegacyTileCreator(this);
            FloorActuatorCreator = new FloorActuatorCreator(this);
            ItemCreator = new LegacyItemCreator(this);
            creatures = new List<Creature>();
            InitializeMapTextures();
            TileInitializers = new List<TileInitializer>();
            tiles = new List<Tile>();
            rand = new Random(1);

            start = startTile ?? new Point(Data.StartPosition.Position.X, Data.StartPosition.Position.Y);
            TilesPositions = new Dictionary<Point, Tile>();
        }


        public override DungeonLevel GetLevel(int level, Dungeon dungeon, Point? startTile)
        {



            DungeonLevel dungeonLevel = null;
            if (Data.Maps.Count <= level)
                throw new ArgumentException("Level does not exist.");
            if (loadedLevels.TryGetValue(level, out dungeonLevel))
                return dungeonLevel;

            Initialize(level, startTile);


            tileInitialized = new TaskCompletionSource<bool>();
            ProcessMapData();
            tileInitialized.SetResult(true);

            SetupNeighbours(TilesPositions, TileInitializers);

            dungeonLevel = new DungeonLevel(dungeon, creatures, level, TilesPositions, TilesPositions[start], legacyTileCreator.MiniMap);

            foreach (var tileInitializer in TileInitializers)
            {
                tileInitializer.Level = dungeonLevel;
                tileInitializer.Initialize();

            }

            loadedLevels.Add(level, dungeonLevel);
            return dungeonLevel;
        }

        private void ProcessMapData()
        {
            var offset = new Point(CurrentMap.OffsetX, CurrentMap.OffsetY);
            for (int y = 0; y < CurrentMap.Height; y++)
            {
                for (int x = 0; x < CurrentMap.Width; x++)
                {
                    var pos = new Point(x, y) + offset;
                    var tile = legacyTileCreator.GetTile(new TileInfo<TileData>
                    {
                        Position = pos,
                        Tile = CurrentMap.GetTileData(pos)
                    });

                    if (tile != null)
                    {

                        TilesPositions.Add(pos, tile);
                        tiles.Add(tile);
                    }
                }
            }
        }



        public IEnumerable<TileInfo<TileData>> GetNeigbourTiles(Point position, DungeonMap map)
        {
            return MapDirection.Sides.Select(side =>
            {
                var p = position + side.RelativeShift;
                return new TileInfo<TileData>
                {
                    Position = p,
                    Tile = map[p.X, p.Y]
                };
            });
        }


        private void InitializeMapTextures()
        {
            defaultDoorTexture = ResourceProvider.Instance.Content.Load<Texture2D>("Textures/DefaultDoor");

            defaultMapDoorTypeTexture = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{Enum.GetName(CurrentMap.DoorType0.GetType(), CurrentMap.DoorType0)}");

            doorTextures = new Texture2D[CurrentMap.DoorDecorationCount];
            for (int i = 0; i < doorTextures.Length; i++)
                doorTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{CurrentMap.DoorDecorations[i].Name}");

            wallTextures = new Texture2D[CurrentMap.WallGraphicsCount];
            for (int i = 0; i < wallTextures.Length; i++)
                wallTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{CurrentMap.WallDecorations[i].Name}");

            floorTextures = new Texture2D[CurrentMap.FloorGraphicsCount];
            for (int i = 0; i < floorTextures.Length; i++)
                floorTextures[i] = ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/{CurrentMap.FloorDecorations[i].Name}");

            championTextures = Data.ChamptionDescriptors
                .Select(x => ResourceProvider.Instance.Content.Load<Texture2D>(x.TexturePath))
                .ToArray();

            WallTexture = ResourceProvider.Instance.Content.Load<Texture2D>("Textures/Wall");
            DoorButtonTexture = ResourceProvider.Instance.Content.Load<Texture2D>("Textures/DoorButton");
            TeleportTexture = ResourceProvider.Instance.Content.Load<Texture2D>("Textures/Teleport");
        }


        public IGrabableItemFactoryBase GetItemFactory(int identifer)
        {
            var itemDescriptor = Data.ItemGlobalIdentifers[identifer];
            switch (itemDescriptor.Category)
            {
                case ObjectCategory.Weapon:
                    return WeaponFactories[itemDescriptor.InCategoryIndex];
                case ObjectCategory.Clothe:
                    return ClothFactories[itemDescriptor.InCategoryIndex];
                case ObjectCategory.Scroll:
                    return ScrollFactories[itemDescriptor.InCategoryIndex];
                case ObjectCategory.Potion:
                    return PotionFactories[itemDescriptor.InCategoryIndex];
                case ObjectCategory.Container:
                    return ContainerFactories[itemDescriptor.InCategoryIndex];
                case ObjectCategory.Miscellenaous:
                    return MiscFactories[itemDescriptor.InCategoryIndex];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<Tuple<Tile, MapDirection>> GetTargetTile(Point? target, MapDirection requesteDirection)
        {
            MapDirection invertMessageDirection = requesteDirection;
            if (target == null)
                return null;

            await tileInitialized.Task;

            Tile tile = null;
            TilesPositions.TryGetValue(target.Value, out tile);

            if (tile == null)
            {
                var targetTileData = CurrentMap.GetTileData(target.Value);
                if (targetTileData.Actuators.Any() && targetTileData.Actuators.All(x => x.ActuatorType == 5 || x.ActuatorType == 6))
                {
                    throw new InvalidOperationException("this should not be possible");
                }
                else if (targetTileData.Actuators.Any() && !targetTileData.Actuators.All(x => x.ActuatorType != 5 && x.ActuatorType != 6))
                {
                    throw new InvalidOperationException("mixture of wall and virtual actuators");
                }
                else
                {//find floor tile where is wall actuator put & thus invert message direction
                    invertMessageDirection = requesteDirection.Opposite;
                    TilesPositions.TryGetValue(target.Value + requesteDirection.RelativeShift, out tile);
                    if (tile == null)
                        throw new InvalidOperationException();
                }
            }

            return Tuple.Create(tile, invertMessageDirection);
        }

        public DoorDescriptor GetCurrentDoor(DoorTypeIndex index)
        {
            var firstType = Data.DoorDescriptors[(int)CurrentMap.DoorType0];
            var secondType = Data.DoorDescriptors[(int)CurrentMap.DoorType1];

            switch (index)
            {
                case DoorTypeIndex.FirstType:
                    return firstType;
                case DoorTypeIndex.SecondType:
                    return secondType;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index), index, null);
            }
        }

        public Renderer GetWallIllusionTileRenderer(WallIlusion wallIlusion, Texture2D wallTexture)
        {
            return new WallIllusionRenderer(wallIlusion);
        }

        public Renderer GetWallIllusionTileSideRenderer(TileSide tileSide, Texture2D wallTexture, Texture2D decoration)
        {
            return new WallIllusionTileSideRenderer(tileSide, wallTexture, decoration);  
        }
    }
}
