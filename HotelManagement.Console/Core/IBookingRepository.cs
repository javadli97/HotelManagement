using System.Collections.Concurrent;
using HotelManagement.Console.Model;

namespace HotelManagement.Console.Core
{
    public interface IBookingRepository
    {
        ConcurrentBag<Booking> GetBookings();
    }
}
