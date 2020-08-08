using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace UnityCode
{
    public class CurrentBeatmapCount : MonoBehaviour
    {
        public static int CurrentMapCount { get; private set; }

        private void OnEnable()
        {
            if (CurrentMapCount == 0)
            {
                CurrentMapCount = GetCurrentBeatmapCount();
            }
        }
        
        private static int GetCurrentBeatmapCount()
        {
            using WebClient webClient = new WebClient();

            string allData =
                webClient.DownloadString("http://scoresaber.com/api.php?function=get-leaderboards&page=1&limit=10000&unique=1&ranked=1");

            return (JObject.Parse(allData)["songs"] ?? throw new NullReferenceException()).Count();
        }
    }
}