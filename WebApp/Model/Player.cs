using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Model
{
    public sealed class Player
    {

        public Player()
        {
            //WhitePlayed = new List<Game>();
            //BlackPlayed = new List<Game>();
        }

        
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Rank { get; set; }
        public int Score { get; set; }
        public DateTime? FirstGame { get; set; }
        public DateTime? PlayingSince { get; set; }
        //public ICollection<Game> WhitePlayed { get; set; }
        //public ICollection<Game> BlackPlayed { get; set; }

    }
}
