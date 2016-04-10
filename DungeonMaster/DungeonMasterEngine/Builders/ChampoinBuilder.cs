using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using DungeonMasterParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterParser.Items;
using DungeonMasterParser.Tiles;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    class ChampoinBuilder
    {
        private LegacyMapBuilder builder;
        private WallTile champoinWall;
        private Tile currentTile;
        public ChampoinBuilder(LegacyMapBuilder builder, WallTile champoinWall, Tile currentTile)
        {
            this.builder = builder;
            this.champoinWall = champoinWall;
            this.currentTile = currentTile;
        }

        public Champoin GetChampoin(ActuatorItem i)
        {
            string[] descriptor = FindChampionDescriptor()?.Split(new char[] { '|' });

            var champoin = new Champoin();
            if (descriptor != null)
            {
                champoin.Name = descriptor[0];
                champoin.Title = descriptor[1] + descriptor[2];
                champoin.IsMale = descriptor[3] == "M";

                champoin.Health = GetValueOfDMHexEncoding(descriptor[4].Substring(0, 4));
                champoin.Stamina = GetValueOfDMHexEncoding(descriptor[4].Substring(4, 4));
                champoin.Mana = GetValueOfDMHexEncoding(descriptor[4].Substring(8, 4));

                champoin.Luck = GetValueOfDMHexEncoding(descriptor[5].Substring(0, 2));
                champoin.Strength = GetValueOfDMHexEncoding(descriptor[5].Substring(2, 2));
                champoin.Dexterity = GetValueOfDMHexEncoding(descriptor[5].Substring(4, 2));
                champoin.Wisdom = GetValueOfDMHexEncoding(descriptor[5].Substring(6, 2));
                champoin.Vitality = GetValueOfDMHexEncoding(descriptor[5].Substring(8, 2));
                champoin.AntiMagic = GetValueOfDMHexEncoding(descriptor[5].Substring(10, 2));
                champoin.AntiFire = GetValueOfDMHexEncoding(descriptor[5].Substring(12, 2));

                champoin.Fighter = GetValueOfDMHexEncoding(descriptor[6].Substring(0, 4));
                champoin.Ninja = GetValueOfDMHexEncoding(descriptor[6].Substring(4, 4));
                champoin.Priest = GetValueOfDMHexEncoding(descriptor[6].Substring(8, 4));
                champoin.Wizard = GetValueOfDMHexEncoding(descriptor[6].Substring(12, 4));
            }

            champoin.Inventory.AddRange(from k in champoinWall.Items
                                        where k is GrabableItem
                                        select (DungeonContent.Items.GrabableItem)new LegacyItemCreator(builder).GetItem(k));
            return champoin;
        }

        private int GetValueOfDMHexEncoding(string encodedValue)
        {
            //each char transform to char code in DMEncoding table and use it as hexa value
            var sb = new StringBuilder();
            foreach (int i in encodedValue)
            {
                int code = i - 'A';

                if (code < 10)
                    sb.Append(code);
                else
                    sb.Append((char)(code - 10 + 'A'));
            }
            return Convert.ToInt16(sb.ToString(), 16);
        }

        private string FindChampionDescriptor()
        {
            //assuming champoin actuator always on wall !!!
            var texts = (from i in builder.CurrentMap[currentTile.GridPosition.X, currentTile.GridPosition.Y].Items
                         where i.GetType() == typeof(TextDataItem)
                         select i as TextDataItem).ToList();

            if (texts.Count == 1)
            {
                texts[0].Processed = true;
                return texts[0].Text;
            }
            else
                return null;
            //throw new InvalidOperationException("Unambigious champoin text descriptor");
        }
    }

}
