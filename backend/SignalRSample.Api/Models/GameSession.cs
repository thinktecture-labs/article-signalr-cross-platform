using System;
using System.Collections.Generic;

namespace SignalRSample.Api.Models
{
    public class GameSession
    {
        public Guid Id { get; set; }
        public string SessionId { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }

        public string ActiveUser { get; set; }

        private List<GameSessionMove> _moves;

        public List<GameSessionMove> Moves
        {
            get { return _moves ??= new List<GameSessionMove>(); }
            set => _moves = value;
        }

        public GameSession()
        {
            Id = Guid.NewGuid();
        }
    }

    public class GameSessionMove
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public int Move { get; set; }
        public string ClientId { get; set; }
    }
}