﻿/* Copyright © 2018 Jeremy Herbison

This file is part of AudioWorks.

AudioWorks is free software: you can redistribute it and/or modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
version.

AudioWorks is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
details.

You should have received a copy of the GNU Lesser General Public License along with AudioWorks. If not, see
<https://www.gnu.org/licenses/>. */

using System;
using System.IO;
using System.Runtime.InteropServices;
using AudioWorks.Extensibility;
using JetBrains.Annotations;

namespace AudioWorks.Extensions.Flac
{
    sealed class AudioStreamDecoder : AudioInfoStreamDecoder
    {
        internal SampleBuffer Samples { get; set; }

        internal AudioStreamDecoder([NotNull] Stream stream)
            : base(stream)
        {
        }

        internal bool ProcessSingle()
        {
            return SafeNativeMethods.StreamDecoderProcessSingle(Handle);
        }

        protected override unsafe DecoderWriteStatus WriteCallback(IntPtr handle, ref Frame frame, IntPtr buffer,
            IntPtr userData)
        {
            if (frame.Header.Channels == 1)
                Samples = new SampleBuffer(
                    new Span<int>(
                        Marshal.ReadIntPtr(buffer).ToPointer(),
                        (int) frame.Header.BlockSize),
                    (int) frame.Header.BitsPerSample);
            else
                Samples = new SampleBuffer(
                    new Span<int>(
                        Marshal.ReadIntPtr(buffer).ToPointer(),
                        (int) frame.Header.BlockSize),
                    new Span<int>(
                        Marshal.ReadIntPtr(buffer, IntPtr.Size).ToPointer(),
                        (int) frame.Header.BlockSize),
                    (int) frame.Header.BitsPerSample);

            return DecoderWriteStatus.Continue;
        }
    }
}