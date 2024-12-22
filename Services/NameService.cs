using RockPaperScissors.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace RockPaperScissors.Services
{
    public class NameService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public List<Action> OnChange;
        public string RockName { get; set; }
        public string PaperName { get; set; }
        public string ScissorsName { get; set; }

        public NameService(ILocalStorageService localStorage, IConfiguration configuration, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _configuration = configuration;
            _httpClient = httpClient;
            OnChange = new List<Action>();
            RockName = string.Empty;
            PaperName = string.Empty;
            ScissorsName = string.Empty;
        }

        public async Task OnNameChanged(Hand hand, string? newName)
        {
            switch (hand)
            {
                case Hand.Rock:
                    RockName = newName ?? "Rock";
                    await _localStorage.SetItemAsync<string>("rockName", RockName);
                    break;
                case Hand.Paper:
                    PaperName = newName ?? "Paper";
                    await _localStorage.SetItemAsync<string>("paperName", PaperName);
                    break;
                case Hand.Scissors:
                    ScissorsName = newName ?? "Scissors";
                    await _localStorage.SetItemAsync<string>("scissorsName", ScissorsName);
                    break;
            }

            NotifyStateChanged();
        }

        public async Task GetRandomNames()
        {
            try
            {
                string[] randomWords = new string[3];
                for (int i = 0; i < 3; i++)
                    while (randomWords[i] == null || randomWords[i].Length > 10)
                    {
                        var result = (await _httpClient.GetFromJsonAsync<string[]>(_configuration["RandomWordApiBaseUrl"] + "random/noun"))!;
                        string word = result[0];

                        // Makes the first letter of the word uppercase to look better when displaying
                        randomWords[i] = word switch
                        {
                            null => throw new ArgumentNullException(nameof(word)),
                            _ => string.Concat(word[0].ToString().ToUpper(), word.AsSpan(1))
                        };
                    }

                RockName = randomWords[0];
                PaperName = randomWords[1];
                ScissorsName = randomWords[2];

                await _localStorage.SetItemAsync<string>("rockName", randomWords[0]);
                await _localStorage.SetItemAsync<string>("paperName", randomWords[1]);
                await _localStorage.SetItemAsync<string>("scissorsName", randomWords[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }

            NotifyStateChanged();
        }

        public async Task LoadNames()
        {
            RockName = await _localStorage.GetItemAsync<string>("rockName") ?? "Rock";
            PaperName = await _localStorage.GetItemAsync<string>("paperName") ?? "Paper";
            ScissorsName = await _localStorage.GetItemAsync<string>("scissorsName") ?? "Scissors";

            NotifyStateChanged();
        }

        public async Task ResetNames()
        {
            RockName = "Rock";
            PaperName = "Paper";
            ScissorsName = "Scissors";

            await _localStorage.SetItemAsync<string>("rockName", "Rock");
            await _localStorage.SetItemAsync<string>("paperName", "Paper");
            await _localStorage.SetItemAsync<string>("scissorsName", "Scissors");

            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            foreach (Action action in OnChange)
                action.Invoke();
        }
    }
}
