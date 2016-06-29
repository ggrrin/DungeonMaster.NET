using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public abstract class GrabableItemFactoryBase : IGrabableItemFactoryBase
    {
        private readonly HashSet<IActionFactory> combos;
        private readonly HashSet<IStorageType> locations;

        public Texture2D Texture { get; }
        public string Name { get; }
        public int Weight { get; }
        public IEnumerable<IActionFactory> ActionCombo => combos;
        public IEnumerable<IStorageType> CarryLocation => locations;

        protected GrabableItemFactoryBase(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, Texture2D texture)
        {
            Name = name;
            Weight = weight;
            Texture = texture;
            combos = new HashSet<IActionFactory>(attackCombo);
            locations = new HashSet<IStorageType>(carryLocation);
        }

        public bool CanBeStoredIn(IStorageType storage) => locations.Contains(storage);

        public abstract IGrabableItem Create();
    }
}