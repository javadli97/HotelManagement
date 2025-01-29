using HotelManagement.Console.Core;
using HotelManagement.Console.Requests;

namespace HotelManagement.Console.Commands
{
    public class AvailabilityCommand : ICommand
    {
        private readonly AvailabilityRequest request;       
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingRepository _bookingRepository;

        public AvailabilityCommand(
            AvailabilityRequest request,
            IHotelRepository hotelRepository,
            IBookingRepository bookingRepository)
        {
            this.request = request;
            _hotelRepository = hotelRepository;
            _bookingRepository = bookingRepository;           
        }

        public async Task<string> ExecuteAsync()
        {
            try
            {
                var hotel = _hotelRepository.GetHotels().FirstOrDefault(h => h.Id.Equals(request.HotelId, StringComparison.OrdinalIgnoreCase));
                if (hotel == null)
                {
                    throw new ArgumentException("Invalid hotel ID");
                }

                int totalCount = hotel.Rooms.Count(r => r.RoomType.Equals(request.RoomType, StringComparison.OrdinalIgnoreCase));

                int bookingCount = _bookingRepository.GetBookings()
                    .Count(b => b.RoomType.Equals(request.RoomType, StringComparison.OrdinalIgnoreCase)
                    & b.HotelId.Equals(request.HotelId, StringComparison.OrdinalIgnoreCase)
                    & b.Arrival < request.EndDate
                    & b.Departure > request.StartDate);

                return await Task.FromResult($"{totalCount - bookingCount}");
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error during handling availability command: {e.Message}");
            }
        }
    }
}
