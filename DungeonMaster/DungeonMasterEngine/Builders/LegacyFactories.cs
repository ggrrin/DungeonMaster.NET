using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Actions;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Spells;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser;
using DungeonMasterParser.Descriptors;
using DungeonMasterParser.Enums;
using HtmlAgilityPack;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders
{
    public class LegacyFactories : IFactories
    {
        public IRenderersSource RenderersSource { get; }
        public DungeonData Data { get; }

        public IReadOnlyList<ISpellFactory<ISpell>> SpellFactories { get; }

        public IReadOnlyList<ISpellSymbol> SpellSymbols { get; }
        public IReadOnlyList<IPowerSymbol> PowerSymbol { get; }

        public IReadOnlyList<ISkillFactory> Skills { get; }
        public IReadOnlyList<HumanActionFactoryBase> FightActions { get; }
        public IReadOnlyList<IReadOnlyList<IActionFactory>> ActionCombos { get; }
        public IReadOnlyList<WeaponItemFactory> WeaponFactories { get; }
        public IReadOnlyList<ClothItemFactory> ClothFactories { get; }
        public IReadOnlyList<ContainerItemFactory> ContainerFactories { get; }
        public IReadOnlyList<ScrollItemFactory> ScrollFactories { get; }
        public IReadOnlyList<PotionFactory> PotionFactories { get; }
        public IReadOnlyList<MiscItemFactory> MiscFactories { get; }
        public IReadOnlyList<CreatureFactory> CreatureFactories { get; }


        public LegacyFactories(DungeonData data, IRenderersSource renderersSource)
        {
            Data = data;
            RenderersSource = renderersSource;

            Skills = InitSkills();
            FightActions = GetFightActionsFactories();
            ActionCombos = GetComboActions();
            WeaponFactories = InitWeaponFactories();
            ClothFactories = InitClothFactories();
            ContainerFactories = InitContainerFactories();
            ScrollFactories = InitScrollFactories();
            MiscFactories = InitMiscFactories();
            PotionFactories = new PotionFactoriesBuilder(this).InitPotionFactories();
            CreatureFactories = InitCreatureFactories();
            PowerSymbol = InitPowerSymbols();
            SpellSymbols = InitSpellSymbols();
            SpellFactories = new LegacySpellCreator(SpellSymbols, Skills, PotionFactories).InitSpellFactories();
        }

        protected virtual IReadOnlyList<ISkillFactory> InitSkills()
        {
            return new ISkillFactory[]
            {
                SkillFactory<FighterSkill>.Instance,
                SkillFactory<NinjaSkill>.Instance,
                SkillFactory<PriestSkill>.Instance,
                SkillFactory<WizardSkill>.Instance,
                SkillFactory<SwingSkill>.Instance,
                SkillFactory<ThrustSkill>.Instance,
                SkillFactory<ClubSkill>.Instance,
                SkillFactory<ParrySkill>.Instance,
                SkillFactory<StealSkill>.Instance,
                SkillFactory<FightSkill>.Instance,
                SkillFactory<ThrowSkill>.Instance,
                SkillFactory<ShootSkill>.Instance,
                SkillFactory<IdentifySkill>.Instance,
                SkillFactory<HealSkill>.Instance,
                SkillFactory<InfluenceSkill>.Instance,
                SkillFactory<DeffendSkill>.Instance,
                SkillFactory<FireSkill>.Instance,
                SkillFactory<AirSkill>.Instance,
                SkillFactory<EarthSkill>.Instance,
                SkillFactory<WaterSkill>.Instance,
            };
        }

        protected virtual IReadOnlyList<ISpellSymbol> InitSpellSymbols()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/SpellSymbols.html"));

            return documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Select(tr =>
                {
                    var levels = tr.Elements("td")
                        .Skip(1)
                        .Select(td => int.Parse(td.InnerText))
                        .ToArray();

                    var res = new SpellSymbol(tr.Element("td").InnerText.Trim(), levels);
                    return res;
                })
                .ToArray();
        }

        protected virtual IReadOnlyList<IPowerSymbol> InitPowerSymbols()
        {
            var documet = new HtmlDocument();
            documet.LoadHtml(File.ReadAllText("Data/PowerSymbols.html"));

            var table = documet.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Select(tr =>
                {
                    return tr.Elements("td")
                        .Skip(1)
                        .Select(td => td.InnerText)
                        .ToArray();
                })
                .ToArray();

            return Enumerable.Range(0, table.First().Length)
                .Select(i => new PowerSymbol(table[2][i].Trim(), int.Parse(table[1][i]), int.Parse(table[0][i])))
                .ToArray();
        }




        private IGroupLayout GetGroupLayout(int layout)
        {
            switch (layout)
            {
                case 0:
                    return Small4GroupLayout.Instance;
                case 1:
                    return Medium2GroupLayout.Instance;
                case 2:
                    return FullTileLayout.Instance;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public IEnumerable<IStorageType> GetStorageTypes(CarrryLocations locations)
        {
            Array values = Enum.GetValues(typeof(CarrryLocations));

            var res = values.Cast<CarrryLocations>()
                .Where(c => (c & locations) == c && c != CarrryLocations.HandsAndBackpack
                && c != CarrryLocations.Hands && c != CarrryLocations.None)
                .Select<CarrryLocations, IStorageType>(c =>
                 {
                     switch (c)
                     {
                         case CarrryLocations.Consumable:
                             return ConsumableStorageType.Instance;
                         case CarrryLocations.Head:
                             return HeadStorageType.Instance;
                         case CarrryLocations.Neck:
                             return NeckStorageType.Instance;
                         case CarrryLocations.Torso:
                             return TorsoStorageType.Instance;
                         case CarrryLocations.Legs:
                             return LegsStorageType.Instance;
                         case CarrryLocations.Feet:
                             return FeetsStorageType.Instance;
                         case CarrryLocations.Quiver1:
                             return BigQuiverStorageType.Instance;
                         case CarrryLocations.Quiver2:
                             return SmallQuiverStorageType.Instance;
                         case CarrryLocations.Pouch:
                             return PouchStorageType.Instance;
                         case CarrryLocations.Chest:
                             return ChestStorageType.Instance;
                         default:
                             throw new InvalidOperationException();
                     }
                 })
                .Concat(new IStorageType[] { ActionHandStorageType.Instance, ReadyHandStorageType.Instance, BackPackStorageType.Instance })
                .ToArray();
            return res;
        }
        protected virtual CreatureFactory[] InitCreatureFactories()
        {
            return Data.CreatureDescriptors
                            .Select(wd => new CreatureFactory(GetGroupLayout(wd.Size), wd.Name, MathHelper.Clamp(wd.MovementDuration * 1000 / 6, 500, 1200),
                            wd.DetectionRange, wd.SightRange, wd.ExperienceClass,
                            ResourceProvider.Instance.Content.Load<Texture2D>($"Textures/Creatures/DM-Creature-SuperNES-{wd.Name}"),
                            wd.WoundFeet, wd.WoundLegs, wd.WoundTorso, wd.WoundLegs,
                            wd.AttackPower,
                            wd.Poison,
                            (CreatureAttackType)wd.DamageType,
                            wd.AttackAnyChampion == 1,
                            wd.AttackDuration * 1000 / 6,
                            wd.Armor, //Armor in documentation is probably defense in source codes
                            wd.BaseHealth,
                            wd.Defense,
                            wd.PoisonResistance,
                            wd.FireResistance)) //Defense in documentation is probably dexterity in source code
                            .ToArray();
        }

        protected virtual ContainerItemFactory[] InitContainerFactories()
        {
            return Data.ContainerDescriptors
           .Select(wd =>
           {
               var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Container, wd.Identifer);
               return new ContainerItemFactory(
                   wd.Name,
                   wd.Weight,
                   ActionCombos[itemDescriptor.AttackCombo],
                   GetStorageTypes(itemDescriptor.CarryLocation),
                   RenderersSource.GetItemRenderer(ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath))
                   );
           })
           .ToArray();
        }

        protected virtual ScrollItemFactory[] InitScrollFactories()
        {
            return Data.ScrollDescriptors
           .Select(wd =>
           {
               var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Scroll, wd.Identifer);
               return new ScrollItemFactory(
                   wd.Name,
                   wd.Weight,
                   ActionCombos[itemDescriptor.AttackCombo],
                   GetStorageTypes(itemDescriptor.CarryLocation),
                   RenderersSource.GetItemRenderer(ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath)));
           })
           .ToArray();
        }



        protected virtual MiscItemFactory[] InitMiscFactories()
        {
            return Data.MiscDescriptors
            .Select(wd =>
            {
                var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Miscellenaous, wd.Identifer);
                switch (wd.Name)
                {
                    case "Name Bones":
                        return new ChampionBonesFactory(
                            wd.Name,
                            wd.Weight,
                            ActionCombos[itemDescriptor.AttackCombo],
                            GetStorageTypes(itemDescriptor.CarryLocation),
                            RenderersSource.GetItemRenderer(ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath)));

                    default:
                        return new MiscItemFactory(
                            wd.Name,
                            wd.Weight,
                            ActionCombos[itemDescriptor.AttackCombo],
                            GetStorageTypes(itemDescriptor.CarryLocation),
                            RenderersSource.GetItemRenderer(ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath)));
                }
            })
            .ToArray();
        }

        protected virtual ClothItemFactory[] InitClothFactories()
        {
            return Data.ArmorDescriptors
           .Select(wd =>
           {
               var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Clothe, wd.Identifer);
               return new ClothItemFactory(
                   wd.Name,
                   wd.Weight,
                   ActionCombos[itemDescriptor.AttackCombo],
                   GetStorageTypes(itemDescriptor.CarryLocation),
                   wd.ArmorStrength,
                   wd.SharpResistance,
                   false, //TODO is shield ? 
                   RenderersSource.GetItemRenderer(ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath)));
           })
           .ToArray(); ;
        }

        protected virtual WeaponItemFactory[] InitWeaponFactories()
        {
            return Data.WeaponDescriptors
                .Select(wd =>
                {
                    var itemDescriptor = Data.GetItemDescriptor(ObjectCategory.Weapon, wd.Identifer);
                    return new WeaponItemFactory(
                        wd.Name,
                        wd.Weight,
                        ActionCombos[itemDescriptor.AttackCombo],
                        GetStorageTypes(itemDescriptor.CarryLocation),
                        wd.DeltaEnergy,
                        (WeaponClass)wd.Class,
                        wd.KineticEnergy,
                        wd.ShootDamage,
                        wd.Strength,
                        RenderersSource.GetItemRenderer(ResourceProvider.Instance.Content.Load<Texture2D>(wd.TexturePath)));
                })
                .ToArray();
        }

        protected virtual IReadOnlyList<HumanActionFactoryBase> GetFightActionsFactories()
        {
            return Data.FightActions.Select<FightActionDescriptor, HumanActionFactoryBase>(action =>
            {
                switch (action.Number)
                {
                    case FightActionEnum.C030_ACTION_BASH:
                    case FightActionEnum.C018_ACTION_HACK:
                    case FightActionEnum.C019_ACTION_BERZERK:
                    case FightActionEnum.C007_ACTION_KICK:
                    case FightActionEnum.C013_ACTION_SWING:
                    case FightActionEnum.C002_ACTION_CHOP:
                        return new SwingAttackFactory(
                            action.Name,
                            action.ExperienceGain,
                            action.DefenseModifier,
                            action.HitProbability,
                            action.Damage,
                            action.Fatigue * 1000 / 6,
                            Skills[action.ImprovedSkill],
                            action.Stamina,
                            -1);//TODO

                    case FightActionEnum.C024_ACTION_DISRUPT:
                    case FightActionEnum.C016_ACTION_JAB:
                    case FightActionEnum.C017_ACTION_PARRY:
                    case FightActionEnum.C014_ACTION_STAB:
                    case FightActionEnum.C009_ACTION_STAB:
                    case FightActionEnum.C031_ACTION_STUN:
                    case FightActionEnum.C015_ACTION_THRUST:
                    case FightActionEnum.C025_ACTION_MELEE:
                    case FightActionEnum.C028_ACTION_SLASH:
                    case FightActionEnum.C029_ACTION_CLEAVE:
                    case FightActionEnum.C006_ACTION_PUNCH:
                        return new MeleeAttackFactory(
                            action.Name,
                            action.ExperienceGain,
                            action.DefenseModifier,
                            action.HitProbability,
                            action.Damage,
                            action.Fatigue * 1000 / 6,
                            Skills[action.ImprovedSkill],
                            action.Stamina,
                            -1);

                    default:
                        return new ActionMocap(
                            action.Name,
                            action.ExperienceGain,
                            action.DefenseModifier,
                            action.HitProbability,
                            action.Damage,
                            action.Fatigue * 1000 / 6,
                            Skills[action.ImprovedSkill],
                            action.Stamina,
                            -1);
                }
            })
            .ToArray();

        }

        protected virtual IReadOnlyList<IReadOnlyList<IActionFactory>> GetComboActions()
        {
            // ReSharper disable once CoVariantArrayConversion
            return Data.FightCombos.Select(c => c.Actions
                    .Select(x => new ComboActionFactory(x.UseCharges == 1, x.MinimumSkillLevel,
                        FightActions[(int)x.ActionDescriptor.Number]))
                    .ToArray())
                .ToArray();
        }



    }

    public class ActionMocap : HumanActionFactoryBase {
        public ActionMocap(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina, int mapDifficulty) : base(name, experienceGain, defenseModifer, hitProbability, damage, fatigue, skillIndex, stamina, mapDifficulty) {}

        public override IAction CreateAction(ILiveEntity actionProvider)
        {
            throw new NotImplementedException();
        }
    }
}