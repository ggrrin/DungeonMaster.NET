using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using DungeonMasterEngine.Items;
using System.Diagnostics;
using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.Player
{
    public class Theron : PointOfViewCamera
    {
        private MouseState prevMouse = Mouse.GetState();
        private KeyboardState prevKeyboard;

        public List<Champoin> PartyGroup { get; } = new List<Champoin> {  new Champoin { Name = "Pepa Mocap 1" }, new Champoin { Name = "Pepa Mocap 2 " }, new Champoin { Name = "Pepa Mocap 3" }, new Champoin { Name = "Pepa Mocap 4" }, };//TODO remove champion mocap

        public GrabableItem Hand { get; private set; }


        public Theron(Game game) : base(game)
        {
            //TODO remove champion mocap
            //var i = new Miscellaneous(Vector3.Zero);
            //i.Identifer = 27;
            //PartyGroup[0].Inventory.Add(i);
            ////////////////////
        }

        protected override void OnLocationChanged(Tile oldLocation, Tile newLocation)
        {
            base.OnLocationChanged(oldLocation, newLocation);

            oldLocation?.OnObjectLeft(this);
            newLocation?.OnObjectEntered(this);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (Game.IsActive && Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released /*|| Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter)*/)
            {
                var tiles = new List<Tile> { Location };
                var aimingTile = Location.Neighbours.GetTile(GetShift(ForwardDirection));
                if (aimingTile != null)
                    tiles.Add(aimingTile);

                List<Tuple<Item, Tile>> intersectingItems = new List<Tuple<Item, Tile>>();

                Tuple<Item, Tile> closest = null;
                float closestDistance = float.MaxValue;


                foreach (var tile in tiles)
                    foreach (var item in tile.SubItems)
                    {
                        float? res = Ray.Intersects(item.Bounding);
                        if (res != null)
                        {
                            intersectingItems.Add(new Tuple<Item, Tile>(item, tile));
                            if (res.Value < closestDistance)
                            {
                                closest = new Tuple<Item, Tile>(item, tile);
                                closestDistance = res.Value;
                            }
                        }
                    }

                if (closest != null)
                {
                    //closest.Item1.Dump(1);
                    $"Click on Item: {closest.Item1}".Dump();
                    if (closest.Item1 is GrabableItem && Hand == null)
                    {
                        Hand = (GrabableItem)closest.Item1;
                        closest.Item2.SubItems.Remove(closest.Item1);
                        closest.Item2.OnObjectLeft(closest.Item1);//notify tile that item disappeareds
                    }
                    else
                    {
                        Hand = closest.Item1.ExchangeItems(Hand);
                    }
                }
            }

            prevMouse = Mouse.GetState();
            prevKeyboard = Keyboard.GetState();
        }

        public void ThrowOutItem()
        {
            if (Hand != null)
            {
                Hand.Location = Location;
                Hand = null;
            }
        }

        public void PutToHand(GrabableItem item) => PutToHand(item, null);

        public void PutToHand(GrabableItem item, Champoin ch)
        {
            Hand = item;

            if (ch != null)
                ch.Inventory.Remove(item);
        }

        public void HandToInventory(Champoin ch)
        {
            if (Hand == null)
                throw new InvalidOperationException("Hand is empty.");
            ch.Inventory.Add(Hand);
            Hand = null;
        }
    }
}