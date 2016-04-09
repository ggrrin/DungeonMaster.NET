using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.Player
{
    public class Champoin
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsMale { get; set; }
        public int Health { get; set; }
        public int Stamina { get; set; }
        public int Mana { get; set; }
        public int Luck { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Wisdom { get; set; }
        public int Vitality { get; set; }
        public int AntiMagic { get; set; }
        public int AntiFire { get; set; }
        public int Fighter { get; set; }
        public int Ninja { get; set; }
        public int Priest { get; set; }
        public int Wizard { get; set; }

        public List<GrabableItem> Inventory { get; } = new List<GrabableItem>();

        public override string ToString()
        {
            return Name;
        }
    }
}