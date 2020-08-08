using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PlaylistCode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityCode
{
    public class CreateAll : MonoBehaviour
    {
        public async void OnClick()
        {
            LoadLoggingScene();
            
            ProcessedBeatmapNotifier.SendNotification("Calculating max beatmap count...");
            int maxCount = await GetCurrentBeatmapCount();
            
            ProcessedBeatmapNotifier.SendNotification("Beatmap processing started...");
            await CreateAllPlaylistCreators(maxCount);
            
            ProcessedBeatmapNotifier.SendNotification("All beatmaps processed.");
            BeatmapLog.SaveLog();
            ProcessedBeatmapNotifier.SendNotification("You can view all new beatmaps in the log.");
        }

        private static void LoadLoggingScene() => SceneManager.LoadScene("LoggingScene", LoadSceneMode.Single);

        private static async Task<int> GetCurrentBeatmapCount()
        {
            using WebClient webClient = new WebClient();

            string allData =
                await webClient.DownloadStringTaskAsync(
                    "http://scoresaber.com/api.php?function=get-leaderboards&page=1&limit=10000&unique=1&ranked=1");

            return (JObject.Parse(allData)["songs"] ?? throw new NullReferenceException()).Count();
        }

        private static async Task CreateAllPlaylistCreators(int count) =>
            await Task.WhenAll(PlaylistCreator.CreateAllLists(count, 20,    CatType.DateRanked),
                               PlaylistCreator.CreateAllLists(count, 20,    CatType.PlayCount),
                               PlaylistCreator.CreateAllLists(count, 20,    CatType.Difficulty),
                               PlaylistCreator.CreateAllLists(count, 25,    CatType.DateRanked),
                               PlaylistCreator.CreateAllLists(count, 25,    CatType.PlayCount),
                               PlaylistCreator.CreateAllLists(count, 25,    CatType.Difficulty),
                               PlaylistCreator.CreateAllLists(count, 30,    CatType.DateRanked),
                               PlaylistCreator.CreateAllLists(count, 30,    CatType.PlayCount),
                               PlaylistCreator.CreateAllLists(count, 30,    CatType.Difficulty),
                               PlaylistCreator.CreateAllLists(count, 50,    CatType.DateRanked),
                               PlaylistCreator.CreateAllLists(count, 50,    CatType.PlayCount),
                               PlaylistCreator.CreateAllLists(count, 50,    CatType.Difficulty),
                               PlaylistCreator.CreateAllLists(count, count, CatType.DateRanked),
                               PlaylistCreator.CreateAllLists(count, count, CatType.PlayCount),
                               PlaylistCreator.CreateAllLists(count, count, CatType.Difficulty));
    }
}