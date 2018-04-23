﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace AudioWorks.Extensions.Apple
{
    class AudioFile : IDisposable
    {
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
        [NotNull] readonly NativeCallbacks.AudioFileReadCallback _readCallback;
        [NotNull] readonly NativeCallbacks.AudioFileGetSizeCallback _getSizeCallback;
        [CanBeNull] readonly NativeCallbacks.AudioFileWriteCallback _writeCallback;
        [CanBeNull] readonly NativeCallbacks.AudioFileSetSizeCallback _setSizeCallback;
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
        [NotNull] readonly Stream _stream;

        [NotNull]
        protected AudioFileHandle Handle { get; }

        internal AudioFile(AudioFileType fileType, [NotNull] Stream stream)
        {
            // This constructor is for reading
            _readCallback = ReadCallback;
            _getSizeCallback = GetSizeCallback;

            _stream = stream;

            SafeNativeMethods.AudioFileOpenWithCallbacks(IntPtr.Zero,
                _readCallback, null, _getSizeCallback, null,
                fileType, out var handle);
            Handle = handle;
        }

        internal AudioFile(AudioStreamBasicDescription description, AudioFileType fileType, [NotNull] Stream stream)
        {
            // This constructor is for writing
            _readCallback = ReadCallback;
            _getSizeCallback = GetSizeCallback;
            _writeCallback = WriteCallback;
            _setSizeCallback = SetSizeCallback;

            _stream = stream;

            SafeNativeMethods.AudioFileInitializeWithCallbacks(IntPtr.Zero,
                _readCallback, _writeCallback, _getSizeCallback, _setSizeCallback,
                fileType, ref description, 0, out var handle);
            Handle = handle;
        }

        internal IntPtr GetProperty(AudioFilePropertyId id, uint size)
        {
            // Callers must release this!
            var unmanagedValue = Marshal.AllocHGlobal((int) size);
            SafeNativeMethods.AudioFileGetProperty(Handle, id, ref size, unmanagedValue);
            return unmanagedValue;
        }

        internal T GetProperty<T>(AudioFilePropertyId id) where T : struct
        {
            var size = (uint) Marshal.SizeOf(typeof(T));
            var unmanagedValue = Marshal.AllocHGlobal((int)size);
            try
            {
                SafeNativeMethods.AudioFileGetProperty(Handle, id, ref size, unmanagedValue);
                return Marshal.PtrToStructure<T>(unmanagedValue);
            }
            finally
            {
                Marshal.FreeHGlobal(unmanagedValue);
            }
        }

        internal void GetPropertyInfo(AudioFilePropertyId id, out uint dataSize, out uint isWritable)
        {
            SafeNativeMethods.AudioFileGetPropertyInfo(Handle, id, out dataSize, out isWritable);
        }

        internal void ReadPackets(
            out uint numBytes,
            [NotNull] AudioStreamPacketDescription[] packetDescriptions,
            long startingPacket,
            ref uint packets,
            IntPtr data)
        {
            SafeNativeMethods.AudioFileReadPackets(Handle, false, out numBytes, packetDescriptions,
                startingPacket, ref packets, data);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Handle.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        [SuppressMessage("Performance", "CA1801:Review unused parameters",
            Justification = "Part of native API")]
        AudioFileStatus ReadCallback(IntPtr userData, long position, uint requestCount, [NotNull] byte[] buffer, out uint actualCount)
        {
            _stream.Position = position;
            actualCount = (uint) _stream.Read(buffer, 0, (int) requestCount);
            return AudioFileStatus.Ok;
        }

        [SuppressMessage("Performance", "CA1801:Review unused parameters",
            Justification = "Part of native API")]
        long GetSizeCallback(IntPtr userData)
        {
            return _stream.Length;
        }

        AudioFileStatus WriteCallback(IntPtr userData, long position, uint requestCount, [NotNull] byte[] buffer, out uint actualCount)
        {
            _stream.Position = position;
            _stream.Write(buffer, 0, (int) requestCount);
            actualCount = requestCount;
            return AudioFileStatus.Ok;
        }

        AudioFileStatus SetSizeCallback(IntPtr userData, long size)
        {
            _stream.SetLength(size);
            return AudioFileStatus.Ok;
        }
    }
}