using Checkers.Models;
using Checkers.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Checkers.Repositories
{
    public class GamesDataRepository
    {
        private const string StatisticsFileName = "statistics.json";

        public GamesDataRepository()
        {
            GameStatistics = LoadGameStatistics();
        }

        public ObservableCollection<GameStatistics> GameStatistics { get; set; }

        public void SaveGameStatistics(GameStatistics gameStatistics)
        {
            try
            {
                GameStatistics.Add(gameStatistics);
                string json = JsonService.Serialize(GameStatistics.ToList());
                File.WriteAllText(StatisticsFileName, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving statistics: {ex.Message}");
            }
        }

        public int WhiteWins()
        {
            return GameStatistics.Count(gs => gs.Winner == PlayerType.White);
        }

        public int BlackWins()
        {
            return GameStatistics.Count(gs => gs.Winner == PlayerType.Black);
        }

        public int TotalGames()
        {
            return GameStatistics.Count;
        }

        private ObservableCollection<GameStatistics> LoadGameStatistics()
        {
            try
            {
                if (!File.Exists(StatisticsFileName))
                {
                    File.Create(StatisticsFileName).Dispose();
                    return new ObservableCollection<GameStatistics>();
                }

                string fileContent = File.ReadAllText(StatisticsFileName);
                List<GameStatistics> statisticsList = JsonService.Deserialize<List<GameStatistics>>(fileContent);

                return new ObservableCollection<GameStatistics>(statisticsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading statistics: {ex.Message}");
                return new ObservableCollection<GameStatistics>();
            }
        }
    }
}
