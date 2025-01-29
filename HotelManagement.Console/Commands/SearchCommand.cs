using HotelManagement.Console.Core;
using HotelManagement.Console.Model;
using HotelManagement.Console.Requests;

namespace HotelManagement.Console.Commands
{
    public class SearchCommand : ICommand
    {
        private readonly SearchRequest request;       
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingRepository _bookingRepository;

        public SearchCommand(
            SearchRequest request,
            IHotelRepository hotelRepository, 
            IBookingRepository bookingRepository)
        {
           
            this.request = request;
            _hotelRepository = hotelRepository;
            _bookingRepository = bookingRepository;
        }

        public Task<string> ExecuteAsync()
        {
            try
            {
                var hotel = _hotelRepository.GetHotels().FirstOrDefault(h => h.Id.Equals(request.HotelId, StringComparison.OrdinalIgnoreCase));
                if (hotel == null)
                {
                    throw new ArgumentException("Invalid hotel ID");
                }

                var totalRooms = hotel.Rooms.Count(r => r.RoomType.Equals(request.RoomType, StringComparison.OrdinalIgnoreCase));

                // Create a dictionary to index bookings by hotel ID and room type
                var bookingDict = _bookingRepository.GetBookings()
                    .Where(b => b.HotelId.Equals(request.HotelId, StringComparison.OrdinalIgnoreCase) && b.RoomType.Equals(request.RoomType, StringComparison.OrdinalIgnoreCase))
                    .GroupBy(b => new { HotelId = b.HotelId.ToUpperInvariant(), RoomType = b.RoomType.ToUpperInvariant() })
                    .ToDictionary(g => g.Key, g => g.ToList());

                var availabilityList = new List<(string DateRange, int AvailableRooms)>();

                DateTime startDate = DateTime.Today;
                DateTime endDate = startDate.AddDays(request.Days);

                DateTime currentStartDate = startDate;
                int currentAvailableRooms = -1;

                for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
                {
                    DateTime nextDate = date.AddDays(1);

                    // Get bookings for the current hotel and room type
                    var relevantBookings = bookingDict.TryGetValue(new { HotelId = request.HotelId.ToUpperInvariant(), RoomType =  request.RoomType.ToUpperInvariant() }, out var bookingsList)
                        ? bookingsList
                        : new List<Booking>();

                    var overlappingBookingsCount = relevantBookings.Count(b =>
                        b.Arrival < nextDate && b.Departure > date);

                    int availableRooms = totalRooms - overlappingBookingsCount;

                    if (currentAvailableRooms == -1)
                    {
                        currentAvailableRooms = availableRooms;
                    }

                    if (availableRooms != currentAvailableRooms)
                    {
                        availabilityList.Add(($"{currentStartDate:yyyyMMdd}-{date:yyyyMMdd}", currentAvailableRooms));
                        currentStartDate = date;
                        currentAvailableRooms = availableRooms;
                    }
                }

                // Add the last range
                availabilityList.Add(($"{currentStartDate:yyyyMMdd}-{endDate:yyyyMMdd}", currentAvailableRooms));


                return Task.FromResult(GetAvailabilityOutput(availabilityList));
            }
            catch (Exception e)
            {

                throw new InvalidOperationException($"Error during handling availability command: {e.Message}");
            }
            
        }

        private string GetAvailabilityOutput(List<(string DateRange, int AvailableRooms)> availabilityList)
        {
            var availabilityOutput = availabilityList
                .Where(a => a.AvailableRooms > 0)
                .Select(a => $"({a.DateRange}, {a.AvailableRooms})")
                .ToList();

            return string.Join(", ", availabilityOutput);
        }
    }
}
