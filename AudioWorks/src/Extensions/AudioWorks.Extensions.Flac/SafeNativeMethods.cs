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
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using JetBrains.Annotations;

namespace AudioWorks.Extensions.Flac
{
    [SuppressUnmanagedCodeSecurity]
    static class SafeNativeMethods
    {
#if LINUX
        const string _flacLibrary = "libFLAC.so.8";
#else
        const string _flacLibrary = "libFLAC";
#endif
#if WINDOWS
        const string _kernelLibrary = "kernel32";

        [DllImport(_kernelLibrary, CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport(_kernelLibrary)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeLibrary(IntPtr module);

        [DllImport(_kernelLibrary, ExactSpelling = true, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern IntPtr GetProcAddress(IntPtr module, string name);
#else
        const string _dlLibrary = "libdl";

        [DllImport(_dlLibrary, EntryPoint = "dlopen", CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern IntPtr DlOpen(string filename, int flags);

        [DllImport(_dlLibrary, EntryPoint = "dlclose")]
        internal static extern int DlClose(IntPtr handle);

        [DllImport(_dlLibrary, EntryPoint = "dlsym", CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern IntPtr DlSym(IntPtr handle, string symbol);
#endif

        [NotNull]
        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_decoder_new",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern StreamDecoderHandle StreamDecoderNew();

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_decoder_init_stream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern int StreamDecoderInitStream(
            [NotNull] StreamDecoderHandle handle,
            [NotNull] NativeCallbacks.StreamDecoderReadCallback readCallback,
            [CanBeNull] NativeCallbacks.StreamDecoderSeekCallback seekCallback,
            [CanBeNull] NativeCallbacks.StreamDecoderTellCallback tellCallback,
            [CanBeNull] NativeCallbacks.StreamDecoderLengthCallback lengthCallback,
            [CanBeNull] NativeCallbacks.StreamDecoderEofCallback eofCallback,
            [NotNull] NativeCallbacks.StreamDecoderWriteCallback writeCallback,
            [CanBeNull] NativeCallbacks.StreamDecoderMetadataCallback metadataCallback,
            [NotNull] NativeCallbacks.StreamDecoderErrorCallback errorCallback,
            IntPtr userData);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_decoder_set_metadata_respond",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamDecoderSetMetadataRespond(
            StreamDecoderHandle handle,
            MetadataType metadataType);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_decoder_process_until_end_of_metadata",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamDecoderProcessUntilEndOfMetadata(
            [NotNull] StreamDecoderHandle handle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_decoder_process_single",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamDecoderProcessSingle(
            [NotNull] StreamDecoderHandle handle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_decoder_get_state",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern DecoderState StreamDecoderGetState(
            [NotNull] StreamDecoderHandle handle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_decoder_finish",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamDecoderFinish(
            [NotNull] StreamDecoderHandle handle);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_decoder_delete",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StreamDecoderDelete(
            IntPtr handle);

        [NotNull]
        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_new",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern StreamEncoderHandle StreamEncoderNew();

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_set_channels",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamEncoderSetChannels(
            [NotNull] StreamEncoderHandle handle,
            uint channels);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_get_channels",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint StreamEncoderGetChannels(
            [NotNull] StreamEncoderHandle handle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_set_bits_per_sample",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamEncoderSetBitsPerSample(
            [NotNull] StreamEncoderHandle handle,
            uint bitsPerSample);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_get_bits_per_sample",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint StreamEncoderGetBitsPerSample(
            [NotNull] StreamEncoderHandle handle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_set_sample_rate",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamEncoderSetSampleRate(
            [NotNull] StreamEncoderHandle handle,
            uint sampleRate);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_set_total_samples_estimate",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamEncoderSetTotalSamplesEstimate(
            [NotNull] StreamEncoderHandle handle,
            ulong totalSamples);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_get_total_samples_estimate",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong StreamEncoderGetTotalSamplesEstimate(
            [NotNull] StreamEncoderHandle handle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_set_compression_level",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern void StreamEncoderSetCompressionLevel(
            [NotNull] StreamEncoderHandle handle,
            uint compressionLevel);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_set_metadata",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamEncoderSetMetadata(
            [NotNull] StreamEncoderHandle handle,
            [NotNull] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] metaData,
            uint blocks);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_init_stream",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern int StreamEncoderInitStream(
            [NotNull] StreamEncoderHandle handle,
            [NotNull] NativeCallbacks.StreamEncoderWriteCallback writeCallback,
            [CanBeNull] NativeCallbacks.StreamEncoderSeekCallback seekCallback,
            [CanBeNull] NativeCallbacks.StreamEncoderTellCallback tellCallback,
            [CanBeNull] NativeCallbacks.StreamEncoderMetadataCallback metadataCallback,
            IntPtr userData);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_process",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamEncoderProcess(
            [NotNull] StreamEncoderHandle handle,
            in IntPtr buffer,
            uint samples);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_process_interleaved",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamEncoderProcessInterleaved(
            [NotNull] StreamEncoderHandle handle,
            in int buffer,
            uint samples);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_finish",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StreamEncoderFinish(
            [NotNull] StreamEncoderHandle handle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_get_state",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern EncoderState StreamEncoderGetState(
            [NotNull] StreamEncoderHandle handle);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(_flacLibrary, EntryPoint = "FLAC__stream_encoder_delete",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StreamEncoderDelete(
            IntPtr handle);

        [NotNull]
        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_object_new", CallingConvention = CallingConvention.Cdecl)]
        internal static extern MetadataBlockHandle MetadataObjectNew(
            MetadataType blockType);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_object_vorbiscomment_entry_from_name_value_pair",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataObjectVorbisCommentEntryFromNameValuePair(
            out VorbisCommentEntry entry,
            IntPtr key,
            IntPtr value);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_object_vorbiscomment_append_comment",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataObjectVorbisCommentAppendComment(
            [NotNull] MetadataBlockHandle handle,
            VorbisCommentEntry vorbisComment,
            [MarshalAs(UnmanagedType.Bool)] bool copy);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_object_seektable_template_append_spaced_points",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataObjectSeekTableTemplateAppendSpacedPoints(
            [NotNull] MetadataBlockHandle handle,
            uint num,
            ulong totalSamples);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_object_picture_set_mime_type",
            CallingConvention = CallingConvention.Cdecl, BestFitMapping = false)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataObjectPictureSetMimeType(
            [NotNull] MetadataBlockHandle handle,
            [MarshalAs(UnmanagedType.LPStr)] string mimeType,
            [MarshalAs(UnmanagedType.Bool)] bool copy);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_object_picture_set_data",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern unsafe bool MetadataObjectPictureSetData(
            [NotNull] MetadataBlockHandle handle,
            byte* data,
            uint length,
            [MarshalAs(UnmanagedType.Bool)] bool copy);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_object_delete",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MetadataObjectDelete(
            IntPtr handle);

        [NotNull]
        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_chain_new", CallingConvention = CallingConvention.Cdecl)]
        internal static extern MetadataChainHandle MetadataChainNew();

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_chain_read_with_callbacks",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataChainReadWithCallbacks(
            [NotNull] MetadataChainHandle handle,
            IntPtr ioHandle,
            IoCallbacks callbacks);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_chain_check_if_tempfile_needed",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataChainCheckIfTempFileNeeded(
            [NotNull] MetadataChainHandle handle,
            [MarshalAs(UnmanagedType.Bool)] bool usePadding);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_chain_write_with_callbacks",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataChainWriteWithCallbacks(
            [NotNull] MetadataChainHandle handle,
            [MarshalAs(UnmanagedType.Bool)] bool usePadding,
            IntPtr ioHandle,
            IoCallbacks callbacks);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_chain_write_with_callbacks_and_tempfile",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataChainWriteWithCallbacksAndTempFile(
            [NotNull] MetadataChainHandle handle,
            [MarshalAs(UnmanagedType.Bool)] bool usePadding,
            IntPtr ioHandle,
            IoCallbacks callbacks,
            IntPtr tempIoHandle,
            IoCallbacks tempCallbacks);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_chain_delete",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MetadataChainDelete(
            IntPtr handle);

        [NotNull]
        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_iterator_new",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern MetadataIteratorHandle MetadataIteratorNew();

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_iterator_init",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MetadataIteratorInit(
            [NotNull] MetadataIteratorHandle handle,
            [NotNull] MetadataChainHandle chainHandle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_iterator_next",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataIteratorNext(
            [NotNull] MetadataIteratorHandle handle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_iterator_get_block",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MetadataIteratorGetBlock(
            [NotNull] MetadataIteratorHandle handle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_iterator_insert_block_after",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataIteratorInsertBlockAfter(
            [NotNull] MetadataIteratorHandle handle,
            [NotNull] MetadataBlockHandle metadataHandle);

        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_iterator_delete_block"
            , CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool MetadataIteratorDeleteBlock(
            [NotNull] MetadataIteratorHandle handle,
            [MarshalAs(UnmanagedType.Bool)] bool replaceWithPadding);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(_flacLibrary, EntryPoint = "FLAC__metadata_iterator_delete",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MetadataIteratorDelete(
            IntPtr handle);
    }
}