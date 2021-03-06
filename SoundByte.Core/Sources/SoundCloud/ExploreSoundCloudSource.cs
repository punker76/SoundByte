﻿/* |----------------------------------------------------------------|
 * | Copyright (c) 2017, Grid Entertainment                         |
 * | All Rights Reserved                                            |
 * |                                                                |
 * | This source code is to only be used for educational            |
 * | purposes. Distribution of SoundByte source code in             |
 * | any form outside this repository is forbidden. If you          |
 * | would like to contribute to the SoundByte source code, you     |
 * | are welcome.                                                   |
 * |----------------------------------------------------------------|
 */

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using SoundByte.Core.Helpers;
using SoundByte.Core.Items.Track;
using SoundByte.Core.Services;

namespace SoundByte.Core.Sources.SoundCloud
{
    [UsedImplicitly]
    public class ExploreSoundCloudSource : ISource<BaseTrack>
    {
        /// <summary>
        ///     The genre to search for.
        /// </summary>
        public string Genre { get; set; } = "all-music";

        /// <summary>
        ///     The kind of item to search for
        /// </summary>
        public string Kind { get; set; } = "top";

        public async Task<SourceResponse<BaseTrack>> GetItemsAsync(int count, string token,
            CancellationTokenSource cancellationToken = default(CancellationTokenSource))
        {
            // Call the SoundCloud API and get the items
            var tracks = await SoundByteV3Service.Current.GetAsync<ExploreTrackHolder>(ServiceType.SoundCloudV2, "/charts",
                new Dictionary<string, string>
                {
                    {"genre", "soundcloud%3Agenres%3A" + Genre},
                    {"kind", Kind},
                    {"limit", count.ToString()},
                    {"offset", token},
                    {"linked_partitioning", "1"}
                }, cancellationToken).ConfigureAwait(false);

            // If there are no tracks
            if (!tracks.Tracks.Any())
            {
                return new SourceResponse<BaseTrack>(null, null, false, "No results found", "No items matching");
            }

            // Parse uri for offset
            var param = new QueryParameterCollection(tracks.NextList);
            var nextToken = param.FirstOrDefault(x => x.Key == "offset").Value;

            // Convert SoundCloud specific tracks to base tracks
            var baseTracks = new List<BaseTrack>();
            tracks.Tracks.ForEach(x => baseTracks.Add(x.Track.ToBaseTrack()));

            // Return the items
            return new SourceResponse<BaseTrack>(baseTracks, nextToken);
        }

        [JsonObject]
        private class ExploreTrackHolder
        {
            /// <summary>
            ///     Collection of tracks
            /// </summary>
            [JsonProperty("collection")]
            public List<ExploreTrackCollection> Tracks { get; set; }

            /// <summary>
            ///     The next list of items
            /// </summary>
            [JsonProperty("next_href")]
            public string NextList { get; set; }
        }

        [JsonObject]
        private class ExploreTrackCollection
        {
            /// <summary>
            ///     The track object
            /// </summary>
            [JsonProperty("track")]
            public SoundCloudTrack Track { get; set; }
        }
    }
}
