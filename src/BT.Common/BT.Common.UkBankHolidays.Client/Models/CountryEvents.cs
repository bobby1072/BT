﻿using System.Text.Json.Serialization;

namespace BT.Common.UkHoliday.Client.Models
{
    public record CountryEvents
    {
        [JsonPropertyName("division")]
        public string? Division { get; init; }

        [JsonPropertyName("events")] public IReadOnlyCollection<HolidayEvent> Events { get; init; } = [];
    }
}
