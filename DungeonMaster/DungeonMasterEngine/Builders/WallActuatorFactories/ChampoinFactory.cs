using DungeonMasterEngine.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class ChampoinFactory : ActuatorFactoryBase, IChampionInitializator
    {
        private string[] descriptor;
        public override bool? RequireItem { get; } = true;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {new ActuatorState
        {
            ActuatorType = 127
        }};

        private int GetValueOfDMHexEncoding(char c) => GetValueOfDMHexEncoding(c.ToString());

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

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            descriptor = FindChampionDescriptor(context).Split('|');

            var champion = new Champion(context.ChampionToken, context.CreatureToken.ToEnumerable(), this)
            {
                Name = descriptor[0],
                Title = descriptor[1] + descriptor[2],
                IsMale = descriptor[3] == "M",
            };

            var items = context.WallActuatorCreator.CurrentGrabableItems.Select(k => context.ItemCreator.CreateItem(k, currentTile));

            var res = new ChampoinActuator(context.GetWallPosition(matchedSequence[0].TilePosition, context.WallActuatorCreator.CurrentTile), champion);
            ((CubeGraphic)res.Graphics).Texture = context.WallTextures[matchedSequence[0].Decoration - 1];

            return res;
        }

        public IEnumerable<IProperty> GetProperties(Champion champion)
        {
            LoadProperty load;
            return new IProperty[]
            {
                load = new LoadProperty(champion),
                new HealthProperty(GetValueOfDMHexEncoding(descriptor[4].Substring(0, 4))),
                new StaminaProperty(GetValueOfDMHexEncoding(descriptor[4].Substring(4, 4))),
                new ManaProperty(GetValueOfDMHexEncoding(descriptor[4].Substring(8, 4))),
                new LuckProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(0, 2))),
                new StrengthProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(2, 2))),
                new DextrityProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(4, 2)), load),
                new WisdomProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(6, 2))),
                new VitalityProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(8, 2))),
                new AntiMagicProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(10, 2))),
                new AntiFireProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(12, 2))),
            };
        }

        public IEnumerable<ISkill> GetSkills(Champion champion)
        {
            SkillBase fighter, ninja, priest, wizard;
            return new ISkill[]
            {
                fighter = new FighterSkill(champion),
                ninja = new NinjaSkill(champion),
                priest = new PriestSkill(champion),
                wizard = new WizardSkill(champion),

                new SwingSkill(champion, fighter, GetValueOfDMHexEncoding(descriptor[6][0])),
                new ThrustSkill(champion, fighter,GetValueOfDMHexEncoding(descriptor[6][1])),
                new ClubSkill(champion, fighter,GetValueOfDMHexEncoding(descriptor[6][2])),
                new ParrySkill(champion, fighter,GetValueOfDMHexEncoding(descriptor[6][3])),

                new FightSkill(champion, ninja,GetValueOfDMHexEncoding(descriptor[6][4])),
                new StealSkill(champion, ninja,GetValueOfDMHexEncoding(descriptor[6][5])),
                new ThrowSkill(champion, ninja,GetValueOfDMHexEncoding(descriptor[6][6])),
                new ShootSkill(champion, ninja,GetValueOfDMHexEncoding(descriptor[6][7])),

                new IdentifySkill(champion, priest,GetValueOfDMHexEncoding(descriptor[6][8])),
                new HealSkill(champion, priest,GetValueOfDMHexEncoding(descriptor[6][9])),
                new InfluenceSkill(champion, priest,GetValueOfDMHexEncoding(descriptor[6][10])),
                new DeffendSkill(champion, priest,GetValueOfDMHexEncoding(descriptor[6][11])),

                new FireSkill(champion, wizard,GetValueOfDMHexEncoding(descriptor[6][12])),
                new AirSkill(champion, wizard,GetValueOfDMHexEncoding(descriptor[6][13])),
                new EarthSkill(champion, wizard,GetValueOfDMHexEncoding(descriptor[6][14])),
                new WaterSkill(champion, wizard,GetValueOfDMHexEncoding(descriptor[6][15])),
            };
        }
    }
}
