using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Misc;
using DungeonMasterEngine.DungeonContent.Magic;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public class Champion : LiveEntity, IRenderable
    {
        protected static readonly Random rand = new Random();
        protected readonly ChampionBones bones;
        protected readonly Animator<Champion, ISpaceRouteElement> animator = new Animator<Champion, ISpaceRouteElement>();

        protected ISpaceRouteElement location;

        protected readonly IDictionary<IPropertyFactory, IProperty> properties;
        protected readonly IDictionary<ISkillFactory, ISkill> skills;

        public event EventHandler<Champion> Died;
        public bool Sleeping { get; protected set; } = false;
        public IRenderer Renderer { get; set; }
        public override float TranslationVelocity => 4.4f;
        public override IGroupLayout GroupLayout => Small4GroupLayout.Instance;
        public override IBody Body { get; } = new HumanBody();

        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsMale { get; set; }
        public ISpellCastingManager SpellManager { get; }

        public override IRelationManager RelationManager { get; }

        public override ISpaceRouteElement Location
        {
            get { return location; }
            set
            {
                if (location != null)
                {
                    animator.MoveTo(this, value, setLocation: false);
                }
                else//set Position at first
                {
                    if (!value.Tile.LayoutManager.TryGetSpace(this, value.Space))
                        throw new ArgumentException("Location is not accessible!");

                    Position = value.StayPoint;
                }

                location = value;
            }
        }


        private TaskCompletionSource<bool> timeEffectStopped;

        public Champion(IChampionInitializer initializator, RelationToken relationToken, IEnumerable<RelationToken> enemiesRelationTokens)
        {
            RelationManager = new DefaultRelationManager(relationToken, enemiesRelationTokens);
            var enumerable = initializator.GetProperties(this);
            properties = enumerable.ToDictionary(p => p.Type);
            skills = initializator.GetSkills(this).ToDictionary(s => s.Type);
            bones = initializator.BonesFactory.Create(new ChampionBonesInitializer { Champion = this });
            SpellManager = new SpellCastingManager(this);

            timeEffectStopped = new TaskCompletionSource<bool>();
            timeEffectStopped.SetResult(true);

            var health = (HealthProperty)properties[PropertyFactory<HealthProperty>.Instance];
            health.ValueChanged += (sender, value) =>
            {
                if (Activated && value <= 0)
                    Kill();
            };
        }

        public async void Rebirth()
        {
            var health = bones.Champion.GetProperty(PropertyFactory<HealthProperty>.Instance);
            health.Value = health.MaxValue;

            //needs be before activate !!!!
            await timeEffectStopped.Task;
            timeEffectStopped = new TaskCompletionSource<bool>();
            Activated = true;

            while (Activated)
            {
                F331_auzz_CHAMPION_ApplyTimeEffects_COPYPROTECTIONF();
                await Task.Delay(30000);//TODO adjust interval
            }

            timeEffectStopped.SetResult(true);
        }

        private void Kill()
        {
            Activated = false;

            Died?.Invoke(this, this);
            location.Tile.LayoutManager.FreeSpace(this, location.Space);

            foreach (var storage in Body.Storages)
            {
                for (int i = 0; i < storage.Storage.Count; i++)
                {
                    var item = storage.TakeItemFrom(i);
                    if (item != null)
                        item.Location = GroupLayout.GetSpaceElement(Location.Space, location.Tile);
                }
            }
            bones.Location = GroupLayout.GetSpaceElement(Location.Space, location.Tile);
            location = null;
        }

        public override IProperty GetProperty(IPropertyFactory propertyType)
        {
            IProperty val;
            if (properties.TryGetValue(propertyType, out val))
                return val;
            else
                return new EmptyProperty();
        }

        public override ISkill GetSkill(ISkillFactory skillType)
        {
            ISkill val;
            if (skills.TryGetValue(skillType, out val))
                return val;
            else
                return new EmptySkill();
        }

        public IEnumerable<IActionFactory> CurrentCombos
        {
            get
            {
                var actionHand = Body.BodyParts.First(x => x.Type == ActionHandStorageType.Instance);
                var item = actionHand.Storage[0];
                return item?.FactoryBase.ActionCombo ?? Enumerable.Empty<IActionFactory>();
            }
        }

        public bool ReadyForAction { get; protected set; } = true;

        public override void MoveTo(ITile newLocation, bool setNewLocation)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            animator.Update(gameTime);
        }

        public override string ToString() => Name;

        public async void DoAction(IActionFactory actionFactory)
        {
            if (ReadyForAction)
            {
                ReadyForAction = false;
                var action = actionFactory.CreateAction(this);
                await Task.Delay(action.ApplyAttack(MapDirection));
                "Action available.".Dump();
                ReadyForAction = true;
            }
        }

        #region TimeEffects
        void F331_auzz_CHAMPION_ApplyTimeEffects_COPYPROTECTIONF()
        {
            //int A1006_ui_GameTime = G313_ul_GameTime;
            //int t1 = A1006_ui_GameTime & 0x0080; //3 bit || 5 bit
            //int t2 = A1006_ui_GameTime & 0x0100; //2 bit || 4 bit
            //int t3 = A1006_ui_GameTime & 0x0040; //0 bit || 2 bit //zero based
            //int L1012_ui_TimeCriteria = ((t1 + (t2 >> 2)) + (t3 << 2)) >> 2;  // => => value max 64;
            int L1012_ui_TimeCriteria = rand.Next(64);

            var mana = GetProperty(PropertyFactory<ManaProperty>.Instance);
            var wisdom = GetProperty(PropertyFactory<WisdomProperty>.Instance);
            var stamina = GetProperty(PropertyFactory<StaminaProperty>.Instance);

            int A1007_ui_StaminaGainCycleCount, A1009_i_StaminaLoss, A1013_i_StaminaAmount;

            //order matters!!!
            AffectMana(L1012_ui_TimeCriteria, mana, wisdom, stamina);
            AffectTemporaryExperience();
            AffectStamina(stamina, out A1007_ui_StaminaGainCycleCount, out A1009_i_StaminaLoss, out A1013_i_StaminaAmount);
            AffectFoodWater(stamina, ref A1007_ui_StaminaGainCycleCount, A1009_i_StaminaLoss, A1013_i_StaminaAmount);
            AffectHealth(L1012_ui_TimeCriteria, stamina);
        }

        private void AffectStamina(IProperty stamina, out int A1007_ui_StaminaGainCycleCount, out int A1009_i_StaminaLoss, out int A1013_i_StaminaAmount)
        {
            A1007_ui_StaminaGainCycleCount = 4;
            int A1009_i_StaminaMagnitude = stamina.MaxValue;
            while (stamina.Value < (A1009_i_StaminaMagnitude >>= 1))
            {
                A1007_ui_StaminaGainCycleCount += 2;
            }
            A1009_i_StaminaLoss = 0;
            A1013_i_StaminaAmount = MathHelper.Clamp((stamina.MaxValue >> 8) - 1, 1, 6);
            if (Sleeping)
                A1013_i_StaminaAmount <<= 1;

            //int A1008_ui_Delay = G313_ul_GameTime - G362_l_LastPartyMovementTime; //TODO calculate movement delay
            //if (A1008_ui_Delay > 13000) //80) //TODO 13 s seems to by too much
            //{
            //    A1013_i_StaminaAmount++;
            //    if (A1008_ui_Delay > 250)
            //    {
            //        A1013_i_StaminaAmount++;
            //    }
            //}
        }

        private void AffectTemporaryExperience()
        {
            //temporary exp
            foreach (var skill in skills.Values)
                skill.TemporaryExperience--;
        }

        private void AffectMana(int L1012_ui_TimeCriteria, IProperty mana, IProperty wisdom, IProperty stamina)
        {
            var priestSkill = GetSkill(SkillFactory<PriestSkill>.Instance);
            var wizardSkill = GetSkill(SkillFactory<WizardSkill>.Instance);
            int A1008_ui_WizardSkillLevel = wizardSkill.SkillLevel + priestSkill.SkillLevel;

            //mana
            if (mana.Value < mana.MaxValue && L1012_ui_TimeCriteria < wisdom.Value + A1008_ui_WizardSkillLevel)
            {
                int A1007_ui_ManaGain = mana.MaxValue / 40;
                if (Sleeping)
                    A1007_ui_ManaGain = A1007_ui_ManaGain << 1;

                A1007_ui_ManaGain++;
                stamina.Value -= A1007_ui_ManaGain * MathHelper.Max(7, 16 - A1008_ui_WizardSkillLevel);
                mana.Value += A1007_ui_ManaGain;
            }
        }

        private void AffectFoodWater(IProperty stamina, ref int A1007_ui_StaminaGainCycleCount, int A1009_i_StaminaLoss, int A1013_i_StaminaAmount)
        {
            var food = GetProperty(PropertyFactory<FoodProperty>.Instance);
            var water = GetProperty(PropertyFactory<WaterProperty>.Instance);
            do//water and food
            {
                bool A1008_ui_StaminaAboveHalf = A1007_ui_StaminaGainCycleCount <= 4;
                if (food.Value < -512)
                {
                    if (A1008_ui_StaminaAboveHalf)
                    {
                        A1009_i_StaminaLoss += A1013_i_StaminaAmount;
                        food.Value -= 2;
                    }
                }
                else
                {
                    if (food.Value >= 0)
                        A1009_i_StaminaLoss -= A1013_i_StaminaAmount;

                    food.Value -= A1008_ui_StaminaAboveHalf ? 2 : A1007_ui_StaminaGainCycleCount >> 1;
                }

                if (water.Value < -512)
                {
                    if (A1008_ui_StaminaAboveHalf)
                    {
                        A1009_i_StaminaLoss += A1013_i_StaminaAmount;
                        water.Value -= 1;
                    }
                }
                else
                {
                    if (water.Value >= 0)
                    {
                        A1009_i_StaminaLoss -= A1013_i_StaminaAmount;
                    }
                    water.Value -= A1008_ui_StaminaAboveHalf ? 1 : A1007_ui_StaminaGainCycleCount >> 2;
                }
                A1007_ui_StaminaGainCycleCount--;
            } while (A1007_ui_StaminaGainCycleCount > 0 && ((stamina.Value - A1009_i_StaminaLoss) < stamina.MaxValue));
            stamina.Value -= A1009_i_StaminaLoss;
        }

        private void AffectHealth(int L1012_ui_TimeCriteria, IProperty stamina)
        {

            //health
            var health = GetProperty(PropertyFactory<HealthProperty>.Instance);
            if ((health.Value < health.MaxValue) && (stamina.Value >= (stamina.MaxValue >> 2))
                && (L1012_ui_TimeCriteria < GetProperty(PropertyFactory<VitalityProperty>.Instance).Value + 12))
            {
                int A1013_i_HealthGain = (health.MaxValue >> 7) + 1;
                if (Sleeping)
                    A1013_i_HealthGain <<= 1;

                //if (F033_aaaz_OBJECT_GetIconIndex(L1010_ps_Champion->Slots[C10_SLOT_NECK]) == C121_ICON_JUNK_EKKHARD_CROSS)
                //{
                //    A1013_i_HealthGain += (A1013_i_HealthGain >> 1) + 1;
                //}

                health.Value += A1013_i_HealthGain;
            }
        }
        #endregion


    }
}