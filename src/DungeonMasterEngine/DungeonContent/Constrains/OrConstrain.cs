using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Constrains
{
    internal class OrConstrain : IConstrain
    {
        public IEnumerable<IConstrain> SubConstrains { get; }
        
        public OrConstrain(IEnumerable<IConstrain> subconstrains)
        {
            SubConstrains = subconstrains;
        }

        public bool IsAcceptable(object item)
        {
            return null != SubConstrains.Where(x => x.IsAcceptable(item)).FirstOrDefault();
        }

        public override string ToString()
        {
            return string.Concat(SubConstrains.Select(x => x.GetType().Name + "\r\n"));
        }
    }
}