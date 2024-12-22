using Blazored.LocalStorage;
using RockPaperScissors.Services;

namespace RockPaperScissors.Models
{
    public class GameModel
    {
        private readonly ILocalStorageService _localStorage;
        private readonly GameStateService _gameState;

        public GameModel(ILocalStorageService localStorage, GameStateService statistics)
        {
            _localStorage = localStorage;
            _gameState = statistics;
        }

        public async Task<Outcome> PlayRound(Hand playerHand, Hand oppoentHand)
        {
            Outcome outcome = DetermineWin(playerHand, oppoentHand);
            await _gameState.UpdateStatistics(outcome);

            return outcome;
        }

        private Outcome DetermineWin(Hand playerHand, Hand oppoentHand)
        {
            if (playerHand == Hand.Rock && oppoentHand == Hand.Scissors) 
                return Outcome.Win;
            if (playerHand == Hand.Paper && oppoentHand == Hand.Rock) 
                return Outcome.Win;
            if (playerHand == Hand.Scissors && oppoentHand == Hand.Paper) 
                return Outcome.Win;

            if (playerHand == oppoentHand) 
                return Outcome.Draw;

            return Outcome.Loss;
        }
    }

    public enum Hand { Rock, Paper, Scissors }
    public enum Outcome { Win, Draw, Loss }
}
