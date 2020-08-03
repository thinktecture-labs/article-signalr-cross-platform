using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalRSample.Api.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public Guid UserOneId { get; set; }
        public Guid UserTwoId { get; set; }

        [ForeignKey("UserOneId")]
        [InverseProperty("UserOneGames")]
        public virtual User UserOne { get; set; }

        [ForeignKey("UserTwoId")]
        [InverseProperty("UserTwoGames")]
        public virtual User UserTwo { get; set; }

        public string Winner { get; set; }
        public DateTime SessionDate { get; set; }
    }
}