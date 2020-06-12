using System.Collections.Generic;

namespace SignalRSample.Api.Models
{
    public class GameSession
    {
        public string SessionId { get; set; }
        public string UserOne { get; set; }
        public string UserTwo { get; set; }

        public string ActiveUser { get; set; }

        private List<KeyValuePair<string, int>> _moves;

        public List<KeyValuePair<string, int>> Moves
        {
            get { return _moves ??= new List<KeyValuePair<string, int>>(); }
            set => _moves = value;
        }
    }
}