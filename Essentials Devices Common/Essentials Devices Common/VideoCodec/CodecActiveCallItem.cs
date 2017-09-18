﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace PepperDash.Essentials.Devices.Common.VideoCodec

{
    public class CodecActiveCallItem
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public eCodecCallType Type { get; set; }

        public eCodecCallStatus Status { get; set; }

        public string Id { get; set; }
    }

    public enum eCodecCallType
    {
        None, Audio, Video
    }

    public enum eCodecCallStatus
    {
        Dialing, Established, Incoming
    }
}