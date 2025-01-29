using System.Text.RegularExpressions;

namespace HotelManagement.Console.Core
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

            var commandName = match.Groups[1].Value.ToLowerInvariant();
            var parameters = match.Groups[2].Value.Split(',').Select(p => p.Trim()).ToArray();

            if (parameters.Length != 3 || parameters.Any(string.IsNullOrEmpty))
            {
                throw new InvalidOperationException("Command must have exactly three non-empty parameters.");
            }

            return (commandName, parameters);
        }
    }
}
