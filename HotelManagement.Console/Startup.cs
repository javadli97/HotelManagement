using HotelManagement.Console.Commands;
using HotelManagement.Console.Core;
using HotelManagement.Console.Services.Implementations;
using HotelManagement.Console.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HotelManagement.Console
{
    public class Startup
    {
        private readonly IFileService _fileService;
        private readonly IJsonService _jsonService;
        private readonly IInitializationService _initializationService;
        private readonly CommandDispatcher _dispatcher;


        public static async Task<Startup> Initialize()
        {
            var services = new ServiceCollection();

            Startup startup = new Startup(services);
            await startup.SeedCollections();
            return startup;
        }

        public Startup(ServiceCollection services)
        {
            _dispatcher = new CommandDispatcher();

            ConfigureServices(services);

            // Create service provider
            var serviceProvider = services.BuildServiceProvider();
            _fileService = serviceProvider.GetRequiredService<IFileService>();
            _jsonService = serviceProvider.GetRequiredService<IJsonService>();
            _initializationService = serviceProvider.GetRequiredService<IInitializationService>();
        }        


        public async Task Run()
        {
            System.Console.WriteLine("There are only two dediticates commands (e.g. Availability(H1, 20240901-20240903, DBL) and Search(H1, 365, SGL))");
            System.Console.WriteLine("Enter a blank line is entered to quit:");

            while (true)
            {
                System.Console.Write("");
                string input = System.Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                try
                {
                    var (commandName, parameters) = CommandParser.ParseCommand(input);
                    string result = await _dispatcher.DispatchAsync(commandName, parameters);
                    System.Console.WriteLine(result);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register services with DI
            services.AddTransient<IFileService, FileService>();           
            services.AddTransient<IInitializationService, InitializationService>();
            services.AddTransient<IJsonService, JsonService>();

            // Register commands with the dispatcher
            _dispatcher.RegisterCommand("Availability", parameters => new AvailabilityCommand(
                parameters[0], parameters[1], parameters[2]
            ));
            _dispatcher.RegisterCommand("Search", parameters => new SearchCommand(
                parameters[0], parameters[1], parameters[2]
            ));
        }

        private async Task SeedCollections()
        {
            await _initializationService.InitializeAsync();
        }
    }
}
