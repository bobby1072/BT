using BT.Common.UkHoliday.Client.Client.Abstract;
using BT.Common.UkHoliday.Client.Models;
using System.Net.Http.Json;

namespace BT.Common.UkHoliday.Client.Client.Concrete
{
    public class UkHolidaysClient : HttpClient, IUkHolidaysClient
    {
        private const string UkHolidaysEndpoint = "https://www.gov.uk/bank-holidays.json";
        public async Task<UkHolidays> InvokeAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(UkHolidaysEndpoint),
            };

            var response = await SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<UkHolidays>() ?? throw new InvalidDataException("Found no response data in response");

            return content;
        }
        public async Task<UkHolidays?> TryInvokeAsync()
        {
            try
            {
                return await InvokeAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
