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

namespace AudioWorks.UI.Modules.Id3.ViewModels
{
    public class Id3MetadataSettingsControlViewModel : BindableBase
    {
        int _versionIndex;
        int _encodingIndex;
        bool _configurePadding;
        int _padding;

        public string Title { get; } = "ID3";

        public string[] Versions => new[] { "2.3", "2.4" };

        public int VersionIndex
        {
            get => _versionIndex;
            set => SetProperty(ref _versionIndex, value);
        }

        public string[] Encodings => new[] { "Latin-1", "UTF-16" };

        public int EncodingIndex
        {
            get => _encodingIndex;
            set => SetProperty(ref _encodingIndex, value);
        }

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

        public Id3MetadataSettingsControlViewModel(
            ICommandService commandService,
            IMetadataSettingService settingService)
        {
            var settings = settingService["mp3"];

            commandService.SaveMetadataSettingsCommand.RegisterCommand(
                new DelegateCommand(() => SaveSettings(settings)));

            if (settings.TryGetValue("TagVersion", out string version) &&
                version.Equals("2.4", StringComparison.Ordinal))
                _versionIndex = 1;

            if (settings.TryGetValue("TagEncoding", out string encoding) &&
                encoding.Equals("UTF16", StringComparison.Ordinal))
                _encodingIndex = 1;

            if (settings.TryGetValue("Padding", out int padding))
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

        void SaveSettings(SettingDictionary settings)
        {
            if (_versionIndex == 1)
                settings["TagVersion"] = "2.4";
            else
                settings.Remove("TagVersion");


            if (_encodingIndex == 1)
                settings["TagEncoding"] = "UTF16";
            else
                settings.Remove("TagEncoding");

            if (_configurePadding)
                settings["Padding"] = _padding;
            else
                settings.Remove("Padding");
        }
    }
}
