using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.Builders.WallActuatorFactories;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
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
                new OnceOnlyExchangerFactory(), 
                new BasicKeyHoleFactory(), 
                new DestroyingKeyHoleFactory(), 
                new ChampoinFactory(), 
                new LeverSwitchFactory(), 
                new TimerSwitchFactory(), 
                new HolderButtonFactory(), 
                new ButtonFactory(), 
                new TimerMultiSwitchFactory(), 
                new MultiKeyHoleFactory(), 
            });
        }

        public void CreateSetupActuators(Tile currentTile)
        {
            CurrentTile = currentTile;

            var sides = 
                MapDirection.AllSides
                .Except(currentTile.Neighbours.Select(t => t.Item2))//sides with walls
                .Select(side =>
                {
                    var pos = CurrentTile.GridPosition + side;
                    var wall = builder.CurrentMap.GetTileData(pos); //get appropriate WallData
                    return wall == null ? null : new Tuple<TileData,Point,IReadOnlyList<ActuatorItemData>>(wall,pos,
                        wall.Actuators.Where(x => x.TilePosition == side.Opposite.ToTilePosition())
                        .ToArray());//select appropriate side
                })
                .Where(x => x != null );//filter map border nonexisting tiles

            foreach (var tuple in sides)
            {
                SetupTags(tuple.Item1, tuple.Item2);
                SetupWallSideActuators(tuple.Item1.GrabableItems, tuple.Item3);
            }
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

        private void SetupWallSideActuators(IEnumerable<GrabableItemData> items, IReadOnlyList<ActuatorItemData> actuators)
        {
            if (actuators.Any())
            {
                CurrentGrabableItems = items;

                var factory = parser.TryMatchFactory(actuators, items.Any());
                if (factory != null)
                {
                    CurrentTile.SubItems.Add(factory.CreateItem(builder, CurrentTile, actuators));
                }
                else
                {
                    if(actuators.All(x => x.ActuatorType != 5 && x.ActuatorType != 6))
                    {
                        foreach (var i in actuators)
                        {
                            Point? absolutePosition = null;
                            if (i.ActionLocation is RemoteTarget)
                                absolutePosition = ((RemoteTarget) i.ActionLocation).Position.Position.ToAbsolutePosition(builder.CurrentMap);

                            CurrentTile.SubItems.Add(new Actuator(builder.GetWallPosition(i.TilePosition, CurrentTile), $"{absolutePosition} {i.DumpString()}"));
                        }
                    }
                    else
                    {
                        
                    }
                }
            }
        }
    }
}
