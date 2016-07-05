using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Misc;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class ChampionBonesFactory : MiscItemFactory
    {
        public ChampionBonesFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, ITextureRenderer renderer) : base(name, weight, attackCombo, carryLocation, renderer) {}

        public new ChampionBones Create<TItemInitiator>(TItemInitiator initiator) where TItemInitiator : IChampionBonesInitializer 
        {
            return new ChampionBones(initiator, this);
        }

        public override IGrabableItem CreateItem()
        {
            return Create(new ChampionBonesInitializer());
        }
    }
}