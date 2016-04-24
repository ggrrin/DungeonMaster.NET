using System;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using System.Linq;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public class FloorActuator : RemoteActuator
    {
        public override bool Activated => CurrentTile.SubItems.Any(Constrain.IsAcceptable);

        public Tile CurrentTile { get; }

        public override ActionStateX TargetAction { get; }

        public bool Enabled { get; private set; } = true;

        public virtual IConstrain Constrain { get; }

        public FloorActuator(Vector3 position, Tile currentTile, Tile targetTile, IConstrain constrain, ActionStateX action) : base(targetTile, position)
        {
            CurrentTile = currentTile;
            TargetAction = action;
            Constrain = constrain;

            currentTile.ObjectEntered += CurrentTile_ObjectEntered;
            currentTile.ObjectLeft += CurrentTile_ObjectLeft;
        }

        private void CurrentTile_ObjectLeft(object sender, object e)
        {
            if (Enabled)
                TestAndRun(e);
        }

        private void CurrentTile_ObjectEntered(object sender, object e)
        {
            if (Enabled)
                TestAndRun(e);
        }

        protected virtual void TestAndRun(object enteringObject)
        {
            if (CurrentTile.SubItems.Any(Constrain.IsAcceptable))
            {
                SendMessage();
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()} {GetType().Name} action: {TargetAction}\r\n {Constrain}";
        }
    }
}

