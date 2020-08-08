using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace UnityCode
{
    [Serializable]
    public class BeatmapLog
    {
        private static string logPath = "Log.json";
        static BeatmapLog()
        {
            previousMaps = new string[0];
            if (!File.Exists(logPath))
            {
                File.WriteAllText(logPath, "");
            }
            else
            {
                previousMaps = JsonConvert.DeserializeObject<BeatmapLog>(File.ReadAllText(logPath)).AllMaps;
            }
        }

        private static readonly IEnumerable<string> previousMaps;
        
        public static List<string> BeatmapNames { get; } = new List<string>();

        public IEnumerable<string> NewMaps { get; set; }
        public IEnumerable<string> AllMaps { get; set; }

        public static void SaveLog()
        {
            BeatmapLog beatmapLog = new BeatmapLog
            {
                NewMaps = BeatmapNames.Distinct().Where(e => !previousMaps.Contains(e)),
                AllMaps = BeatmapNames.Distinct(),
            };
            File.WriteAllText(logPath, JsonConvert.SerializeObject(beatmapLog, Formatting.Indented));
        }
    }
}