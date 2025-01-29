using HotelManagement.Console.Core;
using HotelManagement.Console.Model;

namespace HotelManagement.Console.Commands
{
    public class SearchCommand : ICommand
    {
        private readonly string _hotelId;
        private readonly int _days;
        private readonly string _roomType;

        public SearchCommand(string hotelId, string days, string roomType)
        {
            if (!int.TryParse(days, out _days))
            {
                throw new ArgumentException("Invalid days format. Expected an integer value.");
            }

            _hotelId = hotelId.ToUpperInvariant();
            _roomType = roomType.ToUpperInvariant();
        }

        public Task<string> ExecuteAsync()
        {
            var hotel = GlobalData.Hotels.FirstOrDefault(h => h.Id == _hotelId);
            if (hotel == null)
            {
                throw new ArgumentException("Invalid hotel ID");
            }

            var totalRooms = hotel.Rooms.Count(r => r.RoomType == _roomType);

            // Create a dictionary to index bookings by hotel ID and room type
            var bookingDict = GlobalData.Bookings
                .Where(b => b.HotelId.Equals(_hotelId) && b.RoomType.Equals(_roomType))
                .GroupBy(b => new { b.HotelId, b.RoomType })
                .ToDictionary(g => g.Key, g => g.ToList());

            var availabilityList = new List<(string DateRange, int AvailableRooms)>();

            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(_days);

            DateTime currentStartDate = startDate;
            int currentAvailableRooms = -1;

            for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
            {
                DateTime nextDate = date.AddDays(1);

                // Get bookings for the current hotel and room type
                var relevantBookings = bookingDict.TryGetValue(new { HotelId = _hotelId, RoomType = _roomType }, out var bookingsList)
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
