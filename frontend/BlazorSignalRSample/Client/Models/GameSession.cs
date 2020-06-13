using System.Collections.Generic;

namespace BlazorSignalRSample.Client.Models
{
    public class GameSession
    {
        public string SessionId { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }

        public string ActiveUser { get; set; }

        private List<KeyValuePair<string, int>> _moves;

        public List<KeyValuePair<string, int>> Moves
        {
            get { return _moves ??= new List<KeyValuePair<string, int>>(); }
            set => _moves = value;
        }
    }
}