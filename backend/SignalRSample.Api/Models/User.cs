using System;

namespace SignalRSample.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string ConnectionId { get; set; }
        public string Name { get; set; }
    }
}