﻿using System;
using System.IO;
using System.Text;
using AudioWorks.Common;
using Id3Lib;
using Id3Lib.Exceptions;
using JetBrains.Annotations;

namespace AudioWorks.Extensions.Id3
{
    [AudioMetadataEncoderExport(".mp3")]
    public sealed class Id3AudioMetadataEncoder : IAudioMetadataEncoder
    {
        public SettingInfoDictionary SettingInfo { get; } = new SettingInfoDictionary
        {
            ["TagVersion"] = new StringSettingInfo("2.3", "2.4"),
            ["TagEncoding"] = new StringSettingInfo("Latin1", "UTF16"),
            ["TagPadding"] = new IntSettingInfo(0, 268_435_456)
        };

        public void WriteMetadata(FileStream stream, AudioMetadata metadata, SettingDictionary settings)
        {
            var existingTagLength = GetExistingTagLength(stream);

            var tagModel = settings.TryGetValue("TagEncoding", out var encoding)
                ? new MetadataToTagModelAdapter(metadata, (string) encoding)
                : new MetadataToTagModelAdapter(metadata, "Latin1");

            if (tagModel.Count > 0)
            {
                // Set the version (default to 3)
                if (settings.TryGetValue("TagVersion", out var version) &&
                    string.Equals("2.4", (string) version, StringComparison.Ordinal))
                    tagModel.Header.Version = 4;
                else
                    tagModel.Header.Version = 3;

                // Set the padding (default to 0)
                if (settings.TryGetValue("TagPadding", out var padding))
                    tagModel.Header.PaddingSize = (uint) (int) padding;

                tagModel.UpdateSize();

                if (!settings.ContainsKey("TagPadding") && existingTagLength >= tagModel.Header.TagSizeWithHeaderFooter)
                    Overwrite(stream, existingTagLength, tagModel);
                else
                    FullRewrite(stream, existingTagLength, tagModel);
            }
            else if (existingTagLength > 0)
            {
                // Remove the ID3v2 tag, if present
                using (var tempStream = new MemoryStream())
                {
                    stream.CopyTo(tempStream);
                    stream.Position = 0;
                    stream.SetLength(stream.Length - existingTagLength);
                    tempStream.Position = 0;
                    tempStream.CopyTo(stream);
                }
            }

            // Remove the ID3v1 tag, if present
            if (stream.Length < 128) return;
            stream.Seek(-128, SeekOrigin.End);
            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
                if (string.Equals("TAG", new string(reader.ReadChars(3)), StringComparison.Ordinal))
                    stream.SetLength(stream.Length - 128);
            stream.Position = tagModel.Count == 0
                ? 0
                : tagModel.Header.TagSizeWithHeaderFooter + tagModel.Header.PaddingSize;
        }

        static uint GetExistingTagLength([NotNull] Stream stream)
        {
            try
            {
                var existingTag = TagManager.Deserialize(stream);
                return existingTag.Header.TagSizeWithHeaderFooter + existingTag.Header.PaddingSize;
            }
            catch (TagNotFoundException)
            {
                return 0u;
            }
        }

        static void Overwrite([NotNull] Stream stream, uint existingTagLength, [NotNull] TagModel tagModel)
        {
            // Write the new tag overtop of the old one, leaving unused space as padding
            stream.Position = 0;
            tagModel.Header.PaddingSize = existingTagLength - tagModel.Header.TagSizeWithHeaderFooter;
            TagManager.Serialize(tagModel, stream);
        }

        static void FullRewrite([NotNull] Stream stream, uint existingTagLength, [NotNull] TagModel tagModel)
        {
            // Copy the audio to memory, then rewrite the whole stream
            stream.Position = existingTagLength;
            using (var tempStream = new MemoryStream())
            {
                // Copy the MP3 portion to memory
                stream.CopyTo(tempStream);

                // Rewrite the stream with the new tag in front
                stream.Position = 0;
                stream.SetLength(tagModel.Header.TagSizeWithHeaderFooter + tagModel.Header.PaddingSize + tempStream.Length);
                TagManager.Serialize(tagModel, stream);
                tempStream.Position = 0;
                tempStream.CopyTo(stream);
            }
        }
    }
}
