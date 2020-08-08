using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PlaylistCode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityCode
{
    public class CreateSpecificHandler : MonoBehaviour
    {
        [SerializeField] private Text text;
        private int beatmapCount;
        private int playlistSize;
        private CatType catType;

        private async void Start() => this.text.text = $"How many beatmaps do you want? There are currently {await GetCurrentBeatmapCount()}.";

        public void BeatmapCountValueChanged(string value) => int.TryParse(value, out this.beatmapCount);
        
        public void PlaylistSizeValueChanged(string value) => int.TryParse(value, out this.playlistSize);
        
        public void CategoryValueChanged(int value) => this.catType = (CatType)value;

        public async void CreatePlaylistOnClick()
        {
            LoadLoggingScene();
            
            ProcessedBeatmapNotifier.SendNotification("Beatmap processing started...");
            await PlaylistCreator.CreateAllLists(this.beatmapCount, this.playlistSize, this.catType);
            
            ProcessedBeatmapNotifier.SendNotification("All beatmaps processed.");
            BeatmapLog.SaveLog();
            ProcessedBeatmapNotifier.SendNotification("You can view all new beatmaps in the log.");
        }
        
        private static async Task<int> GetCurrentBeatmapCount()
        {
            using WebClient webClient = new WebClient();

            string allData =
                await webClient.DownloadStringTaskAsync(
                    "http://scoresaber.com/api.php?function=get-leaderboards&page=1&limit=10000&unique=1&ranked=1");

            return (JObject.Parse(allData)["songs"] ?? throw new NullReferenceException()).Count();
        }

        private static void LoadLoggingScene() => SceneManager.LoadScene("LoggingScene", LoadSceneMode.Single);
    }
}