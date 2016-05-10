using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Items;
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

        public sealed override bool ContentActivated
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
                Texture = ResourceProvider.Instance.DrawRenderTarget($"open:{IsOpen};\r\nvisible:{Visible}\r\nscp:{ScopeConstrain}\r\n{TargetTilePosition}\r\n{NextLevelIndex}", Color.Blue, Color.White),
                Position = Position
            });
        }





        public override void OnObjectEntered(IItem item)
        {
            base.OnObjectEntered(item);
            TeleportItem(item);
        }


        private void TeleportItem(IItem obj)
        {
            if (IsOpen && ScopeConstrain.IsAcceptable(obj) && NextLevelEnter != null)//TODO how to set taget location creatures
            {
                obj.Location = NextLevelEnter; //TODO rename in interface property
            }
        }

    }
}
