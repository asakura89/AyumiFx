using System.Collections.Generic;
using System.Linq;

namespace WK.DBUtility
{
    class CommandData
    {
        public string CommandString { get; set; }
        public Dictionary<string, object> ParameterList { get; set; }

        private CommandData(string pCommandString, Dictionary<string, object> pParameterList)
        {
            CommandString = pCommandString;
            ParameterList = pParameterList;
        }

        public static CommandData CreateObject<T>(string commandString, T dataObject)
            where T : new()
        {
            var parameterList = GetParametersForCommand(dataObject, commandString);
            return new CommandData(commandString, parameterList);
        }

        public static Dictionary<string, object> GetParametersForCommand<T>(T dataObject, string commandString)
            where T : new()
        {
            var paramList = ReflectionUtility.GetAllParametersWithValue(dataObject);
            var filteredParamList = paramList.Where(param => commandString.Contains(param.Key));

            return filteredParamList.ToDictionary(param => param.Key, param => param.Value);
        }

    }
}
