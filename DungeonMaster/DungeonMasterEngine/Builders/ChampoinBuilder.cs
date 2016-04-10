using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using DungeonMasterParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;
using DungeonMasterParser.Tiles;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    class ChampoinBuilder
    {
        private readonly LegacyMapBuilder builder;
        private readonly WallTileData champoinWall;
        private readonly Tile currentTile;

        public ChampoinBuilder(LegacyMapBuilder builder, WallTileData champoinWall, Tile currentTile)
        {
            this.builder = builder;
            this.champoinWall = champoinWall;
            this.currentTile = currentTile;
        }

        public Champoin GetChampoin(ActuatorItemData i)
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

            var champoinItems = champoinWall.GrabableItems.Select(k => (DungeonContent.Items.GrabableItem)builder.LegacyItemCreator.CreateItem(k));
            champoin.Inventory.AddRange(champoinItems);
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
            try
            {
                var descriptor = builder.CurrentMap.GetTileData(currentTile.GridPosition).TextTags.Single(x => !x.Processed);
                descriptor.Processed = true;
                return descriptor.Text;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unambigious champoin text descriptor", e);
            }
        }
    }

}
