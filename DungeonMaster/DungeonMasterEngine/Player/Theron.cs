using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Magic.Spells;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.DungeonContent.Actuators.Floor;

namespace DungeonMasterEngine.Player
{
    public class Theron : PointOfViewCamera
    {
        private MouseState prevMouse = Mouse.GetState();
        private KeyboardState prevKeyboard;

        public List<Champoin> PartyGroup { get; } = new List<Champoin> {
            //TODO remove champion mocap
            new Champoin { Name = "Pepa Mocap 0" },
            new Champoin { Name = "Pepa Mocap 1" },
            new Champoin { Name = "Pepa Mocap 2 "},
            new Champoin { Name = "Pepa Mocap 3" }};//TODO remove champion mocap

        public GrabableItem Hand { get; private set; }

        public Theron(Game game) : base(game)
        { }

        protected override void OnLocationChanged(Tile oldLocation, Tile newLocation)
        {
            base.OnLocationChanged(oldLocation, newLocation);

            oldLocation?.SubItems.Remove(this);
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
                    $"Click on Item: {closest.Item1}".Dump();
                    if (closest.Item1 is GrabableItem && Hand == null)
                    {
                        Hand = (GrabableItem)closest.Item1;
                        closest.Item1.Location = null;
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

        public bool ThrowOutItem(uint distance = 0)
        {
            if (Hand != null )
            {
                var targetLocation = CheckRoute(distance);

                if (targetLocation != null)
                {
                    Hand.Location = targetLocation;
                    Hand = null;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private Tile CheckRoute(uint distance)
        {
            var direction = GetShift(ForwardDirection);
            var curLocation = Location;
            for (int i = 0; i < distance; i++)
            {
                curLocation = curLocation.Neighbours.GetTile(direction);
                if (!curLocation.IsAccessible)
                    return null;
            }
            return curLocation;
        }

        public void PutToHand(GrabableItem item) => PutToHand(item, null);

        public void PutToHand(GrabableItem item, Champoin ch)
        {
            Hand = item;

            ch?.Inventory.Remove(item);
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