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
    [Serializable]
    public class SoundByteNotLoadedException : Exception
    {
        public SoundByteNotLoadedException() : base("The SoundByte service has not been loaded. Cannot continue.")
        {
        }
    }
}
