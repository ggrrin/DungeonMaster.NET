using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public interface ITile : IWorldObject
    {
        bool ContentActivated { get; }
        Point GridPosition { get; }
        bool IsAccessible { get; }
        LayoutManager LayoutManager { get; }
        DungeonLevel Level { get; }
        int LevelIndex { get; }
        TileNeighbours Neighbours { get; }
        IEnumerable<TileSide> Sides { get; }
        Vector3 StayPoint { get; }
        List<IItem> SubItems { get; }

        event EventHandler<IItem> ObjectEntered;
        event EventHandler<IItem> ObjectLeft;

        void ActivateTileContent();
        void DeactivateTileContent();
        void OnObjectEntered(IItem obj);
        void OnObjectEntering(IItem obj);
        void OnObjectLeaving(IItem obj);
        void OnObjectLeft(IItem obj);
        void Update(GameTime gameTime);
    }
}