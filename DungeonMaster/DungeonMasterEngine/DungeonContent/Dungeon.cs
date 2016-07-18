using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent
{
    public class Dungeon : DungeonBase<IFactories, LegacyLeader>
    {
        public Dungeon(IDungonBuilder<IFactories> builder, IFactories factoreis, LegacyLeader leader, GraphicsDevice graphicsDevice) : base(builder, factoreis, leader, graphicsDevice) { }
    }
}