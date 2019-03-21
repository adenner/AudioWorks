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

namespace AudioWorks.UI.Modules.ReplayGain.ViewModels
{
    public class ReplayGainAnalysisSettingsControlViewModel : BindableBase
    {
        int _peakAnalysisTypeIndex;

        public string Title { get; } = "ReplayGain";

        public string[] PeakAnalysisTypes => new[] { "Simple", "Interpolated" };

        public int PeakAnalysisTypeIndex
        {
            get => _peakAnalysisTypeIndex;
            set => SetProperty(ref _peakAnalysisTypeIndex, value);
        }

        public ReplayGainAnalysisSettingsControlViewModel(
            ICommandService commandService,
            IAnalysisSettingService settingService)
        {
            var settings = settingService["ReplayGain"];

            commandService.SaveAnalysisSettingsCommand.RegisterCommand(new DelegateCommand(() =>
                SaveSettings(settings)));

            if (settings.TryGetValue("PeakAnalysis", out string peakAnalysis) &&
                peakAnalysis.Equals("Interpolated", StringComparison.Ordinal))
                _peakAnalysisTypeIndex = 1;
        }

        void SaveSettings(SettingDictionary settings)
        {
            if (_peakAnalysisTypeIndex == 1)
                settings["PeakAnalysis"] = "Interpolated";
            else
                settings.Remove("PeakAnalysis");
        }
    }
}
