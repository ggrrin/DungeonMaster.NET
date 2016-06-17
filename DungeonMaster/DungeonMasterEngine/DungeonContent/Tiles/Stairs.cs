using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public override IEnumerable<TileSide> Sides => Enumerable.Empty<TileSide>();
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
                    Neighbours = new MultiTileNeighbours(new TileNeighbours(Neighbours), new TileNeighbours(value.Neighbours));
                    //instead assing symetricaly //NextLevelEnter.Neighbours = Neighbours;
                }
            }

        }

        public Point TargetTilePosition => GridPosition;

    }

    public class StairsRenderer : Renderer
    {
        private readonly Texture2D wallTexture;

        public StairsRenderer(Stairs stairs, Texture2D wallTexture)
        {
            this.wallTexture = wallTexture;
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            return GetCurrentTransformation(ref currentTransformation);
        }

        public override Matrix GetCurrentTransformation(ref Matrix parentTransformation)
        {
            return Matrix.Identity;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            return false;
        }
    }



}
