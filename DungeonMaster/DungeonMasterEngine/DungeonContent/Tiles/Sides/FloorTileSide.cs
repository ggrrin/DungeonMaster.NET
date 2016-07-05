using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles.Sides
{
    public class FloorTileSide : TileSide, IEnumerable<object>
    {
        public event EventHandler SubItemsChanged;

        public FloorSpaceManager SpaceManager { get; }

        protected readonly List<object> subItems = new List<object>();

        public IReadOnlyList<FloorItemStorage> Spaces { get; protected set; }

        public FloorTileSide(bool randomDecoration, MapDirection face, IEnumerable<IGrabableItem> topLeftItems, IEnumerable<IGrabableItem> topRightItems, IEnumerable<IGrabableItem> bottomLeftItems, IEnumerable<IGrabableItem> bottomRightItems) : base(face)
        {
            Spaces = new[]
            {
                new FloorItemStorage(new Point(0, 0), topLeftItems),
                new FloorItemStorage(new Point(1, 0),topRightItems),
                new FloorItemStorage(new Point(0, 1),bottomLeftItems),
                new FloorItemStorage(new Point(1, 1),bottomRightItems),
            };
            SpaceManager = new FloorSpaceManager(Spaces);

            subItems.AddRange(Spaces.SelectMany(s => s.Items));

            foreach (var space in Spaces)
            {
                space.ItemAdding += (sender, item) => OnObjectEntered(item, addToSpacd: false);
                space.ItemRemoving += (s, item) => OnObjectLeft(item);
            }
        }

        public virtual void OnObjectEntered(object localizable, bool addToSpacd = true)
        {
            if (subItems.Contains(localizable))
                throw new InvalidOperationException("item already in collection");

            subItems.Add(localizable);

            if (addToSpacd)
            {
                var grabable = localizable as IGrabableItem;
                if (grabable != null)
                {
                    var space = SpaceManager.GetSpace(grabable);
                    space.AddItem(grabable, false);
                }
            }

            SubItemsChanged?.Invoke(this, new EventArgs());
        }


        public virtual void OnObjectLeft(object localizable)
        {

            if (!subItems.Contains(localizable))
                throw new InvalidOperationException("item is not in collection");

            subItems.Remove(localizable);

            var grabable = localizable as IGrabableItem;
            if (grabable != null)
            {
                foreach (var storage in Spaces)
                    storage.RemoveItem(grabable, false);
            }
            SubItemsChanged?.Invoke(this, new EventArgs());
        }

        public IEnumerator<object> GetEnumerator()
        {
            return subItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}