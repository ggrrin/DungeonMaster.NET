using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.GameConsoleContent.Base;
using DungeonMasterEngine.Player;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class HandCommand : Interpreter
    {
        private Theron theron;

        public override async Task Run()
        {
            theron = ConsoleContext.AppContext.Theron;

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
                    case "throw":
                        ThrowItem();
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

        private void ThrowItem()
        {
            uint distance = 0;
            if (Parameters.Length == 1 || (Parameters.Length == 2 && uint.TryParse(Parameters[1], out distance)))
            {
                if (!theron.ThrowOutItem(distance))
                    Output.WriteLine("Invalid Location");
            }
            else
            {
                Output.WriteLine("Invalid Parameters!");
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