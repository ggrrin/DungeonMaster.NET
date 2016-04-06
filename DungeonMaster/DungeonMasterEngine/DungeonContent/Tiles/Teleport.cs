using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
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
        public IConstrain ScopeConstrain { get; }

        public bool Visible { get; } //TODO use this value

        public bool IsOpen { get; }

        public Tile NextLevelEnter { get; set; }

        public int NextLevelIndex { get; }

        public Point TargetTilePosition { get; }

        public Teleport(Vector3 position, int targetMapIndex, Point targetGridPosition, bool teleportOpen, bool teleportVisible, IConstrain scopeConstrain) : base(position)
        {
            NextLevelIndex = targetMapIndex;
            TargetTilePosition = targetGridPosition;
            IsOpen = teleportOpen;
            Visible = teleportVisible;
            ScopeConstrain = scopeConstrain;

            InitializeGraphics();
        }

        private void InitializeGraphics()
        {
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
