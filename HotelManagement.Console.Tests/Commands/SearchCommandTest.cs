using System.Collections.Concurrent;
using HotelManagement.Console.Commands;
using HotelManagement.Console.Core;
using HotelManagement.Console.Model;

namespace HotelManagement.Console.Tests.Commands
{
    public class SearchCommandTest
    {
        [Fact]
        public async Task ExecuteAsync_ShouldReturnCorrectAvailability_WhenHotelAndRoomTypeExist()
        {
            // Arrange
            var hotelId = "H1";
            var days = "3";
            var roomType = "dbl";
            var command = new SearchCommand(hotelId, days, roomType);

            GlobalData.Hotels = new ConcurrentBag<Hotel>
            {
                new Hotel
                {
                    Id = hotelId,
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = roomType },
                        new Room { RoomType = roomType },
                        new Room { RoomType = "sgl" }
                    }
                }
            };

            GlobalData.Bookings = new ConcurrentBag<Booking>
            {
                new Booking { HotelId = hotelId, RoomType = roomType, Arrival = DateTime.Today, Departure = DateTime.Today.AddDays(1) },
                new Booking { HotelId = hotelId, RoomType = roomType, Arrival = DateTime.Today.AddDays(1), Departure = DateTime.Today.AddDays(2) }
            };

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            Assert.Equal("(20231010-20231011, 1), (20231011-20231012, 1), (20231012-20231013, 2)", result);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenDaysIsInvalid()
        {
            // Arrange
            var hotelId = "H1";
            var days = "invalid";
            var roomType = "dbl";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new SearchCommand(hotelId, days, roomType));
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowArgumentException_WhenHotelIdIsInvalid()
        {
            // Arrange
            var hotelId = "InvalidHotelId";
            var days = "3";
            var roomType = "dbl";
            var command = new SearchCommand(hotelId, days, roomType);

            GlobalData.Hotels = new ConcurrentBag<Hotel>
            {
                new Hotel
                {
                    Id = "H1",
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = roomType }
                    }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => command.ExecuteAsync());
        }
    }
}
