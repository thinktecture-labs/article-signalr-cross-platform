using System;

namespace SignalRSample.Client.Models
{
    public class GameHistory
    {
        public Guid Id { get; set; }
        public string Opponent { get; set; }
        public string GameResult { get; set; }
        public DateTime SessionDate { get; set; }
    }
}