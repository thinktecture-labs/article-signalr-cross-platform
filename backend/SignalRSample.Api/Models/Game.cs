using System;

namespace SignalRSample.Api.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }
        public string Winner { get; set; }
        public DateTime SessionDate { get; set; }
    }
}