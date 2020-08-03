using System;
using System.Collections.Generic;

namespace SignalRSample.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string ConnectionId { get; set; }
        public string UserSubId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Game> UserOneGames { get; set; }
        public virtual ICollection<Game> UserTwoGames { get; set; }
    }
}