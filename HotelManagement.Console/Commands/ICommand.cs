namespace HotelManagement.Console.Commands
{
    public interface ICommand
    {
        Task<string> ExecuteAsync();
    }
}
