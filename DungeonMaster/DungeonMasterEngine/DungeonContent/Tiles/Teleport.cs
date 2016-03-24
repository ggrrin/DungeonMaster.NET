using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Items;
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
    public class Teleport : Floor, ILevelConnector
    {
        public Teleport(Vector3 position, int targetMapIndex, Point targetGridPosition, bool teleportOpen, bool teleportVisible, IConstrain scopeConstrain) : base(position)
        {
            NextLevelIndex = targetMapIndex;
            TargetTilePosition = targetGridPosition;
            IsOpen = teleportOpen;
            Visible = teleportVisible;
            ScopeConstrain = scopeConstrain;
        }

        public IConstrain ScopeConstrain { get; }

        public bool Visible { get; } //TODO user this value

        public bool IsOpen { get; }

        public Tile NextLevelEnter { get; set; }

        public int NextLevelIndex { get; }

        public Point TargetTilePosition { get; }

        public override void OnObjectEntered(object obj)
        {
            base.OnObjectEntered(obj);

            if (IsOpen && ScopeConstrain.IsAcceptable(obj))
            {
                var item = obj as ILocalizable<Tile>; 
                if (item != null)
                {
                    item.Location = NextLevelEnter; //TODO rename in interface property
                }


            }
        }
    }
}
