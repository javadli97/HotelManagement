using System.Globalization;
using System.Text.RegularExpressions;

namespace HotelManagement.Console.Commands
{
    public static class CommandParser
    {
        
        public static (string CommandName, string[] Parameters) ParseCommand(string input)
        {
            var match = Regex.Match(input, @"(\w+)\((.*)\)");
            if (!match.Success)
            {
                throw new InvalidOperationException("Invalid command format.");
            }

            var commandName = match.Groups[1].Value;
            var parameters = match.Groups[2].Value.Split(',').Select(p => p.Trim()).ToArray();

            return (commandName, parameters);
        }
    }
}
