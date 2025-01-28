namespace HotelManagement.Console.Commands
{
    public class CommandDispatcher
    {
        private readonly Dictionary<string, Func<string[], ICommand>> _commands = new Dictionary<string, Func<string[], ICommand>>();

        public void RegisterCommand(string commandName, Func<string[], ICommand> commandFactory)
        {
            _commands[commandName] = commandFactory;
        }

        public async Task<string> DispatchAsync(string commandName, string[] parameters)
        {
            if (_commands.TryGetValue(commandName, out var commandFactory))
            {
                var command = commandFactory(parameters);
                return await command.ExecuteAsync();
            }

            throw new InvalidOperationException($"Command '{commandName}' not found.");
        }
    }
}
