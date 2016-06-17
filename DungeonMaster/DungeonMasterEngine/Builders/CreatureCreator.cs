using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders
{
    public class CreatureFactory
    {
        public Texture2D Texture { get; }
        public string Name { get; }
        public IGroupLayout Layout { get; }
        public int MoveDuration { get; }
        public int DetectRange { get; }
        public int SightRange { get; }
        public int Experience { get; }

        public CreatureFactory(IGroupLayout layout, string name, int moveDuration, int detectRange, int sightRange, int experience, Texture2D texture)
        {
            Layout = layout;
            MoveDuration = moveDuration;
            Texture = texture;
            DetectRange = detectRange;
            SightRange = sightRange;
            Name = name;
            Experience = experience;
        }


        public Creature Create<TCreatureInitializer>(TCreatureInitializer initiator) where TCreatureInitializer : ICreatureInitializer
        {
            return new Creature(initiator, this);
        }
    }

    public interface ICreatureInitializer
    {
        int HitPoints { get; set;  }
        ISpaceRouteElement Location { get; }
        RelationToken RelationToken { get; }
        IEnumerable<RelationToken> EnemiesTokens { get; }
    }

    class CreatureInitializer : ICreatureInitializer {
        public int HitPoints { get; set; }
        public ISpaceRouteElement Location { get; set; }
        public RelationToken RelationToken { get; set; }
        public IEnumerable<RelationToken> EnemiesTokens { get; set; }
    }

    internal class CreatureCreator
    {
        private readonly LegacyMapBuilder builder;

        public CreatureCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        public IEnumerable<Creature> AddCreature(CreatureItem creatureData, Tile tile)
        {
            var factory = builder.CreatureFactories[(int)creatureData.Type];

            var res = new List<Creature>();

            foreach (var tuple in creatureData.GetCreaturesInfo().Take(creatureData.CreaturesCount + 1))
            {
                var space = factory.Layout.AllSpaces.First(s => tuple.Item1.ToDirections().All(s.Sides.Contains));

                var creature = factory.Create(new CreatureInitializer
                {
                    HitPoints = tuple.Item2,
                    Location = factory.Layout.GetSpaceElement(space, tile),
                    RelationToken = builder.CreatureToken,
                    EnemiesTokens = builder.ChampionToken.ToEnumerable(),
                });
                creature.Renderer = builder.RendererSource.GetCreatureRenderer(creature, factory.Texture);

                res.Add(creature);
            }

            return res;
        }


    }
}