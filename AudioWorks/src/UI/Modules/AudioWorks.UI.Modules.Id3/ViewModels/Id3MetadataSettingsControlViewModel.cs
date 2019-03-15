﻿/* Copyright © 2019 Jeremy Herbison

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

namespace AudioWorks.UI.Modules.Id3.ViewModels
{
    public class Id3MetadataSettingsControlViewModel : BindableBase
    {
        bool _configurePadding;
        int _padding;

        public string Title { get; } = "ID3";

        public bool ConfigurePadding
        {
            get => _configurePadding;
            set => SetProperty(ref _configurePadding, value);
        }

        public int Padding
        {
            get => _padding;
            set => SetProperty(ref _padding, value);
        }

        public Id3MetadataSettingsControlViewModel(ICommandService commandService)
        {
            commandService.SaveMetadataSettingsCommand.RegisterCommand(new DelegateCommand(SaveSettings));

            if (SettingManager.MetadataEncoderSettings.TryGetValue(".mp3", out var settings) &&
                settings.TryGetValue("Padding", out int padding))
            {
                _padding = padding;
                _configurePadding = true;
            }
            else
            {
                _padding = 2048;
                _configurePadding = false;
            }
        }

        void SaveSettings()
        {
            if (!SettingManager.MetadataEncoderSettings.TryGetValue(".mp3", out var settings))
            {
                settings = new SettingDictionary();
                SettingManager.MetadataEncoderSettings.Add(".mp3", settings);
            }

            if (_configurePadding)
                settings["Padding"] = _padding;
            else
                settings.Remove("Padding");
        }
    }
}
