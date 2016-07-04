using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Misc;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class MiscItemFactory : GrabableItemFactoryBase
    {
        public MiscItemFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, IRenderer renderer) : base(name, weight, attackCombo, carryLocation, renderer)
        {
            
        }

        public Miscellaneous Create<TItemInitiator>(TItemInitiator initiator) where TItemInitiator : IMiscInitializer
        {
            return new Miscellaneous(initiator, this);
        }

        public override IGrabableItem CreateItem()
        {
            return Create(new MiscInitializer());
        }
    }
}