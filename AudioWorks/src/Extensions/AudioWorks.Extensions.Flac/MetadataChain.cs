﻿/* Copyright © 2018 Jeremy Herbison

This file is part of AudioWorks.

AudioWorks is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public
License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
version.

AudioWorks is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
details.

You should have received a copy of the GNU Affero General Public License along with AudioWorks. If not, see
<https://www.gnu.org/licenses/>. */

using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace AudioWorks.Extensions.Flac
{
    sealed class MetadataChain : IDisposable
    {
        [NotNull] readonly MetadataChainHandle _handle = SafeNativeMethods.MetadataChainNew();
        readonly IoCallbacks _callbacks;

        internal MetadataChain([NotNull] Stream stream)
        {
            _callbacks = InitializeCallbacks(stream);
        }

        internal void Read()
        {
            SafeNativeMethods.MetadataChainReadWithCallbacks(_handle, IntPtr.Zero, _callbacks);
        }

        internal bool CheckIfTempFileNeeded(bool usePadding)
        {
            return SafeNativeMethods.MetadataChainCheckIfTempFileNeeded(_handle, usePadding);
        }

        internal void Write(bool usePadding)
        {
            SafeNativeMethods.MetadataChainWriteWithCallbacks(_handle, usePadding, IntPtr.Zero, _callbacks);
        }

        internal void WriteWithTempFile(bool usePadding, [NotNull] Stream tempStream)
        {
            SafeNativeMethods.MetadataChainWriteWithCallbacksAndTempFile(
                _handle,
                usePadding,
                IntPtr.Zero,
                _callbacks,
                IntPtr.Zero,
                InitializeCallbacks(tempStream));
        }

        [NotNull]
        internal MetadataIterator GetIterator()
        {
            return new MetadataIterator(_handle);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }

        static IoCallbacks InitializeCallbacks([NotNull] Stream stream)
        {
            return new IoCallbacks
            {
                Read = (readBuffer, bufferSize, numberOfRecords, handle) =>
                {
                    var totalBufferSize = bufferSize.ToInt32() * numberOfRecords.ToInt32();
                    var buffer = ArrayPool<byte>.Shared.Rent(totalBufferSize);
                    try
                    {
                        var bytesRead = stream.Read(buffer, 0, totalBufferSize);
                        Marshal.Copy(buffer, 0, readBuffer, totalBufferSize);
                        return new IntPtr(bytesRead);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }
                },

                Write = (writeBuffer, bufferSize, numberOfRecords, handle) =>
                {
                    var castNumberOfRecords = numberOfRecords.ToInt32();
                    var totalBufferSize = bufferSize.ToInt32() * castNumberOfRecords;
                    var buffer = ArrayPool<byte>.Shared.Rent(totalBufferSize);
                    try
                    {
                        Marshal.Copy(writeBuffer, buffer, 0, totalBufferSize);
                        stream.Write(buffer, 0, totalBufferSize);
                        return new IntPtr(castNumberOfRecords);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }
                },

                Seek = (handle, offset, whence) =>
                {
                    stream.Seek(offset, whence);
                    return 0;
                },

                Tell = handle => stream.Position,

                Eof = handle => stream.Position < stream.Length ? 0 : 1
            };
        }
    }
}