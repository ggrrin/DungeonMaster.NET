using DungeonMasterEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Items;
using DungeonMasterEngine.DungeonContent.Items.Actuators;

namespace DungeonMasterEngine.Tiles
{
    public class Gateway : Floor
    {
        private GraphicsCollection graphics;

        private ModelGraphic doorFrame;

        private Door door;

        private bool IsOpen
        {
            get { return !door.Visible; }
            set
            {
                door.Visible = !value;
            }
        }

        public Gateway(Vector3 position, bool isWestEast, bool isOpen, Door door) : base(position)
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

            IsOpen = isOpen;

            if (door.HasButton)
            {
                Vector3 shift = !isWestEast ? new Vector3(0,0, 0.4f) : new Vector3(0.4f, 0, 0);
                var t = new SwitchActuator(position + new Vector3(0, 0.2f, 0)+shift, this, isOpen);
                //t.Position = position + new Vector3(0, 0.4f, 0);
                SubItems.Add(t);
            }
        }

        public override void ActivateTileContent()
        {
            base.ActivateTileContent();
            IsOpen = true;
        }

        public override void DeactivateTileContent()
        {
            base.DeactivateTileContent();
            IsOpen = false;
        }

        public override bool IsAccessible =>  IsOpen;
    }
}
