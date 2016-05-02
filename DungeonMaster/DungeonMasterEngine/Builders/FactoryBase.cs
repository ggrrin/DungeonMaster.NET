using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.Builders
{
    public abstract class FactoryBase<TState, TStateData, TDataContext, TFactoryResult> where TState : IEquatable<TStateData>
    {
        public abstract bool? RequireItem { get; }
        public abstract IReadOnlyList<TState> MatchingSequence { get; }

        public virtual bool MatchLine(IEnumerable<TStateData> sequence)
        {
            return MatchingSequence.SequenceSimilar(sequence);
        }

        public abstract TFactoryResult CreateItem(TDataContext context, Tile currentTile, IReadOnlyList<TStateData> matchedSequence);
    }
}