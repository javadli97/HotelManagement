using System.Globalization;

namespace HotelManagement.Console.Requests
{
    public class AvailabilityRequest
    {
        private readonly DateTime _endDate;
        private readonly DateTime _startDate;

        public string HotelId { get; }
        public DateTime StartDate { get => _startDate; }
        public DateTime EndDate { get => _endDate; }
        public string RoomType { get; }



        public AvailabilityRequest(string hotelId, string dateRange, string roomType)
        {
            HotelId = hotelId ?? throw new ArgumentNullException($"{nameof(hotelId)} is empty");
            RoomType = roomType ?? throw new ArgumentNullException($"{nameof(roomType)} is empty");

            var dates = dateRange.Split('-');
            if (dates.Length != 2 || !DateTime.TryParseExact(dates[0], "yyyyMMdd", null, DateTimeStyles.None, out _startDate) || !DateTime.TryParseExact(dates[1], "yyyyMMdd", null, DateTimeStyles.None, out _endDate))
            {
                throw new ArgumentException("Invalid date range format. Expected format: yyyyMMdd-yyyyMMdd");
            }
        }
    }
}
