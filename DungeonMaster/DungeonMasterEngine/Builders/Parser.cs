using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.Builders.WallActuatorFactories;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.Builders
{
    public class Parser<TState, TStateData, TDataContext, TFactoryResult> where TState : IEquatable<TStateData>
    {
        public IEnumerable<FactoryBase<TState, TStateData, TDataContext, TFactoryResult>> Factories { get; }

        public Parser(IEnumerable<FactoryBase<TState, TStateData, TDataContext, TFactoryResult>> factories)
        {
            Factories = factories;
        }

        public FactoryBase<TState, TStateData, TDataContext, TFactoryResult> TryMatchFactory(IEnumerable<TStateData> sequence, bool hasItems)
        {
            if (!sequence.Any())
                return null;

            return Factories.SingleOrDefault(x => x.RequireItem.OptionalyEquals(hasItems) && x.MatchLine(sequence)); //TODO optimize
        }
    }
}
