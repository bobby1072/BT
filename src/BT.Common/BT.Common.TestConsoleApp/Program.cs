using BT.Common.UkHoliday.Client.Client.Concrete;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

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