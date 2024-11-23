using RockPaperScissors.Models.Enums;

namespace RockPaperScissors.Models
{
    public class StatisticsModel
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int Games => Wins + Losses + Draws;
        public double WinRate => Wins / Games * 100; 
        public Dictionary<Hand, (int played, int won)> PlayedHands { get; set; }

        public StatisticsModel() 
        {
            PlayedHands = new Dictionary<Hand, (int played, int won)>
            {
                { Hand.Rock, (0, 0) },
                { Hand.Paper, (0, 0) },
                { Hand.Scissors, (0, 0) }
            };
        }

        public StatisticsModel(int wins, int losses, int draws, Dictionary<Hand, (int played, int won)> playedHands)
        {
            Wins = wins;
            Losses = losses;
            Draws = draws;
            PlayedHands = playedHands;
        }
    }
}
