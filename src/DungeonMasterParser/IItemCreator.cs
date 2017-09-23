using DungeonMasterParser.Items;

namespace DungeonMasterParser
{
    public interface IItemCreator<out T>
    {
        T CreateActuator(ActuatorItemData actuator);
        T CreateCloth(ClothItemData cloth);
        T CreateContainer(ContainerItemData container);
        T CreateCreature(CreatureItem creature);
        T CreateDoor(DoorItem door);
        T CreateMisc(MiscellaneousItemData misc);
        T CreatePotion(PotionItemData potion);
        T CreateScrool(ScrollItemData scroll);
        T CreateTeleport(TeleporterItem teleport);
        T CreateTextData(TextDataItem textTag);
        T CreateWeapon(WeaponItemData weapon);
    }
}
