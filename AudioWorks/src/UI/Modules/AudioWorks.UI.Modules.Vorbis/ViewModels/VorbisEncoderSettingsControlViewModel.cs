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

namespace AudioWorks.UI.Modules.Vorbis.ViewModels
{
    public class VorbisEncoderSettingsControlViewModel : BindableBase
    {
        int _modeIndex;
        bool _qualityEnabled;
        bool _bitRateEnabled;
        int _quality;
        int _bitRate;
        bool _forceCbr;

        public string Title { get; } = "Vorbis";

        public string[] Modes => new[] { "Quality", "File Size" };

        public int ModeIndex
        {
            get => _modeIndex;
            set
            {
                SetProperty(ref _modeIndex, value);
                QualityEnabled = _modeIndex == 0;
                BitRateEnabled = _modeIndex == 1;
            }
        }

        public bool QualityEnabled
        {
            get => _qualityEnabled;
            set => SetProperty(ref _qualityEnabled, value);
        }

        public bool BitRateEnabled
        {
            get => _bitRateEnabled;
            set => SetProperty(ref _bitRateEnabled, value);
        }

        public int Quality
        {
            get => _quality;
            set => SetProperty(ref _quality, value);
        }

        public int BitRate
        {
            get => _bitRate;
            set => SetProperty(ref _bitRate, value);
        }

        public bool ForceCbr
        {
            get => _forceCbr;
            set => SetProperty(ref _forceCbr, value);
        }

        public VorbisEncoderSettingsControlViewModel(
            ICommandService commandService,
            IEncoderSettingService settingService)
        {
            var settings = settingService["Vorbis"];

            commandService.SaveEncoderSettingsCommand.RegisterCommand(new DelegateCommand(() =>
                SaveSettings(settings)));

            if (settings.TryGetValue("BitRate", out int bitRate))
            {
                ModeIndex = 1;
                _bitRate = bitRate;

                if (settings.TryGetValue("ForceCBR", out bool forceCbr))
                    _forceCbr = forceCbr;
            }
            else
            {
                ModeIndex = 0;
                _bitRate = 128;
            }

            _quality = settings.TryGetValue("Quality", out int vbrQuality)
                ? vbrQuality
                : 5;
        }

        void SaveSettings(SettingDictionary settings)
        {
            if (_qualityEnabled && _quality != 5)
                settings["Quality"] = _quality;
            else
                settings.Remove("Quality");

            if (_bitRateEnabled)
            {
                settings["BitRate"] = _bitRate;

                if (_forceCbr)
                    settings["ForceCBR"] = true;
                else
                    settings.Remove("ForceCBR");
            }
            else
            {
                settings.Remove("BitRate");
                settings.Remove("ForceCBR");
            }
        }
    }
}
