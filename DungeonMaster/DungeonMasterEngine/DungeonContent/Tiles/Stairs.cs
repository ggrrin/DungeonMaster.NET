using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class Stairs : Stairs<Message>
    {

        public Stairs(StairsInitializer initializer) : base(initializer) { }
    }

    public class Stairs<TMessage> : Tile<TMessage>, ILevelConnector where TMessage : Message
    {
        public bool Up { get; private set; }

        public int NextLevelIndex => LevelIndex + (Up ? -1 : +1);

        /// <summary>
        /// Creates fake stairs
        /// </summary>
        /// <param name="position"></param>
        public Stairs(StairsInitializer initializer) : base(initializer)
        {
            initializer.Initializer += Initialize;
            Up = !initializer.Down;
        }

        private void Initialize(StairsInitializer initializer)
        {

            initializer.Initializer -= Initialize;
        }

        public override bool IsAccessible => true;

        public override IEnumerable<ITileSide> Sides => Enumerable.Empty<ITileSide>();
        private readonly List<object> subItems = new List<object>();
        public override IEnumerable<object> SubItems => subItems;
        public override Vector3 StayPoint => base.StayPoint + 0.5f * (Up ? Vector3.Up : Vector3.Down);

        public override void OnObjectEntered(object localizable)
        {
            base.OnObjectEntered(localizable);
            subItems.Add(localizable);
        }

        public override void OnObjectLeft(object localizable)
        {
            base.OnObjectLeft(localizable);
            subItems.Remove(localizable);
        }

        private ITile nextLevelEnter;
        public ITile NextLevelEnter
        {
            get { return nextLevelEnter; }
            set
            {
                if (value != null)
                {
                    nextLevelEnter = value;
                    Neighbors = new MultiTileNeighbors(new TileNeighbors(Neighbors), new TileNeighbors(value.Neighbors));
                    //instead assing symetricaly //NextLevelEnter.Neighbours = Neighbours;
                }
            }

        }

        public Point TargetTilePosition => GridPosition;

    }
}
