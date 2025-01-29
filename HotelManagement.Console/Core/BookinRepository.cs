using System.Collections.Concurrent;
using HotelManagement.Console.Model;

namespace HotelManagement.Console.Core
{
    public class BookingRepository : IBookingRepository
    {
        public ConcurrentBag<Booking> GetBookings() => GlobalData.Bookings;
    }
}
