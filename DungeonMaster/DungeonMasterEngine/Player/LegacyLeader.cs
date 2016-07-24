using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.Builders.ActuatorCreators;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actions;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Player
{
    public class LegacyLeader : PointOfViewCamera, ILeader
    {
        private readonly IFactories factorie;
        private MouseState prevMouse = Mouse.GetState();
        private KeyboardState prevKeyboard;

        private readonly List<Champion> partyGroup = new List<Champion>();

        public IReadOnlyList<Champion> PartyGroup => partyGroup;


        public object Interactor => Ray;

        IReadOnlyList<ILiveEntity> ILeader.PartyGroup => PartyGroup;

        public IGrabableItem Hand { get; set; }

        public ILiveEntity Leader => PartyGroup.FirstOrDefault();
        public bool Enabled { get; set; } = true;
        public ushort MagicalLightAmount { get; set; }

        public LegacyLeader(IFactories factorie)
        {
            this.factorie = factorie;
        }

        private void InitMocap()
        {
            var builder = new ChampionMocapCreator();
            var x = new[]
            {
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

        protected override void OnLocationChanged(ITile oldLocation, ITile newLocation, bool moved)
        {
            base.OnLocationChanged(oldLocation, newLocation, moved);
            if (!moved)
            {
                foreach (var champion in PartyGroup)
                    champion.Location = champion.Location.GetNew(newLocation);
            }
        }

        protected override bool CanMoveToTile(ITile tile) => PartyGroup.All(ch => !ch.Moving) && base.CanMoveToTile(tile) && tile.LayoutManager.WholeTileEmpty;

        protected void RotateParty(MapDirection oldDirection, MapDirection newDirection)
        {
            //$"Direction changed: {oldDirection} -> {newDirection}".Dump();

            if (oldDirection != newDirection.Opposite)
            {
                RotatePartyPiOver2(oldDirection, newDirection);

                foreach (var champion in PartyGroup)
                    champion.MapDirection = MapDirection;
            }
            else
            {
                var midle = oldDirection.NextClockWise;
                RotatePartyPiOver2(oldDirection, midle);


                foreach (var champion in PartyGroup)
                    champion.MapDirection = midle;
            }

        }

        public override async Task MoveToAsync(ISpaceRouteElement newLocation)
        {
            var b = base.MoveToAsync(newLocation);
            var p = MovePartyAsync(newLocation.Tile);
            await Task.WhenAll(p, b);
        }


        protected virtual async Task MovePartyAsync(ITile newTile)
        {
            if (!newTile.LayoutManager.WholeTileEmpty)
                throw new InvalidOperationException();

            await Task.WhenAll(PartyGroup.Select(ch => ch.MoveToAsync(ch.Location.GetNew(newTile))));
        }


        protected virtual void RotatePartyPiOver2(MapDirection oldDirection, MapDirection newDirection)
        {
            var targetLocation = partyGroup.FirstOrDefault()?.Location?.Tile;

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
                    champoin.MoveToWithoutFree(new FourthSpaceRouteElement(newSpace, targetLocation), true);
                }
            }
        }

        public virtual bool AddChampoinToGroup(ILiveEntity entity)
        {
            var champion = entity as Champion;
            if (champion == null || partyGroup.Count == 4)
                return false;

            var freeSpace = Small4GroupLayout.Instance.AllSpaces.Except(partyGroup.Select(ch => ch.Location?.Space).Where(x => x != null)).First();
            champion.Location = new FourthSpaceRouteElement(freeSpace, Location.Tile);
            champion.MapDirection = MapDirection;
            partyGroup.Add(champion);
            champion.Died += (sender, deadChampion) => partyGroup.Remove(deadChampion);

            return true;
        }

        public override void Update(GameTime time)
        {
            if (!Enabled)
                return;

            base.Update(time);

            if (PartyGroup.Any() && PartyGroup.All(ch => !ch.Moving && ch.MapDirection != MapDirection))
            {
                RotateParty(partyGroup.First().MapDirection, MapDirection);
            }

            foreach (var champoin in PartyGroup)
            {
                champoin.Update(time);
            }

            if (IsActive && Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released
                || Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
            {
                var tiles = new[] { Location.Tile }.Concat(Location.Tile.Neighbors
                    .Select(x => x.Item1))
                    .ToArray();

                var matrix = Matrix.Identity;
                var anyTriggered = false;
                foreach (var tile in tiles)
                    if (tile.Renderer.Interact(this, ref matrix, null))
                    {
                        anyTriggered = true;
                        break;
                    }

                if (!anyTriggered && Hand != null)
                    ThrowItem();
            }

            prevMouse = Mouse.GetState();
            prevKeyboard = Keyboard.GetState();
        }

        protected void ThrowItem()
        {
            var storageType = ActionHandStorageType.Instance;
            var actionHand = Leader.Body.GetBodyStorage(storageType);
            var item = actionHand.TakeItemFrom(0);
            actionHand.AddItemTo(Hand, 0);
            Hand = null;

            var action = new ThrowAttack((ThrowActionFactory)factorie.FightActions[42], Leader, storageType);
            action.Apply(MapDirection);

            if (item != null)
                actionHand.AddItemTo(item, 0);
        }

        public bool IsActive => true;

        public virtual void Draw(BasicEffect effect)
        {
            Vector2 statisticPosition = new Vector2(600, 0);
            foreach (var champion in PartyGroup)
            {
                var mat = Matrix.Identity;
                champion.Renderer.Render(ref mat, effect, null);

                DrawChampionStatistic(champion, statisticPosition);
                statisticPosition.Y += 80;
            }

        }

        private void DrawChampionStatistic(Champion champion, Vector2 position)
        {
            var batcher = GameConsole.Instance?.Batcher;
            var whiteTexture = GameConsole.Instance?.WhiteTexture;
            var font = ResourceProvider.Instance.DefaultFont;

            var properties = new IPropertyFactory[]
            {
                PropertyFactory<HealthProperty>.Instance,
                PropertyFactory<StaminaProperty>.Instance,
                PropertyFactory<ManaProperty>.Instance,
            };

            var str = champion.Name + Environment.NewLine + string.Join(Environment.NewLine, properties.Select(pf =>
            {
                var p = champion.GetProperty(pf);
                return $"{p.GetType().Name}: {p.Value}/{p.MaxValue}";
            }));

            batcher.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            var rec = new Rectangle(position.ToPoint(), new Point(200, 80));
            var c = new Color(Color.Black, 0.5f);
            batcher.Draw(whiteTexture, rec, c);
            batcher.DrawString(font, str, position, Color.White);

            batcher.End();
        }

    }
}