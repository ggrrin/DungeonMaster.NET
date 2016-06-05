using System;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ActionState = DungeonMasterEngine.DungeonContent.Actuators.ActionState;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class Door : Door<Message>
    {
        public Door(DoorInitializer initializer) : base(initializer) {}
    }

    public class DoorInitializer : FloorInitializer
    {
        public new event Initializer<DoorInitializer> Initializing;

        public MapDirection Direction { get; set;  }
        public bool Open { get; set; }

        //...
        //TODO
    }


    public class Door<TMessage> : Floor<TMessage> where TMessage : Message
    {
        private readonly GraphicsCollection graphics;

        private ModelGraphic doorFrame;

        private Items.DoorItem doorItem;

        public bool HasButton => doorItem.HasButton;

        public sealed override bool ContentActivated
        {
            get
            {
                return !doorItem.Visible;
            }

            protected set
            {
                doorItem.Visible = !value;
            }
        }

        public void oor(Vector3 position, bool isWestEast, bool isOpen, Items.DoorItem doorItem) 
        {
            doorFrame = new ModelGraphic();
            doorFrame.Model = doorFrame.Resources.Content.Load<Model>("Models/outterDoor");
            doorFrame.Rotation = isWestEast ? new Vector3(0, MathHelper.PiOver2, 0) : Vector3.Zero;
            doorFrame.Position = position + (isWestEast ? new Vector3(0.4f, 0, 0) : new Vector3(0, 0, 0.4f));
            doorFrame.Scale = new Vector3(1, 0.98f, 0.2f);

            this.doorItem = doorItem;
            doorItem.Graphic.Rotation = isWestEast ? new Vector3(0, MathHelper.PiOver2, 0) : Vector3.Zero;
            doorItem.Position = position + new Vector3((1 - doorItem.Size.X) / 2f, 0, (1 - doorItem.Size.Z) / 2f);
            SubItems.Add(doorItem);

            graphics.SubDrawable.Add(doorItem);

            ContentActivated = isOpen;

            if (doorItem.HasButton)
            {
                Vector3 shift = !isWestEast ? new Vector3(0, 0, 0.4f) : new Vector3(0.4f, 0, 0);
                //TODO
                //var t = new SwitchActuator(position + new Vector3(0, 0.2f, 0) + shift, this, new ActionStateX(ActionState.Toggle, 0, isOnceOnly: false));
                //SubItems.Add(t);
            }
        }


        public override bool IsAccessible => ContentActivated;

        public Door(DoorInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;

        }

        private void Initialize(DoorInitializer initializer)
        {
            //TODO initialize
            initializer.Initializing -= Initialize;
        }
    }
}
