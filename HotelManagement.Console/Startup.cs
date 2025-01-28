using HotelManagement.Console.Commands;
using HotelManagement.Console.Core;
using HotelManagement.Console.Model;
using HotelManagement.Console.Services.Implementations;
using HotelManagement.Console.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace HotelManagement.Console
{
    public class Startup
    {
        private readonly IFileService _fileService;
        private readonly IJsonService _jsonService;
        private readonly CommandDispatcher _dispatcher;

        public Startup()
        {     
            var services = new ServiceCollection();
            _dispatcher = new CommandDispatcher();

            ConfigureServices(services);

            // Create service provider
            var serviceProvider = services.BuildServiceProvider();
            _fileService = serviceProvider.GetRequiredService<IFileService>();
            _jsonService = serviceProvider.GetRequiredService<IJsonService>();
        }

        public async Task ConfigureAsync()
        {
            try
            {
                using Stream hotelStream = await _fileService.OpenReadStreamAsync(GlobalSettings.HotelPath);
                GlobalData.Hotels = await _jsonService.DeserializeJsonStreamAsync<List<Hotel>>(hotelStream);

                using Stream bookingStream = await _fileService.OpenReadStreamAsync(GlobalSettings.BookingPath);
                GlobalData.Bookings = await _jsonService.DeserializeJsonStreamAsync<List<Booking>>(bookingStream);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public async Task Run()
        {
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

            // Register JsonService with DI
            services.AddTransient<IJsonService>(provider => new JsonService(GlobalSettings.JsonSettings));

            // Register commands with the dispatcher
            _dispatcher.RegisterCommand("Availability", parameters => new AvailabilityCommand(
                parameters[0], parameters[1], parameters[2]
            ));
            _dispatcher.RegisterCommand("Search", parameters => new SearchCommand(
                parameters[0], parameters[1], parameters[2]
            ));
        }
    }
}
