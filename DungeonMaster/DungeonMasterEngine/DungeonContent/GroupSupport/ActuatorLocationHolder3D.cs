using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class ActuatorLocationHolder3D : ILocationHolder<Item,Ray>
    {
        public IList<TSource> Intersects<TSource>(Ray requestor, IEnumerable<TSource> items) where TSource : Item
        {
            throw new NotImplementedException();
        }
    }
}