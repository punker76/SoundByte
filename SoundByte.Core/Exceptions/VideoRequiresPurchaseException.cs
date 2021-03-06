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

using System;

namespace SoundByte.Core.Exceptions
{
    /// <summary>
    /// Thrown when the video is a paid Youtube Red video since then we have no access to more video details
    /// </summary>
    public class VideoRequiresPurchaseException : Exception
    {
        /// <summary>
        /// Id of the free preview video
        /// </summary>
        public string PreviewVideoId { get; }

        /// <inheritdoc />
        public VideoRequiresPurchaseException(string previewVideoId)
            : base("The video is a paid Youtube Red video and cannot be processed")
        {
            PreviewVideoId = previewVideoId;
        }
    }
}
