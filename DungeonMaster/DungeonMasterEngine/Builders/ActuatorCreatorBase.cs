using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;

namespace DungeonMasterEngine.Builders
{
    public class ActuatorCreatorBase
    {
        protected readonly LegacyMapBuilder builder;

        public ActuatorCreatorBase(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        protected async Task<TInitializer> SetupInitializer<TInitializer>(TInitializer initializer, ActuatorItemData data) where TInitializer : SensorInitializerX
        {
            var local = data.ActionLocation as LocalTarget;
            var remote = data.ActionLocation as RemoteTarget;

            initializer.Audible = data.HasSoundEffect;
            initializer.Effect = (SensorEffect)data.Action;
            initializer.LocalEffect = data.IsLocal;
            initializer.ExperienceGain = local?.ExperienceGain ?? false;
            initializer.Rotate = local?.RotateAutors ?? false;
            initializer.OnceOnly = data.IsOnceOnly;
            initializer.RevertEffect = data.IsRevertable;
            initializer.TimeDelay = 1000 / 6 * data.ActionDelay;

            initializer.Specifer = remote?.Position.Direction.ToMapDirection() ?? MapDirection.North; 
            
            var tileResult = await builder.GetTargetTile(remote?.Position.Position.ToAbsolutePosition(builder.CurrentMap), initializer.Specifer);
            initializer.TargetTile = tileResult?.Item1;
            if (tileResult != null) //invertDirection
                initializer.Specifer = tileResult.Item2; 


            return initializer;
        }
    }
}