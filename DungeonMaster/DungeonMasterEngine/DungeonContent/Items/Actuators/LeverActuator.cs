using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class LeverActuator : Actuator
    {
        private bool used = false;

        public bool Activated { get; private set; }

        public bool OnceOnly { get; }

        private Texture2D upTexture;

        public Texture2D UpTexture
        {
            get { return upTexture; }
            set
            {
                upTexture = value;
                UpdateTextures();
            }
        }

        private void UpdateTextures()
        {
            ((CubeGraphic)Graphics).Texture = Activated ? downTexture : upTexture;
        }

        private Texture2D downTexture;

        public Texture2D DownTexture
        {
            get { return downTexture; }
            set
            {
                downTexture = value;
                UpdateTextures();
            }
        }

        public Tile TargetTile { get; set; }
        public ActionState Action { get; private set; }
        public DungeonMasterParser.Direction ActionDirection { get; private set; }

        public LeverActuator(Vector3 position, Tile targetTile, bool onceOnly, ActionState action, DungeonMasterParser.Direction actionDirection) : base(position)
        {
            TargetTile = targetTile;
            OnceOnly = onceOnly;
            Action = action;
            ActionDirection = actionDirection;
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if (OnceOnly && !used)
                Switch();
            else if (!OnceOnly)
                Switch();

            used = true;
            return base.ExchangeItems(item);
        }

        private void Switch()
        {
            Activated ^= true;
            UpdateTextures();
            if (Activated)
                Activate();
            else
                Deactivate();

            TargetTile.ExecuteContentActivator(new LogicTileActivator((int)ActionDirection, Action));
        }

        private void Deactivate()
        {
            TargetTile.DeactivateTileContent();
        }

        private void Activate()
        {
            TargetTile.ActivateTileContent();
        }
    }


    public class LogicTileActivator : TileContentActivator
    {
        public int BitIndex { get; }
        public ActionState BitAction { get; }

        public LogicTileActivator(int bitIndex, ActionState bitAction)
        {
            BitIndex = bitIndex;
            BitAction = bitAction;
        }

        public override void ActivateContent(LogicTile t)
        {
            foreach (var gate in t.Gates)
            {
                switch (BitAction)
                {
                    case ActionState.Clear:
                        gate[BitIndex] = false;
                        break;
                    case ActionState.Set:
                        gate[BitIndex] = true;
                        break;
                    case ActionState.Toggle:
                        gate[BitIndex] ^= true;
                        break;
                    case ActionState.Hold:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
