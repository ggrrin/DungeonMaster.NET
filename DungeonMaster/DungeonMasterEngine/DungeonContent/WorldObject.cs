using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterEngine
{
    public abstract class WorldObject
    {
        public WorldObject(Vector3 position)
        {
            Position = position;
        }

        public abstract IGraphicProvider GraphicsProvider { get; }

        public virtual Vector3 Position { get; set; }
    }
}
