using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.ChampionSpecific;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders.ActuatorCreator
{
    public class ChampionCreator : IChampionInitializer
    {
        protected string[] descriptor;
        protected readonly LegacyMapBuilder builder;
        public virtual ChampionBonesFactory BonesFactory { get; }

        public ChampionCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
            if (builder != null)
                BonesFactory = (ChampionBonesFactory)builder.GetItemFactory(198);// 198 is identifier of bones => http://dmweb.free.fr/?q=node/886
        }

        private int GetValueOfDMHexEncoding(char c) => GetValueOfDMHexEncoding(c.ToString());

        private int GetValueOfDMHexEncoding(string encodedValue)
        {
            //each char transform to char code in DMEncoding table and use it as hex value
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

        private string FindChampionDescriptor(LegacyMapBuilder context, Point pos)
        {
            try
            {
                var descriptor = context.CurrentMap.GetTileData(pos).TextTags.Single(x => !x.Processed);
                descriptor.Processed = true;
                return descriptor.Text;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unambiguous champion text descriptor.", e);
            }
        }

        protected virtual void SetupChampionBody(IBody body, IEnumerable<IGrabableItem> items)
        {
            var hands = new IStorageType[]
            {
                ActionHandStorageType.Instance,
                ReadyHandStorageType.Instance
            };

            var handsEnumerator = hands.Cast<IStorageType>().GetEnumerator();

            foreach (var i in items)
            {
                var carryLocations = i.FactoryBase.CarryLocation;
                var storage = carryLocations.Except(hands.Concat(carryLocations.Where(x => !x.IsBodyPart))).FirstOrDefault();

                if (storage == null)
                {
                    storage = handsEnumerator.MoveNext() ? handsEnumerator.Current : BackPackStorageType.Instance;
                }

                if (storage == ConsumableStorageType.Instance)
                    storage = BackPackStorageType.Instance;

                if (!body.GetStorage(storage).AddItem(i))
                    throw new InvalidOperationException("Shouldn't be possible.");
            }
        }


        public virtual Champion GetChampion(Point pos, Texture2D face, IEnumerable<IGrabableItem> items)
        {
            descriptor = FindChampionDescriptor(builder, pos).Split('|');

            var res = new Champion(this, builder.ChampionToken, builder.CreatureToken.ToEnumerable())
            {
                Name = descriptor[0],
                Title = descriptor[1] + descriptor[2],
                IsMale = descriptor[3] == "M",
            };

            SetupChampionBody(res.Body, items);

            res.Renderer = builder.RendererSource.GetChampionRenderer(res, face);
            return res;
        }

        public virtual IEnumerable<IProperty> GetProperties(Champion champion)
        {
            LoadProperty load;
            Random rand = new Random();
            return new IProperty[]
            {
                load = new LoadProperty(champion),
                new HealthProperty(GetValueOfDMHexEncoding(descriptor[4].Substring(0, 4))),
                new StaminaProperty(GetValueOfDMHexEncoding(descriptor[4].Substring(4, 4))),
                new ManaProperty(GetValueOfDMHexEncoding(descriptor[4].Substring(8, 4))),
                new LuckProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(0, 2))),
                new StrengthProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(2, 2))),
                new ChampionDextrityProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(4, 2)), load),
                new WisdomProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(6, 2))),
                new VitalityProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(8, 2))),
                new AntiMagicProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(10, 2))),
                new AntiFireProperty(GetValueOfDMHexEncoding(descriptor[5].Substring(12, 2))),
                new ChampionDefenseProperty(champion, 0), 
                new SpellShieldDefenseProperty(),
                new FireShieldDefenseProperty(),
                new ShieldDefenseProperty(),
                new WaterProperty(1500 + rand.Next(256), 2048), 
                new FoodProperty(1500 + rand.Next(256), 2048), 
                new PoisonProperty() 
            };
        }

        public virtual IEnumerable<ISkill> GetSkills(Champion champion)
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