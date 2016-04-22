using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public abstract class FloorActuator : RemoteActuator
    {
        protected bool objectEntered;

        public Tile CurrentTile { get; }

        public bool Enabled { get; private set; } = true;

        public FloorActuator(Vector3 position, Tile currentTile, Tile targetTile, ActionStateX action) : base(targetTile, action, position)
        {
            CurrentTile = currentTile;

            currentTile.ObjectEntered += CurrentTile_ObjectEntered;
            currentTile.ObjectLeft += CurrentTile_ObjectLeft;
        }

        private void CurrentTile_ObjectLeft(object sender, object e)
        {
            objectEntered = false;
            if (Enabled)
                TestAndRun(e, objectEntered);
        }

        private void CurrentTile_ObjectEntered(object sender, object e)
        {
            objectEntered = true;
            if (Enabled)
                TestAndRun(e, objectEntered);
        }

        protected abstract void TestAndRun(object enteringObject, bool objectEntered);

        protected void AffectTile()
        {
            if (TargetTile == null)//TODO is it necessary ? :O 
                return;

            if (objectEntered)
                SendMessage();

            //TODO does it work this way or as in SendMessage() ????
            //case ActionState.Hold:
            //    if (objectEntered)
            //    {
            //        if (TargetTile.ContentActivated)
            //            TargetTile.DeactivateTileContent();
            //        else
            //            TargetTile.ActivateTileContent();
            //    }
            //    else
            //    {
            //        if (TargetTile.ContentActivated)
            //            TargetTile.DeactivateTileContent();
            //        else
            //            TargetTile.ActivateTileContent();
            //    }
            //    break;
        }

        public override string ToString()
        {
            return $"{base.ToString()} {GetType().Name} action: {TargetAction}";
        }

    }
}

