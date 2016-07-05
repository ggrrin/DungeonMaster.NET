using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public abstract class GrabableItemFactoryBase : IGrabableItemFactoryBase
    {
        private readonly HashSet<IActionFactory> combos;
        private readonly HashSet<IStorageType> locations;

        public string Name { get; }
        public int Weight { get; }
        public IEnumerable<IActionFactory> ActionCombo => combos;
        public IEnumerable<IStorageType> CarryLocation => locations;
        public ITextureRenderer Renderer { get; }

        protected GrabableItemFactoryBase(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, ITextureRenderer renderer)
        {
            Name = name;
            Weight = weight;
            combos = new HashSet<IActionFactory>(attackCombo);
            locations = new HashSet<IStorageType>(carryLocation);
            Renderer = renderer;
        }

        public bool CanBeStoredIn(IStorageType storage) => locations.Contains(storage);


        public abstract IGrabableItem CreateItem();
    }
}