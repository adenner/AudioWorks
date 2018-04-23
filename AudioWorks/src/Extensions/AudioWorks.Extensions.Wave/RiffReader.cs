﻿using System;
using System.IO;
using System.Text;
using AudioWorks.Common;
using JetBrains.Annotations;

namespace AudioWorks.Extensions.Wave
{
    sealed class RiffReader : BinaryReader
    {
        internal uint RiffChunkSize { get; private set; }

        internal RiffReader([NotNull] Stream input)
            : base(input, Encoding.ASCII, true)
        {
        }

        internal void Initialize()
        {
            BaseStream.Position = 0;
            if (!string.Equals("RIFF", new string(ReadChars(4)), StringComparison.OrdinalIgnoreCase))
                throw new AudioInvalidException("Not a valid RIFF stream.");

            RiffChunkSize = ReadUInt32();
        }

        [NotNull]
        internal string ReadFourCc()
        {
            BaseStream.Position = 8;
            return new string(ReadChars(4));
        }

        internal uint SeekToChunk([NotNull] string chunkId)
        {
            BaseStream.Position = 12;

            var currentChunkId = new string(ReadChars(4));
            var currentChunkLength = ReadUInt32();

            while (!string.Equals(chunkId, currentChunkId, StringComparison.Ordinal))
            {
                // Chunks are word-aligned:
                BaseStream.Seek(currentChunkLength + currentChunkLength % 2, SeekOrigin.Current);

                if (BaseStream.Position >= RiffChunkSize + 8)
                    return 0;

                currentChunkId = new string(ReadChars(4));
                currentChunkLength = ReadUInt32();
            }

            return currentChunkLength;
        }
    }
}