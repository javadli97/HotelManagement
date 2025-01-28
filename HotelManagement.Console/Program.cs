using HotelManagement.Console;

public class Program
{
    public static async Task Main(string[] args)
    {
        var startup = new Startup();

        try
        {
            await startup.ConfigureAsync();
            await startup.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }
}

