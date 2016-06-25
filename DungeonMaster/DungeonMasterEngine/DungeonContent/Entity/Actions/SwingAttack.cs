using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    class SwingAttack : MeleeAttack
    {
        //case C030_ACTION_BASH:
        //case C018_ACTION_HACK:
        //case C019_ACTION_BERZERK:
        //case C007_ACTION_KICK:
        //case C013_ACTION_SWING:
        //case C002_ACTION_CHOP:

        public SwingAttack(HumanActionFactoryBase factoryBase, ILiveEntity attackProvider) : base(factoryBase, attackProvider) { }


        protected override void PerformAttack(MapDirection direction, ref int delay)
        {
            var targetTile = attackProvider.Location.Tile.Neighbours.GetTile(direction) as IHasEntity;
            if (targetTile?.Entity != null)
            {
                //F064_aadz_SOUND_RequestPlay_COPYPROTECTIOND(C16_SOUND_COMBAT, G306_i_PartyMapX, G307_i_PartyMapY, C01_MODE_PLAY_IF_PRIORITIZED);
                delay = 6;
                F232_dzzz_GROUP_IsDoorDestroyedByAttack(targetTile.Entity, F312_xzzz_CHAMPION_GetStrength(ActionHandStorageType.Instance), false);
                //F064_aadz_SOUND_RequestPlay_COPYPROTECTIOND(C04_SOUND_WOODEN_THUD, G306_i_PartyMapX, G307_i_PartyMapY, C02_MODE_PLAY_ONE_TICK_LATER);
            }
            else
            {
                base.PerformAttack(direction, ref delay);
            }
        }
        protected void F232_dzzz_GROUP_IsDoorDestroyedByAttack(IEntity door, int P506_i_Attack, bool P507_B_MagicAttack)
        {
            var doorHealth = door.GetProperty(PropertyFactory<HealthProperty>.Instance);
            var doorDefense = door.GetProperty(PropertyFactory<DefenseProperty>.Instance);
            var doorAntiMagic = door.GetProperty(PropertyFactory<AntiMagicProperty>.Instance);

            if (P507_B_MagicAttack)
            {
                //TODO test !!
                doorHealth.Value -= MathHelper.Clamp(P506_i_Attack - doorAntiMagic.Value, 0, int.MaxValue);
            }
            else
            {
                doorHealth.Value -= MathHelper.Clamp(P506_i_Attack - doorDefense.Value, 0, int.MaxValue);
            }
        }
    }
}