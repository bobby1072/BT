using BT.Common.UkHoliday.Client.Client.Concrete;
using System.Text.Json;

namespace BT.Common.TestConsoleApp
{
    public static class Program
    {
        public static async Task Main()
        {
            var holidays = await new UkHolidaysClient().InvokeAsync();
            Console.WriteLine(JsonSerializer.Serialize(holidays));

        }
    }
}