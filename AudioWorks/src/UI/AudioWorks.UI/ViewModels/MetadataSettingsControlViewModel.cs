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

using AudioWorks.UI.Services;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace AudioWorks.UI.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    public class MetadataSettingsControlViewModel : DialogViewModelBase
    {
        public DelegateCommand SaveCommand { get; }

        public MetadataSettingsControlViewModel(ICommandService commandService)
        {
            SaveCommand = new DelegateCommand(() =>
            {
                if (commandService.SaveMetadataSettingsCommand.CanExecute(null))
                    commandService.SaveMetadataSettingsCommand.Execute(null);
                RaiseRequestClose(new DialogResult(true));
            });
        }
    }
}
