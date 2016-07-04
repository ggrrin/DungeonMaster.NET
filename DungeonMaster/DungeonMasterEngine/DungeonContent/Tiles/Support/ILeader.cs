using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Support
{
    public interface ILeader : ILocalizable<ITile>
    {
        event EventHandler LocationChanged;
        IReadOnlyList<ILiveEntity> PartyGroup { get; }
        IGrabableItem Hand { get; set; }

        object Interactor { get; }
        ILiveEntity Leader { get; }
        Matrix View { get; }
        Matrix Projection { get; }
        bool Enabled { get; set; }

        bool AddChampoinToGroup(ILiveEntity entity);
        void Draw(BasicEffect effect);
    }
}