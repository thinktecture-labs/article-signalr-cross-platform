using System.Collections.Generic;
using SignalRSample.Api.Models;

namespace SignalRSample.Api.Services
{
    public class GameSessionManager
    {
        private List<GameSession> _sessions = new List<GameSession>();

        public GameSessionManager()
        {
        }
    }
}