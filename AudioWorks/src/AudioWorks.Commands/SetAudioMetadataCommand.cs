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

using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using AudioWorks.Common;

namespace AudioWorks.Commands
{
    [Cmdlet(VerbsCommon.Set, "AudioMetadata"), OutputType(typeof(ITaggedAudioFile))]
    public sealed class SetAudioMetadataCommand : LoggingCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public ITaggedAudioFile? AudioFile { get; set; }

        [Parameter, ValidateNotNull]
        public string? Title { get; set; }

        [Parameter, ValidateNotNull]
        public string? Artist { get; set; }

        [Parameter, ValidateNotNull]
        public string? Album { get; set; }

        [Parameter, ValidateNotNull]
        public string? AlbumArtist { get; set; }

        [Parameter, ValidateNotNull]
        public string? Composer { get; set; }

        [Parameter, ValidateNotNull]
        public string? Genre { get; set; }

        [Parameter, ValidateNotNull]
        public string? Comment { get; set; }

        [Parameter, ValidateNotNull]
        public string? Day { get; set; }

        [Parameter, ValidateNotNull]
        public string? Month { get; set; }

        [Parameter, ValidateNotNull]
        public string? Year { get; set; }

        [Parameter, ValidateNotNull]
        public string? TrackNumber { get; set; }

        [Parameter, ValidateNotNull]
        public string? TrackCount { get; set; }

        [Parameter, ValidateNotNull]
        public ICoverArt? CoverArt { get; set; }

        [Parameter]
        public SwitchParameter PassThru { get; set; }

        protected override void ProcessRecord()
        {
            var metadata = AudioFile!.Metadata;

            try
            {
                if (Title != null)
                    metadata.Title = Title;
                if (Artist != null)
                    metadata.Artist = Artist;
                if (Album != null)
                    metadata.Album = Album;
                if (AlbumArtist != null)
                    metadata.AlbumArtist = AlbumArtist;
                if (Composer != null)
                    metadata.Composer = Composer;
                if (Genre != null)
                    metadata.Genre = Genre;
                if (Comment != null)
                    metadata.Comment = Comment;
                if (Day != null)
                    metadata.Day = Day;
                if (Month != null)
                    metadata.Month = Month;
                if (Year != null)
                    metadata.Year = Year;
                if (TrackNumber != null)
                    metadata.TrackNumber = TrackNumber;
                if (TrackCount != null)
                    metadata.TrackCount = TrackCount;
                if (CoverArt != null)
                    metadata.CoverArt = CoverArt;
            }
            catch (AudioMetadataInvalidException e)
            {
                WriteError(new ErrorRecord(e, e.GetType().Name, ErrorCategory.InvalidData, AudioFile));
            }

            ProcessLogMessages();

            if (PassThru)
                WriteObject(AudioFile);
        }
    }
}
