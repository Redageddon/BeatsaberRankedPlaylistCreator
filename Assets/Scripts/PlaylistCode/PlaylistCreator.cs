using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityCode;

namespace PlaylistCode
{
    /// <summary>
    ///     The class responsible for turning the inputted data into new <see cref="BeatMapPlaylist" />s.
    /// </summary>
    public static class PlaylistCreator
    {
        private const string BeatSaverLink = "https://beatsaver.com";
        private const string MockBrowserName = "user-agent";

        private const string MockBrowserValue =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";

        private const string ScoreSaberApiLink = "http://scoresaber.com/api.php?function=get-leaderboards&page=1";

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlaylistCreator" /> class.
        /// </summary>
        /// <param name="mapCount"> The amount of beatmaps that are going to be queued. </param>
        /// <param name="playlistSize"> The amount of beatmaps in each beatmap playlist. </param>
        /// <param name="type"> The category type of beatmap playlist. </param>
        public static async Task CreateAllLists(int mapCount, int playlistSize, CatType type)
        {
            string categoryType = $"&cat={(int)type}";
            string limit = $"&limit={mapCount}";
            const string isUnique = "&unique=1";
            const string isRanked = "&ranked=1";

            (IEnumerable<string> allIds, IEnumerable<string> allNames) = await GetAllIdsAndNames(categoryType, limit, isUnique, isRanked);
            LogAllNames(allNames);
            await CreateLists(mapCount, playlistSize, allIds, type);
        }

        private static async Task<(IEnumerable<string>, IEnumerable<string>)> GetAllIdsAndNames(
            string categoryType,
            string limit,
            string isUnique,
            string isRanked)
        {
            using WebClient webClient = new WebClient();
            string allData = await webClient.DownloadStringTaskAsync(ScoreSaberApiLink + categoryType + limit + isUnique + isRanked);

            JToken allSongs = JObject.Parse(allData)["songs"] ?? throw new NullReferenceException();

            IEnumerable<string> allIds = allSongs.Select(i => i["id"].ToString().ToUpper());
            IEnumerable<string> allNames = allSongs.Select(i => i["name"].ToString());

            return categoryType == "&cat=1" ? (allIds.Reverse(), allNames.Reverse()) : (allIds, allNames);
        }

        private static async Task CreateLists(int mapCount, int playlistSize, IEnumerable<string> idList, CatType catType)
        {
            IEnumerable<Task> tasks = Enumerable.Range(0, (int)Math.Ceiling(mapCount / (double)playlistSize))
                                                .Select(i => CreateBeatMapPlaylist(mapCount, playlistSize, idList, catType, i));

            await Task.WhenAll(tasks);
            ProcessedBeatmapNotifier.SendNotification($"Playlist size: {playlistSize}, Category: {catType}, finished");
        }

        private static async Task CreateBeatMapPlaylist(int mapCount, int playlistSize, IEnumerable<string> idList, CatType catType, int i)
        {
            int trueValue = Math.Min(mapCount - (playlistSize * i), playlistSize);

            HashBase[] hashBases = new HashBase[trueValue];

            for (int j = 0; j < trueValue; j++)
            {
                hashBases[j] = new HashBase { hash = idList.ElementAt(j + (playlistSize * i)) };
            }

            byte[] imageData = await GetImageData(idList.ElementAt(i * playlistSize));
            new BeatMapPlaylist(imageData, hashBases, i, playlistSize, catType);
        }

        private static async Task<byte[]> GetImageData(string hash)
        {
            try
            {
                using WebClient webClient = new WebClient();
                webClient.Headers.Add(MockBrowserName, MockBrowserValue);

                string allData = await webClient.DownloadStringTaskAsync(BeatSaverLink + "/api/maps/by-hash/" + hash);
                string url = BeatSaverLink + JObject.Parse(allData)["coverURL"];

                webClient.Headers.Add(MockBrowserName, MockBrowserValue);

                byte[] imageBytes = await webClient.DownloadDataTaskAsync(url);

                return imageBytes;
            }
            catch (Exception)
            {
                return Array.Empty<byte>();
            }
        }

        private static void LogAllNames(IEnumerable<string> allNames)
        {
            foreach (string name in allNames)
            {
                BeatmapLog.BeatmapNames.Add(name);
            }
        }
    }
}