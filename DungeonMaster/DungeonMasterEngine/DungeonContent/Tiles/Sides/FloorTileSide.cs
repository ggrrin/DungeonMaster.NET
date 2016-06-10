using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class FloorTileSide : TileSide
    {
        public IReadOnlyList<FloorItemStorage> Spaces { get; }

        public FloorTileSide(bool randomDecoration, MapDirection face, IEnumerable<IGrabableItem> topLeftItems, IEnumerable<IGrabableItem> topRightItems, IEnumerable<IGrabableItem> bottomLeftItems, IEnumerable<IGrabableItem> bottomRightItems) : base(face, randomDecoration)
        {
            Spaces = new[]
            {
                new FloorItemStorage(new Point(-1,1),  topLeftItems),
                new FloorItemStorage(new Point(1,1),topRightItems),
                new FloorItemStorage(new Point(-1,-1),bottomLeftItems),
                new FloorItemStorage(new Point(1,-1),bottomRightItems),
            };
        }

    }
}