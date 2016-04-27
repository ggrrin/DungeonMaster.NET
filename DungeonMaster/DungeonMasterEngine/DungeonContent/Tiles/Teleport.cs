using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class Teleport : Floor, ILevelConnector
    {
        public IConstrain ScopeConstrain { get; }

        public bool Visible { get; } //TODO use this value

        public bool IsOpen => ContentActivated;

        public override bool ContentActivated
        {
            get
            {
                return base.ContentActivated;
            }

            protected set
            {
                base.ContentActivated = value;
                if(IsOpen)
                {
                    foreach (var i in SubItems.Where(x => !(x is Actuator)).ToArray())
                        TeleportItem(i);
                }
                InitializeGraphics(reinit:true);

            }
        }

        public Tile NextLevelEnter { get; set; }

        public int NextLevelIndex { get; }

        public Point TargetTilePosition { get; }

        public Teleport(Vector3 position, int targetMapIndex, Point targetGridPosition, bool teleportOpen, bool teleportVisible, IConstrain scopeConstrain) : base(position)
        {
            InitializeGraphics();
            NextLevelIndex = targetMapIndex;
            TargetTilePosition = targetGridPosition;
            ContentActivated = teleportOpen;
            Visible = teleportVisible;
            ScopeConstrain = scopeConstrain;

        }

        private void InitializeGraphics(bool reinit =false)
        {
            if(reinit)
                graphicsProviders.SubProviders.RemoveAt(graphicsProviders.SubProviders.Count-1);

            graphicsProviders.SubProviders.Add(
            new CubeGraphic
            {
                DrawFaces = CubeFaces.All,
                Outter = false,
                Scale = new Vector3(0.5f, 0.5f, 0.5f),
                Texture = DrawRenderTarget(),
                Position = Position
            });
        }

        private Texture2D DrawRenderTarget()
        {
            var device = ResourceProvider.Instance.Device;
            RenderTarget2D target = new RenderTarget2D(device, 128, 128);
            SpriteBatch spriteBatch = new SpriteBatch(device);

            // Set the device to the render target
            device.SetRenderTarget(target);

            device.Clear(Color.Gray);

            spriteBatch.Begin();

            spriteBatch.DrawString(ResourceProvider.Instance.DefaultFont, $"open:{IsOpen};\r\nvisible:{Visible}\r\nscp:{ScopeConstrain}\r\n{TargetTilePosition}\r\n{NextLevelIndex}", new Vector2(10), Color.White);
            spriteBatch.End();

            // Reset the device to the back buffer
            device.SetRenderTarget(null);

            return target;
        }



        public override void OnObjectEntered(object obj)
        {
            base.OnObjectEntered(obj);
            TeleportItem(obj);
        }


        private void TeleportItem(object obj)
        {
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
