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
using Prism.Mvvm;

namespace AudioWorks.UI.Modules.Lame.ViewModels
{
    public class LameEncoderSettingsControlViewModel : BindableBase
    {
        const int _defaultQuality = 3;
        const int _defaultBitRate = 128;
        const int _defaultPadding = 2048;

        readonly SettingDictionary _settings;

        public string Title { get; } = "MP3";

        public string[] Modes => new[] { "Quality", "File Size" };

        public int ModeIndex
        {
            get => _settings.ContainsKey("BitRate") ? 1 : 0;
            set
            {
                if (value == 0)
                {
                    _settings.Remove("BitRate");
                    RaisePropertyChanged("BitRate");

                    _settings.Remove("ForceCBR");
                    RaisePropertyChanged("ForceCBR");
                }
                else
                {
                    _settings.Remove("VBRQuality");
                    RaisePropertyChanged("VBRQuality");

                    _settings["BitRate"] = _defaultBitRate;
                    RaisePropertyChanged("BitRate");
                }

                RaisePropertyChanged("QualityEnabled");
                RaisePropertyChanged("BitRateEnabled");
                RaisePropertyChanged();
            }
        }

        public bool QualityEnabled => ModeIndex == 0;

        public bool BitRateEnabled => ModeIndex == 1;

        public int VbrQuality
        {
            get => _settings.TryGetValue("VBRQuality", out int vbrQuality)
                ? vbrQuality
                : _defaultQuality;
            set
            {
                if (value != _defaultQuality)
                    _settings["VBRQuality"] = value;
                else
                    _settings.Remove("VBRQuality");
                RaisePropertyChanged();
            }
        }

        public int BitRate
        {
            get => _settings.TryGetValue("BitRate", out int bitRate)
                ? bitRate
                : _defaultBitRate;
            set
            {
                _settings["BitRate"] = value;
                RaisePropertyChanged();
            }
        }

        public bool ForceCbr
        {
            get => _settings.TryGetValue("ForceCBR", out bool forceCbr) && forceCbr;
            set
            {
                if (value)
                    _settings["ForceCBR"] = true;
                else
                    _settings.Remove("ForceCBR");
                RaisePropertyChanged();
            }
        }

        public string[] ApplyGainValues => new[] { "None", "Track", "Album" };

        public int ApplyGainIndex
        {
            get
            {
                if (_settings.TryGetValue("ApplyGain", out string applyGain))
                    switch (applyGain)
                    {
                        case "Track":
                            return 1;
                        case "Album":
                            return 2;
                    }

                return 0;
            }
            set
            {
                switch (value)
                {
                    case 1:
                        _settings["ApplyGain"] = "Track";
                        break;
                    case 2:
                        _settings["ApplyGain"] = "Album";
                        break;
                    default:
                        _settings.Remove("ApplyGain");
                        break;
                }
                RaisePropertyChanged();
            }
        }

        public string[] TagVersions => new[] { "2.3", "2.4" };

        public int TagVersionIndex
        {
            get => _settings.TryGetValue("TagVersion", out string tagVersion) &&
                   tagVersion.Equals("2.4", StringComparison.Ordinal)
                ? 1
                : 0;
            set
            {
                if (value == 1)
                    _settings["TagVersion"] = "2.4";
                else
                    _settings.Remove("TagVersion");
                RaisePropertyChanged();
            }
        }

        public string[] TagEncodings => new[] { "Latin-1", "UTF-16" };

        public int TagEncodingIndex
        {
            get => _settings.TryGetValue("TagEncoding", out string tagVersion) &&
                   tagVersion.Equals("UTF-16", StringComparison.Ordinal)
                ? 1
                : 0;
            set
            {
                if (value == 1)
                    _settings["TagEncoding"] = "UTF-16";
                else
                    _settings.Remove("TagEncoding");
                RaisePropertyChanged();

            }
        }

        public int TagPadding
        {
            get => _settings.TryGetValue("Padding", out int padding)
                ? padding
                : _defaultPadding;
            set
            {
                if (value != _defaultPadding)
                    _settings["Padding"] = value;
                else
                    _settings.Remove("Padding");
                RaisePropertyChanged();
            }
        }

        public LameEncoderSettingsControlViewModel(IEncoderSettingService settingService) =>
            _settings = settingService["LameMP3"];
    }
}
