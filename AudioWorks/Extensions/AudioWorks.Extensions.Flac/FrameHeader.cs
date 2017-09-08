﻿using System.Runtime.InteropServices;

namespace AudioWorks.Extensions.Flac
{
    [StructLayout(LayoutKind.Explicit)]
    struct FrameHeader
    {
        [FieldOffset(0)] readonly uint BlockSize;

        [FieldOffset(4)] readonly uint SampleRate;

        [FieldOffset(8)] readonly uint Channels;

        [FieldOffset(12)] readonly int ChannelAssignment;

        [FieldOffset(16)] readonly uint BitsPerSample;

        [FieldOffset(20)] readonly int NumberType;

        [FieldOffset(24)] readonly uint FrameNumber;

        [FieldOffset(24)] readonly ulong SampleNumber;

        [FieldOffset(32)] readonly byte Crc;
    }
}