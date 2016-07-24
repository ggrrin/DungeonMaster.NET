using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public class ViPotion : DrinkablePotion
    {

        public override bool ApplyEffect(ILiveEntity entity)
        {
            if (Used)
                return false;


            int L1086_ui_Counter = ((511 - Power) / (32 + (Power + 1) / 8)) >> 1;
            int A1088_ui_HealWoundIterationCount = MathHelper.Max(1, Power / 42);

            var health = entity.GetProperty(PropertyFactory<HealthProperty>.Instance);
            health.Value += health.MaxValue / L1086_ui_Counter;

            var bodyParts = entity.Body.BodyParts.ToArray();

            if (bodyParts.Any(x => x.IsWounded))
            { /* If the champion is wounded */
                L1086_ui_Counter = 10;
                do
                {
                    bool atLeasOneHealed = false;
                    int A1085_ui_Counter;
                    for (A1085_ui_Counter = 0; A1085_ui_Counter < A1088_ui_HealWoundIterationCount; A1085_ui_Counter++)
                    {
                        //flowing code should be similar to => // L1083_ps_Champion->Wounds &= M06_RANDOM(65536);
                        var maxHealCount = rand.Next(bodyParts.Length) + 1;
                        var healIndices = Enumerable.Range(0, maxHealCount)
                            .Select(x => rand.Next(bodyParts.Length))
                            .ToArray();

                        atLeasOneHealed = false;
                        foreach (int index in healIndices)
                        {
                            var part = bodyParts[index];
                            atLeasOneHealed |= part.IsWounded;
                            part.IsWounded = false;
                        }
                    }
                    A1088_ui_HealWoundIterationCount = 1;

                    if (atLeasOneHealed)
                        break;
                } while (--L1086_ui_Counter >= 0); /* Loop until at least one wound is healed or there are no more heal iterations */
            }
            return Used = true;
        }
    }
}