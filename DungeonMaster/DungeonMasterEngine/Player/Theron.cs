using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DungeonMasterEngine.Builders;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Player
{
    public class Theron : PointOfViewCamera, ILeader
    {
        private MouseState prevMouse = Mouse.GetState();
        private KeyboardState prevKeyboard;



        public readonly List<Champion> partyGoup = new List<Champion>();
        private IGrabableItem hand;

        public IReadOnlyList<Champion> PartyGroup => partyGoup;


        public object Interactor => Ray;

        IReadOnlyList<ILiveEntity> ILeader.PartyGroup => PartyGroup;

        public IGrabableItem Hand
        {
            get { return hand; }
            set
            {
                if (hand != value && hand != null && value != null)
                    throw new InvalidOperationException("Assign null first;");
                hand = value;
            }
        }

        private Champion leader;
        public ILiveEntity Leader => leader;



        public Theron(Tile location)
        {
            Location = location;

            var builder = new ChampionMocapFactory();
            var x = new[]
            {
                //TODO remove champion mocap
                builder.GetChampion("CHANI|SAYYADINA SIHAYA||F|AACPACJOAABB|DJCFCPDJCFCPCF|BDACAAAAAAAADCDB"),
                builder.GetChampion("IAIDO|RUYITO CHIBURI||M|AADAACIKAAAL|CICLDHCICDCNDC|CDACAAAABBBCAAAA"),
                builder.GetChampion("HAWK|$CFEARLESS||M|AAEGADFCAAAK|CICNCDCGDHCDCD|CAACAAAAADADAAAA"),
                builder.GetChampion("ZED|DUKE OF BANVILLE||M|AADMACFIAAAK|DKCICICIDCCICI|CBBCCBCBBCBBBCBB"),
            };
            if (x.Any(champoin => !AddChampoinToGroup(champoin)))
            {
                throw new Exception();
            }
        }

        protected override bool CanMoveToTile(ITile tile) => base.CanMoveToTile(tile) && tile.LayoutManager.WholeTileEmpty;

        protected override void OnMapDirectionChanged(MapDirection oldDirection, MapDirection newDirection)
        {
            base.OnMapDirectionChanged(oldDirection, newDirection);
            //TODO $"Direction changed: {oldDirection} -> {newDirection}".Dump();

            if (oldDirection != newDirection.Opposite)
                RotateParty(oldDirection, newDirection);
            else
            {
                var midle = oldDirection.NextClockWise;
                RotateParty(oldDirection, midle);
                RotateParty(midle, newDirection);
            }
        }

        protected override void OnLocationChanging(ITile oldLocation, ITile newLocation)
        {
            base.OnLocationChanging(oldLocation, newLocation);

            oldLocation?.OnObjectLeaving(this);
            newLocation?.OnObjectEntering(this);

            MovePartyToRight(newLocation);
        }

        private void MovePartyToRight(ITile newLocation)
        {
            if (!newLocation.LayoutManager.WholeTileEmpty)
                throw new InvalidOperationException();

            foreach (var champion in PartyGroup)
            {
                var prevLocation = champion.Location;
                if (!newLocation.LayoutManager.TryGetSpace(champion, prevLocation.Space))
                    throw new InvalidOperationException("not expected");

                champion.Location = new FourthSpaceRouteElement(prevLocation.Space, newLocation);
                prevLocation.Tile.LayoutManager.FreeSpace(champion, prevLocation.Space);
            }
        }

        protected override void OnLocationChanged(ITile oldLocation, ITile newLocation)
        {
            base.OnLocationChanged(oldLocation, newLocation);

            oldLocation?.OnObjectLeft(this);
            newLocation?.OnObjectEntered(this);
        }

        private void RotateParty(MapDirection oldDirection, MapDirection newDirection)
        {
            var targetLocation = partyGoup.FirstOrDefault()?.Location?.Tile;

            if (targetLocation != null)
            {

                var counterClockWiseGridPoints = new[]
                {
                    Tuple.Create(new Point(0, 0), new Point(0, 1)),
                    Tuple.Create(new Point(0, 1), new Point(1, 1)),
                    Tuple.Create(new Point(1, 1), new Point(1, 0)),
                    Tuple.Create(new Point(1, 0), new Point(0, 0)),
                };

                Func<Point, Point> nextGridPoint = p =>
                {
                    if (oldDirection.NextCounterClockWise == newDirection)
                        return Array.Find(counterClockWiseGridPoints, t => t.Item1 == p).Item2;
                    else if (oldDirection.NextClockWise == newDirection)
                        return Array.Find(counterClockWiseGridPoints, t => t.Item2 == p).Item1;
                    else
                        throw new Exception();
                };

                foreach (var champoin in PartyGroup)
                    targetLocation.LayoutManager.FreeSpace(champoin, champoin.Location.Space);

                foreach (var champoin in PartyGroup)
                {
                    var newSpace = champoin.GroupLayout.AllSpaces.First(s => s.GridPosition == nextGridPoint(champoin.Location.Space.GridPosition));
                    champoin.Location = new FourthSpaceRouteElement(newSpace, targetLocation);
                    Debug.Assert(targetLocation.LayoutManager.TryGetSpace(champoin, champoin.Location.Space));
                }
            }
        }

        public IGrabableItem ExchangeItems(IGrabableItem item) => item;


        public bool ThrowOutItem(uint distance = 0)
        {
            if (Hand != null)
            {
                var targetLocation = CheckRoute(distance);

                if (targetLocation != null)
                {
                    //TODO throwing
                    Hand = null;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private ITile CheckRoute(uint distance)
        {
            var direction = MapDirection;
            var curLocation = Location;
            for (int i = 0; i < distance; i++)
            {
                curLocation = curLocation.Neighbours.GetTile(direction);
                if (curLocation == null || !curLocation.IsAccessible)
                    return null;
            }
            return curLocation;
        }

        public bool AddChampoinToGroup(Champion champion)
        {
            if (partyGoup.Count == 4)
                return false;

            var freeSpace = Small4GroupLayout.Instance.AllSpaces.Except(partyGoup.Select(ch => ch.Location?.Space).Where(x => x != null)).First();
            champion.Location = new FourthSpaceRouteElement(freeSpace, Location);
            partyGoup.Add(champion);
            return true;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            foreach (var champoin in PartyGroup)
            {
                champoin.Update(time);
            }

            if (IsActive && Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released
                || Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
            {
                var tiles = new[] { Location }.Concat(Location.Neighbours
                    .Select(x => x.Item1))
                    .ToArray();

                var matrix = Matrix.Identity;
                foreach (var tile in tiles)
                    if(tile.Renderer.Interact(this, ref matrix, null))
                        break;

            }

            prevMouse = Mouse.GetState();
            prevKeyboard = Keyboard.GetState();
        }

        public bool IsActive
        {
            get { return true; }
            set { }
        }

        public ILiveEntity GetEnemy(ILiveEntity champoin)
        {
            var enemyTile = Location.Neighbours.GetTile(MapDirection);
            return enemyTile.LayoutManager.Entities.MinObj(e => Vector3.Distance(e.Position, champoin.Position));
        }

        private void Fight()
        {
            var enemyTile = Location.Neighbours.GetTile(MapDirection);
            var GroupLayout = partyGoup.First().GroupLayout;
            if (enemyTile != null)
            {
                var sortedEnemyLocation = GroupLayout.AllSpaces
                    .Where(s => s.Sides.Contains(MapDirection.Opposite))
                    .Concat(GroupLayout.AllSpaces
                        .Where(s => s.Sides.Contains(MapDirection)))
                    .Where(s => !enemyTile.LayoutManager.IsFree(s));

                var hitLocation = sortedEnemyLocation.FirstOrDefault();

                var enemy = enemyTile.LayoutManager.GetEntities(hitLocation).FirstOrDefault();
                //((Creature)enemy)?.Kill();
            }
        }

        public void Draw(BasicEffect effect)
        {

            foreach (var champoin in PartyGroup)
            {
                var mat = Matrix.Identity;
                champoin.Renderer.Render(ref mat, effect, null);
            }
        }

        private void ObsloteInteraction(List<Tile> tiles)
        {
            //var aimingTile = Location.Neighbours.GetTile(MapDirection);
            //if (aimingTile != null)
            //    tiles.Add(aimingTile);

            //var intersectingItems = new List<Tuple<IItem, Tile>>();

            //Tuple<IItem, Tile> closest = null;
            //float closestDistance = float.MaxValue;

            //foreach (var tile in tiles)
            //    foreach (var item in tile.SubItems)
            //    {
            //        float? res = Ray.Intersects(item.Bounding);
            //        if (res != null)
            //        {
            //            intersectingItems.Add(new Tuple<IItem, Tile>(item, tile));
            //            if (res.Value < closestDistance)
            //            {
            //                closest = new Tuple<IItem, Tile>(item, tile);
            //                closestDistance = res.Value;
            //            }
            //        }
            //    }

            //if (closest != null)
            //{
            //    $"Click on Item: {closest.Item1}".Dump();
            //    if (closest.Item1 is GrabableItem && Hand == null)
            //    {
            //        Hand = (GrabableItem)closest.Item1;
            //        closest.Item1.Location = null;
            //    }
            //    else
            //    {
            //        Hand = closest.Item1.ExchangeItems(Hand);
            //    }
            //}
            //else
            //{
            //    Fight();
            //}
        }

    }
}