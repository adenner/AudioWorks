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
using System.Collections.Generic;
using System.Linq;
using AudioWorks.Api.Tests.DataTypes;
using JetBrains.Annotations;

namespace AudioWorks.Api.Tests.DataSources
{
    public static class SaveMetadataValidFileSource
    {
        [NotNull, ItemNotNull] static readonly List<object[]> _data = new List<object[]>
        {
            #region FLAC

            // All fields
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo.flac",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "4707C81E467497975458C152234AD13F",
                "79528DB2721970437C6D8877F655E273"
#elif OSX
                "79528DB2721970437C6D8877F655E273"
#else
                "2F062B3856D25219B8DEFA6C483CD7BB"
#endif
            },

            // Day unset
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo.flac",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "8233F3397D5097D50A061504D10AA644",
                "B903A8E9B17014CDDB563A3CC73AB7F5"
#elif OSX
                "B903A8E9B17014CDDB563A3CC73AB7F5"
#else
                "55FF7D3E035A4B343E01E4FF534BDED2"
#endif
            },

            // Month unset
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo.flac",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "8233F3397D5097D50A061504D10AA644",
                "B903A8E9B17014CDDB563A3CC73AB7F5"
#elif OSX
                "B903A8E9B17014CDDB563A3CC73AB7F5"
#else
                "55FF7D3E035A4B343E01E4FF534BDED2"
#endif
            },

            // TrackNumber unset
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo.flac",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "5449B08E4A9CEB9D86B3BEA443F02662",
                "C830D5913F20AEF93853E1A01462708D"
#elif OSX
                "C830D5913F20AEF93853E1A01462708D"
#else
                "967D764E5C8DA7F50DD53EA853C389EB"
#endif
            },

            // TrackCount unset
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo.flac",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "34B8ED1508B895CE54E48035C2F8F459",
                "27C19A7309E49A5B07C0AD932A7B2875"
#elif OSX
                "27C19A7309E49A5B07C0AD932A7B2875"
#else
                "FF631CBCB258EDCFC0C9B0F11BB81710"
#endif
            },

            // Existing tag
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo (Tagged using defaults).flac",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "4707C81E467497975458C152234AD13F",
                "79528DB2721970437C6D8877F655E273"
#elif OSX
                "79528DB2721970437C6D8877F655E273"
#else
                "2F062B3856D25219B8DEFA6C483CD7BB"
#endif
            },

            // No padding
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo.flac",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                new TestSettingDictionary
                {
                    ["Padding"] = 0
                },
#if LINUX
                "C6F468D2AFF114854647623FEC1B8F8F",
                "4D1C0B9CD6909E81C2D5BD38A6F7CF4B"
#elif OSX
                "4D1C0B9CD6909E81C2D5BD38A6F7CF4B"
#else
                "15A03AA8FB01162EC667746E136D20AD"
#endif
            },

            // 100 bytes of padding
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo.flac",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                new TestSettingDictionary
                {
                    ["Padding"] = 100
                },
#if LINUX
                "C8D99C071580D2C34E42B815B680A1CA",
                "D10242261CDBC3FF2D9BEB2C232DC1F7"
#elif OSX
                "D10242261CDBC3FF2D9BEB2C232DC1F7"
#else
                "C810ECB77EBD08015B1CA2DF2EB42380"
#endif
            },

            // Existing tag removal
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo (Tagged using defaults).flac",
                new TestAudioMetadata(),
                null,
                null,
#if LINUX
                "D858D62481CDF540B881F2151C0ABB80",
                "3983A342A074A7E8871FEF4FBE0AC73F"
#elif OSX
                "3983A342A074A7E8871FEF4FBE0AC73F"
#else
                "B2B05B6CA5D2637A53EE6B3DA1E2E81E"
#endif
            },

            // Nothing to do
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo.flac",
                new TestAudioMetadata(),
                null,
                null,
#if LINUX
                "D858D62481CDF540B881F2151C0ABB80",
                "3983A342A074A7E8871FEF4FBE0AC73F"
#elif OSX
                "3983A342A074A7E8871FEF4FBE0AC73F"
#else
                "B2B05B6CA5D2637A53EE6B3DA1E2E81E"
#endif
            },

            // PNG CoverArt
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo.flac",
                new TestAudioMetadata(),
                "PNG 24-bit 1280 x 935.png",
                null,
