using DungeonMasterParser.Items;
using DungeonMasterParser.Tiles;

namespace DungeonMasterParser
{
    public class ItemSorter : IItemCreator<ItemData>
    {
        private TileData currentTile;
        public ItemData CreateItem(ItemData itemData, TileData tile)
        {
            currentTile = tile;
            return itemData.CreateItem(this);
        }

        public ItemData CreateActuator(ActuatorItemData actuator)
        {
            currentTile.Actuators.Add(actuator);
            return actuator;
        }

        public ItemData CreateCloth(ClothItemData cloth)
        {
            currentTile.GrabableItems.Add(cloth);
            return cloth;
        }

        public ItemData CreateContainer(ContainerItemData container)
        {
            currentTile.GrabableItems.Add(container);
            return container;
        }

        public ItemData CreateMisc(MiscellaneousItemData misc)
        {
            currentTile.GrabableItems.Add(misc);
            return misc;
        }

        public ItemData CreatePotion(PotionItemData potion)
        {
            currentTile.GrabableItems.Add(potion);
            return potion;
        }

        public ItemData CreateScrool(ScrollItemData scroll)
        {
            currentTile.GrabableItems.Add(scroll);
            return scroll;
        }

        public ItemData CreateWeapon(WeaponItemData weapon)
        {
            currentTile.GrabableItems.Add(weapon);
            return weapon;
        }

        public ItemData CreateTeleport(TeleporterItem teleport)
        {
            ((TeleporterTileData)currentTile).Teleport = teleport;
            return teleport;
        }

        public ItemData CreateDoor(DoorItem door)
        {
            ((DoorTileData)currentTile).Door = door;
            return door;
        }

        public ItemData CreateTextData(TextDataItem textTag)
        {
            currentTile.TextTags.Add(textTag);
            return textTag;
        }

        public ItemData CreateCreature(CreatureItem creature)
        {
            currentTile.Creatures.Add(creature);
            return creature;
        }
    }
}
