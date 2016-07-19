using System.Linq;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Potions;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterParser.Descriptors;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Support;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders
{
    class PotionFactoriesBuilder
    {
        private readonly LegacyFactories builder;

        public PotionFactoriesBuilder(LegacyFactories builder)
        {
            this.builder = builder;
        }

        public virtual PotionFactory[] InitPotionFactories()
        {
            return builder.Data.PotionDescriptors
                .Select(p =>
                {
                    var itemDescriptor = builder.Data.GetItemDescriptor(ObjectCategory.Potion, p.Identifer);
                    return SortPotion(p, itemDescriptor);
                })
                .ToArray();
        }

        private PotionFactory SortPotion(PotionDescriptor p, ItemDescriptor itemDescriptor)
        {
            switch (itemDescriptor.InCategoryIndex)
            {
                case 6:
                    return GetDrinkablePotionFactory<RosPotion>(p, itemDescriptor);
                case 7:
                    return GetDrinkablePotionFactory<KuPotion>(p, itemDescriptor);
                case 8:
                    return GetDrinkablePotionFactory<DanePotion>(p, itemDescriptor);
                case 9:
                    return GetDrinkablePotionFactory<NetaPotion>(p, itemDescriptor);
                case 10:
                    return GetDrinkablePotionFactory<BroPotion>(p, itemDescriptor);
                case 11:
                    return GetDrinkablePotionFactory<MaPotion>(p, itemDescriptor);
                case 12:
                    return GetDrinkablePotionFactory<YaPotion>(p, itemDescriptor);
                case 13:
                    return GetDrinkablePotionFactory<EePotion>(p, itemDescriptor);
                case 14:
                    return GetDrinkablePotionFactory<ViPotion>(p, itemDescriptor);
                case 15:
                    return GetDrinkablePotionFactory<WaterPotion>(p, itemDescriptor);
                //case 20: empty flask
                default:
                    return GetPotionFactory(p, itemDescriptor);
            }
        }

        private DrinkablePotionFactory<T> GetDrinkablePotionFactory<T>(PotionDescriptor potionDescriptor, ItemDescriptor itemDescriptor) where T : DrinkablePotion, new()
        {
            return new DrinkablePotionFactory<T>(
                potionDescriptor.Name,
                potionDescriptor.Weight,
                builder.ActionCombos[itemDescriptor.AttackCombo],
                builder.GetStorageTypes(itemDescriptor.CarryLocation),
                builder.RenderersSource.GetItemRenderer(ResourceProvider.Instance.Content.Load<Texture2D>(potionDescriptor.TexturePath)));

        }

        private PotionFactory GetPotionFactory(PotionDescriptor p, DungeonMasterParser.Support.ItemDescriptor itemDescriptor)
        {
            return new PotionFactory(
                p.Name,
                p.Weight,
                builder.ActionCombos[itemDescriptor.AttackCombo],
                builder.GetStorageTypes(itemDescriptor.CarryLocation),
                builder.RenderersSource.GetItemRenderer(ResourceProvider.Instance.Content.Load<Texture2D>(p.TexturePath)));
        }
    }
}