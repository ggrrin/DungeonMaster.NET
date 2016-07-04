using System;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    public class ChampionDextrityProperty : Property {
        private static readonly Random rand = new Random();
        private readonly LoadProperty load;

        public override int MaxValue => F311_wzzz_CHAMPION_GetDexterity();

        public ChampionDextrityProperty(int value, LoadProperty load)
        {
            this.load = load;
            BaseValue = this.value = value;
        }

        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<DextrityProperty>.Instance; 

        int F311_wzzz_CHAMPION_GetDexterity()
        {
            int L0934_i_Dexterity = rand.Next(8) + BaseValue + AdditionalValues.Sum(x => x.Value);
            L0934_i_Dexterity -= (/*(long)*/(L0934_i_Dexterity >> 1) * /*(long)*/load.Value/*P649_ps_Champion->Load*/) / load.MaxValue;/*P649_ps_Champion*/
            //TODO party sleeping
            //if (G300_B_PartyIsSleeping) {
            //        L0934_i_Dexterity >>= 1;
            //}
            return MathHelper.Clamp(L0934_i_Dexterity >> 1, 1 + rand.Next(8), 100 - rand.Next(8));// F026_a003_MAIN_GetBoundedValue(1 + M03_RANDOM(8), L0934_i_Dexterity >> 1, 100 - M03_RANDOM(8));
        }
    }
}