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

using System;
using AudioWorks.Common;
using AudioWorks.UI.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace AudioWorks.UI.Modules.Lame.ViewModels
{
    public class LameEncoderSettingsControlViewModel : BindableBase
    {
        int _modeIndex;
        bool _qualityEnabled;
        bool _bitRateEnabled;
        int _vbrQuality;
        int _bitRate;
        bool _forceCbr;
        int _tagVersionIndex;
        int _tagEncodingIndex;
        int _applyGainIndex;
        int _tagPadding;

        public string Title { get; } = "MP3";

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

        public int VbrQuality
        {
            get => _vbrQuality;
            set => SetProperty(ref _vbrQuality, value);
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

        public string[] ApplyGainValues => new[] { "None", "Track", "Album" };

        public int ApplyGainIndex
        {
            get => _applyGainIndex;
            set => SetProperty(ref _applyGainIndex, value);
        }

        public string[] TagVersions => new[] { "2.3", "2.4" };

        public int TagVersionIndex
        {
            get => _tagVersionIndex;
            set => SetProperty(ref _tagVersionIndex, value);
        }

        public string[] TagEncodings => new[] { "Latin-1", "UTF-16" };

        public int TagEncodingIndex
        {
            get => _tagEncodingIndex;
            set => SetProperty(ref _tagEncodingIndex, value);
        }

        public int TagPadding
        {
            get => _tagPadding;
            set => SetProperty(ref _tagPadding, value);
        }

        public LameEncoderSettingsControlViewModel(
            ICommandService commandService,
            IEncoderSettingService settingService)
        {
            var settings = settingService["LameMP3"];

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

            _vbrQuality = settings.TryGetValue("VBRQuality", out int vbrQuality)
                ? vbrQuality
                : 3;

            if (settings.TryGetValue("ApplyGain", out string applyGain))
                switch (applyGain)
                {
                    case "Track":
                        _applyGainIndex = 1;
                        break;
                    case "Album":
                        _applyGainIndex = 2;
                        break;
                }

            if (settings.TryGetValue("TagVersion", out string version) &&
                version.Equals("2.4", StringComparison.Ordinal))
                _tagVersionIndex = 1;

            if (settings.TryGetValue("TagEncoding", out string encoding) &&
                encoding.Equals("UTF16", StringComparison.Ordinal))
                _tagEncodingIndex = 1;

            _tagPadding = settings.TryGetValue("Padding", out int padding)
                ? padding
                : 2048;
        }

        void SaveSettings(SettingDictionary settings)
        {
            if (_qualityEnabled && _vbrQuality != 3)
                settings["VBRQuality"] = _vbrQuality;
            else
                settings.Remove("VBRQuality");

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

            switch (_applyGainIndex)
            {
                case 1:
                    settings["ApplyGain"] = "Track";
                    break;
                case 2:
                    settings["ApplyGain"] = "Album";
                    break;
                default:
                    settings.Remove("ApplyGain");
                    break;
            }

            if (_tagVersionIndex == 1)
                settings["TagVersion"] = "2.4";
            else
                settings.Remove("TagVersion");


            if (_tagEncodingIndex == 1)
                settings["TagEncoding"] = "UTF16";
            else
                settings.Remove("TagEncoding");

            if (_tagPadding != 2048)
                settings["Padding"] = _tagPadding;
            else
                settings.Remove("Padding");
        }
    }
}
