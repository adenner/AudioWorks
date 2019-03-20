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

namespace AudioWorks.UI.Modules.Apple.ViewModels
{
    public class AacEncoderSettingsControlViewModel : BindableBase
    {
        int _modeIndex;
        bool _qualityEnabled;
        bool _bitRateEnabled;
        int _vbrQuality;
        int _bitRate;
        int _controlModeIndex;
        int _padding;

        public string Title { get; } = "AAC";

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

        public string[] ControlModes => new[] { "Constrained", "Average", "Constant" };

        public int ControlModeIndex
        {
            get => _controlModeIndex;
            set => SetProperty(ref _controlModeIndex, value);
        }

        public int Padding
        {
            get => _padding;
            set => SetProperty(ref _padding, value);
        }

        public AacEncoderSettingsControlViewModel(
            ICommandService commandService,
            IEncoderSettingService settingService)
        {
            var settings = settingService["AppleAAC"];

            commandService.SaveEncoderSettingsCommand.RegisterCommand(new DelegateCommand(() =>
                SaveSettings(settings)));

            if (settings.TryGetValue("BitRate", out int bitRate))
            {
                ModeIndex = 1;
                _bitRate = bitRate;

                if (settings.TryGetValue("ControlMode", out string controlMode))
                {
                    if (controlMode.Equals("Average", StringComparison.Ordinal))
                        _controlModeIndex = 1;
                    else if (controlMode.Equals("Constant", StringComparison.Ordinal))
                        _controlModeIndex = 2;
                }
            }
            else
            {
                ModeIndex = 0;
                _bitRate = 128;
            }

            _vbrQuality = settings.TryGetValue("VBRQuality", out int vbrQuality)
                ? vbrQuality
                : 9;

            _padding = settings.TryGetValue("Padding", out int padding)
                ? padding
                : 2048;
        }

        void SaveSettings(SettingDictionary settings)
        {
            if (_qualityEnabled && _vbrQuality != 9)
                settings["VBRQuality"] = _vbrQuality;
            else
                settings.Remove("VBRQuality");

            if (_bitRateEnabled)
            {
                settings["BitRate"] = _bitRate;

                switch (_controlModeIndex)
                {
                    case 1:
                        settings["ControlMode"] = "Average";
                        break;
                    case 2:
                        settings["ControlMode"] = "Constant";
                        break;
                    default:
                        settings.Remove("ControlMode");
                        break;
                }
            }
            else
            {
                settings.Remove("BitRate");
                settings.Remove("ControlMode");
            }

            if (_padding != 2048)
                settings["Padding"] = _padding;
            else
                settings.Remove("Padding");
        }
    }
}
