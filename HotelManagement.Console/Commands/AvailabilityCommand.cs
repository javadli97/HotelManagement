using System.Globalization;
using HotelManagement.Console.Core;

namespace HotelManagement.Console.Commands
{
    public class AvailabilityCommand : ICommand
    {
        private readonly string _hotelId;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly string _roomType;

        public AvailabilityCommand(string hotelId, string dateRange, string roomType)
        {
            _hotelId = hotelId;
            _roomType = roomType;

            var dates = dateRange.Split('-');
            if (dates.Length != 2 || !DateTime.TryParseExact(dates[0], "yyyyMMdd", null, DateTimeStyles.None, out _startDate) || !DateTime.TryParseExact(dates[1], "yyyyMMdd", null, DateTimeStyles.None, out _endDate))
            {
                throw new ArgumentException("Invalid date range format. Expected format: yyyyMMdd-yyyyMMdd");
            }
        }

        public async Task<string> ExecuteAsync()
        {
            try
            {
                var hotel = GlobalData.Hotels.FirstOrDefault(h => h.Id == _hotelId);
                if (hotel == null)
                {
                    throw new ArgumentException("Invalid hotel ID");
                }

                int totalCount = hotel.Rooms.Count(r => r.RoomType == _roomType);

                int bookingCount = GlobalData.Bookings
                    .Count(b => b.RoomType == _roomType
                    & b.HotelId == _hotelId
                    & b.Arrival < _endDate
                    & b.Departure > _startDate);

                return await Task.FromResult($"{totalCount - bookingCount}");

            }
            catch
            {
                throw new InvalidOperationException($"Error during handling availability command");
            }
        }
    }
}
