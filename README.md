# Hotel Management Console Application

## Introduction
This is a console application for managing hotel bookings and availability. It reads data from hotels.json and bookings.json and allows users to search for available rooms and check room availability for specific dates.


## Design
  The application follows a monolithic design with separation of concerns.
  It is a simple console application, persistance of data after reading from json files and settings located in static class.
  As count of parameeters for both commands is same the Command design patter is used. Both command have own behaviour inside execute method. There is CommandDispatcher for regsitering and dispatchicg from commands dictionary.
  

 The main components are:

- **Commands**: Handle the execution of specific actions like checking availability and searching for rooms.
- **Core**: Contains the core data structures and repositories for accessing hotel and booking data, command parser and global settings.  
- **Model**: Defines the data models for hotels, rooms, and bookings.
- **Requests**: Encapsulates the parameters for commands.
- **Services**: Provides various services like file handling, initialization, and JSON serialization.
- **Tests**: Contains unit tests for the commands.

## Behavior
- **AvailabilityCommand**: Checks the availability of rooms in a hotel for a specific date range.
- **SearchCommand**: Searches for available rooms in a hotel for a specified number of days ahead to query for availability.

## How to Run
1. json files located under **data** folder and there are data source of this application. If necessary it is possible to change content of files.
1. Build the Project:
  dotnet build
3. Run the Application:
   dotnet run --project HotelManagement.Console

## Usage
1. Check Availability:
  -Command: Availability("HotelId","DateRange","RoomType")
  -Example: Availability("H1","20231005-20231006","sgl")

2. Search for Rooms:
  -Command: Search("HotelId","days","RoomType")
  -Example: Search("H1","30","sgl")

## Libraries Used
The following libraries are used in this project:

- **Newtonsoft.Json**: For JSON serialization and deserialization.
  - [Newtonsoft.Json on NuGet](https://www.nuget.org/packages/Newtonsoft.Json/)
- **Moq**: For creating mock objects in unit tests.
  - [Moq on NuGet](https://www.nuget.org/packages/Moq/)
- **xUnit**: For unit testing.
  - [xUnit on NuGet](https://www.nuget.org/packages/xunit/)

## How Copilot Was Used
Here are some specific ways it was utilized:

  - Code Generation: Unit tests, dispatcher(CommandDispatcher) for Command design patterm, parsing command with regex
  - Code Suggestions: Grouping result by date range after getting availibility count from start date to next date.
  - Documentation: Generating template of this README file
