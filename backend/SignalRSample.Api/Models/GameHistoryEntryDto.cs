using System;

namespace SignalRSample.Api.Models
{
    public class GameHistoryEntryDto
    {
        public Guid Id { get; set; }
        public string Opponent { get; set; }
        public string GameResult { get; set; }
        public DateTime SessionDate { get; set; }
    }
}