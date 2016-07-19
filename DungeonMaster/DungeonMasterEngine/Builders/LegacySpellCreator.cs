using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.Magic.Spells;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Projectiles.Impacts;
using DungeonMasterEngine.Interfaces;
using HtmlAgilityPack;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders
{
    public class LegacySpellCreator
    {
        public IFactories Factories { get; }
        protected readonly Dictionary<string, ISpellSymbol> symbolTokens;

        public LegacySpellCreator(IFactories factories)
        {
            Factories = factories;
            symbolTokens = Factories.SpellSymbols.ToDictionary(t => t.Name.ToLowerInvariant());
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
            return Factories.Skills[identifer];
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
                    return new ExplosionProjectileSpellFactory<FireballExplosionImpact>
                        (initializer, Factories.RenderersSource, Factories.RenderersSource.Content.Load<Texture2D>("Textures/Explosions/" + token));
                case "WeakenNonmaterialBeings":
                    return new ExplosionProjectileSpellFactory<HarmNonMaterialExplosionImpact>
                        (initializer, Factories.RenderersSource, Factories.RenderersSource.Content.Load<Texture2D>("Textures/Explosions/" + token));
                case "PoisonBolt":
                    return new ExplosionProjectileSpellFactory<PoisonBoltExplosionImpact>
                        (initializer, Factories.RenderersSource, Factories.RenderersSource.Content.Load<Texture2D>("Textures/Explosions/" + token));
                case "PoisonCloud":
                    return new ExplosionProjectileSpellFactory<PoisonCloudExplosionImpact>
                        (initializer, Factories.RenderersSource, Factories.RenderersSource.Content.Load<Texture2D>("Textures/Explosions/" + token));
                case "LightningBolt":
                    return new ExplosionProjectileSpellFactory<LightingBoltExplosionImpact>
                        (initializer, Factories.RenderersSource, Factories.RenderersSource.Content.Load<Texture2D>("Textures/Explosions/" + token));
                case "PoisonPotion":
                    return new PotionSpellFactory(initializer, Factories.PotionFactories[3], Factories.PotionFactories);
                case "DexterityPotion":
                    return new PotionSpellFactory(initializer, Factories.PotionFactories[6], Factories.PotionFactories);
                case "StrengthPotion":
                    return new PotionSpellFactory(initializer, Factories.PotionFactories[7], Factories.PotionFactories);
                case "WisdomPotion":
                    return new PotionSpellFactory(initializer, Factories.PotionFactories[8], Factories.PotionFactories);
                case "VitalityPotion":
                    return new PotionSpellFactory(initializer, Factories.PotionFactories[9], Factories.PotionFactories);
                case "CurePoisonPotion":
                    return new PotionSpellFactory(initializer, Factories.PotionFactories[10], Factories.PotionFactories);
                case "StaminaPotion":
                    return new PotionSpellFactory(initializer, Factories.PotionFactories[11], Factories.PotionFactories);
                case "ShieldPotion":
                    return new PotionSpellFactory(initializer, Factories.PotionFactories[12], Factories.PotionFactories);
                case "ManaPotion":
                    return new PotionSpellFactory(initializer, Factories.PotionFactories[13], Factories.PotionFactories);
                case "HealthPotion":
                    return new PotionSpellFactory(initializer, Factories.PotionFactories[14], Factories.PotionFactories);
                case "FireShield":
                case "Shield":
                case "Darkness":
                case "Torch":
                    return new MagicTorchSpellFactory(initializer, Factories.LightPowerToLightAmount);
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
}