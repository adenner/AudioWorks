﻿using AudioWorks.Common;
using JetBrains.Annotations;
using System;
using System.Globalization;

namespace AudioWorks.Extensions.Flac
{
    sealed class VorbisCommentToMetadataAdapter : AudioMetadata
    {
        internal void Set([NotNull] string field, [NotNull] string value)
        {
            try
            {
                switch (field.ToUpperInvariant())
                {
                    case "TITLE":
                        Title = value;
                        break;

                    case "ARTIST":
                        Artist = value;
                        break;

                    case "ALBUM":
                        Album = value;
                        break;

                    case "GENRE":
                        Genre = value;
                        break;

                    case "DESCRIPTION":
                    case "COMMENT":
                        Comment = value;
                        break;

                    case "DATE":
                    case "YEAR":
                        // The DATE comment may contain a full date, or only the year
                        if (DateTime.TryParse(value, CultureInfo.CurrentCulture,
                            DateTimeStyles.NoCurrentDateDefault, out var result))
                        {
                            Day = result.Day.ToString(CultureInfo.InvariantCulture);
                            Month = result.Month.ToString(CultureInfo.InvariantCulture);
                            Year = result.Year.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                            Year = value;
                        break;

                    case "TRACKNUMBER":
                        // The track number and count may be packed into the same comment
                        var segments = value.Split('/');
                        TrackNumber = segments[0];
                        if (segments.Length > 1)
                            TrackCount = segments[1];
                        break;

                    case "TRACKCOUNT":
                    case "TRACKTOTAL":
                    case "TOTALTRACKS":
                        TrackCount = value;
                        break;
                }
            }
            catch (AudioMetadataInvalidException)
            {
                // If a field is invalid, just leave it blank
            }
        }
    }
}