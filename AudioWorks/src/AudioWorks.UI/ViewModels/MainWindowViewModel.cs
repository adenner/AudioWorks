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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AudioWorks.Api;
using AudioWorks.UI.Services;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace AudioWorks.UI.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    public class MainWindowViewModel : BindableBase
    {
        List<AudioFileViewModel> _selectedAudioFiles = new List<AudioFileViewModel>(0);

        public ObservableCollection<AudioFileViewModel> AudioFiles { get; } =
            new ObservableCollection<AudioFileViewModel>();

        public DelegateCommand<IList> SelectionChangedCommand { get; }

        public DelegateCommand OpenFilesCommand { get; }

        public DelegateCommand EditSelectionCommand { get; }

        public DelegateCommand RevertSelectionCommand { get; }

        public DelegateCommand SaveSelectionCommand { get; }

        public DelegateCommand RemoveSelectionCommand { get; }

        public DelegateCommand ExitCommand { get; }

        public InteractionRequest<INotification> EditNotificationRequest { get; } = new InteractionRequest<INotification>();

        public MainWindowViewModel(IFileSelectionService fileSelectionService, IAppShutdownService appShutdownService)
        {
            SelectionChangedCommand = new DelegateCommand<IList>(selectedItems =>
            {
                _selectedAudioFiles = selectedItems.Cast<AudioFileViewModel>().ToList();
                EditSelectionCommand.RaiseCanExecuteChanged();
                RevertSelectionCommand.RaiseCanExecuteChanged();
                SaveSelectionCommand.RaiseCanExecuteChanged();
                RemoveSelectionCommand.RaiseCanExecuteChanged();
            });

            OpenFilesCommand = new DelegateCommand(() =>
            {
                var newFiles = fileSelectionService.SelectFiles().ToList();

                // Skip files that may have been added previously
                foreach (var existingFile in AudioFiles.Select(audioFile => audioFile.Path))
                    if (newFiles.Contains(existingFile, StringComparer.OrdinalIgnoreCase))
                        newFiles.Remove(existingFile);

                AudioFiles.AddRange(newFiles.Select(file => new AudioFileViewModel(new TaggedAudioFile(file))));
            });

            EditSelectionCommand = new DelegateCommand(
                () => EditNotificationRequest.Raise(new Notification
                {
                    Title = _selectedAudioFiles.Count > 1
                        ? $"Editing {_selectedAudioFiles.Count} files"
                        : "Editing 1 file",
                    Content = _selectedAudioFiles
                }),
                () => _selectedAudioFiles.Count > 0);

            RevertSelectionCommand = new DelegateCommand(() =>
            {
                foreach (var audioFile in _selectedAudioFiles.Where(audioFile => audioFile.RevertCommand.CanExecute()))
                    audioFile.RevertCommand.Execute();
                RevertSelectionCommand.RaiseCanExecuteChanged();
            }, () => _selectedAudioFiles.Any(audioFile => audioFile.RevertCommand.CanExecute()));

            SaveSelectionCommand = new DelegateCommand(() =>
            {
                foreach (var audioFile in _selectedAudioFiles.Where(audioFile => audioFile.SaveCommand.CanExecute()))
                    audioFile.SaveCommand.Execute();
            }, () => _selectedAudioFiles.Count > 0);

            RemoveSelectionCommand = new DelegateCommand(() =>
            {
                foreach (var audioFile in _selectedAudioFiles)
                    AudioFiles.Remove(audioFile);
            }, () => _selectedAudioFiles.Count > 0);

            ExitCommand = new DelegateCommand(appShutdownService.Shutdown);
        }
    }
}
