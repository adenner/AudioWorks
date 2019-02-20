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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using AudioWorks.Api;
using Prism.Commands;
using Prism.Mvvm;

namespace AudioWorks.UI.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    public class MainWindowViewModel : BindableBase
    {
        public ListCollectionView AudioFiles { get; } = new ListCollectionView(new List<TaggedAudioFile>());

        public DelegateCommand SelectFilesCommand { get; }

        public DelegateCommand ExitCommand { get; }

        public MainWindowViewModel(IFileSelectionService fileSelectionService, IAppShutdownService appShutdownService)
        {
            SelectFilesCommand = new DelegateCommand(() =>
            {
                var newFiles = fileSelectionService.SelectFiles().ToList();

                // Skip files that were added previously
                foreach (var existingFile in ((List<TaggedAudioFile>) AudioFiles.SourceCollection)
                    .Select(audioFile => audioFile.Path))
                    if (newFiles.Contains(existingFile, StringComparer.OrdinalIgnoreCase))
                        newFiles.Remove(existingFile);

                // Parse the audio files asynchronously
                foreach (var newAudioFile in newFiles.Select(file => new TaggedAudioFile(file)))
                    AudioFiles.AddNewItem(newAudioFile);
            });

            ExitCommand = new DelegateCommand(appShutdownService.Shutdown);
        }
    }
}
