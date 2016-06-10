﻿using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class Stairs : Stairs<Message>
    {
        public Stairs(StairsInitializer initializer) : base(initializer) {}
    }

    public class Stairs<TMessage> : Tile<TMessage>, ILevelConnector where TMessage : Message
    {
        public GraphicsCollection graphics;

        private readonly bool up = false;

        public int NextLevelIndex => LevelIndex + (up ? -1 : +1);

        /// <summary>
        /// Creates fake stairs
        /// </summary>
        /// <param name="position"></param>
        public Stairs(StairsInitializer initializer) : base(initializer)
        {
            initializer.Initializer += Initialize;
        }

        private void Initialize(StairsInitializer initializer)
        {
            //TODO initalize

            initializer.Initializer -= Initialize;
        }

        void lStairs(Vector3 position, bool westEast, bool shapeL) 
        {
            //Position = position;

            var original = new CubeGraphic { Position = position, DrawFaces = CubeFaces.All ^ CubeFaces.Front ^ CubeFaces.Floor };
            var nextFloor = new CubeGraphic { Position = original.Position + Vector3.Down, DrawFaces = CubeFaces.Sides ^ CubeFaces.Front };
            var stairs = new ModelGraphic { Position = nextFloor.Position };

            original.Texture = nextFloor.Texture = nextFloor.Resources.Content.Load<Texture2D>("Textures/Wall");

            if (shapeL)
            {
                stairs.Model = stairs.Resources.Content.Load<Model>("Models/stairs1");

                if (westEast)
                    nextFloor.DrawFaces ^= CubeFaces.Right;
                else
                    nextFloor.DrawFaces ^= CubeFaces.Left;
            }
            else
            {
                stairs.Model = stairs.Resources.Content.Load<Model>("Models/stairs");
                nextFloor.DrawFaces ^= CubeFaces.Back;
            }

            if (westEast)
            {
                original.Rotation = nextFloor.Rotation = new Vector3(0, MathHelper.PiOver2, 0);
                stairs.Rotation = new Vector3(0, MathHelper.PiOver2, 0);
                stairs.MirrorX = true;
            }

            graphics = new GraphicsCollection(original, nextFloor, stairs);
            //graphicsProviders.SubProviders.Add(graphics);
        }

        public override bool IsAccessible
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<TileSide> Sides => Enumerable.Empty<TileSide>();
        public override Vector3 StayPoint { get { return base.StayPoint + 0.5f * (up ? Vector3.Up : Vector3.Down); } }


        private Tile nextLevelEnter;
        public Tile NextLevelEnter
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

}
