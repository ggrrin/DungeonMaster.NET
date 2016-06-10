using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface ILocationHolder<in TSourceConstrain, in TRequestor> 
    {
        IList<TSource> Intersects<TSource>(TRequestor requestor, IEnumerable<TSource> items ) where TSource : TSourceConstrain;
    }
}