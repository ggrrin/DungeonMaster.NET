using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles.Support
{
    public interface ITile : IWorldObject,IStopable, INeighbourable<ITile>
    {
        
        bool ContentActivated { get; }

        bool IsAccessible { get; }
        LayoutManager<ILiveEntity> LayoutManager { get; }
        DungeonLevel Level { get; }
        IEnumerable<TileSide> Sides { get; }
        new TileNeighbours Neighbours { get; }// TODO generic??

        event EventHandler<object> ObjectEntered;
        event EventHandler<object> ObjectLeft;

        void ActivateTileContent();
        void DeactivateTileContent();

        IEnumerable<object> SubItems { get; }
        void OnObjectEntered(object localizable);
        void OnObjectEntering(object localizable);
        void OnObjectLeaving(object localizable);
        void OnObjectLeft(object localizable);

        void Update(GameTime gameTime);
    }
}