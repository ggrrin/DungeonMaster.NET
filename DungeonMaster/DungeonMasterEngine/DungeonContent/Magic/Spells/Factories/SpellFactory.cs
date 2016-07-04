using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.GameConsoleContent;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public abstract class SpellFactory<TSpell> : ISpellFactory<TSpell> where TSpell : class, ISpell
        {
            protected readonly Random rand = new Random();
            public IEnumerable<ISpellSymbol> CastingSequence { get; }
            public string Name { get; }
            public int Difficulty { get; }
            public int Duration { get; }
            public ISkillFactory Skill { get; }
            public int SkillLevel { get; }

            public SpellFactory(SpellFactoryInitializer initializer)
            {
                CastingSequence = initializer.CastingSequence.ToArray();
                Name = initializer.Name;
                Difficulty = initializer.Difficulty;
                Duration = initializer.Duration;
                Skill = initializer.Skill;
                SkillLevel = initializer.SkillLevel;
            }

            protected abstract TSpell ApplySpellEffect(ILiveEntity l1270PsChampion, IPowerSymbol l1268IPowerSymbolOrdinal, int a1267UiSkillLevel);

            public virtual TSpell CastSpell(IPowerSymbol powerSymbol, ILiveEntity caster)
            {
                return F412_xxxx_MENUS_GetChampionSpellCastResult(caster, powerSymbol);
            }


            TSpell F412_xxxx_MENUS_GetChampionSpellCastResult(ILiveEntity P795_i_ChampionIndex, IPowerSymbol powerSymbol)
            {
                int A1269_ui_RequiredSkillLevel;

                var L1270_ps_Champion = P795_i_ChampionIndex;

                IPowerSymbol L1268_i_PowerSymbolOrdinal = powerSymbol;
                int L1273_ui_Experience = rand.Next(8) + ((A1269_ui_RequiredSkillLevel = SkillLevel + L1268_i_PowerSymbolOrdinal.LevelOrdinal) << 4) + (((L1268_i_PowerSymbolOrdinal.LevelOrdinal - 1) * SkillLevel) << 3) + (A1269_ui_RequiredSkillLevel * A1269_ui_RequiredSkillLevel);

                var usedSkill = P795_i_ChampionIndex.GetSkill(Skill);
                int A1267_ui_SkillLevel = usedSkill.SkillLevel;
                if (A1267_ui_SkillLevel < A1269_ui_RequiredSkillLevel)
                {
                    int L1274_i_MissingSkillLevelCount = A1269_ui_RequiredSkillLevel - A1267_ui_SkillLevel;
                    while (L1274_i_MissingSkillLevelCount-- > 0)
                    {
                        if (rand.Next(128) > MathHelper.Min(L1270_ps_Champion.GetProperty(PropertyFactory<WisdomProperty>.Instance).Value + 15, 115))
                        {
                            usedSkill.AddExperience(L1273_ui_Experience >> (A1269_ui_RequiredSkillLevel - A1267_ui_SkillLevel));
                            F410_xxxx_MENUS_PrintSpellFailureMessage(L1270_ps_Champion, "C00_FAILURE_NEEDS_MORE_PRACTICE", Skill);
                            return null;
                        }
                    }
                }

                TSpell spell = ApplySpellEffect(L1270_ps_Champion, L1268_i_PowerSymbolOrdinal, A1267_ui_SkillLevel);
                usedSkill.AddExperience(L1273_ui_Experience);
                //TODO =>F330_szzz_CHAMPION_DisableAction(P795_i_ChampionIndex, Duration);
                return spell;
            }


            private void F410_xxxx_MENUS_PrintSpellFailureMessage(ILiveEntity l1270PsChampion, object c00FailureNeedsMorePractice, object p2)
            {
                GameConsole.Instance.Out.WriteLine(c00FailureNeedsMorePractice);
            }
        }
    }
