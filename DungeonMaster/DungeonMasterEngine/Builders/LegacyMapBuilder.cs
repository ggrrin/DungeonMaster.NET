using DungeonMasterEngine.Helpers;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterParser.Descriptors;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
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

        public List<ILiveEntity> Creatures { get; private set; } = new List<ILiveEntity>();

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
        public virtual IWallGraphicSource RendererSource { get; }

        public virtual IReadOnlyList<ISkillFactory> Skills { get; } = new ISkillFactory[]
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

        public virtual IReadOnlyList<HumanActionFactoryBase> FightActions { get; }
        public virtual IReadOnlyList<IReadOnlyList<IActionFactory>> ActionCombos { get; }

        //item factories
        public virtual IReadOnlyList<WeaponItemFactory> WeaponFactories { get; }
        public virtual IReadOnlyList<ClothItemFactory> ClothFactories { get; }
        public virtual IReadOnlyList<ContainerItemFactory> ContainerFactories { get; }
        public virtual IReadOnlyList<ScrollItemFactory> ScrollFactories { get; }
        public virtual IReadOnlyList<PotionItemFactory> PotionFactories { get; }
        public virtual IReadOnlyList<MiscItemFactory> MiscFactories { get; }
        public virtual IReadOnlyList<CreatureFactory> CreatureFactories { get; }

        public virtual Texture2D RandomWallDecoration
        {
            get
            {
                int val = rand.Next(29);
                return val < CurrentMap.WallDecorationGraphicsCount ? WallTextures[val] : null;
            }
        }

        public virtual Texture2D RandomFloorDecoration
        {
            get
            {
                int val = rand.Next(29);
                return val < CurrentMap.FloorDecorationGraphicsCount ? FloorTextures[val] : null;
            }
        }

        public Texture2D DoorButtonTexture { get; protected set; }
        public Texture2D TeleportTexture { get; protected set; }

        public LegacyMapBuilder()
        {
            var parser = new DungeonParser();
            parser.Parse();
            Data = parser.Data;

            ItemCreator = new LegacyItemCreator(this);

            FightActions = GetFightActionsFactories();
            ActionCombos = GetComboActions();

            WeaponFactories = Data.WeaponDescriptors
                .Select(wd =>
                {
                    var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Weapon, wd.Identifer);
                    return new WeaponItemFactory(wd.Name, wd.Weight, ActionCombos[itemDescriptor.AttackCombo],
                        ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation),
                        wd.DeltaEnergy, (WeaponClass)wd.Class, wd.KineticEnergy, wd.ShootDamage, wd.Strength, ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath));
                })
                .ToArray();

            ClothFactories = Data.ArmorDescriptors
               .Select(wd =>
               {
                   var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Clothe, wd.Identifer);
                   return new ClothItemFactory(wd.Name, wd.Weight, ActionCombos[itemDescriptor.AttackCombo]
                       , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation), ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath));
               })
               .ToArray(); ;

            ContainerFactories = Data.ContainerDescriptors
               .Select(wd =>
               {
                   var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Container, wd.Identifer);
                   return new ContainerItemFactory(wd.Name, wd.Weight, ActionCombos[itemDescriptor.AttackCombo]
                       , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation), ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath));
               })
               .ToArray();

            ScrollFactories = Data.ScrollDescriptors
               .Select(wd =>
               {
                   var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Scroll, wd.Identifer);
                   return new ScrollItemFactory(wd.Name, wd.Weight, ActionCombos[itemDescriptor.AttackCombo]
                       , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation), ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath));
               })
               .ToArray();


            MiscFactories = Data.MiscDescriptors
                .Select(wd =>
                {
                    var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Miscellenaous, wd.Identifer);
                    return new MiscItemFactory(wd.Name, wd.Weight, ActionCombos[itemDescriptor.AttackCombo]
                        , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation), ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath));
                })
                .ToArray();

            PotionFactories = Data.PotionDescriptors
                .Select(p =>
                {
                    var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Potion, p.Identifer);
                    return new PotionItemFactory(p.Name, p.Weight, ActionCombos[itemDescriptor.AttackCombo]
                        , ItemCreator.GetStorageTypes(itemDescriptor.CarryLocation), ResourceProvider.Instance.Content.Load<Texture2D>(p.TexturePath));
                })
                .ToArray();

            CreatureFactories = Data.CreatureDescriptors
                .Select(wd => new CreatureFactory(GetGroupLayout(wd.Size), wd.Name, MathHelper.Clamp(wd.MovementDuration * 1000 / 6, 500, 1200),
                wd.DetectionRange, wd.SightRange, wd.ExperienceClass,
                ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/Creatures/DM-Creature-SuperNES-{wd.Name}")))
                .ToArray();




            RendererSource = new DefaultWallGrahicsSource();
        }

        private IGroupLayout GetGroupLayout(int size)
        {
            switch (size)
            {
                case 0:
                    return Small4GroupLayout.Instance;
                case 1:
                    return Medium2GroupLayout.Instance;
                case 2:
                    return FullTileLayout.Instance;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }






        protected virtual IReadOnlyList<HumanActionFactoryBase> GetFightActionsFactories()
        {
            return Data.FightActions.Select<FightActionDescriptor, HumanActionFactoryBase>(action =>
             {
                 switch (action.Number)
                 {
                     case FightActionEnum.C030_ACTION_BASH:
                     case FightActionEnum.C018_ACTION_HACK:
                     case FightActionEnum.C019_ACTION_BERZERK:
                     case FightActionEnum.C007_ACTION_KICK:
                     case FightActionEnum.C013_ACTION_SWING:
                     case FightActionEnum.C002_ACTION_CHOP:
                         return new SwingAttackFactory(
                             action.Name,
                             action.ExperienceGain,
                             action.DefenseModifier,
                             action.HitProbability,
                             action.Damage,
                             action.Fatigue * 1000 / 6,
                             Skills[action.ImprovedSkill],
                             action.Stamina,
                             -1);

                     case FightActionEnum.C024_ACTION_DISRUPT:
                     case FightActionEnum.C016_ACTION_JAB:
                     case FightActionEnum.C017_ACTION_PARRY:
                     case FightActionEnum.C014_ACTION_STAB:
                     case FightActionEnum.C009_ACTION_STAB:
                     case FightActionEnum.C031_ACTION_STUN:
                     case FightActionEnum.C015_ACTION_THRUST:
                     case FightActionEnum.C025_ACTION_MELEE:
                     case FightActionEnum.C028_ACTION_SLASH:
                     case FightActionEnum.C029_ACTION_CLEAVE:
                     case FightActionEnum.C006_ACTION_PUNCH:
                         return new MeleeAttackFactory(
                             action.Name,
                             action.ExperienceGain,
                             action.DefenseModifier,
                             action.HitProbability,
                             action.Damage,
                             action.Fatigue * 1000 / 6,
                             Skills[action.ImprovedSkill],
                             action.Stamina,
                             -1);

                     default:
                         return null;//TODO
                 }
             })
            .ToArray();

        }

        private IReadOnlyList<IReadOnlyList<IActionFactory>> GetComboActions()
        {
            // ReSharper disable once CoVariantArrayConversion
            return Data.FightCombos.Select(c => c.Actions
                    .Select(x => new ComboActionFactory(x.UseCharges == 1, x.MinimumSkillLevel,
                        FightActions[(int)x.ActionDescriptor.Number]))
                    .ToArray())
                .ToArray();
        }

        protected virtual void Initialize(int level, Point? startTile)
        {
            CurrentLevel = level;
            CurrentMap = Data.Maps[level];
            legacyTileCreator = new LegacyTileCreator(this);
            FloorActuatorCreator = new FloorActuatorCreator(this);
            ItemCreator = new LegacyItemCreator(this);
            Creatures = new List<ILiveEntity>();
            InitializeMapTextures();
            TileInitializers = new List<TileInitializer>();
            tiles = new List<Tile>();
            rand = new Random(1);

            start = startTile ?? new Point(Data.StartPosition.Position.X, Data.StartPosition.Position.Y);
            TilesPositions = new Dictionary<Point, Tile>();
        }


        public override DungeonLevel GetLevel(int level, Point? startTile)
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

            dungeonLevel = new DungeonLevel(Creatures, level, TilesPositions, TilesPositions[start], legacyTileCreator.MiniMap);

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


        protected virtual void InitializeMapTextures()
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


        public virtual IGrabableItemFactoryBase GetItemFactory(int identifer)
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

        public virtual async Task<Tuple<Tile, MapDirection>> GetTargetTile(Point? target, MapDirection requesteDirection)
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

        public virtual DoorDescriptor GetCurrentDoor(DoorTypeIndex index)
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


    }
}
