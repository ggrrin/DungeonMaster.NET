using DungeonMasterEngine.Builders;
using DungeonMasterEngine.Builders.CreatureCreator;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public class CreatureFactory
    {
        public Texture2D Texture { get; }
        public string Name { get; }
        public IGroupLayout Layout { get; }
        public int MoveDuration { get; }
        public int DetectRange { get; }
        public int SightRange { get; }
        public int Experience { get; }

        public CreatureFactory(IGroupLayout layout, string name, int moveDuration, int detectRange, int sightRange, int experience, Texture2D texture)
        {
            Layout = layout;
            MoveDuration = moveDuration;
            Texture = texture;
            DetectRange = detectRange;
            SightRange = sightRange;
            Name = name;
            Experience = experience;
        }


        public Creature Create<TCreatureInitializer>(TCreatureInitializer initiator) where TCreatureInitializer : ICreatureInitializer
        {
            return new Creature(initiator, this);
        }
    }
}