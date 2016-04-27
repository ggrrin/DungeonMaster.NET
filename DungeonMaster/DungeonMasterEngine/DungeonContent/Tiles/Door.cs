using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class Door : Floor
    {
        private readonly GraphicsCollection graphics;

        private readonly ModelGraphic doorFrame;

        private readonly Items.Door door;

        public bool HasButton => door.HasButton;

        public sealed override bool ContentActivated
        {
            get
            {
                return !door.Visible;
            }

            protected set
            {
                door.Visible = !value;
            }
        }

        public Door(Vector3 position, bool isWestEast, bool isOpen, Items.Door door) : base(position)
        {
            doorFrame = new ModelGraphic();
            doorFrame.Model = doorFrame.Resources.Content.Load<Model>("Models/outterDoor");
            doorFrame.Rotation = isWestEast ? new Vector3(0, MathHelper.PiOver2, 0) : Vector3.Zero;
            doorFrame.Position = position + (isWestEast ? new Vector3(0.4f, 0, 0) : new Vector3(0, 0, 0.4f));
            doorFrame.Scale = new Vector3(1, 0.98f, 0.2f);

            this.door = door;
            door.Graphic.Rotation = isWestEast ? new Vector3(0, MathHelper.PiOver2, 0) : Vector3.Zero;
            door.Position = position + new Vector3((1 - door.Size.X) / 2f, 0, (1 - door.Size.Z) / 2f);
            SubItems.Add(door);

            graphics = new GraphicsCollection(wallGraphic, doorFrame);
            graphics.SubDrawable.Add(door);
            graphicsProviders.SubProviders.Add(graphics);

            ContentActivated = isOpen;

            if (door.HasButton)
            {
                Vector3 shift = !isWestEast ? new Vector3(0, 0, 0.4f) : new Vector3(0.4f, 0, 0);
                var t = new SwitchActuator(position + new Vector3(0, 0.2f, 0) + shift, this, new ActionStateX(ActionState.Toggle, 0, isOnceOnly: false));
                SubItems.Add(t);
            }
        }


        public override bool IsAccessible => ContentActivated;
    }
}
