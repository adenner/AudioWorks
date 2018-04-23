﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using AudioWorks.Common;
using JetBrains.Annotations;

namespace AudioWorks.Extensions.Vorbis
{
    sealed class MetadataToVorbisCommentAdapter : IDisposable
    {
        VorbisComment _comment;
        bool _unmanagedMemoryAllocated;

        internal MetadataToVorbisCommentAdapter([NotNull] AudioMetadata metadata)
        {
            SafeNativeMethods.VorbisCommentInit(out _comment);

            if (!string.IsNullOrEmpty(metadata.Title))
                AddTag("TITLE", metadata.Title);
            if (!string.IsNullOrEmpty(metadata.Artist))
                AddTag("ARTIST", metadata.Artist);
            if (!string.IsNullOrEmpty(metadata.Album))
                AddTag("ALBUM", metadata.Album);
            if (!string.IsNullOrEmpty(metadata.AlbumArtist))
                AddTag("ALBUMARTIST", metadata.AlbumArtist);
            if (!string.IsNullOrEmpty(metadata.Composer))
                AddTag("COMPOSER", metadata.Composer);
            if (!string.IsNullOrEmpty(metadata.Genre))
                AddTag("GENRE", metadata.Genre);
            if (!string.IsNullOrEmpty(metadata.Comment))
                AddTag("DESCRIPTION", metadata.Comment);

            if (!string.IsNullOrEmpty(metadata.Day) &&
                !string.IsNullOrEmpty(metadata.Month) &&
                !string.IsNullOrEmpty(metadata.Year))
                AddTag("DATE", $"{metadata.Year}-{metadata.Month}-{metadata.Day}");
            else if (!string.IsNullOrEmpty(metadata.Year))
                AddTag("YEAR", metadata.Year);

            if (!string.IsNullOrEmpty(metadata.TrackNumber))
                AddTag("TRACKNUMBER", !string.IsNullOrEmpty(metadata.TrackCount)
                    ? $"{metadata.TrackNumber}/{metadata.TrackCount}"
                    : metadata.TrackNumber);

            if (!string.IsNullOrEmpty(metadata.TrackPeak))
                AddTag("REPLAYGAIN_TRACK_PEAK", metadata.TrackPeak);
            if (!string.IsNullOrEmpty(metadata.AlbumPeak))
                AddTag("REPLAYGAIN_ALBUM_PEAK", metadata.AlbumPeak);
            if (!string.IsNullOrEmpty(metadata.TrackGain))
                AddTag("REPLAYGAIN_TRACK_GAIN", $"{metadata.TrackGain} dB");
            if (!string.IsNullOrEmpty(metadata.AlbumGain))
                AddTag("REPLAYGAIN_ALBUM_GAIN", $"{metadata.AlbumGain} dB");

            // Always store images in JPEG format since Vorbis is also lossy
            if (metadata.CoverArt != null)
                AddTag("METADATA_BLOCK_PICTURE", CoverArtAdapter.ToComment(
                    CoverArtFactory.ConvertToLossy(metadata.CoverArt)));
        }

        [SuppressMessage("Performance", "CA1806:Do not ignore method results",
            Justification = "Native method is always expected to return 0")]
        internal void HeaderOut(out OggPacket packet)
        {
            SafeNativeMethods.VorbisCommentHeaderOut(ref _comment, out packet);
        }

        [SuppressMessage("Performance", "CA1806:Do not ignore method results",
            Justification = "Native method is always expected to return 0")]
        internal void HeaderOut(
            IntPtr dspState,
            out OggPacket first,
            out OggPacket second,
            out OggPacket third)
        {
            SafeNativeMethods.VorbisAnalysisHeaderOut(dspState, ref _comment, out first, out second, out third);
        }

        public void Dispose()
        {
            FreeUnmanaged();
            GC.SuppressFinalize(this);
        }

        unsafe void AddTag([NotNull] string tag, [NotNull] string contents)
        {
            // Optimization - avoid allocating on the heap
            Span<byte> tagSpan = stackalloc byte[Encoding.ASCII.GetByteCount(tag) + 1];
            Encoding.ASCII.GetBytes(
                (char*) Unsafe.AsPointer(ref MemoryMarshal.GetReference(tag.AsSpan())), tag.Length,
                (byte*) Unsafe.AsPointer(ref MemoryMarshal.GetReference(tagSpan)), tagSpan.Length);

            var valueSize = Encoding.UTF8.GetByteCount(contents) + 1;
            if (valueSize < 0x2000_0000)
            {
                Span<byte> contentsSpan = stackalloc byte[valueSize];
                Encoding.UTF8.GetBytes(
                    (char*) Unsafe.AsPointer(ref MemoryMarshal.GetReference(contents.AsSpan())), contents.Length,
                    (byte*) Unsafe.AsPointer(ref MemoryMarshal.GetReference(contentsSpan)), contentsSpan.Length);

                SafeNativeMethods.VorbisCommentAddTag(ref _comment,
                    new IntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(tagSpan))),
                    new IntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(contentsSpan))));
            }
            else
            {
                // Use heap allocations for comments > 512kB (usually pictures)
                SafeNativeMethods.VorbisCommentAddTag(ref _comment,
                    new IntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(tagSpan))),
                    Encoding.UTF8.GetBytes(contents));
            }
            _unmanagedMemoryAllocated = true;
        }

        void FreeUnmanaged()
        {
            if (_unmanagedMemoryAllocated)
                SafeNativeMethods.VorbisCommentClear(ref _comment);
        }

        ~MetadataToVorbisCommentAdapter()
        {
            FreeUnmanaged();
        }
    }
}