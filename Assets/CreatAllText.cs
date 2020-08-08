using UnityCode;
using UnityEngine;
using UnityEngine.UI;

public class CreatAllText : MonoBehaviour
{
    [SerializeField] private Text text;
    private void Start() =>
        this.text.text =
            $"Creates playlists with sizes of {CurrentBeatmapCount.CurrentMapCount}, 50, 30, 25, 20. With each of Date ranked, Difficulty, and Play Count";
}
