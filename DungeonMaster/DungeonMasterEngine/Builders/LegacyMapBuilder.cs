using DungeonMasterEngine.Helpers;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DungeonMasterEngine.Builders.ActuatorCreator;
using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.Builders.TileCreator;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Actions;
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
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser.Descriptors;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Tiles;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    public class LegacyMapBuilder : IDungonBuilder<IFactories>
    {
        protected static readonly Random rand = new Random();
        public Random Rand => rand;
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
        public IFactories Factories { get; protected set; }
        public virtual IRenderersSource RendererSource { get; }

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

        public LegacyMapBuilder(DungeonData data, IRenderersSource renderersSource)
        {
            Data = data; 

            ItemCreator = new LegacyItemCreator(this);
            RendererSource = renderersSource;
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

            start = startTile ?? new Point(Data.StartPosition.Position.X, Data.StartPosition.Position.Y);
            TilesPositions = new Dictionary<Point, Tile>();
        }

        public DungeonLevel GetLevel(IFactories factories, int level, Point? startTile)
        {
            this.Factories = factories;
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

            dungeonLevel = new DungeonLevel(Creatures, level, TilesPositions, TilesPositions[start], legacyTileCreator.MiniMap, CurrentMap.Difficulty);

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
                        TilesPositions.Add(pos, tile);
                }
            }
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
                    return Factories.WeaponFactories[itemDescriptor.InCategoryIndex];
                case ObjectCategory.Clothe:
                    return Factories.ClothFactories[itemDescriptor.InCategoryIndex];
                case ObjectCategory.Scroll:
                    return Factories.ScrollFactories[itemDescriptor.InCategoryIndex];
                case ObjectCategory.Potion:
                    return Factories.PotionFactories[itemDescriptor.InCategoryIndex];
                case ObjectCategory.Container:
                    return Factories.ContainerFactories[itemDescriptor.InCategoryIndex];
                case ObjectCategory.Miscellenaous:
                    return Factories.MiscFactories[itemDescriptor.InCategoryIndex];
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

        protected void SetupNeighbours(IDictionary<Point, Tile> tilesPositions,  IEnumerable<TileInitializer> initializers)
        {
            foreach (var t in initializers )
            {
                t.SetupNeighbours(tilesPositions);
            }
        }

    }
}
