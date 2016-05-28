using DungeonMasterEngine.DungeonContent.EntitySupport.Skills;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Attacks
{
    class HumanAttackFactory
    {
        public string Name { get; }
        public int ExperienceGain { get; }
        public int DefenseModifer { get; }
        public int HitProbability { get; }
        public int Damage { get; }
        public int Fatigue { get; }
        public ISkillFactory SkillIndex { get; }
        public int Stamina { get; }

        public int MapDifficulty { get; set; }

        public IAttack CreateAttack(IEntity entity) => new HumanAttack(this, entity);
    }
}