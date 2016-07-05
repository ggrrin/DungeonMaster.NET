using DungeonMasterEngine.DungeonContent.Actions;

namespace DungeonMasterEngine.DungeonContent.Projectiles
{
    public struct AttackInfo
    {
        //G367_i_ProjectileAttackType 
        public CreatureAttackType Type;

        //G366_i_ProjectilePoisonAttack 
        public int PoisonAttack;
        public int Attack;
    }
}