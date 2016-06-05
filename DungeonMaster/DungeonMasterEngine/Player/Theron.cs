using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
    public class Theron : PointOfViewCamera, IItem, ILeader
    {
        private MouseState prevMouse = Mouse.GetState();
        private KeyboardState prevKeyboard;

        public IGraphicProvider GraphicsProvider => null;

        public IRenderer Renderer { get; set; }
        public IInteractor Inter { get; set; }

        public BoundingBox Bounding => default(BoundingBox);
        public bool AcceptMessages { get; set; } = false;

        public List<Champion> partyGoup = new List<Champion>();
        private IGrabableItem hand;

        public IReadOnlyList<Champion> PartyGroup => partyGoup;


        public object Interactor => Ray; 

        IReadOnlyList<IEntity> ILeader.PartyGroup => PartyGroup;

        public IGrabableItem Hand
        {
            get { return hand; }
            set
            {
                if(hand != value && hand != null && value != null)
                    throw new InvalidOperationException("Assign null first;");
                hand = value;
            }
        }

        public Champion Leader { get; private set; }

        GrabableItem ILeader.Hand
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Theron(Tile location, Game game) : base(game)
        {
            Location = location;

            //var x = new[]{
            //TODO remove champion mocap
            //new Champion(new RelationToken(0), new RelationToken(1u).ToEnumerable()) { Name = "Mocap1" },
            //new Champion(new RelationToken(0), new RelationToken(1u).ToEnumerable()) { Name = "Mocap2" },
            //new Champion(new RelationToken(0), new RelationToken(1u).ToEnumerable()) { Name = "Mocap3" },
            //new Champion(new RelationToken(0), new RelationToken(1u).ToEnumerable()) { Name = "Mocap4" }};
            //if (x.Any(champoin => !AddChampoinToGroup(champoin)))
            //{
            //    throw new Exception();
            //}
        }

        protected override bool CanMoveToTile(Tile tile) => base.CanMoveToTile(tile) && tile.LayoutManager.WholeTileEmpty;

        protected override void OnMapDirectionChanged(MapDirection oldDirection, MapDirection newDirection)
        {
            base.OnMapDirectionChanged(oldDirection, newDirection);
            $"Direction changed: {oldDirection} -> {newDirection}".Dump();

            if (oldDirection != newDirection.Opposite)
                RotateParty(oldDirection, newDirection);
            else
            {
                var midle = oldDirection.NextClockWise;
                RotateParty(oldDirection, midle);
                RotateParty(midle, newDirection);
            }
        }

        protected override void OnLocationChanging(Tile oldLocation, Tile newLocation)
        {
            base.OnLocationChanging(oldLocation, newLocation);

            oldLocation?.OnObjectLeaving(this);
            newLocation?.OnObjectEntering(this);

            MovePartyToRight(newLocation);
        }

        private void MovePartyToRight(Tile newLocation)
        {
            if(!newLocation.LayoutManager.WholeTileEmpty)
                throw  new InvalidOperationException();

            foreach (var champion in PartyGroup)
            {
                var prevLocation = champion.Location;
                newLocation.LayoutManager.TryGetSpace(champion, prevLocation.Space);
                champion.Location = new FourthSpaceRouteElement(prevLocation.Space, newLocation);
                prevLocation.Tile.LayoutManager.FreeSpace(champion, prevLocation.Space);
            }
        }

        protected override void OnLocationChanged(Tile oldLocation, Tile newLocation)
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



        void IItem.Update(GameTime gameTime)
        {
            /*Do NOT update fromtile, because Theron is component*/
            foreach (var champoin in PartyGroup)
            {
                champoin.Update(gameTime);
            }
        }

        public bool ThrowOutItem(uint distance = 0)
        {
            if (Hand != null)
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

            if (Game.IsActive && Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released
                || Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
            {
                var tiles = new List<Tile> { Location };
                var aimingTile = Location.Neighbours.GetTile(MapDirection);
                if (aimingTile != null)
                    tiles.Add(aimingTile);

                var intersectingItems = new List<Tuple<IItem, Tile>>();

                Tuple<IItem, Tile> closest = null;
                float closestDistance = float.MaxValue;

                foreach (var tile in tiles)
                    foreach (var item in tile.SubItems)
                    {
                        float? res = Ray.Intersects(item.Bounding);
                        if (res != null)
                        {
                            intersectingItems.Add(new Tuple<IItem, Tile>(item, tile));
                            if (res.Value < closestDistance)
                            {
                                closest = new Tuple<IItem, Tile>(item, tile);
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
                else
                {
                    Fight();
                }
            }

            prevMouse = Mouse.GetState();
            prevKeyboard = Keyboard.GetState();
        }

        public IEntity GetEnemy(IEntity champoin)
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
                ((Creature) enemy)?.Kill();
            }
        }

        public void Draw(BasicEffect effect)
        {

            foreach (var champoin in PartyGroup)
            {
                throw new NotImplementedException();
                //champoin.GraphicsProvider?.Draw(effect);

            }
        }
    }
}