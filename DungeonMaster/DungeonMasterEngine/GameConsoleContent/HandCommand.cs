using System.Threading.Tasks;
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
                    Output.WriteLine(theron.Hand.DumpString());
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
                    case "put":
                        await PutItem();
                        break;
                    case "throw":
                        ThrowItem();
                        break;
                    case "list":
                        ListContainer();
                        break;
                    case "extract":
                        await ExtractContainer();
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

        private async Task ExtractContainer()
        {
            var container = theron.Hand as Container;
            if (container != null)
            {
                var ch = await GetFromItemIndex(theron.PartyGroup);
                if (ch != null)
                {
                    var items = ((Container)theron.Hand).SubItems;
                    ch.Inventory.AddRange(items);
                    items.Clear();
                }
            }
            else
            {
                Output.WriteLine("Item is not container");
            }
        }

        private void ListContainer()
        {
            var container = theron.Hand as Container;
            if (container != null)
            {
                foreach (var i in ((Container)theron.Hand).SubItems)
                    Output.WriteLine(i.DumpString());
            }
            else
            {
                Output.WriteLine("Item is not container");
            }
        }

        private async Task PutItem()
        {
            if (theron.Hand == null)
            {
                var ch = await GetFromItemIndex(theron.PartyGroup);

                if (ch != null)
                {
                    var item = await GetFromItemIndex(ch.Inventory);
                    if (item != null)
                    {
                        theron.PutToHand(item, ch);
                    }
                }
            }
            else
            {
                Output.WriteLine("Hand is not empty!");
            }
        }

        private async Task TakeItem()
        {
            if (theron.Hand != null)
            {
                var ch = await GetFromItemIndex(theron.PartyGroup);

                if (ch != null)
                {
                    theron.HandToInventory(ch);
                }
            }
            else
                Output.WriteLine("Hand is empty!");
        }
    }
}