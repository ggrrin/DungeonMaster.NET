using DungeonMasterEngine.Builders;
using DungeonMasterEngine.Builders.CreatureCreator;
using DungeonMasterEngine.DungeonContent.Actions;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public class CreatureFactory
    {
        public Texture2D Texture { get; }
        public string Name { get; }
        public IGroupLayout Layout { get; }


        public bool AttackAny { get; }
        public int Experience { get; }

        //unsigned char CreatureAspectIndex;
        //unsigned char AttackSoundOrdinal;
        //unsigned int Attributes; /* Bits 15-14 Unreferenced */
        //unsigned int GraphicInfo; /* Bits 11 and 6 Unreferenced */

        //unsigned char MovementTicks; /* Value 255 means the creature cannot move */
        public int MoveDuration { get; }

        //unsigned char AttackTicks; /* Minimum ticks between attacks */
        public int AttackTicks { get; }

        //unsigned char Defense;
        public int Defense { get; }

        //unsigned char BaseHealth;
        public int BaseHealth { get; }

        //unsigned char Attack;
        public int Attack { get; }

        //unsigned char PoisonAttack;
        public int PoisonAttack { get; }

        //unsigned char Dexterity; /* 1 byte of padding inserted by compiler */
        public int Dexterity { get; }

        //unsigned int Ranges; /* Bits 7-4 Unreferenced */
        public int DetectRange { get; }
        public int SightRange { get; }

        //unsigned int Properties;

        //unsigned int Resistances; /* Bits 15-12 and 3-0 Unreferenced */
        public int PoisonResistance { get; }
        public int FireResistance { get; }



        //unsigned int AnimationTicks; /* Bits 15-12 Unreferenced */

        //unsigned int WoundProbabilities; /* Contains 4 probabilities to wound a champion's Head (Bits 15-12), Legs (Bits 11-8), Torso (Bits 7-4) and Feet (Bits 3-0) */
        public int WoundFeet { get; }
        public int WoundLegs { get; }
        public int WoundTorso { get; }
        public int WoundHead { get; }

        //unsigned char AttackType; /* 1 byte of padding inserted by compiler *
        public CreatureAttackType AttackType { get; }


        public CreatureFactory(IGroupLayout layout, string name, int moveDuration, int detectRange, int sightRange, int experience,
            Texture2D texture, int woundFeet, int woundLegs, int woundTorso, int woundHead, int attack, int poisonAttack, CreatureAttackType attackType, bool attackAny, int attackTicks, int defense, int baseHealth, int dexterity, int poisonResistance, int fireResistance)
        {
            Layout = layout;
            MoveDuration = moveDuration;
            Texture = texture;
            WoundFeet = woundFeet;
            WoundLegs = woundLegs;
            WoundTorso = woundTorso;
            WoundHead = woundHead;
            Attack = attack;
            PoisonAttack = poisonAttack;
            AttackType = attackType;
            AttackAny = attackAny;
            AttackTicks = attackTicks;
            Defense = defense;
            BaseHealth = baseHealth;
            Dexterity = dexterity;
            PoisonResistance = poisonResistance;
            FireResistance = fireResistance;
            DetectRange = detectRange;
            SightRange = sightRange;
            Name = name;
            Experience = experience;
            PoisonAttack = poisonAttack;
        }


        public Creature Create<TCreatureInitializer>(TCreatureInitializer initiator) where TCreatureInitializer : ICreatureInitializer
        {
            return new Creature(initiator, this);
        }


    }
}