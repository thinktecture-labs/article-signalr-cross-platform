using System.Collections.Generic;
using SignalRSample.Api.Models;

namespace SignalRSample.Api.Hubs
{
    public class GamesHub
    {
        private List<GameSession> _sessions = new List<GameSession>();

        public GamesHub()
        {
        }
    }
}