using System.Collections.Concurrent;
using HotelManagement.Console.Model;
using HotelManagement.Console.Services.Implementations;
using Newtonsoft.Json;

namespace HotelManagement.Console.Tests.Services
{
    public class JsonServiceTests
    {
        private readonly JsonService _jsonService;

        public JsonServiceTests()
        {
            _jsonService = new JsonService();
        }

        [Fact]
        public async Task DeserializeJsonStreamAsync_ShouldThrowArgumentNullException_WhenStreamIsNotValid()
        {
            // Arrange
            Stream jsonStream = null;

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _jsonService.DeserializeJsonStreamAsync<List<Hotel>>(jsonStream));

            // Assert
            Assert.Equal("Exception occured during JSON deserialization", exception.Message);
        }

        [Fact]
        public async Task DeserializeJsonStreamAsync_ShouldReturnDeserializedObject_WhenStreamIsValid()
        {
            // Arrange
            var hotels = new List<Hotel>
        {
            new Hotel { Id = "H1", Name = "Hotel One" },
            new Hotel { Id = "H2", Name = "Hotel Two" }
        };
            var json = JsonConvert.SerializeObject(hotels);
            var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));

            // Act
            var result = await _jsonService.DeserializeJsonStreamAsync<ConcurrentBag<Hotel>>(jsonStream);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("H1", result.First(h=>h.Id == "H1").Id);
        }

        [Fact]
        public async Task DeserializeJsonStreamAsync_ShouldReturnDeserializedObjectWithCorrectDateFormat_WhenStreamIsValid()
        {
            // Arrange        
            var bookingJson = @"
            [
                {
                ""hotelId"": ""H1"",
                ""arrival"": ""20240901"",
                ""departure"": ""20240903"",
                ""roomType"": ""DBL"",
                ""roomRate"": ""Prepaid""
                }
            ]";

            var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(bookingJson));

            // Act
            var result = await _jsonService.DeserializeJsonStreamAsync<ConcurrentBag<Booking>>(jsonStream);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(new DateTime(2024, 9, 1), result.First().Arrival);
            Assert.Equal(new DateTime(2024, 9, 3), result.First().Departure);
        }
    }
}
