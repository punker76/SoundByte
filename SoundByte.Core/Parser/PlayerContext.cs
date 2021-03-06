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

namespace SoundByte.Core.Parser
{
    internal class PlayerContext
    {
        public string SourceUrl { get; }

        public string Sts { get; }

        public PlayerContext(string sourceUrl, string sts)
        {
            SourceUrl = sourceUrl;
            Sts = sts;
        }
    }
}