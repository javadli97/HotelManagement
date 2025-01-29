using System.Collections.Concurrent;
using HotelManagement.Console.Core;
using HotelManagement.Console.Model;
using HotelManagement.Console.Services.Interfaces;

namespace HotelManagement.Console.Services.Implementations
{
    public class InitializationService : IInitializationService
    {
        private readonly IFileService _fileService;
        private readonly IJsonService _jsonService;

        public InitializationService(IFileService fileService, IJsonService jsonService)
        {
            _fileService = fileService;
            _jsonService = jsonService;
        }

        public async Task InitializeAsync()
        {
            try
            {
                using Stream hotelStream = await _fileService.OpenReadStreamAsync(GlobalSettings.HotelPath);
                GlobalData.Hotels = await _jsonService.DeserializeJsonStreamAsync<ConcurrentBag<Hotel>>(hotelStream);

                using Stream bookingStream = await _fileService.OpenReadStreamAsync(GlobalSettings.BookingPath);
                GlobalData.Bookings = await _jsonService.DeserializeJsonStreamAsync<ConcurrentBag<Booking>>(bookingStream);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }        
    }
}
