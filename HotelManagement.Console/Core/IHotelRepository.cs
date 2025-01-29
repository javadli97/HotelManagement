using System.Collections.Concurrent;
using HotelManagement.Console.Model;

namespace HotelManagement.Console.Core
{
    public interface IHotelRepository
    {
        ConcurrentBag<Hotel> GetHotels();
    }
}
