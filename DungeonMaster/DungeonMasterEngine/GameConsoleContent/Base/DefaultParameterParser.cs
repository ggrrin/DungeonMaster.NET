using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
