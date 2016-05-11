using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    internal class CreatureCreator
    {
        private readonly LegacyMapBuilder builder;

        public CreatureCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        public IEnumerable<Creature> AddCreature(CreatureItem creature, Tile tile)
        {
            var creatureDescriptor = builder.Data.CreatureDescriptors[(int)creature.Type];
            var duration = creatureDescriptor.MovementDuration * 1000 / 6;
            duration = MathHelper.Clamp(duration, 500, 1200);
            var layout = GetGroupLayout(creature);
            var res = new List<Creature>();

            foreach (var tuple in creature.GetCreaturesInfo().Take(creature.CreaturesCount + 1))
            {
                var space = layout.AllSpaces.First(s => tuple.Item1.ToDirections().All(s.Sides.Contains));
                res.Add(new Creature(layout, new FourthSpaceRouteElement(space, tile), builder.CreatureToken,
                    builder.ChampionToken.ToEnumerable(), duration, creatureDescriptor.DetectionRange, creatureDescriptor.SightRange));
            }

            return res;
        }

        public IGroupLayout GetGroupLayout(CreatureItem creature)
        {
            return Small4GroupLayout.Instance;
            switch (creature.Type)
            {
                //1
                case CreatureType.GiantScorpion_Scorpion:
                case CreatureType.StoneGolem:
                case CreatureType.BlackFlame:
                case CreatureType.Couatl:
                case CreatureType.WaterElemental:
                case CreatureType.Oitu:
                case CreatureType.LordChaos:
                case CreatureType.RedDragon_Dragon:
                case CreatureType.LordOrder_:
                case CreatureType.GreyLord:
                    return new Large1GroupLayout();

                //4
                case CreatureType.SwampSlime_SlimeDevil:
                case CreatureType.Giggler:
                case CreatureType.WizardEye_FlyingEye:
                case CreatureType.Screamer:
                case CreatureType.Rockpile_RockPile:
                case CreatureType.Ghost_Rive:
                case CreatureType.Mummy:
                case CreatureType.Skeleton:
                case CreatureType.Vexirk:
                case CreatureType.Trolin_AntMan:
                case CreatureType.GiantWasp_Muncher:
                case CreatureType.AnimatedArmour_DethKnight:
                case CreatureType.Materializer_Zytaz:
                case CreatureType.Demon_Demon:
                    return Small4GroupLayout.Instance;

                //2
                case CreatureType.MagentaWorm_Worm:
                case CreatureType.PainRat_Hellhound:
                case CreatureType.Ruster:
                    return new Medium2GroupLayout();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}