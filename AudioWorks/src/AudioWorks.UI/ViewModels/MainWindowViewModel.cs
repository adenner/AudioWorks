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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AudioWorks.Api;
using AudioWorks.UI.Services;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AudioWorks.UI.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    public class MainWindowViewModel : BindableBase
    {
        readonly object _lock = new object();
        List<AudioFileViewModel> _selectedAudioFiles = new List<AudioFileViewModel>(0);

        public ObservableCollection<AudioFileViewModel> AudioFiles { get; } =
            new ObservableCollection<AudioFileViewModel>();

        public DelegateCommand<IList> SelectionChangedCommand { get; }

        public DelegateCommand OpenFilesCommand { get; }

        public DelegateCommand OpenDirectoryCommand { get; }

        public DelegateCommand<KeyEventArgs> KeyDownCommand { get; }

        public DelegateCommand<DragEventArgs> DropCommand { get; }

        public DelegateCommand EditSelectionCommand { get; }

        public DelegateCommand OpenMetadataSettingsCommand { get; }

        public DelegateCommand RevertSelectionCommand { get; }

        public DelegateCommand RevertModifiedCommand { get; }

        public DelegateCommand SaveSelectionCommand { get; }

        public DelegateCommand SaveModifiedCommand { get; }

        public DelegateCommand SaveAllCommand { get; }

        public DelegateCommand RemoveSelectionCommand { get; }

        public DelegateCommand<CancelEventArgs> ExitCommand { get; }

        public MainWindowViewModel(
            IFileSelectionService fileSelectionService,
            IDirectorySelectionService directorySelectionService,
            IDialogService prismDialogService,
            IDialogCoordinator metroDialogCoordinator)
        {
            BindingOperations.EnableCollectionSynchronization(AudioFiles, _lock);
            AudioFiles.CollectionChanged += (sender, e) =>
            {
                if (e.NewItems != null)
                    foreach (var newItem in e.NewItems)
                    {
                        ((AudioFileViewModel) newItem).Metadata.PropertyChanged += Metadata_PropertyChanged;
                        SaveAllCommand.RaiseCanExecuteChanged();
                    }

                if (e.OldItems != null)
                    foreach (var oldItem in e.OldItems)
                    {
                        ((AudioFileViewModel) oldItem).Metadata.PropertyChanged -= Metadata_PropertyChanged;
                        RevertModifiedCommand.RaiseCanExecuteChanged();
                        SaveModifiedCommand.RaiseCanExecuteChanged();
                        SaveAllCommand.RaiseCanExecuteChanged();
                    }
            };

            SelectionChangedCommand = new DelegateCommand<IList>(selectedItems =>
            {
                _selectedAudioFiles = selectedItems.Cast<AudioFileViewModel>().ToList();
                EditSelectionCommand.RaiseCanExecuteChanged();
                RevertSelectionCommand.RaiseCanExecuteChanged();
                SaveSelectionCommand.RaiseCanExecuteChanged();
                RemoveSelectionCommand.RaiseCanExecuteChanged();
            });

            OpenFilesCommand = new DelegateCommand(async () => await AddFilesAsync(fileSelectionService.SelectFiles()));

            OpenDirectoryCommand = new DelegateCommand(async () =>
                await AddFilesAsync(GetFilesRecursively(directorySelectionService.SelectDirectory())));

            KeyDownCommand = new DelegateCommand<KeyEventArgs>(e =>
            {
                if (e.Key == Key.Delete)
                {
                    if (RemoveSelectionCommand.CanExecute())
                        RemoveSelectionCommand.Execute();
                    e.Handled = true;
                }
            });

            DropCommand = new DelegateCommand<DragEventArgs>(async e =>
                await AddFilesAsync(((DataObject) e.Data).GetFileDropList().Cast<string>().SelectMany(path =>
                    Directory.Exists(path) ? GetFilesRecursively(path) : new[] { path })));

            EditSelectionCommand = new DelegateCommand(() =>
                    prismDialogService.ShowDialog("EditControl",
                        new DialogParameters { { "AudioFiles", _selectedAudioFiles } }, null),
                () => _selectedAudioFiles.Count > 0);

            OpenMetadataSettingsCommand = new DelegateCommand(() =>
                prismDialogService.ShowDialog("MetadataSettingsControl", new DialogParameters(), null));

            RevertSelectionCommand = new DelegateCommand(() =>
            {
                foreach (var audioFile in _selectedAudioFiles.Where(audioFile => audioFile.RevertCommand.CanExecute()))
                    audioFile.RevertCommand.Execute();
            }, () => _selectedAudioFiles.Any(audioFile => audioFile.RevertCommand.CanExecute()));

            RevertModifiedCommand = new DelegateCommand(() =>
            {
                foreach (var audioFile in AudioFiles.Where(audioFile => audioFile.RevertCommand.CanExecute()))
                    audioFile.RevertCommand.Execute();
            }, () => AudioFiles.Any(audioFile => audioFile.RevertCommand.CanExecute()));

            SaveSelectionCommand = new DelegateCommand(() =>
            {
                foreach (var audioFile in _selectedAudioFiles.Where(audioFile => audioFile.SaveCommand.CanExecute()))
                    audioFile.SaveCommand.Execute();
            }, () => _selectedAudioFiles.Count > 0);

            SaveModifiedCommand = new DelegateCommand(() =>
            {
                foreach (var audioFile in AudioFiles.Where(audioFile =>
                    audioFile.Metadata.Modified && audioFile.SaveCommand.CanExecute()))
                    audioFile.SaveCommand.Execute();
            }, () => AudioFiles.Any(audioFile => audioFile.Metadata.Modified && audioFile.SaveCommand.CanExecute()));

            SaveAllCommand = new DelegateCommand(async () =>
            {
                if (await metroDialogCoordinator.ShowMessageAsync(this, "Are You Sure?",
                        "All files will be re-written according to the current metadata encoder settings.",
                        MessageDialogStyle.AffirmativeAndNegative) != MessageDialogResult.Affirmative)
                    return;

                foreach (var audioFile in AudioFiles.Where(audioFile => audioFile.SaveCommand.CanExecute()))
                    audioFile.SaveCommand.Execute();
            }, () => AudioFiles.Count > 0);

            RemoveSelectionCommand = new DelegateCommand(async () =>
            {
                var modifications = _selectedAudioFiles.Count(audioFile => audioFile.Metadata.Modified);
                if (modifications > 0)
                    switch (await metroDialogCoordinator.ShowMessageAsync(this, "Unsaved Changes",
                        $"There are {modifications} unsaved change(s) in the files you're removing. Do you want to save them now?",
                        MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                        new MetroDialogSettings
                        {
                            AffirmativeButtonText = "Yes",
                            NegativeButtonText = "No",
                            FirstAuxiliaryButtonText = "Cancel",
                            DefaultButtonFocus = MessageDialogResult.FirstAuxiliary
                        }))
                    {
                        case MessageDialogResult.Affirmative:
                            SaveSelectionCommand.Execute();
                            break;
                        case MessageDialogResult.FirstAuxiliary:
                            return;
                    }

                foreach (var audioFile in _selectedAudioFiles)
                    AudioFiles.Remove(audioFile);
            }, () => _selectedAudioFiles.Count > 0);

            ExitCommand = new DelegateCommand<CancelEventArgs>(e =>
            {
                var modifications = AudioFiles.Count(audioFile => audioFile.Metadata.Modified);
                if (modifications == 0) return;

                switch (metroDialogCoordinator.ShowModalMessageExternal(this, "Unsaved Changes",
                    $"There are {modifications} unsaved change(s). Do you want to save them now?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Yes",
                        NegativeButtonText = "No",
                        FirstAuxiliaryButtonText = "Cancel",
                        DefaultButtonFocus = MessageDialogResult.FirstAuxiliary
                    }))
                {
                    case MessageDialogResult.Affirmative:
                        SaveAllCommand.Execute();
                        break;
                    case MessageDialogResult.FirstAuxiliary:
                        e.Cancel = true;
                        break;
                }
            });
        }

        void Metadata_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RevertSelectionCommand.RaiseCanExecuteChanged();
            RevertModifiedCommand.RaiseCanExecuteChanged();
            SaveModifiedCommand.RaiseCanExecuteChanged();
        }

        static IEnumerable<string> GetFilesRecursively(string directoryPath) =>
            string.IsNullOrEmpty(directoryPath)
                ? Enumerable.Empty<string>()
                : Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories);

        async Task AddFilesAsync(IEnumerable<string> filePaths) =>
            await Task.Run(() =>
            {
                var validExtensions = AudioFileManager.GetFormatInfo().Select(info => info.Extension).ToList();
                var existingFiles = AudioFiles.Select(audioFile => audioFile.Path);

                foreach (var newFile in filePaths.Where(file =>
                        validExtensions.Contains(new FileInfo(file).Extension, StringComparer.OrdinalIgnoreCase) &&
                        !existingFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
                    .Select(file => new AudioFileViewModel(new TaggedAudioFile(file))))
                    AudioFiles.Add(newFile);
            });
    }
}
