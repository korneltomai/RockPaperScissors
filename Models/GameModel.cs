using RockPaperScissors.Models.Enums;

namespace RockPaperScissors.Models
{
    public class GameModel
    {
        public StatisticsModel Statistics { get; set; }

        public GameModel()
        {
            Statistics = GetStatistics();
        }

        public bool PlayRound(Hand playerHand, Hand oppoentHand)
        {
            bool PlayerWon = DetermineWin(playerHand, oppoentHand);
            return PlayerWon;
        }

        // TODO: Get statistics from local storage, if available.
        private StatisticsModel GetStatistics()
        {
            return new StatisticsModel();
        }

        private bool DetermineWin(Hand playerHand, Hand oppoentHand)
        {
            if (playerHand == Hand.Rock && oppoentHand == Hand.Scissors) return true;
            if (playerHand == Hand.Paper && oppoentHand == Hand.Rock) return true;
            if (playerHand == Hand.Scissors && oppoentHand == Hand.Paper) return true;
            return false;
        }
    }
}
