using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.Builders.WallActuatorFactories;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DungeonMasterParser.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    public class WallActuatorCreator
    {
        private readonly LegacyMapBuilder builder;
        private readonly Parser<ActuatorState, ActuatorItemData, LegacyMapBuilder, Actuator> parser;

        public Tile CurrentTile { get; private set; }
        public IEnumerable<GrabableItemData> CurrentGrabableItems { get; private set; }

        public WallActuatorCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
            parser = new Parser<ActuatorState, ActuatorItemData, LegacyMapBuilder, Actuator>(new ActuatorFactoryBase[]
            {
                //TODO add factories
                new AlcoveHidingSwitchFactory(), 
                new BasicAlcoveFactory(),
                new BasicDecorationFactory(),
                new BasicExchangerFactory(),
                new BasicExchangerFactoryReverse(), 
                new BasicKeyHoleFactory(), 
                new DestroyingKeyHoleFactory(), 
                new ChampoinFactory(), 
                new LeverSwitchFactory(), 
            });
        }

        private void SetupTags(TileData wall, Point textTagTilePosition)
        {
            foreach (var textTag in wall.TextTags.Where(x => !x.Processed && x.GetParentPosition(textTagTilePosition) == CurrentTile.GridPosition))
            {
                textTag.Processed = true;
                var tag = new TextTag(builder.GetWallPosition(textTag.TilePosition, CurrentTile), textTag.IsVisible,
                    textTag.TilePosition == TilePosition.East_TopRight || textTag.TilePosition == TilePosition.West_BottomRight, textTag.Text.Replace("|", Environment.NewLine))
                {
                    AcceptMessages = textTag.HasTargetingActuator
                };
                CurrentTile.SubItems.Add(tag);
            }
        }

        public void CreateSetupActuators(Tile currentTile)
        {
            CurrentTile = currentTile;

            var sides = currentTile.Neighbours
                .Where(n => n.Key == null) //only side where is a wall
                .Select(n =>
                {
                    var pos = CurrentTile.GridPosition + n.Value;
                    var wall = builder.CurrentMap.GetTileData(pos); //get appropriate WallData
                    return wall == null ? null : new Tuple<TileData,Point,IReadOnlyList<ActuatorItemData>>(wall,pos,
                        wall.Actuators.Where(x => x.TilePosition == (new Point(-1) * n.Value).ToTilePosition()).ToArray());//select appropriate side
                })
                .Where(x => x != null );//filter border nonexisting tiles

            foreach (var tuple in sides)
            {
                SetupTags(tuple.Item1, tuple.Item2);
                SetupWallSideActuators(tuple.Item1.GrabableItems, tuple.Item3);
            }
        }

        private void SetupWallSideActuators(IEnumerable<GrabableItemData> items, IReadOnlyList<ActuatorItemData> actuators)
        {
            if (actuators.Any())
            {
                CurrentGrabableItems = items;

                var factory = parser.TryMatchFactory(actuators);
                if (factory != null)
                {
                    CurrentTile.SubItems.Add(factory.CreateItem(builder, CurrentTile, actuators));
                }
                else
                {
                    foreach (var i in actuators)
                    {
                        Point? absolutePosition = null;
                        if (i.ActLoc is RmtTrg)
                            absolutePosition = ((RmtTrg) i.ActLoc).Position.Position.ToAbsolutePosition(builder.CurrentMap);

                        CurrentTile.SubItems.Add(new Actuator(builder.GetWallPosition(i.TilePosition, CurrentTile), $"{absolutePosition} {i.DumpString()}"));
                    }
                }
            }
        }
    }
}
