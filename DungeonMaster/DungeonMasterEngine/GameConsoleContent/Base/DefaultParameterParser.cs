using System;

namespace DungeonMasterEngine.GameConsoleContent.Base
{
    public class DefaultParameterParser : IParameterParser
    {
        public string[] ParseParameters(string parametersString)
        {
            return parametersString.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
