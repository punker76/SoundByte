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

using Newtonsoft.Json;

namespace SoundByte.API.Items.User
{
    public class FanburstUser : IUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        public BaseUser AsBaseUser=> ToBaseUser();

        public BaseUser ToBaseUser()
        {
            return new BaseUser
            {
                ServiceType = ServiceType.Fanburst,
                Id = Id,
                Username = Name,
                ArtworkLink = AvatarUrl,
                Country = Location,
                PermalinkUri = Permalink,
                TrackCount = 0,
                FollowersCount = 0,
                PlaylistCount = 0,
                FollowingsCount = 0
            };
        }
    }
}