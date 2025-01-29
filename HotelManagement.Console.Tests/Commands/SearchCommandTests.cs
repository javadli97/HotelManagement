using System.Collections.Concurrent;
using HotelManagement.Console.Commands;
using HotelManagement.Console.Core;
using HotelManagement.Console.Model;
using HotelManagement.Console.Requests;
using Moq;

namespace HotelManagement.Console.Tests.Commands
{
    public class SearchCommandTests
    {
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;

        public SearchCommandTests()
        {
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();

            // Initialize mock data
            var hotels = new ConcurrentBag<Hotel>
            {
            new Hotel
                {
                    Id = "H1",
                    Name = "Hotel One",
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = "SGL", RoomId = "102" },
                        new Room { RoomType = "DBL", RoomId = "101" }
                    }
                },
                new Hotel
                {
                    Id = "H2",
                    Name = "Hotel Two",
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = "sgl", RoomId = "102" },
                        new Room { RoomType = "dbl", RoomId = "101" }
                    }
                }
            };

            var bookings = new ConcurrentBag<Booking>
            {
                new Booking {HotelId = "H1", RoomType = "sgl", Arrival = DateTime.Today, Departure = DateTime.Today.AddDays(1) }
            };

            _hotelRepositoryMock.Setup(repo => repo.GetHotels()).Returns(hotels);
            _bookingRepositoryMock.Setup(repo => repo.GetBookings()).Returns(bookings);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnAvailableRooms_WhenRoomsAreAvailable()
        {
            // Arrange
            var request = new SearchRequest("H2", "1", "sgl");
            var command = new SearchCommand(request, _hotelRepositoryMock.Object, _bookingRepositoryMock.Object);

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            Assert.Contains("(20250129-20250130, 1)", result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNoAvailableRooms_WhenNoRoomsAreAvailable()
        {
            // Arrange
            var request = new SearchRequest("H1", "1", "sgl");
            var command = new SearchCommand(request, _hotelRepositoryMock.Object, _bookingRepositoryMock.Object);

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenHotelDoesNotExist()
        {
            // Arrange
            var request = new SearchRequest("NonExistentHotel", "1", "Single");
            var command = new SearchCommand(request, _hotelRepositoryMock.Object, _bookingRepositoryMock.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync());
            Assert.Equal("Error during handling availability command: Invalid hotel ID", exception.Message);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenDaysFormatIsInvalid()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new SearchRequest("H1", "invalid-days", "Single"));
        }       
    }
}
