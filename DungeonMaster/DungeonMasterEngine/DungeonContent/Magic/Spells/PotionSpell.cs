using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Potions;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.GameConsoleContent;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public class PotionSpell : Spell
    {
        public PotionSpellFactory Factory { get; }
        public IPowerSymbol PowerSymbol { get; }

        public PotionSpell(IPowerSymbol powerSymbol, PotionSpellFactory factory)
        {
            Factory = factory;
            PowerSymbol = powerSymbol;
        }

        public override void Run(ILiveEntity caster, MapDirection direction)
        {
            var actionHandStorage = caster.Body.GetBodyStorage(ActionHandStorageType.Instance);
            var actionHandItem = actionHandStorage.Storage[0] as EmptyPotion;
            var readyHandStorage = caster.Body.GetBodyStorage(ReadyHandStorageType.Instance);
            var readyHandItem = readyHandStorage.Storage[0] as EmptyPotion;
            EmptyPotion flask = actionHandItem ?? readyHandItem;
            if (flask != null)
            {
                IBodyPart usedStorage = flask == actionHandItem ? actionHandStorage : readyHandStorage;
                usedStorage.TakeItemFrom(0);
                var item = Factory.PotionFactory.Create(new PotionInitializer
                {
                    PotionPower = rand.Next(16) + PowerSymbol.LevelOrdinal * 40
                });

                usedStorage.AddItemTo(item, 0);
            }
            else
            {
                GameConsole.Instance.Out.WriteLine(caster + "C10_FAILURE_NEEDS_FLASK_IN_HAND");
            }
        }

    }
}