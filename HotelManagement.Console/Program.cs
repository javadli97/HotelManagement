using HotelManagement.Console;

public class Program
{
    public static async Task Main(string[] args)
    {
        var startup =  await Startup.Initialize();
        
        try
        {
            await startup.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

