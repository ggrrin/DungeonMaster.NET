using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles.Support
{
    public interface ITile : IWorldObject,IStopable, INeighbourable<ITile>
    {
        bool IsAccessible { get; }
        bool CanFlyItems { get; }
        bool IsTransparent { get; }
        LayoutManager<ILiveEntity> LayoutManager { get; }
        new TileNeighbors Neighbors { get; }
        DungeonLevel Level { get; }

        //IEnumerable<TileSide> Sides { get; }
        //IEnumerable<object> SubItems { get; }
        void Update(GameTime gameTime);


        bool ContentActivated { get; }
        void ActivateTileContent();
        void DeactivateTileContent();
        void AcceptMessageBase(Message message);

        event EventHandler<object> ObjectEntered;
        event EventHandler<object> ObjectLeft;

        void OnObjectEntered(object localizable);
        void OnObjectEntering(object localizable);
        void OnObjectLeaving(object localizable);
        void OnObjectLeft(object localizable);


    }
}