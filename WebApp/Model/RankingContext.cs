using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Model
{
    public class RankingContext : DbContext
    {

        public RankingContext(DbContextOptions<RankingContext> options) : base(options)
        {
        }
        public DbSet<News> NewsItems { get; set; }
        public DbSet<Game> GameItems { get; set; }
        public DbSet<Tournament> TournamentItems { get; set; }
        public DbSet<Player> PlayerItems { get; set; }


        public void FillTournaments()
        {
            if (TournamentItems.Count() == 0)
            {
                TournamentItems.AddRange(
                    new Tournament { Name = "First testing tournament" },
                    new Tournament { Name = "Second official tournament" },
                    new Tournament { Name = "Maybe empty tournament" }
                );
                SaveChanges();
            }
        }
        public void FillNews()
        {
            if (NewsItems.Count() == 0)
            {
                NewsItems.AddRange(
                    new News { Content = "I guess we are fine with news for now", Date = new DateTime(2017, 06, 04) },
                    new News { Content = "Hello, this is first news post!", Date = new DateTime(2017, 06, 02) },
                    new News { Content = "Let's fill this with couple news", Date = new DateTime(2017, 06, 03) }
                );
                this.SaveChanges();
            }
        }
        public void FillGames()
        {
            if (PlayerItems.Count() == 0) FillPlayers();
            if (TournamentItems.Count() == 0) FillTournaments();
            if (GameItems.Count() == 0)
            {
                GameItems.AddRange(
                    new Game { WhitePlayerId = 1, BlackPlayerId = 2, WhiteScoreChange = 5, BlackScoreChange = -5, Date = new DateTime(2017, 06, 04), TournamentId = 1  },
                    new Game { WhitePlayerId = 1, BlackPlayerId = 3, WhiteScoreChange = -5, BlackScoreChange = 5, Date = new DateTime(2017, 06, 04), TournamentId = 2  },
                    new Game { WhitePlayerId = 2, BlackPlayerId = 3, WhiteScoreChange = 0, BlackScoreChange = 0, Date = new DateTime(2017, 06, 04), TournamentId = 1   },
                    new Game { WhitePlayerId = 3, BlackPlayerId = 1, WhiteScoreChange = 5, BlackScoreChange = -5, Date = new DateTime(2017, 06, 04), TournamentId = 2  },
                    new Game { WhitePlayerId = 3, BlackPlayerId = 2, WhiteScoreChange = 5, BlackScoreChange = -5, Date = new DateTime(2017, 06, 04), TournamentId = 1  }
                );
                SaveChanges();
            }
            FixRanks();
        }
        public void FillPlayers()
        {
            if (PlayerItems.Count() == 0)
            {
                PlayerItems.AddRange(
                    new Player { Name = "Mark", Rank = 1, Score = 995, PlayingSince = new DateTime(2017, 06, 02) },
                    new Player { Name = "Frankie", Rank = 1, Score = 990, PlayingSince = new DateTime(2017, 06, 03) },
                    new Player { Name = "Iris", Rank = 1, Score = 1015, PlayingSince = new DateTime(2017, 06, 04) }
                );
                SaveChanges();
            }
        }

        public void FixRanks()
        {
           var playerList = PlayerItems.OrderByDescending(e => e.Score).ToList();
           for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].Rank = i + 1;
            }
            SaveChanges();
        }
    }
}
