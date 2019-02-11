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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using AudioWorks.Common;
using AudioWorks.Extensibility;
using JetBrains.Annotations;

namespace AudioWorks.Extensions.ReplayGain
{
    [AudioAnalyzerExport("ReplayGain", "ReplayGain 2.0")]
    public sealed class ReplayGainAnalyzer : IAudioAnalyzer
    {
        const int _referenceLevel = -18;

        [CanBeNull] R128Analyzer _analyzer;
        [CanBeNull] GroupState _groupState;

        public SettingInfoDictionary SettingInfo { get; } = new SettingInfoDictionary
        {
            ["PeakAnalysis"] = new StringSettingInfo("Simple", "Interpolated")
        };

        public void Initialize(AudioInfo info, SettingDictionary settings, GroupToken groupToken)
        {
            _analyzer = new R128Analyzer((uint) info.Channels, (uint) info.SampleRate,
                settings.TryGetValue("PeakAnalysis", out string peakAnalysis) &&
                peakAnalysis.Equals("Interpolated", StringComparison.Ordinal));

            _groupState = (GroupState) groupToken.GetOrSetGroupState(new GroupState());
            // ReSharper disable once PossibleNullReferenceException
            _groupState.Handles.Enqueue(_analyzer.Handle);
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void Submit(SampleBuffer samples)
        {
            if (samples.Frames == 0) return;

            Span<float> buffer = stackalloc float[samples.Frames * samples.Channels];
            samples.CopyToInterleaved(buffer);
            _analyzer.AddFrames(buffer, (uint) samples.Frames);
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public AudioMetadata GetResult()
        {
            var peak = _analyzer.GetPeak();
            _groupState.AddPeak(peak);

            return new AudioMetadata
            {
                TrackPeak = peak.ToString(CultureInfo.InvariantCulture),
                TrackGain = (_referenceLevel - _analyzer.GetLoudness())
                    .ToString(CultureInfo.InvariantCulture)
            };
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public AudioMetadata GetGroupResult()
        {
            return new AudioMetadata
            {
                AlbumPeak = _groupState.GroupPeak.ToString(CultureInfo.InvariantCulture),
                AlbumGain = (_referenceLevel - R128Analyzer.GetLoudnessMultiple(_groupState.Handles.ToArray()))
                    .ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}
