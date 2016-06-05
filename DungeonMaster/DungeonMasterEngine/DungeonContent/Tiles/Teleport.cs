using System;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class Teleport : Teleport<Message>
    {
        public Teleport(TeleprotInitializer initializer) : base(initializer) {}
    }

    public class TeleprotInitializer : FloorInitializer
    {
        public new event Initializer<TeleprotInitializer> Initializing;
        
    }

    public class Teleport<TMessage> : Floor<TMessage>, ILevelConnector where TMessage : Message
    {

        public Teleport(TeleprotInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;

        }

        private void Initialize(TeleprotInitializer initializer)
        {
            //tODO
            initializer.Initializing -= Initialize;
        }

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

        public MapDirection Direction { get; }

        public void xTeleport(Vector3 position, int targetMapIndex, Point targetGridPosition, MapDirection direction, bool teleportOpen, bool teleportVisible, IConstrain scopeConstrain) 
        {
            InitializeGraphics();
            //TODO uncoommment
            //NextLevelIndex = targetMapIndex;
            //TargetTilePosition = targetGridPosition;
            //Direction = direction;
            //ContentActivated = teleportOpen;
            //Visible = teleportVisible;
            //ScopeConstrain = scopeConstrain;

        }

        private void InitializeGraphics(bool reinit =false)
        {
            //if(reinit)
            //    graphicsProviders.SubProviders.RemoveAt(graphicsProviders.SubProviders.Count-1);

            //graphicsProviders.SubProviders.Add(
            //new CubeGraphic
            //{
            //    DrawFaces = CubeFaces.All,
            //    Outter = false,
            //    Scale = new Vector3(0.5f, 0.5f, 0.5f),
            //    Texture = ResourceProvider.Instance.DrawRenderTarget($"open:{IsOpen};\r\nvisible:{Visible}\r\nscp:{ScopeConstrain}\r\n{TargetTilePosition}\r\n{NextLevelIndex}", Color.Blue, Color.White),
            //    Position = Position
            //});
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
                obj.MapDirection = Direction;
            }
        }

    }

}
