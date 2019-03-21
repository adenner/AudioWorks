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

namespace AudioWorks.UI.Modules.Opus.ViewModels
{
    public class OpusEncoderSettingsControlViewModel : BindableBase
    {
        int _bitRate;
        int _controlModeIndex;
        int _signalTypeIndex;
        int _applyGainIndex;

        public string Title { get; } = "Opus";

        public int BitRate
        {
            get => _bitRate;
            set => SetProperty(ref _bitRate, value);
        }

        public string[] ControlModes => new[] { "Variable", "Constrained", "Constant" };

        public int ControlModeIndex
        {
            get => _controlModeIndex;
            set => SetProperty(ref _controlModeIndex, value);
        }

        public string[] SignalTypes => new[] { "Music", "Speech" };

        public int SignalTypeIndex
        {
            get => _signalTypeIndex;
            set => SetProperty(ref _signalTypeIndex, value);
        }

        public string[] ApplyGainValues => new[] { "None", "Track", "Album" };

        public int ApplyGainIndex
        {
            get => _applyGainIndex;
            set => SetProperty(ref _applyGainIndex, value);
        }

        public OpusEncoderSettingsControlViewModel(
            ICommandService commandService,
            IEncoderSettingService settingService)
        {
            var settings = settingService["Opus"];

            commandService.SaveEncoderSettingsCommand.RegisterCommand(new DelegateCommand(() =>
                SaveSettings(settings)));

            _bitRate = settings.TryGetValue("BitRate", out int bitRate)
                ? bitRate
                : 128;

            if (settings.TryGetValue("ControlMode", out string controlMode))
            {
                if (controlMode.Equals("Constrained", StringComparison.Ordinal))
                    _controlModeIndex = 1;
                else if (controlMode.Equals("Constant", StringComparison.Ordinal))
                    _controlModeIndex = 2;
            }
            else
                _controlModeIndex = 1;

            if (settings.TryGetValue("SignalType", out string signalType) &&
                signalType.Equals("Speech", StringComparison.Ordinal))
                _signalTypeIndex = 1;

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
        }

        void SaveSettings(SettingDictionary settings)
        {
            settings["BitRate"] = _bitRate;

            switch (_controlModeIndex)
            {
                case 0:
                    settings["ControlMode"] = "Variable";
                    break;
                case 2:
                    settings["ControlMode"] = "Constant";
                    break;
                default:
                    settings.Remove("ControlMode");
                    break;
            }

            if (_signalTypeIndex == 1)
                settings["SignalType"] = "Speech";
            else
                settings.Remove("SignalType");

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
        }
    }
}
