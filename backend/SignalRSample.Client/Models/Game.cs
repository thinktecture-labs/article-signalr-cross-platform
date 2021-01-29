using System;
using SignalRSample.Client.Models;
namespace SignalRSample.Client.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }
        public User Winner { get; set; }
        public DateTime SessionDate { get; set; }
    }
}
