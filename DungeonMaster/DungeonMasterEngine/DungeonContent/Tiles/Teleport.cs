using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterEngine.Tiles
{
    class Teleport : Floor, ILevelConnector
    {
        public Teleport(Vector3 position, int targetMapIndex, Point targetGridPosition) : base(position)
        {
            NextLevelIndex = targetMapIndex;
            TargetTilePosition = targetGridPosition;
        }

        public Tile NextLevelEnter { get; set; }

        public int NextLevelIndex { get; }

        public Point TargetTilePosition { get; }

        public override void OnObjectEntered(object obj)
        {
            base.OnObjectEntered(obj);
            var theron = obj as Theron;
            if(theron != null)
            {
                theron.Location = NextLevelEnter; //TODO rename in interface property
            }
        }
    }
}