#if LINUX
                "86C7A296259858EC6063A3B0740D18CB",
                "8E3D1A13C4F9BE314C0AD61892472AC6"
#elif OSX
                "8E3D1A13C4F9BE314C0AD61892472AC6"
#else
                "5719C9D92A8AE4AA99C90EB7BCFAE33D"
#endif
            },

            // JPEG CoverArt
            new object[]
            {
                "FLAC Level 5 16-bit 44100Hz Stereo.flac",
                new TestAudioMetadata(),
                "JPEG 24-bit 1280 x 935.jpg",
                null,
#if LINUX
                "CB279829CACD8B102E3288CA3360BC52",
                "F489D849B9ACD653B8986D45C487742E"
#elif OSX
                "F489D849B9ACD653B8986D45C487742E"
#else
                "FF1BC4971773506753CA018296BC2A96"
#endif
            },

            #endregion

            #region MP4

            // All fields
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "7F7316891B26F9FC3DE7D2D1304F9CFE",
                "7F7316891B26F9FC3DE7D2D1304F9CFE"
#else
                "7F7316891B26F9FC3DE7D2D1304F9CFE"
#endif
            },

            // Day unset
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "9957A905E53C3D2411A3A4BF3DA4DBA3",
                "9957A905E53C3D2411A3A4BF3DA4DBA3"
#else
                "9957A905E53C3D2411A3A4BF3DA4DBA3"
#endif
            },

            // Month unset
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "9957A905E53C3D2411A3A4BF3DA4DBA3",
                "9957A905E53C3D2411A3A4BF3DA4DBA3"
#else
                "9957A905E53C3D2411A3A4BF3DA4DBA3"
#endif
            },

            // TrackNumber unset
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "847E8C85293966F62132E8F851FA7BAE",
                "847E8C85293966F62132E8F851FA7BAE"
#else
                "847E8C85293966F62132E8F851FA7BAE"
#endif
            },

            // TrackCount unset
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "BD8C422B4F08A50532168922680B0C2E",
                "BD8C422B4F08A50532168922680B0C2E"
#else
                "BD8C422B4F08A50532168922680B0C2E"
#endif
            },

            // Existing tag
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo (Tagged).m4a",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "7F7316891B26F9FC3DE7D2D1304F9CFE",
                "7F7316891B26F9FC3DE7D2D1304F9CFE"
#else
                "7F7316891B26F9FC3DE7D2D1304F9CFE"
#endif
            },

            // Updated creation time
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata(),
                null,
                new TestSettingDictionary
                {
                    ["CreationTime"] = new DateTime(2018, 9, 1)
                },
#if LINUX
                "4AF6E13DD50245A06F6FEA52F2325C47",
                "4AF6E13DD50245A06F6FEA52F2325C47"
#else
                "4AF6E13DD50245A06F6FEA52F2325C47"
#endif
            },

            // Updated modification time
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata(),
                null,
                new TestSettingDictionary
                {
                    ["ModificationTime"] = new DateTime(2018, 9, 1)
                },
#if LINUX
                "D196EAFE7E8617F867136C526718CEF2",
                "D196EAFE7E8617F867136C526718CEF2"
#else
                "D196EAFE7E8617F867136C526718CEF2"
#endif
            },

            // Existing tag removal
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo (Tagged).m4a",
                new TestAudioMetadata(),
                null,
                null,
#if LINUX
                "090FD975097BAFC4164370A3DEA9E696",
                "090FD975097BAFC4164370A3DEA9E696"
#else
                "090FD975097BAFC4164370A3DEA9E696"
#endif
            },

            // Nothing to do
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata(),
                null,
                null,
#if LINUX
                "090FD975097BAFC4164370A3DEA9E696",
                "090FD975097BAFC4164370A3DEA9E696"
#else
                "090FD975097BAFC4164370A3DEA9E696"
#endif
            },

            // Default padding (explicit)
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata(),
                null,
                new TestSettingDictionary
                {
                    ["Padding"] = 2048
                },
#if LINUX
                "090FD975097BAFC4164370A3DEA9E696",
                "090FD975097BAFC4164370A3DEA9E696"
#else
                "090FD975097BAFC4164370A3DEA9E696"
#endif
            },

            // Disabled padding
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata(),
                null,
                new TestSettingDictionary
                {
                    ["Padding"] = 0
                },
#if LINUX
                "2A66A8458C32EC663AE48C6294E829AB",
                "2A66A8458C32EC663AE48C6294E829AB"
