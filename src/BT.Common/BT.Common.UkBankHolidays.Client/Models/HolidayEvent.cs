using System.Text.Json.Serialization;

namespace BT.Common.UkHoliday.Client.Models
{
    public record HolidayEvent
    {
        [JsonPropertyName("title")]
        public string Title { get; init; }
        [JsonPropertyName("date")]
        public DateOnly Date { get; init; }
        [JsonPropertyName("notes")]
        public string Notes { get; init; }
        [JsonPropertyName("bunting")]
        public bool Bunting { get; init; }
        [JsonConstructor]
        internal HolidayEvent(string title, DateOnly date, string notes, bool bunting)
        {
            Title = title;
            Date = date;
            Notes = notes;
            Bunting = bunting;
        }
    }
}
