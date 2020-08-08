#nullable enable
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCode
{
    public class NotifyTextUpdater : MonoBehaviour
    {
        [SerializeField] private Text text = null!;
        
        public void Update() => this.text.text = string.Join(Environment.NewLine, ProcessedBeatmapNotifier.GetNotifications);
    }
}