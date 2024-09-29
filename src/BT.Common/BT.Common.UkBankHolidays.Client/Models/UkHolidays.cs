using BT.Common.UkHoliday.Client.Models;
using System.Text.Json.Serialization;

namespace BT.Common.UkBankHolidays.Client.Models
{
    public record UkHolidays
    {
        [JsonPropertyName("englandandwales")]
        public CountryEvents EnglandAndWales { get; init; }
        [JsonPropertyName("scotland")]
        public CountryEvents Scotland { get; init; }
        [JsonPropertyName("northernireland")]
        public CountryEvents NorthernIreland { get; init; }
        [JsonConstructor]
        internal UkHolidays(CountryEvents englandandwales, CountryEvents scotland, CountryEvents northernireland)
        {
            EnglandAndWales = englandandwales;
            Scotland = scotland;
            NorthernIreland = northernireland;
        }
    }
}