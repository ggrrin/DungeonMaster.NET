using System;
using System.Linq;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{

    public class Pit : Pit<Message>
    {
        public Pit(PitInitializer initializer) : base(initializer) { }
    }

    public class PitTileNeighbours : TileNeighbours
    {
        public ITile Up { get; set; }
        public ITile Down { get; set; }

        public override ITile GetTile(MapDirection mapDirection)
        {
            var res = base.GetTile(mapDirection);
            if (res == null)
            {
                if (mapDirection == MapDirection.Up)
                    return Up;
                else if (mapDirection == MapDirection.Down)
                    return Down;
                else
                    return null;
            }
            else
                return res;
        }

        public PitTileNeighbours(Tile north, Tile south, Tile east, Tile west) : base(north, south, east, west)
        {
        }
    }

    public class Pit<TMessage> : FloorTile<TMessage>, ILevelConnector where TMessage : Message
    {
        private ITile nextLevelEnter;

        public ITile NextLevelEnter
        {
            get { return nextLevelEnter; }
            set
            {
                nextLevelEnter = value;
                PitNeighbours.Down = value;
            }
        }

        public int NextLevelIndex => LevelIndex + 1;
        public Point TargetTilePosition => GridPosition;


        public PitTileNeighbours PitNeighbours => (PitTileNeighbours)Neighbours;

        public override TileNeighbours Neighbours
        {
            get { return base.Neighbours; }
            protected set
            {
                base.Neighbours = (PitTileNeighbours)value;
            }
        }

        public bool Invisible { get; private set; }
        public bool Imaginary { get; private set; }

        public bool IsOpen => ContentActivated;

        public override bool IsAccessible => true;

        public override bool ContentActivated
        {
            get { return base.ContentActivated; }
            protected set
            {
                base.ContentActivated = value;
                foreach (var subItem in SubItems.ToArray())
                {
                    var movable = subItem as IMovable<ITile>;

                    if (movable != null)
                        MakeItemFall(subItem);
                }
                
            }
        }

        public override void OnObjectEntered(object localizable)
        {
            base.OnObjectEntered(localizable);

            MakeItemFall(localizable);

        }

        private void MakeItemFall(object localizable)
        {
            if (IsOpen)
            {
                var loc = localizable as IMovable<ITile>;
                if (loc != null)
                {
                    //loc.Location = PitNeighbours.Down;
                    loc.MoveTo(PitNeighbours.Down, true);
                }
            }
        }

        public Pit(PitInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(PitInitializer initializer)
        {
            ContentActivated = initializer.IsOpen;
            Invisible = initializer.Invisible;
            Imaginary = initializer.Imaginary;

            initializer.Initializing -= Initialize;
        }

    }
}
