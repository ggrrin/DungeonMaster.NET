using DungeonMasterEngine.DungeonContent.GroupSupport;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public abstract class LivingItem : Item
    {
        public abstract IRelationManager RelationManager { get; }

        protected  LivingItem(Vector3 position) : base(position) { }
    }
}