using System.Text.Json.Serialization;

namespace BT.Common.UkHoliday.Client.Models
{
    public record UkHolidays
    {
        [JsonPropertyName("england-and-wales")]
        public CountryEvents 
            EnglandAndWales { get; init; }
        [JsonPropertyName("scotland")]
        public CountryEvents Scotland { get; init; }
        [JsonPropertyName("northern-ireland")]
        public CountryEvents NorthernIreland { get; init; }
    }
}