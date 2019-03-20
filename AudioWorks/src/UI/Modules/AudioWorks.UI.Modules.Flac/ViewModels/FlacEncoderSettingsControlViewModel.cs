/* Copyright © 2019 Jeremy Herbison

This file is part of AudioWorks.

AudioWorks is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public
License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
version.

AudioWorks is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
details.

You should have received a copy of the GNU Affero General Public License along with AudioWorks. If not, see
<https://www.gnu.org/licenses/>. */

using AudioWorks.Common;
using AudioWorks.UI.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace AudioWorks.UI.Modules.Flac.ViewModels
{
    public class FlacEncoderSettingsControlViewModel : BindableBase
    {
        int _compressionLevel;
        int _seekPointInterval;
        int _padding;

        public string Title { get; } = "FLAC";

        public int CompressionLevel
        {
            get => _compressionLevel;
            set => SetProperty(ref _compressionLevel, value);
        }

        public int SeekPointInterval
        {
            get => _seekPointInterval;
            set => SetProperty(ref _seekPointInterval, value);
        }

        public int Padding
        {
            get => _padding;
            set => SetProperty(ref _padding, value);
        }

        public FlacEncoderSettingsControlViewModel(
            ICommandService commandService,
            IEncoderSettingService settingService)
        {
            var settings = settingService["FLAC"];

            commandService.SaveEncoderSettingsCommand.RegisterCommand(new DelegateCommand(() =>
                SaveSettings(settings)));

            _compressionLevel = settings.TryGetValue("CompressionLevel", out int compressionLevel)
                ? compressionLevel
                : 5;

            _seekPointInterval = settings.TryGetValue("SeekPointInterval", out int seekPointInterval)
                ? seekPointInterval
                : 10;

            _padding = settings.TryGetValue("Padding", out int padding)
                ? padding
                : 8192;
        }

        void SaveSettings(SettingDictionary settings)
        {
            if (_compressionLevel != 5)
                settings["CompressionLevel"] = _compressionLevel;
            else
                settings.Remove("CompressionLevel");

            if (_padding != 8192)
                settings["Padding"] = _padding;
            else
                settings.Remove("Padding");

            if (_seekPointInterval != 10)
                settings["SeekPointInterval"] = _seekPointInterval;
            else
                settings.Remove("SeekPointInterval");
        }
    }
}
