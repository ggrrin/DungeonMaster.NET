using DungeonMasterEngine.Helpers;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DungeonMasterEngine.Builders.ActuatorCreators;
using DungeonMasterEngine.Builders.ItemCreators;
using DungeonMasterEngine.Builders.TileCreators;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
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

    public class LegacyMapBuilderInitializer
    {
        public event EventHandler<LegacyMapBuilderInitializer> Initializing;
        public DungeonData Data { get; set; }
        public LegacyTileCreator TileCreator { get; set; }
        public ILegacyItemCreator ItemCreator { get; set; }

        public void Initialize()
        {
            Initializing?.Invoke(this, this);
        }
    }

    

    public class LegacyMapBuilder : IDungonBuilder<IFactories>
    {
        private Point start;
        private TaskCompletionSource<bool> tileInitialized;
        protected readonly Dictionary<int, DungeonLevel> loadedLevels = new Dictionary<int, DungeonLevel>();

        private Texture2D[] doorTextures;
        private Texture2D[] wallTextures;
        private Texture2D[] floorTextures;
        private Texture2D[] championTextures;

        public DungeonData Data { get; private set; }
        public IFactories Factories { get; protected set; }
        public ILegacyItemCreator ItemCreator { get; private set; }
        public ILegacyTileCreator TileCreator { get; private set; }
        public RelationToken CreatureToken { get; } = new RelationToken(1); //TODO RelationTokenFactory.GetNextToken();
        public RelationToken ChampionToken { get; } = new RelationToken(0); //TODO RelationTokenFactory.GetNextToken();
        
        public int CurrentLevelIndex { get; private set; }
        public DungeonMap CurrentMap { get; private set; }
        public Dictionary<Point, Tile> TilePositions { get; private set; }
        public List<TileInitializer> TileInitializers { get; private set; }

        public Texture2D DefaultDoorTexture { get; private set; }
        public Texture2D DoorButtonTexture { get; protected set; }
        public Texture2D TeleportTexture { get; protected set; }
        public Texture2D WallTexture { get; private set; }
        public IReadOnlyList<Texture2D> DoorTextures => doorTextures;
        public IReadOnlyList<Texture2D> WallTextures => wallTextures;
        public IReadOnlyList<Texture2D> FloorTextures => floorTextures;
        public IReadOnlyList<Texture2D> ChampionTextures => championTextures;

        public LegacyMapBuilder(LegacyMapBuilderInitializer initializer)
        {
            initializer.Initializing += InitializerOnInitializing;
        }

        private void InitializerOnInitializing(object sender, LegacyMapBuilderInitializer initializer)
        {
            Data = initializer.Data;
            ItemCreator = initializer.ItemCreator;
            TileCreator = initializer.TileCreator;

            initializer.Initializing -= InitializerOnInitializing;
        }

        protected virtual void Initialize(int level, Point? startTile)
        {
            CurrentLevelIndex = level;
            CurrentMap = Data.Maps[level];
            TileCreator.Reset();
            ItemCreator.Reset();
            InitializeMapTextures();
            TileInitializers = new List<TileInitializer>();

            start = startTile ?? new Point(Data.StartPosition.Position.X, Data.StartPosition.Position.Y);
            TilePositions = new Dictionary<Point, Tile>();
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

            SetupNeighbours(TilePositions, TileInitializers);

            dungeonLevel = new DungeonLevel(level, TilePositions, TilePositions[start], TileCreator.MiniMap, CurrentMap.Difficulty);

            foreach (var tileInitializer in TileInitializers)
            {
                tileInitializer.Level = dungeonLevel;
                tileInitializer.Initialize();
            }

            foreach (var tileInitializer in TileInitializers)
                tileInitializer.NotifyInitialized();

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
                    var tile = TileCreator.GetTile(new TileInfo<TileData>
                    {
                        Position = pos,
                        Tile = CurrentMap.GetTileData(pos)
                    });

                    if (tile != null)
                        TilePositions.Add(pos, tile);
                }
            }
        }

        protected virtual void InitializeMapTextures()
        {
            DefaultDoorTexture = ResourceProvider.Instance.Content.Load<Texture2D>("Textures/DefaultDoor");

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
            TilePositions.TryGetValue(target.Value, out tile);

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
                    TilePositions.TryGetValue(target.Value + requesteDirection.RelativeShift, out tile);
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

        protected void SetupNeighbours(IDictionary<Point, Tile> tilesPositions, IEnumerable<TileInitializer> initializers)
        {
            foreach (var t in initializers)
            {
                t.SetupNeighbours(tilesPositions);
            }
        }

    }
}