#else
                "2A66A8458C32EC663AE48C6294E829AB"
#endif
            },

            // Maximum padding
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata(),
                null,
                new TestSettingDictionary
                {
                    ["Padding"] = 16_777_216
                },
#if LINUX
                "D4615919A461B54512B3863ADD487D4B",
                "D4615919A461B54512B3863ADD487D4B"
#else
                "D4615919A461B54512B3863ADD487D4B"
#endif
            },

            // PNG CoverArt (ALAC)
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata(),
                "PNG 24-bit 1280 x 935.png",
                null,
#if LINUX
                "767F47AEEA8A8F85DA214D51E0751CD5",
                "767F47AEEA8A8F85DA214D51E0751CD5"
#else
                "767F47AEEA8A8F85DA214D51E0751CD5"
#endif
            },

            // JPEG CoverArt (ALAC)
            new object[]
            {
                "ALAC 16-bit 44100Hz Stereo.m4a",
                new TestAudioMetadata(),
                "JPEG 24-bit 1280 x 935.jpg",
                null,
#if LINUX
                "D419908A6F39E2D402BFDE1CB4DA8821",
                "D419908A6F39E2D402BFDE1CB4DA8821"
#else
                "D419908A6F39E2D402BFDE1CB4DA8821"
#endif
            },

            // PNG CoverArt (AAC, converted)
            new object[]
            {
                "QAAC TVBR 91 44100Hz Stereo.m4a",
                new TestAudioMetadata(),
                "PNG 24-bit 1280 x 935.png",
                null,
#if LINUX
                "679251C0E61FC8EB10286525FE64F60F",
                "679251C0E61FC8EB10286525FE64F60F"
#else
                "679251C0E61FC8EB10286525FE64F60F"
#endif
            },

            // JPEG CoverArt (AAC)
            new object[]
            {
                "QAAC TVBR 91 44100Hz Stereo.m4a",
                new TestAudioMetadata(),
                "JPEG 24-bit 1280 x 935.jpg",
                null,
#if LINUX
                "679251C0E61FC8EB10286525FE64F60F",
                "679251C0E61FC8EB10286525FE64F60F"
#else
                "679251C0E61FC8EB10286525FE64F60F"
#endif
            },

            #endregion

            #region ID3

            // All fields
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "D44FACB05A7F0CB3CB5F5A31B9B52022",
                "D44FACB05A7F0CB3CB5F5A31B9B52022"
#else
                "D44FACB05A7F0CB3CB5F5A31B9B52022"
#endif
            },

            // Day unset
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "C64330448267EA7595D19378766B38C5",
                "C64330448267EA7595D19378766B38C5"
#else
                "C64330448267EA7595D19378766B38C5"
#endif
            },

            // Month unset
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "C64330448267EA7595D19378766B38C5",
                "C64330448267EA7595D19378766B38C5"
#else
                "C64330448267EA7595D19378766B38C5"
#endif
            },

            // TrackNumber unset
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "8E71F084665F7763560EAEF79292B1ED",
                "8E71F084665F7763560EAEF79292B1ED"
#else
                "8E71F084665F7763560EAEF79292B1ED"
#endif
            },

            // TrackCount unset
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "B4BAD75711B480844A735B8EF169F82A",
                "B4BAD75711B480844A735B8EF169F82A"
#else
                "B4BAD75711B480844A735B8EF169F82A"
#endif
            },

            // Existing tag
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo (ID3v2.3 Latin1).mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "076B838A43E883DCC9F0D0ABE8A263D6",
                "076B838A43E883DCC9F0D0ABE8A263D6"
#else
                "076B838A43E883DCC9F0D0ABE8A263D6"
#endif
            },

            // Tag version 2.4
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                new TestSettingDictionary
                {
                    ["TagVersion"] = "2.4"
                },
#if LINUX
                "E00628B6DA2A93F2832EAC420B2D8DF0",
                "E00628B6DA2A93F2832EAC420B2D8DF0"
#else
                "E00628B6DA2A93F2832EAC420B2D8DF0"
#endif
            },

            // UTF-16 encoding
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                new TestSettingDictionary
                {
                    ["TagEncoding"] = "UTF16"
                },
#if LINUX
                "476D2A59E830366CBFF9F0AE0305B8E2",
                "476D2A59E830366CBFF9F0AE0305B8E2"
#else
                "476D2A59E830366CBFF9F0AE0305B8E2"
