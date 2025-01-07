using Blazored.LocalStorage;
using RockPaperScissors.Models;

namespace RockPaperScissors.Services
{
    public class GameStateService
    {
        private readonly ILocalStorageService _localStorage;
        public List<Action> OnChange;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int GamesPlayed => Wins + Losses + Draws;
        public int WinRate => GamesPlayed > 0 ? (int)(Convert.ToDouble(Wins) / Convert.ToDouble(GamesPlayed) * 100) : 0;

        public GameStateService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            OnChange = new List<Action>();
        }

        public async Task UpdateStatistics(Outcome outcome)
        {
            switch (outcome)
            {
                case Outcome.Win:
                    Wins++;
                    int wins = await _localStorage.ContainKeyAsync("wins") ? await _localStorage.GetItemAsync<int>("wins") : default;
                    await _localStorage.SetItemAsync<int>("wins", ++wins);
                    break;
                case Outcome.Draw:
                    Draws++;
                    int draws = await _localStorage.ContainKeyAsync("draws") ? await _localStorage.GetItemAsync<int>("draws") : default;
                    await _localStorage.SetItemAsync<int>("draws", ++draws);
                    break;
                case Outcome.Loss:
                    Losses++;
                    int losses = await _localStorage.ContainKeyAsync("losses") ? await _localStorage.GetItemAsync<int>("losses") : default;
                    await _localStorage.SetItemAsync<int>("losses", ++losses);
                    break;
            }

            NotifyStateChanged();
        }

        public async Task LoadStatistics()
        {
            Wins = await _localStorage.GetItemAsync<int>("wins");
            Draws = await _localStorage.GetItemAsync<int>("draws");
            Losses = await _localStorage.GetItemAsync<int>("losses");

            NotifyStateChanged();
        }

        public async Task ResetStatistics()
        {
            Wins = 0;
            Draws = 0;
            Losses = 0;

            await _localStorage.SetItemAsync<int>("wins", 0);
            await _localStorage.SetItemAsync<int>("draws", 0);
            await _localStorage.SetItemAsync<int>("losses", 0);

            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            foreach (Action action in OnChange)
                action.Invoke();
        }
    }
}
