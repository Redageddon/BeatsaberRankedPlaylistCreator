using System;
using System.IO;
using Newtonsoft.Json;

namespace UnityCode
{
    [Serializable]
    public class Options
    {
        private const string settingsPath = "Settings.json";
        
        static Options()
        {
            if (!File.Exists(settingsPath))
            {
                Default = new Options();
                
                string json = JsonConvert.SerializeObject(Default);
                
                File.WriteAllText(settingsPath,json);
            }
            else
            {
                Default = JsonConvert.DeserializeObject<Options>(File.ReadAllText(settingsPath)); 
            }
        }
        public static Options Default { get; }

        public bool UseOutputImage { get; set; } = false;
    }
}