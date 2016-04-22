using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using DungeonMasterParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;
using DungeonMasterParser.Tiles;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class ChampoinFactory : ActuatorFactoryBase
    {
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

        private string FindChampionDescriptor(LegacyMapBuilder context)
        {
            try
            {
                var descriptor = context.CurrentMap.GetTileData(context.WallActuatorCreator.CurrentTile.GridPosition).TextTags.Single(x => !x.Processed);
                descriptor.Processed = true;
                return descriptor.Text;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unambigious champoin text descriptor", e);
            }
        }

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {new ActuatorState
        {
            ActuatorType = 127
        }};

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            string[] descriptor = FindChampionDescriptor(context).Split('|');

            var champion = new Champoin
            {
                Name = descriptor[0],
                Title = descriptor[1] + descriptor[2],
                IsMale = descriptor[3] == "M",
                Health = GetValueOfDMHexEncoding(descriptor[4].Substring(0, 4)),
                Stamina = GetValueOfDMHexEncoding(descriptor[4].Substring(4, 4)),
                Mana = GetValueOfDMHexEncoding(descriptor[4].Substring(8, 4)),
                Luck = GetValueOfDMHexEncoding(descriptor[5].Substring(0, 2)),
                Strength = GetValueOfDMHexEncoding(descriptor[5].Substring(2, 2)),
                Dexterity = GetValueOfDMHexEncoding(descriptor[5].Substring(4, 2)),
                Wisdom = GetValueOfDMHexEncoding(descriptor[5].Substring(6, 2)),
                Vitality = GetValueOfDMHexEncoding(descriptor[5].Substring(8, 2)),
                AntiMagic = GetValueOfDMHexEncoding(descriptor[5].Substring(10, 2)),
                AntiFire = GetValueOfDMHexEncoding(descriptor[5].Substring(12, 2)),
                Fighter = GetValueOfDMHexEncoding(descriptor[6].Substring(0, 4)),
                Ninja = GetValueOfDMHexEncoding(descriptor[6].Substring(4, 4)),
                Priest = GetValueOfDMHexEncoding(descriptor[6].Substring(8, 4)),
                Wizard = GetValueOfDMHexEncoding(descriptor[6].Substring(12, 4))
            };

            var items = context.WallActuatorCreator.CurrentGrabableItems.Select(k => context.ItemCreator.CreateItem(k, currentTile ));
            champion.Inventory.AddRange(items);


            var res = new ChampoinActuator(context.GetWallPosition(matchedSequence[0].TilePosition, context.WallActuatorCreator.CurrentTile), champion);
            ((CubeGraphic)res.Graphics).Texture = context.WallTextures[matchedSequence[0].Decoration - 1];

            return res;
        }
    }

}
