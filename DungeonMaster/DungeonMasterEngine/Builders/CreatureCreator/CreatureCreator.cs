using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders.CreatureCreator
{
    internal class CreatureCreator
    {
        private readonly LegacyMapBuilder builder;

        public CreatureCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        private IEnumerable<Tuple<TilePosition, int, IGrabableItem[]>> GetCreatures(CreatureItem creatureData)
        {
            int creatureCount = creatureData.CreaturesCount + 1;
            var possesions = GetPossesions(creatureData, creatureCount);
            return creatureData.GetCreaturesInfo().Zip(possesions, (x,y) => Tuple.Create(x.Item1, x.Item2, y));
        }

        public virtual IEnumerable<Creature> AddCreature(CreatureItem creatureData, Tile tile, Point position)
        {
            var factory = builder.Factories.CreatureFactories[(int)creatureData.Type];

            var res = new List<Creature>();

            foreach (var tuple in GetCreatures(creatureData))
            {
                var space = factory.Layout.AllSpaces.First(s => tuple.Item1.ToDirections().All(s.Sides.Contains));

                var creature = factory.Create(new CreatureInitializer
                {
                    HitPoints = tuple.Item2,
                    Location = factory.Layout.GetSpaceElement(space, tile),
                    RelationToken = builder.CreatureToken,
                    EnemiesTokens = builder.ChampionToken.ToEnumerable(),
                    PossessionItems = tuple.Item3
                });
                creature.Renderer = builder.RendererSource.GetCreatureRenderer(creature, factory.Texture);

                res.Add(creature);
            }

            return res;
        }

        private IGrabableItem[][] GetPossesions(CreatureItem creatureData, int creatureCount)
        {
            var possessions = creatureData.PossessionItems.Select(x => builder.ItemCreator.CreateItem(x));
            var que = new Queue<IGrabableItem>(possessions);
            var possesionsCountForCreature = que.Count / creatureCount;

            return Enumerable.Range(0, creatureCount).Select((j, x) =>
            {
                if (j < creatureCount - 1)
                    return Enumerable.Range(0, possesionsCountForCreature).Select(zz => que.Dequeue()).ToArray();
                else
                    return que.ToArray();
            })
            .ToArray();
        }
    }
}