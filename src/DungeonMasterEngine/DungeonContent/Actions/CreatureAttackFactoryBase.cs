using System;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.DungeonContent.Actions
{
    public class CreatureAttackFactoryBase : IActionFactory
    {
        public IAction CreateAction(ILiveEntity actionProvider)
        {
            throw new NotImplementedException();
        }
    }
}