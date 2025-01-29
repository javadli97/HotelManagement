using HotelManagement.Console.Core;

namespace HotelManagement.Console.Tests.Core
{
    public class CommandParserTests
    {
        [Fact]
        public void ParseCommand_ShouldReturnCommandNameAndParameters_WhenInputIsValid()
        {
            // Arrange
            var input = "Search(hotel, 3, extra)";

            // Act
            var result = CommandParser.ParseCommand(input);

            // Assert
            Assert.Equal("search", result.CommandName);
            Assert.Equal(3, result.Parameters.Length);
            Assert.Equal("hotel", result.Parameters[0]);
            Assert.Equal("3", result.Parameters[1]);
            Assert.Equal("extra", result.Parameters[2]);
        }

        [Fact]
        public void ParseCommand_ShouldThrowInvalidOperationException_WhenInputIsInvalid()
        {
            // Arrange
            var input = "InvalidCommand";

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => CommandParser.ParseCommand(input));
            Assert.Equal("Invalid command format.", exception.Message);
        }

        [Fact]
        public void ParseCommand_ShouldThrowInvalidOperationException_WhenParametersAreEmpty()
        {
            // Arrange
            var input = "command(,,)";

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => CommandParser.ParseCommand(input));
            Assert.Equal("Command must have exactly three non-empty parameters.", exception.Message);
        }

        [Fact]
        public void ParseCommand_ShouldTrimParameters()
        {
            // Arrange
            var input = "Command(param1 , param2 , param3)";

            // Act
            var result = CommandParser.ParseCommand(input);

            // Assert
            Assert.Equal("command", result.CommandName);
            Assert.Equal(3, result.Parameters.Length);
            Assert.Equal("param1", result.Parameters[0]);
            Assert.Equal("param2", result.Parameters[1]);
            Assert.Equal("param3", result.Parameters[2]);
        }

        [Fact]
        public void ParseCommand_ShouldThrowInvalidOperationException_WhenParametersAreMoreThanThree()
        {
            // Arrange
            var input = "Command(param1, param2, param3, param4)";

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => CommandParser.ParseCommand(input));
            Assert.Equal("Command must have exactly three non-empty parameters.", exception.Message);
        }

        [Fact]
        public void ParseCommand_ShouldThrowInvalidOperationException_WhenParametersAreLessThanThree()
        {
            // Arrange
            var input = "Command(param1, param2)";

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => CommandParser.ParseCommand(input));
            Assert.Equal("Command must have exactly three non-empty parameters.", exception.Message);
        }

        [Fact]
        public void ParseCommand_ShouldThrowInvalidOperationException_WhenAnyParameterIsEmpty()
        {
            // Arrange
            var input = "Command(param1, , param3)";

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => CommandParser.ParseCommand(input));
            Assert.Equal("Command must have exactly three non-empty parameters.", exception.Message);
        }
    }
}
