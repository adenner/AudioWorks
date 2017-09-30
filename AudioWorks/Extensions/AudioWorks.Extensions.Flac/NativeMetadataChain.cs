﻿using JetBrains.Annotations;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AudioWorks.Extensions.Flac
{
    sealed class NativeMetadataChain : IDisposable
    {
        [NotNull] readonly NativeMetadataChainHandle _handle = SafeNativeMethods.MetadataChainNew();
        readonly IoCallbacks _callbacks;

        internal NativeMetadataChain([NotNull] Stream stream)
        {
            _callbacks = InitializeCallbacks(stream);
        }

        internal void Read()
        {
            SafeNativeMethods.MetadataChainReadWithCallbacks(_handle, _callbacks);
        }

        internal bool CheckIfTempFileNeeded(bool usePadding)
        {
            return SafeNativeMethods.MetadataChainCheckIfTempFileNeeded(_handle, usePadding);
        }

        internal void Write(bool usePadding)
        {
            SafeNativeMethods.MetadataChainWriteWithCallbacks(_handle, usePadding, _callbacks);
        }

        internal void WriteWithTempFile(bool usePadding, [NotNull] Stream tempStream)
        {
            SafeNativeMethods.MetadataChainWriteWithCallbacksAndTempFile(
                _handle,
                usePadding,
                _callbacks,
                InitializeCallbacks(tempStream));
        }

        [NotNull]
        internal NativeMetadataIterator GetIterator()
        {
            return new NativeMetadataIterator(_handle);
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
                    var managedBuffer = new byte[totalBufferSize];
                    var bytesRead = stream.Read(managedBuffer, 0, totalBufferSize);
                    Marshal.Copy(managedBuffer, 0, readBuffer, totalBufferSize);
                    return new IntPtr(bytesRead);
                },

                Write = (writeBuffer, bufferSize, numberOfRecords, handle) =>
                {
                    var castNumberOfRecords = numberOfRecords.ToInt32();
                    var totalBufferSize = bufferSize.ToInt32() * castNumberOfRecords;
                    var managedBuffer = new byte[totalBufferSize];
                    Marshal.Copy(writeBuffer, managedBuffer, 0, totalBufferSize);
                    stream.Write(managedBuffer, 0, totalBufferSize);
                    return new IntPtr(castNumberOfRecords);
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