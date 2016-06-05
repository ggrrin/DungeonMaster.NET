using System;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class FloorInitializer : TileInitializer
    {
        public new event Initializer<FloorInitializer> Initializing;

        public TileSide NorthSide { get; set; }
        public TileSide EastSide { get; set; }
        public TileSide SouthSide{ get; set; }
        public TileSide WestSide { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            Initializing?.Invoke(this);
        }
    }

    public class Floor : Floor<Message>
    {
        public Floor(FloorInitializer initializer ) : base(initializer) {}
    }

    public class Floor<TMessage> : Tile<TMessage> where TMessage : Message 
    {
        public Floor(FloorInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(FloorInitializer initializer)
        {
            //TODO

            initializer.Initializing -= Initialize;
        }

        public override TileNeighbours Neighbours
        {
            get
            {
                return base.Neighbours;
            }

            set
            {
                base.Neighbours = value;
                UpdateWall();
            }
        }

        protected virtual void UpdateWall()
        {
            //TODO
            //wallGraphic.DrawFaces = CubeFaces.All; //reset

            //if (Neighbours.North != null)
            //    wallGraphic.DrawFaces ^= CubeFaces.Back;
            //if (Neighbours.East != null)
            //    wallGraphic.DrawFaces ^= CubeFaces.Right;
            //if (Neighbours.South != null)
            //    wallGraphic.DrawFaces ^= CubeFaces.Front;
            //if (Neighbours.West != null)
            //    wallGraphic.DrawFaces ^= CubeFaces.Left;
        }
        public override bool IsAccessible => true;
    }




    public abstract class InitializerBase
    {
        public abstract void Initialize();
    }

    public delegate void Initializer<TObject>(TObject initializer) where TObject : InitializerBase;

    class MocapBaseInitializer : InitializerBase
    {
        public event Initializer<MocapBaseInitializer> Initializing;

        public float baseVAlue { get; set; }

        public override void Initialize()
        {
            Initializing?.Invoke(this);
        }
    }

    class MocapDefensiveInitializer : MocapBaseInitializer
    {
        public string AValue { get; set; }
        public new event Initializer<MocapDefensiveInitializer> Initializing;

        public override void Initialize()
        {
            base.Initialize();
            Initializing?.Invoke(this);
        }
    }

    class MocapBase
    {
        private float x;

        public MocapBase(MocapBaseInitializer initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(MocapBaseInitializer initializer)
        {
            x = initializer.baseVAlue;
            initializer.Initializing -= Initialize;
        }
    }


    class MocapDefensive : MocapBase
    {
        private readonly MocapDefensiveInitializer initializer;
        private string avalue;

        public MocapDefensive(MocapDefensiveInitializer initializer) : base(initializer)
        {
            this.initializer = initializer;
            initializer.Initializing += Initialize;
        }

        private void Initialize(MocapDefensiveInitializer initalizer)
        {
            avalue = initalizer.AValue;

            initalizer.Initializing -= Initialize;
        }

    }
}