#endif
            },

            // Default padding (explicit)
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                new TestSettingDictionary
                {
                    ["TagPadding"] = 2048
                },
#if LINUX
                "D44FACB05A7F0CB3CB5F5A31B9B52022",
                "D44FACB05A7F0CB3CB5F5A31B9B52022"
#else
                "D44FACB05A7F0CB3CB5F5A31B9B52022"
#endif
            },

            // No padding
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                new TestSettingDictionary
                {
                    ["TagPadding"] = 0
                },
#if LINUX
                "076B838A43E883DCC9F0D0ABE8A263D6",
                "076B838A43E883DCC9F0D0ABE8A263D6"
#else
                "076B838A43E883DCC9F0D0ABE8A263D6"
#endif
            },

            // Maximum padding
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                new TestSettingDictionary
                {
                    ["TagPadding"] = 16_777_216
                },
#if LINUX
                "189A55453749DA1FFAFFEAC6A06DF99B",
                "189A55453749DA1FFAFFEAC6A06DF99B"
#else
                "189A55453749DA1FFAFFEAC6A06DF99B"
#endif
            },

            // Existing v1 tag
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo (ID3v1).mp3",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "D44FACB05A7F0CB3CB5F5A31B9B52022",
                "D44FACB05A7F0CB3CB5F5A31B9B52022"
#else
                "D44FACB05A7F0CB3CB5F5A31B9B52022"
#endif
            },

            // Existing tag removal
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo (ID3v2.3 Latin1).mp3",
                new TestAudioMetadata(),
                null,
                null,
#if LINUX
                "963D578D818C25DE5FEE6625BE7BFA98",
                "963D578D818C25DE5FEE6625BE7BFA98"
#else
                "963D578D818C25DE5FEE6625BE7BFA98"
#endif
            },

            // Nothing to do
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata(),
                null,
                null,
#if LINUX
                "963D578D818C25DE5FEE6625BE7BFA98",
                "963D578D818C25DE5FEE6625BE7BFA98"
#else
                "963D578D818C25DE5FEE6625BE7BFA98"
#endif
            },

            // PNG CoverArt (converted)
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata(),
                "PNG 24-bit 1280 x 935.png",
                null,
#if LINUX
                "1BF5C0A314A84C08A71E620ECFAC27FF",
                "1BF5C0A314A84C08A71E620ECFAC27FF"
#else
                "1BF5C0A314A84C08A71E620ECFAC27FF"
#endif
            },

            // JPEG CoverArt
            new object[]
            {
                "Lame CBR 128 44100Hz Stereo.mp3",
                new TestAudioMetadata(),
                "JPEG 24-bit 1280 x 935.jpg",
                null,
#if LINUX
                "1BF5C0A314A84C08A71E620ECFAC27FF",
                "1BF5C0A314A84C08A71E620ECFAC27FF"
#else
                "1BF5C0A314A84C08A71E620ECFAC27FF"
#endif
            },

            #endregion

            #region Ogg Vorbis

            // All fields
            new object[]
            {
                "Vorbis Quality 3 44100Hz Stereo.ogg",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "7DE488CE969207C50F33962EA5A9DDDE",
                "7DE488CE969207C50F33962EA5A9DDDE"
#elif OSX
                "11F166F272E635021ABD6BAF37A3BFA5"
#else
                "69A75975D90E906C158C194236FC7125"
#endif
            },

            // Day unset
            new object[]
            {
                "Vorbis Quality 3 44100Hz Stereo.ogg",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "D9C7D27040EF2ECE830AE4FC9B5BF25E",
                "D9C7D27040EF2ECE830AE4FC9B5BF25E"
#elif OSX
                "63BB3732D651F6D3A8496091FCF14725"
#else
                "719EDC12E5D730C0C8EB37C90F63563D"
#endif
            },

            // Month unset
            new object[]
            {
                "Vorbis Quality 3 44100Hz Stereo.ogg",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "D9C7D27040EF2ECE830AE4FC9B5BF25E",
                "D9C7D27040EF2ECE830AE4FC9B5BF25E"
#elif OSX
                "63BB3732D651F6D3A8496091FCF14725"
#else
                "719EDC12E5D730C0C8EB37C90F63563D"
#endif
            },

            // TrackNumber unset
            new object[]
            {
                "Vorbis Quality 3 44100Hz Stereo.ogg",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "39B3BD086D3587C78FB2E76B52BD8F12",
                "39B3BD086D3587C78FB2E76B52BD8F12"
#elif OSX
                "ED7210753CB1A870F74DB01E959D2FFE"
#else
                "B49EE9F48912935E98EB5B703A0CFEAE"
#endif
            },

            // TrackCount unset
            new object[]
            {
                "Vorbis Quality 3 44100Hz Stereo.ogg",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "9E57B6A4089942A55ABD25FC13F7F749",
                "9E57B6A4089942A55ABD25FC13F7F749"
#elif OSX
                "17F5C68F72027655E21870F0E7F06CB8"
#else
                "CE24BB00216950315A1E5964FDDC62DC"
#endif
            },

            // Existing tag
            new object[]
            {
                "Vorbis Quality 3 44100Hz Stereo (Tagged using defaults).ogg",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12",
                    TrackPeak = "0.5",
                    AlbumPeak = "0.6",
                    TrackGain = "0.7",
                    AlbumGain = "0.8"
                },
                null,
                null,
