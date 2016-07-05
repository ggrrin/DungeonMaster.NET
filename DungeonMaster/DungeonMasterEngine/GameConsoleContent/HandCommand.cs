using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Potions;
using DungeonMasterEngine.GameConsoleContent.Base;
using DungeonMasterEngine.Player;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class HandCommand : Interpreter
    {
        private Theron theron;

        public override async Task Run()
        {
            theron = ConsoleContext.AppContext.Leader;

            if (Parameters.Length == 0)
            {
                if (theron.Hand != null)
                    Output.WriteLine(theron.Hand.ToString());
                else
                    Output.WriteLine("Empty");
            }
            else if (Parameters.Length > 0)
            {
                switch (Parameters[0])
                {
                    case "take":
                        await TakeItem();
                        break;
                    case "takesub":
                        await TakeSubItem();
                        break;
                    case "put":
                        await PutItem();
                        break;
                    case "putsub":
                        await PutSubItem();
                        break;
                    case "use":
                        await UseItem();
                        break;
                    default:
                        Output.WriteLine("invalid command!");
                        break;
                }
            }
            else
            {
                Output.WriteLine("Invalid paramtetes");
            }
        }

        private async Task UseItem()
        {
            if (theron.Hand == null)
            {
                Output.WriteLine("No item in hand!.");
                return;
            }

            var effect = theron.Hand as IHasEffect;
            if (effect != null)
            {
                if (effect.Used)
                {
                    Output.WriteLine("Item is already used!.");
                }
                else
                {
                    Champion champion = await GetFromItemIndex(theron.PartyGroup);
                    if (champion != null)
                    {
                        if (effect.ApplyEffect(champion))
                        {
                            theron.Hand = effect.GetUsedOutcomeItem(ConsoleContext.AppContext.Factories);
                            Output.WriteLine(effect.Message);
                        }
                    }
                }
            }
            else
            {
                Output.WriteLine("Item cannot be used!.");
            }
        }

        private async Task PutSubItem()
        {
            if (theron.Hand == null)
            {
                var ch = await GetFromItemIndex(theron.PartyGroup);

                if (ch != null)
                {
                    var inventory = await GetFromItemIndex(ch.Body.Storages);
                    if (inventory != null)
                    {
                        var chest = await GetFromItemIndex(inventory.Storage.OfType<IInventory>());
                        if (chest != null)
                        {
                            var itemIndex = await GetItemIndex(chest.Storage);
                            if (itemIndex != null)
                            {
                                var item = chest.TakeItemFrom(itemIndex.Value);
                                if (item != null)
                                    theron.Hand = item;
                                else
                                {
                                    Output.WriteLine("Slot is empty.");
                                }

                            }
                        }
                    }
                }
            }
            else
                Output.WriteLine("Hand is not empty!");
        }

        private async Task PutItem()
        {
            if (theron.Hand == null)
            {
                var ch = await GetFromItemIndex(theron.PartyGroup);

                if (ch != null)
                {
                    var inventory = await GetFromItemIndex(ch.Body.Storages);
                    if (inventory != null)
                    {
                        var itemIndex = await GetItemIndex(inventory.Storage);
                        if (itemIndex != null)
                        {
                            theron.Hand = inventory.TakeItemFrom(itemIndex.Value);
                        }
                    }
                }
            }
            else
                Output.WriteLine("Hand is not empty!");
        }

        private async Task TakeSubItem()
        {
            if (theron.Hand != null)
            {
                var ch = await GetFromItemIndex(theron.PartyGroup);

                if (ch != null)
                {
                    var inventory = await GetFromItemIndex(ch.Body.Storages);
                    if (inventory != null)
                    {
                        var chest = await GetFromItemIndex(inventory.Storage.OfType<IInventory>());
                        if (chest != null)
                        {
                            var itemIndex = await GetItemIndex(chest.Storage);
                            if (itemIndex != null)
                            {
                                theron.Hand = chest.TakeItemFrom(itemIndex.Value);
                            }
                        }
                    }
                }
            }
            else
                Output.WriteLine("Hand is empty!");
        }

        private async Task TakeItem()
        {
            if (theron.Hand != null)
            {
                var ch = await GetFromItemIndex(theron.PartyGroup);

                if (ch != null)
                {
                    var inventory = await GetFromItemIndex(ch.Body.Storages);
                    if (inventory != null)
                    {
                        if (inventory.AddItem(theron.Hand))
                        {
                            theron.Hand = null;
                        }
                        else
                        {
                            Output.WriteLine("Unable to add item to inventory.");
                        }

                    }
                }
            }
            else
                Output.WriteLine("Hand is empty!");
        }
    }
}