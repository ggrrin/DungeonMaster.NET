using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterParser.Items;

namespace DungeonMasterParser
{
    public interface IItemCreator<out T>
    {
        T GetItem(ActuatorItem i);      
        T GetItem(ClothItem i);
        T GetItem(ContainerItem i);
        T GetItem(CreatureItem i);
        T GetItem(DoorItem i);
        T GetItem(MiscellaneousItem i);
        T GetItem(PotionItem i);
        T GetItem(ScrollItem i);
        T GetItem(TeleporterItem i);
        T GetItem(TextDataItem i);
        T GetItem(WeaponItem i);
    }
}
