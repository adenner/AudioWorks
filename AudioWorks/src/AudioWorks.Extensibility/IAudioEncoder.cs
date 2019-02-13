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

using System.IO;
using AudioWorks.Common;

namespace AudioWorks.Extensibility
{
    /// <summary>
    /// An extension that can encode audio samples into a specific format.
    /// </summary>
    public interface IAudioEncoder : ISampleProcessor
    {
        /// <summary>
        /// Gets information about the settings that can be passed to the <see cref="Initialize"/> method.
        /// </summary>
        /// <value>The setting information.</value>
        SettingInfoDictionary SettingInfo { get; }

        /// <summary>
        /// Gets the file extension used by the encoder.
        /// </summary>
        /// <value>The file extension.</value>
        string FileExtension { get; }

        /// <summary>
        /// Initializes the encoder.
        /// </summary>
        /// <param name="stream">The output stream.</param>
        /// <param name="info">The audio information.</param>
        /// <param name="metadata">The audio metadata.</param>
        /// <param name="settings">The settings.</param>
        void Initialize(Stream stream, AudioInfo info, AudioMetadata metadata, SettingDictionary settings);

        /// <summary>
        /// Finishes encoding.
        /// </summary>
        void Finish();
    }
}