using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityCode;

namespace PlaylistCode
{
    /// <summary>
    ///     The class responsible for actually creating the image and beatmap playlist file.
    /// </summary>
    public class BeatMapPlaylist
    {
        private const    string Base64ImagePrefix = "data:image/jpeg;base64,";

        [JsonProperty] private string     playlistAuthor;
        [JsonProperty] private string     playlistTitle;
        [JsonProperty] private string     image;
        [JsonProperty] private IEnumerable<HashBase> songs;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BeatMapPlaylist" /> class.
        /// </summary>
        /// <param name="image"> The data of the jpg to be saved. </param>
        /// <param name="songs"> All Song id's. </param>
        /// <param name="fileNumber"> The playlist number. </param>
        /// <param name="playlistSize"> The amount of maps per file. </param>
        /// <param name="catType"> The category type. </param>
        public BeatMapPlaylist(byte[] image, IEnumerable<HashBase> songs, int fileNumber, int playlistSize, CatType catType)
        {
            this.playlistTitle  = $"{catType} Part{fileNumber}";
            this.image          = Base64ImagePrefix + Convert.ToBase64String(image);
            this.songs          = songs;
            this.playlistAuthor = "Rubiksmaster02";

            string path = $"Playlists/BeatmapSize{playlistSize}/{catType}/";

            Directory.CreateDirectory(path);

            path += $"{catType} part{fileNumber}";

            if (Options.Default.UseOutputImage)
            {
                File.WriteAllBytes(path + ".jpg", image);
            }
            
            File.WriteAllText(path + ".bplist", JsonConvert.SerializeObject(this, Formatting.Indented));

            ProcessedBeatmapNotifier.SendNotification(
                $"Playlist size: {playlistSize}, Category: {catType}, File number: {fileNumber}, has been processed.");
        }
    }
}