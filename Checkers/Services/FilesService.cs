using Checkers.Models;
using Microsoft.Win32;
using System.IO;

namespace Checkers.Services
{
    public class FilesService
    {
        public void SaveObjectToFile<T>(T objectToSerialize)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Save as JSON"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string jsonString = JsonService.Serialize(objectToSerialize);
                File.WriteAllText(saveFileDialog.FileName, jsonString);
            }
        }

        public T LoadObjectFromFile<T>()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Open JSON File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string jsonString = File.ReadAllText(openFileDialog.FileName);
                return JsonService.Deserialize<T>(jsonString);
            }

            return default;
        }

        public void SaveStatisticsToFile(GameStatistics gameStatistics)
        {
            try
            {
                string fileName = "statistics.json";

                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Dispose();
                }

                List<GameStatistics> statistics = JsonService.Deserialize<List<GameStatistics>>(File.ReadAllText(fileName));

                statistics.Add(gameStatistics);

                File.WriteAllText(fileName, JsonService.Serialize(statistics));
            }
            catch (Exception)
            {
                //MessageBox.Show("Error while saving statistics");
            }

        }

        public List<GameStatistics> LoadStatisticsFromFile()
        {
            try
            {
                string fileName = "statistics.json";

                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Dispose();
                }

                return JsonService.Deserialize<List<GameStatistics>>(File.ReadAllText(fileName));
            }
            catch (Exception)
            {
                // MessageBox.Show("Error while loading statistics");
                return new List<GameStatistics>();
            }
        }

    }
}
