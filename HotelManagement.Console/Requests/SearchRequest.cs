namespace HotelManagement.Console.Requests
{
    public class SearchRequest
    {
        private readonly int _days;

        public string HotelId { get; }
        public int Days { get => _days; }
        public string RoomType { get; }

        public SearchRequest(string hotelId, string days, string roomType)
        {
            HotelId = hotelId ?? throw new ArgumentNullException($"{nameof(hotelId)} is empty");
            RoomType = roomType ?? throw new ArgumentNullException($"{nameof(roomType)} is empty");

            if (!int.TryParse(days, out _days))
            {
                throw new ArgumentException("Invalid days format. Expected an integer value.");
            }
        }
    }
}
