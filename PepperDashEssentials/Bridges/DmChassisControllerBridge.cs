﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.DeviceSupport;

using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.DM;

namespace PepperDash.Essentials.Bridges
{
    public static class DmChassisControllerApiExtentions 
    {
        public static void LinkToApi(this DmChassisController dmChassis, BasicTriList trilist, uint joinStart, string joinMapKey)
        {
            var joinMap = JoinMapHelper.GetJoinMapForDevice(joinMapKey) as DmChassisControllerJoinMap;

            if (joinMap == null)
                joinMap = new DmChassisControllerJoinMap();

            joinMap.OffsetJoinNumbers(joinStart);

            Debug.Console(1, dmChassis, "Linking device '{0}' to Trilist '{1}'", dmChassis.Key, trilist.ID);

            dmChassis.IsOnline.LinkInputSig(trilist.BooleanInput[joinMap.IsOnline]);

            // Link up outputs
            for (uint i = 1; i <= dmChassis.Chassis.NumberOfOutputs - 1; i++)
            {
                var ioSlot = i;

                // Control
                trilist.SetUShortSigAction(joinMap.OutputVideo + ioSlot, new Action<ushort>(o => dmChassis.ExecuteSwitch(o, ioSlot, eRoutingSignalType.Video)));
                trilist.SetUShortSigAction(joinMap.OutputAudio + ioSlot, new Action<ushort>(o => dmChassis.ExecuteSwitch(o, ioSlot, eRoutingSignalType.Audio)));

                // Feedback
                dmChassis.VideoOutputFeedbacks[ioSlot].LinkInputSig(trilist.UShortInput[joinMap.OutputVideo + ioSlot]);
                dmChassis.AudioOutputFeedbacks[ioSlot].LinkInputSig(trilist.UShortInput[joinMap.OutputAudio + ioSlot]);

                dmChassis.VideoInputSyncFeedbacks[ioSlot].LinkInputSig(trilist.BooleanInput[joinMap.VideoSyncStatus + ioSlot]);

                dmChassis.OutputNameFeedbacks[ioSlot].LinkInputSig(trilist.StringInput[joinMap.OutputNames + ioSlot]);
                dmChassis.InputNameFeedbacks[ioSlot].LinkInputSig(trilist.StringInput[joinMap.InputNames + ioSlot]);
                dmChassis.OutputVideoRouteNameFeedbacks[ioSlot].LinkInputSig(trilist.StringInput[joinMap.OutputCurrentVideoInputNames + ioSlot]);
                dmChassis.OutputAudioRouteNameFeedbacks[ioSlot].LinkInputSig(trilist.StringInput[joinMap.OutputCurrentAudioInputNames + ioSlot]);



            }
        }


        public class DmChassisControllerJoinMap : JoinMapBase
        {
            public uint IsOnline { get; set; }
            public uint OutputVideo { get; set; }
            public uint OutputAudio { get; set; }
            public uint VideoSyncStatus { get; set; }
            public uint InputNames { get; set; }
            public uint OutputNames { get; set; }
            public uint OutputCurrentVideoInputNames { get; set; }
            public uint OutputCurrentAudioInputNames { get; set; }
            public uint InputCurrentResolution { get; set; }
            public uint InputSlotOnline { get; set; }
            //public uint HdcpSupport { get; set; }
            //public uint HdcpSupportCapability { get; set; }


            public DmChassisControllerJoinMap()
            {
                IsOnline = 11;
                OutputVideo = 100; //101-299
                OutputAudio = 300; //301-499
                VideoSyncStatus = 100; //101-299
                InputNames = 100; //101-299
                OutputNames = 300; //301-499
                OutputCurrentVideoInputNames = 2000; //2001-2199
                OutputCurrentAudioInputNames = 2200; //2201-2399
                InputCurrentResolution = 2400; // 2401-2599
                InputSlotOnline = 500;
                //HdcpSupport = 1000; //1001-1199
               //HdcpSupportCapability = 1200; //1201-1399

            }

            public override void OffsetJoinNumbers(uint joinStart)
            {
                var joinOffset = joinStart - 1;

                IsOnline = IsOnline + joinOffset;
                OutputVideo = OutputVideo + joinOffset;
                OutputAudio = OutputAudio + joinOffset;
                VideoSyncStatus = VideoSyncStatus + joinOffset;
                InputNames = InputNames + joinOffset;
                OutputNames = OutputNames + joinOffset;
                OutputCurrentVideoInputNames = OutputCurrentVideoInputNames + joinOffset;
                OutputCurrentAudioInputNames = OutputCurrentAudioInputNames + joinOffset;
                InputCurrentResolution = InputCurrentResolution + joinOffset;
                InputSlotOnline = InputSlotOnline + joinOffset;
                //HdcpSupport = HdcpSupport + joinOffset;
                //HdcpSupportCapability = HdcpSupportCapability + joinOffset;
            }
        }
    }
}