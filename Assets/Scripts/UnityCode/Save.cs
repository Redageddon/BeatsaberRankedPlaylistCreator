using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityCode
{
    public class Save : MonoBehaviour
    {
        public void OnClicked() => 
            File.WriteAllText("Settings.json", JsonConvert.SerializeObject(Options.Default));
    }
}