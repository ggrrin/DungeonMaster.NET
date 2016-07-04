using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Actions;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Spells;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using HtmlAgilityPack;

namespace DungeonMasterEngine.Builders
{
    public class LegacySpellCreator
    {
        protected readonly Dictionary<string, ISpellSymbol> symbolTokens;
        public IReadOnlyList<ISpellSymbol> SpellSymbol { get; }
        public IReadOnlyList<ISkillFactory> Skills { get; }
        public IReadOnlyList<PotionFactory> PotionFactories { get; }

        public LegacySpellCreator(IReadOnlyList<ISpellSymbol> spellSymbol, IReadOnlyList<ISkillFactory> skills, IReadOnlyList<PotionFactory> potionFactories)
        {
            SpellSymbol = spellSymbol;
            symbolTokens = SpellSymbol.ToDictionary(t => t.Name.ToLowerInvariant());
            Skills = skills;
            PotionFactories = potionFactories;
        }

        protected virtual HtmlDocument GetDocument()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/Spells.html"));
            return documet;
        }

        public virtual IReadOnlyList<ISpellFactory<ISpell>> InitSpellFactories()
        {
            var documet = GetDocument();

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)
                .Select(tr =>
                {
                    var row = tr.Elements("td")
                        .Select(td => td.InnerText.Trim())
                        .ToArray();

                    var initializer = new SpellFactoryInitializer
                    {
                        Skill = ParseSkill(row[5]),
                        Name = row[2],
                        SkillLevel = int.Parse(row[6]),
                        Difficulty = int.Parse(row[3]),
                        Duration = int.Parse(row[4]),
                        CastingSequence = ParseCastingSequence(row[0])
                    };

                    var res = GetSpell(row[2], initializer);
                    if (res == null)
                        throw new InvalidOperationException("Unrecognized spell.");
                    return res;
                })
                .ToArray();
        }

        private ISkillFactory ParseSkill(string s)
        {
            var identifer = int.Parse(new string(s.TakeWhile(x => x >= '0' && x <= '9').ToArray()));
            return Skills[identifer];
        }

        private IEnumerable<ISpellSymbol> ParseCastingSequence(string tokensString)
        {
            var tokens = tokensString
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLowerInvariant());

            return tokens.Select(x => symbolTokens[x]);
        }

        protected virtual ISpellFactory<ISpell> GetSpell(string token, SpellFactoryInitializer initializer)
        {
            switch (token)
            {
                case "Fireball":
                    return new ExplosionProjectileSpellFactory<FireballExplosionImpact>(initializer);
                case "WeakenNonmaterialBeings":
                    return new ExplosionProjectileSpellFactory<HarmNonMaterialExplosionImpact>(initializer);
                case "PoisonBolt":
                    return new ExplosionProjectileSpellFactory<PoisonBoltExplosionImpact>(initializer);
                case "PoisonCloud":
                    return new ExplosionProjectileSpellFactory<PoisonCloudExplosionImpact>(initializer);
                case "LightningBolt":
                    return new ExplosionProjectileSpellFactory<LightingBoltExplosionImpact>(initializer);
                case "PoisonPotion":
                    return new PotionSpellFactory(initializer, PotionFactories[3]);
                case "DexterityPotion":
                    return new PotionSpellFactory(initializer, PotionFactories[6]);
                case "StrengthPotion":
                    return new PotionSpellFactory(initializer, PotionFactories[7]);
                case "WisdomPotion":
                    return new PotionSpellFactory(initializer, PotionFactories[8]);
                case "VitalityPotion":
                    return new PotionSpellFactory(initializer, PotionFactories[9]);
                case "CurePoisonPotion":
                    return new PotionSpellFactory(initializer, PotionFactories[10]);
                case "StaminaPotion":
                    return new PotionSpellFactory(initializer, PotionFactories[11]);
                case "ShieldPotion":
                    return new PotionSpellFactory(initializer, PotionFactories[12]);
                case "ManaPotion":
                    return new PotionSpellFactory(initializer, PotionFactories[13]);
                case "HealthPotion":
                    return new PotionSpellFactory(initializer, PotionFactories[14]);
                case "FireShield":
                case "Shield":
                case "Darkness":
                case "Torch":
                case "Light":
                case "OpenDoor":
                case "MagicFootprints":
                case "SeeThroughWalls":
                case "Invisibility":
                case "ZokathraSpell":
                default:
                    return new SpellFactoryMocap(initializer);
            }
        }
    }

    public class SpellFactoryMocap : SpellFactory<ISpell>
    {
        protected override ISpell ApplySpellEffect(ILiveEntity l1270PsChampion, IPowerSymbol l1268IPowerSymbolOrdinal, int a1267UiSkillLevel)
        {
            throw new NotImplementedException();
        }

        public SpellFactoryMocap(SpellFactoryInitializer initializer) : base(initializer) {}
    }
}