#if LINUX
                "7DE488CE969207C50F33962EA5A9DDDE",
                "7DE488CE969207C50F33962EA5A9DDDE"
#elif OSX
                "11F166F272E635021ABD6BAF37A3BFA5"
#else
                "69A75975D90E906C158C194236FC7125"
#endif
            },

            // Existing tag removal
            new object[]
            {
                "Vorbis Quality 3 44100Hz Stereo (Tagged using defaults).ogg",
                new TestAudioMetadata(),
                null,
                null,
#if LINUX
                "46539B96ACD38AA00671E0D5F82E57B1",
                "46539B96ACD38AA00671E0D5F82E57B1"
#elif OSX
                "873E47897BB3645B63B5B8D8B932198E"
#else
                "C4744A0D9349D8423FF188BF79823868"
#endif
            },

            // Nothing to do
            new object[]
            {
                "Vorbis Quality 3 44100Hz Stereo.ogg",
                new TestAudioMetadata(),
                null,
                null,
#if LINUX
                "46539B96ACD38AA00671E0D5F82E57B1",
                "46539B96ACD38AA00671E0D5F82E57B1"
#elif OSX
                "873E47897BB3645B63B5B8D8B932198E"
#else
                "C4744A0D9349D8423FF188BF79823868"
#endif
            },

            // PNG CoverArt (Converted)
            new object[]
            {
                "Vorbis Quality 3 44100Hz Stereo.ogg",
                new TestAudioMetadata(),
                "PNG 24-bit 1280 x 935.png",
                null,
#if LINUX
                "FD245B933B515151B8AF17307FF6ECFE",
                "FD245B933B515151B8AF17307FF6ECFE"
#elif OSX
                "2682E9CD4EF8B4FBA041B907FD86614D"
#else
                "8C9453958E15AF2EFBE2054C8F07EFAF"
#endif
            },

            // JPEG CoverArt
            new object[]
            {
                "Vorbis Quality 3 44100Hz Stereo.ogg",
                new TestAudioMetadata(),
                "JPEG 24-bit 1280 x 935.jpg",
                null,
#if LINUX
                "FD245B933B515151B8AF17307FF6ECFE",
                "FD245B933B515151B8AF17307FF6ECFE"
#elif OSX
                "2682E9CD4EF8B4FBA041B907FD86614D"
#else
                "8C9453958E15AF2EFBE2054C8F07EFAF"
#endif
            },

            #endregion

            #region Opus

            // All fields
            new object[]
            {
                "Opus VBR 44100Hz Stereo.opus",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12"
                },
                null,
                null,
#if LINUX
                "9A53EDE65A60F0C1684D16744E33DCFC",
                "9A53EDE65A60F0C1684D16744E33DCFC"
#elif OSX
                "5284C0343D8510F5F529D3E8881A014E"
#else
                "5284C0343D8510F5F529D3E8881A014E"
#endif
            },

            // Day unset
            new object[]
            {
                "Opus VBR 44100Hz Stereo.opus",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12"
                },
                null,
                null,
#if LINUX
                "4A397361C11EC1FE03A43E176E673856",
                "4A397361C11EC1FE03A43E176E673856"
#elif OSX
                "F111890A03DD3DD638106687B486300C"
#else
                "F111890A03DD3DD638106687B486300C"
#endif
            },

            // Month unset
            new object[]
            {
                "Opus VBR 44100Hz Stereo.opus",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12"
                },
                null,
                null,
