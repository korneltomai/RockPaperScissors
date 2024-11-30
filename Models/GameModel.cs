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
            await UpdateStatistics(outcome);
            await _gameState.UpdateStatistics();

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

        private async Task UpdateStatistics(Outcome outcome)
        {
            switch (outcome)
            {
                case Outcome.Win:
                    int wins = await _localStorage.ContainKeyAsync("wins") ? await _localStorage.GetItemAsync<int>("wins") : default;
                    await _localStorage.SetItemAsync<int>("wins", ++wins);
                    break;
                case Outcome.Draw:
                    int draws = await _localStorage.ContainKeyAsync("draws") ? await _localStorage.GetItemAsync<int>("draws") : default;
                    await _localStorage.SetItemAsync<int>("draws", ++draws);
                    break;
                case Outcome.Loss:
                    int losses = await _localStorage.ContainKeyAsync("losses") ? await _localStorage.GetItemAsync<int>("losses") : default;
                    await _localStorage.SetItemAsync<int>("losses", ++losses);
                    break;
            }
        }
    }

    public enum Hand { Rock, Paper, Scissors }
    public enum Outcome { Win, Draw, Loss }
}
