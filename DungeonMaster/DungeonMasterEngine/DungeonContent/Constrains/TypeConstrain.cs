using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterEngine.Items
{
    class TypeConstrain : IConstrain
    {
        public Type AcceptableType { get; }

        public bool AccepChild { get; }

        public TypeConstrain(Type acceptableType, bool acceptChild = true)
        {
            AcceptableType = acceptableType;
            AccepChild = acceptChild;
        }

        public bool IsAcceptable(object item)
        {
            if (item == null)
                return true;

            if (AccepChild)
                return AcceptableType.IsAssignableFrom(item.GetType());
            else
                return item.GetType() == AcceptableType;
        }

        public override string ToString()
        {
            return $"T:{AcceptableType.Name}\r\n->child:{AccepChild}";
        }
    }
}
