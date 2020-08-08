using System;
using System.IO;
using Newtonsoft.Json;

namespace UnityCode
{
    [Serializable]
    public class Options
    {
        static Options()
        {
            string settingsPath = "Settings.json";
            if (!File.Exists(settingsPath))
            {
                Default = new Options();
                File.WriteAllText(settingsPath, JsonConvert.SerializeObject(Default));
            }
            else
            {
                Default = JsonConvert.DeserializeObject<Options>(File.ReadAllText(settingsPath)); 
            }
        }
        public static Options Default { get; }

        public bool UseOutputImage { get; set; } = true;
    }
}