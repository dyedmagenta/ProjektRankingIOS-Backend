using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Model
{
    public class Game
    {
        
        public long Id { get; set; }
        public long WhitePlayerId { get; set; }
        public long BlackPlayerId { get; set; }
        public long TournamentId { get; set; }
        public int WhiteScoreChange { get; set; }
        public int BlackScoreChange { get; set; }
        public DateTime Date { get; set; }
        
        public virtual Player WhitePlayer { get; set; }
        public virtual Player BlackPlayer { get; set; }
        public virtual Tournament Tournament { get; set; }
        
    }
}
