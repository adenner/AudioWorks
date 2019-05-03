/* Copyright © 2018 Jeremy Herbison

This file is part of AudioWorks.

AudioWorks is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public
License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
version.

AudioWorks is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
details.

You should have received a copy of the GNU Affero General Public License along with AudioWorks. If not, see
<https://www.gnu.org/licenses/>. */

using System.Management.Automation;
using AudioWorks.Common;

namespace AudioWorks.Commands
{
    [Cmdlet(VerbsCommon.Clear, "AudioMetadata"), OutputType(typeof(ITaggedAudioFile))]
    public sealed class ClearAudioMetadataCommand : LoggingCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public ITaggedAudioFile? AudioFile { get; set; }

        [Parameter]
        public SwitchParameter Title { get; set; }

        [Parameter]
        public SwitchParameter Artist { get; set; }

        [Parameter]
        public SwitchParameter Album { get; set; }

        [Parameter]
        public SwitchParameter AlbumArtist { get; set; }

        [Parameter]
        public SwitchParameter Composer { get; set; }

        [Parameter]
        public SwitchParameter Genre { get; set; }

        [Parameter]
        public SwitchParameter Comment { get; set; }

        [Parameter]
        public SwitchParameter Day { get; set; }

        [Parameter]
        public SwitchParameter Month { get; set; }

        [Parameter]
        public SwitchParameter Year { get; set; }

        [Parameter]
        public SwitchParameter TrackNumber { get; set; }

        [Parameter]
        public SwitchParameter TrackCount { get; set; }

        [Parameter]
        public SwitchParameter Loudness { get; set; }

        [Parameter]
        public SwitchParameter CoverArt { get; set; }

        [Parameter]
        public SwitchParameter PassThru { get; set; }

        [SuppressMessage("Maintainability", "CA1502:Avoid excessive complexity", Justification =
            "Complexity cannot easily be reduced and the method is easy to understand, test, and maintain.")]
        protected override void ProcessRecord()
        {
            var metadata = AudioFile!.Metadata;

            if (Title) metadata.Title = string.Empty;
            if (Artist) metadata.Artist = string.Empty;
            if (Album) metadata.Album = string.Empty;
            if (AlbumArtist) metadata.AlbumArtist = string.Empty;
            if (Composer) metadata.Composer = string.Empty;
            if (Genre) metadata.Genre = string.Empty;
            if (Comment) metadata.Comment = string.Empty;
            if (Day) metadata.Day = string.Empty;
            if (Month) metadata.Month = string.Empty;
            if (Year) metadata.Year = string.Empty;
            if (TrackNumber) metadata.TrackNumber = string.Empty;
            if (TrackCount) metadata.TrackCount = string.Empty;
            if (Loudness)
            {
                metadata.TrackPeak = string.Empty;
                metadata.AlbumPeak = string.Empty;
                metadata.TrackGain = string.Empty;
                metadata.AlbumGain = string.Empty;
            }
            if (CoverArt) metadata.CoverArt = null;

            // If no switches were specified, clear everything
            if (!(Title || Artist || Album || AlbumArtist || Composer || Genre || Comment ||
                  Day || Month || Year || TrackNumber || TrackCount || Loudness || CoverArt))
                metadata.Clear();

            ProcessLogMessages();

            if (PassThru)
                WriteObject(AudioFile);
        }
    }
}
