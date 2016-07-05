using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class ClothItemFactory : GrabableItemFactoryBase
    {

        public int ArmorStrength { get; }
        public int SharpResistance { get; }
        public bool IsShield { get; }

        public ClothItemFactory(string name, int weight, IReadOnlyList<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, int armorStrength, int sharpResistance, bool isShield, ITextureRenderer renderer) : base(name, weight, attackCombo, carryLocation, renderer)
        {
            ArmorStrength = armorStrength;
            SharpResistance = sharpResistance;
            IsShield = isShield;
        }

        public Cloth Create<TItemInitiator>(TItemInitiator initiator) where TItemInitiator : IClothInitializer
        {
            return new Cloth(initiator, this);
        }

        public override IGrabableItem CreateItem()
        {
            return Create(new ClothInitializer
            {
                IsBroken = false,
                IsCruised = false
            });
        }
    }
}