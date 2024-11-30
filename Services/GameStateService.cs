using Blazored.LocalStorage;

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
        public string RockName { get; set; }
        public string PaperName { get; set; }
        public string ScissorsName { get; set; }

        public GameStateService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            OnChange = new List<Action>();
            RockName = string.Empty;
            PaperName = string.Empty;
            ScissorsName = string.Empty;
        }

        public async Task UpdateStatistics()
        {
            Wins = await _localStorage.GetItemAsync<int>("wins");
            Draws = await _localStorage.GetItemAsync<int>("draws");
            Losses = await _localStorage.GetItemAsync<int>("losses");

            NotifyStateChanged();
        }

        public async Task UpdateNames()
        {
            RockName = await _localStorage.GetItemAsync<string>("rockName") ?? "Rock";
            PaperName = await _localStorage.GetItemAsync<string>("paperName") ?? "Paper";
            ScissorsName = await _localStorage.GetItemAsync<string>("scissorsName") ?? "Scissors";

            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            foreach (Action action in OnChange)
                action.Invoke();
        }
    }
}
