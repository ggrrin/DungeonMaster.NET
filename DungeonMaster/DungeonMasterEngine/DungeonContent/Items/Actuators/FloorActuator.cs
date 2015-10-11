using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public enum ActionState
    {
        Set = 0, Clear, Toggle, Hold
    }

    public abstract class FloorActuator : Actuator
    {
        public Tile CurrentTile { get; }
        public Tile TargetTile { get; }
        public bool Enabled { get; private set; } = true;
        public ActionState Action { get; }
        protected bool objectEntered;

        public FloorActuator(Vector3 position, Tile currentTile, Tile targetTile, ActionState action) : base(position)
        {
            CurrentTile = currentTile;
            TargetTile = targetTile;
            Action = action;

            currentTile.ObjectEntered += CurrentTile_ObjectEntered;
            currentTile.ObjectLeft += CurrentTile_ObjectLeft;
        }

        private void CurrentTile_ObjectLeft(object sender, object e)
        {
            objectEntered = false;
            if (Enabled) TestAndRun(e, false);
        }

        private void CurrentTile_ObjectEntered(object sender, object e)
        {
            objectEntered = true;
            if (Enabled) TestAndRun(e, true);
        }

        protected abstract void TestAndRun(object enteringObject, bool objectEntered);

        protected void AffectTile()
        {
            if (TargetTile == null)
                return;

            switch (Action)
            {
                case ActionState.Set:
                    if (objectEntered)
                        TargetTile.ActivateTileContent();
                    break;

                case ActionState.Clear:
                    if (objectEntered)
                        TargetTile.DeactivateTileContent();
                    break;

                case ActionState.Toggle:
                    if (objectEntered)
                    {
                        if (TargetTile.ContentActivated)
                            TargetTile.DeactivateTileContent();
                        else
                            TargetTile.ActivateTileContent();
                    }
                    break;
                case ActionState.Hold:
                    if (objectEntered)
                    {
                        if (TargetTile.ContentActivated)
                            TargetTile.DeactivateTileContent();
                        else
                            TargetTile.ActivateTileContent();
                    }
                    else
                    {
                        if (TargetTile.ContentActivated)
                            TargetTile.DeactivateTileContent();
                        else
                            TargetTile.ActivateTileContent();
                    }
                    break;
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()} {GetType().Name} action: {Action}";
        }

    }
}