#if LINUX
                "4A397361C11EC1FE03A43E176E673856",
                "4A397361C11EC1FE03A43E176E673856"
#elif OSX
                "F111890A03DD3DD638106687B486300C"
#else
                "F111890A03DD3DD638106687B486300C"
#endif
            },

            // TrackNumber unset
            new object[]
            {
                "Opus VBR 44100Hz Stereo.opus",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackCount = "12"
                },
                null,
                null,
#if LINUX
                "666B7D0315E667A3D6A943294A32BE1F",
                "666B7D0315E667A3D6A943294A32BE1F"
#elif OSX
                "949E7F89B463AD682322D354C8170CF5"
#else
                "949E7F89B463AD682322D354C8170CF5"
#endif
            },

            // TrackCount unset
            new object[]
            {
                "Opus VBR 44100Hz Stereo.opus",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01"
                },
                null,
                null,
#if LINUX
                "910BF18671BC79D50FB8B9EAE8E4F0DA",
                "910BF18671BC79D50FB8B9EAE8E4F0DA"
#elif OSX
                "7252D2507D8414BFA6D5BC659F2CBA98"
#else
                "7252D2507D8414BFA6D5BC659F2CBA98"
#endif
            },

            // Existing tag
            new object[]
            {
                "Opus VBR 44100Hz Stereo (Tagged using defaults).opus",
                new TestAudioMetadata
                {
                    Title = "Test Title",
                    Artist = "Test Artist",
                    Album = "Test Album",
                    AlbumArtist = "Test Album Artist",
                    Composer = "Test Composer",
                    Genre = "Test Genre",
                    Comment = "Test Comment",
                    Day = "31",
                    Month = "01",
                    Year = "2017",
                    TrackNumber = "01",
                    TrackCount = "12"
                },
                null,
                null,
#if LINUX
                "86AD4DC307C9523B2C1213FE2359FDEA",
                "86AD4DC307C9523B2C1213FE2359FDEA"
#elif OSX
                "C5F2FFAB1A2000CBACFE1295A3335728"
#else
                "C5F2FFAB1A2000CBACFE1295A3335728"
#endif
            },

            // Existing tag removal
            new object[]
            {
                "Opus VBR 44100Hz Stereo (Tagged using defaults).opus",
                new TestAudioMetadata(),
                null,
                null,
#if LINUX
                "C2351EF6BFA1D50983183ADC6136FB9C",
                "C2351EF6BFA1D50983183ADC6136FB9C"
#elif OSX
                "51C23E67B35D82097EB7F920D275DBF1"
#else
                "51C23E67B35D82097EB7F920D275DBF1"
#endif
            },

            // Nothing to do
            new object[]
            {
                "Opus VBR 44100Hz Stereo.opus",
                new TestAudioMetadata(),
                null,
                null,
#if LINUX
                "CEF9DD8611EBFEF035761B450D816E95",
                "CEF9DD8611EBFEF035761B450D816E95"
#elif OSX
                "E13DC4ACD53A04B955C724B37042EFB6"
#else
                "E13DC4ACD53A04B955C724B37042EFB6"
#endif
            },

            // PNG CoverArt (Converted)
            new object[]
            {
                "Opus VBR 44100Hz Stereo.opus",
                new TestAudioMetadata(),
                "PNG 24-bit 1280 x 935.png",
                null,
#if LINUX
                "1636B4EF09C4EF2B16135E9C2670473B",
                "1636B4EF09C4EF2B16135E9C2670473B"
#elif OSX
                "3F066B0708B269330B53C3C96119F57A"
#else
                "3F066B0708B269330B53C3C96119F57A"
#endif
            },

            // JPEG CoverArt
            new object[]
            {
                "Opus VBR 44100Hz Stereo.opus",
                new TestAudioMetadata(),
                "JPEG 24-bit 1280 x 935.jpg",
                null,
#if LINUX
                "1636B4EF09C4EF2B16135E9C2670473B",
                "1636B4EF09C4EF2B16135E9C2670473B"
#elif OSX
                "3F066B0708B269330B53C3C96119F57A"
#else
                "3F066B0708B269330B53C3C96119F57A"
#endif
            }

            #endregion
        };

        [NotNull, ItemNotNull]
        public static IEnumerable<object[]> Data
        {
            // Prepend an index to each row
            [UsedImplicitly] get => _data.Select((item, index) => item.Prepend(index).ToArray());
        }
    }
}
