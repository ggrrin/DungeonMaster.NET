using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.Builders;
using DungeonMasterEngine.Builders.WallActuatorFactories;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class Parser<TState, TStateData, TDataContext, TFactoryResult> where TState : IEquatable<TStateData>
    {
        public IEnumerable<FactoryBase<TState, TStateData, TDataContext, TFactoryResult>> Factories { get; }

        public Parser(IEnumerable<FactoryBase<TState, TStateData, TDataContext, TFactoryResult>> factories)
        {
            Factories = factories;
        }

        public FactoryBase<TState, TStateData, TDataContext, TFactoryResult> TryMatchFactory(IEnumerable<TStateData> sequence)
        {
            if (!sequence.Any())
                return null;

            return Factories.SingleOrDefault(x => x.MatchLine(sequence)); //TODO optimize
        }
    }
}
