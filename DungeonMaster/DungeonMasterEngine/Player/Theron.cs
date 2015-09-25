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

        public List<Champoin> PartyGroup { get; } = new List<Champoin> { };// new Champoin { Name = "Pepa Mocap 1" }, new Champoin { Name = "Pepa Mocap 2 " }, new Champoin { Name = "Pepa Mocap 3" }, new Champoin { Name = "Pepa Mocap 4" }, };//TODO remove champion mocap

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

            if (Game.IsActive && Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
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
                    closest.Item1.Dump(1);
                    if (closest.Item1 is GrabableItem && Hand == null)
                    {
                        Hand = (GrabableItem)closest.Item1;
                        closest.Item2.SubItems.Remove(closest.Item1);
                    }
                    else
                    {
                        Hand = closest.Item1.ExchangeItems(Hand);
                    }
                }
            }

            prevMouse = Mouse.GetState();
        }

        public void ThrowOutItem()
        {
            if (Hand != null)
            {
                Hand.Position = Location.Position;
                Location.SubItems.Add(Hand);
                Hand = null;
            }
        }

        public void PutToHand(GrabableItem item, Champoin ch)
        {
            Hand = item;
            ch.Inventory.Remove(item);
        }

        public void HandToInventory(Champoin ch)
        {
            ch.Inventory.Add(Hand);
            Hand = null;
        }
    }
}