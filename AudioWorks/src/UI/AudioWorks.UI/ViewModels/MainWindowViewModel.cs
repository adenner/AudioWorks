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
        bool _isBusy;
        bool _groupingDisabled;
        bool _groupByDirectory;
        bool _groupByAlbum;
        readonly object _lock = new object();
        readonly GroupDescription _directoryGroupDescription = new PropertyGroupDescription
            { Converter = new GroupByDirectoryConverter() };
        readonly GroupDescription _albumGroupDescription = new PropertyGroupDescription
            { Converter = new GroupByAlbumConverter() };
        List<AudioFileViewModel> _selectedAudioFiles = new List<AudioFileViewModel>(0);

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool GroupingDisabled
        {
            get => _groupingDisabled;
            set
            {
                if (!value) return;

                SetGroupDescriptions(null);
                SetProperty(ref _groupingDisabled, value);
                _groupByDirectory = false;
                RaisePropertyChanged("GroupByDirectory");
                _groupByAlbum = false;
                RaisePropertyChanged("GroupByAlbum");
            }
        }

        public bool GroupByDirectory
        {
            get => _groupByDirectory;
            set
            {
                if (!value) return;

                SetGroupDescriptions(_directoryGroupDescription);
                SetProperty(ref _groupByDirectory, value);
                _groupingDisabled = false;
                RaisePropertyChanged("GroupingDisabled");
                _groupByAlbum = false;
                RaisePropertyChanged("GroupByAlbum");
            }
        }

        public bool GroupByAlbum
        {
            get => _groupByAlbum;
            set
            {
                if (!value) return;

                SetGroupDescriptions(_albumGroupDescription);
                SetProperty(ref _groupByAlbum, value);
                _groupingDisabled = false;
                RaisePropertyChanged("GroupingDisabled");
                _groupByDirectory = false;
                RaisePropertyChanged("GroupByDirectory");
            }
        }

        public AudioAnalyzerInfo[] Analyzers { get; } = AudioAnalyzerManager.GetAnalyzerInfo().ToArray();

        public AudioEncoderInfo[] Encoders { get; } = AudioEncoderManager.GetEncoderInfo().ToArray();

        public ICollectionView AudioFiles { get; }

        public DelegateCommand<IList> SelectionChangedCommand { get; }

        public DelegateCommand OpenFilesCommand { get; }

        public DelegateCommand OpenDirectoryCommand { get; }

        public DelegateCommand<KeyEventArgs> KeyDownCommand { get; }

        public DelegateCommand<DragEventArgs> DropCommand { get; }

        public DelegateCommand EditSelectionCommand { get; }

        public DelegateCommand RevertSelectionCommand { get; }

        public DelegateCommand RevertModifiedCommand { get; }

        public DelegateCommand SaveSelectionCommand { get; }

        public DelegateCommand SaveModifiedCommand { get; }

        public DelegateCommand SaveAllCommand { get; }

        public DelegateCommand RemoveSelectionCommand { get; }

        public DelegateCommand<string> AnalyzeAllCommand { get; }

        public DelegateCommand<string> EncodeAllCommand { get; }

        public DelegateCommand<CancelEventArgs> ExitCommand { get; }

        public MainWindowViewModel(
            IFileSelectionService fileSelectionService,
            IDialogService prismDialogService,
            IEncoderSettingService encoderSettingService,
            IDialogCoordinator metroDialogCoordinator)
        {
            var audioFilesCollection = new ObservableCollection<AudioFileViewModel>();
            BindingOperations.EnableCollectionSynchronization(audioFilesCollection, _lock);
            AudioFiles = CollectionViewSource.GetDefaultView(audioFilesCollection);

            GroupByDirectory = true;

            audioFilesCollection.CollectionChanged += (sender, e) =>
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
                        AnalyzeAllCommand.RaiseCanExecuteChanged();
                        EncodeAllCommand.RaiseCanExecuteChanged();
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

            OpenFilesCommand = new DelegateCommand(async () =>
                {
                    IsBusy = true;
                    await AddFilesAsync(fileSelectionService.SelectFiles());
                    IsBusy = false;
                }, () => !IsBusy)
                .ObservesProperty(() => IsBusy);

            OpenDirectoryCommand = new DelegateCommand(async () =>
                {
                    IsBusy = true;
                    await AddFilesAsync(GetFilesRecursively(fileSelectionService.SelectDirectory()));
                    IsBusy = false;
                }, () => !IsBusy)
                .ObservesProperty(() => IsBusy);

            KeyDownCommand = new DelegateCommand<KeyEventArgs>(e =>
            {
                if (e.Key != Key.Delete) return;

                if (RemoveSelectionCommand.CanExecute())
                    RemoveSelectionCommand.Execute();
                e.Handled = true;
            });

            DropCommand = new DelegateCommand<DragEventArgs>(async e =>
                {
                    IsBusy = true;
                    await AddFilesAsync(((DataObject) e.Data).GetFileDropList().Cast<string>().SelectMany(path =>
                        Directory.Exists(path) ? GetFilesRecursively(path) : new[] { path }));
                    IsBusy = false;
                }, e => !IsBusy)
                .ObservesProperty(() => IsBusy);

            EditSelectionCommand = new DelegateCommand(() =>
                    prismDialogService.ShowDialog("EditControl",
                        new DialogParameters { { "AudioFiles", _selectedAudioFiles } }, null),
                () => _selectedAudioFiles.Count > 0);

            RevertSelectionCommand = new DelegateCommand(() =>
                {
                    IsBusy = true;
                    foreach (var audioFile in _selectedAudioFiles.Where(audioFile =>
                        audioFile.RevertCommand.CanExecute()))
                        audioFile.RevertCommand.Execute();
                    IsBusy = false;
                }, () => !IsBusy && _selectedAudioFiles.Any(audioFile => audioFile.RevertCommand.CanExecute()))
                .ObservesProperty(() => IsBusy);

            RevertModifiedCommand = new DelegateCommand(() =>
                {
                    IsBusy = true;
                    foreach (var audioFile in audioFilesCollection
                        .Where(audioFile => audioFile.RevertCommand.CanExecute()))
                        audioFile.RevertCommand.Execute();
                    IsBusy = false;
                }, () => !IsBusy && audioFilesCollection.Any(audioFile => audioFile.RevertCommand.CanExecute()))
                .ObservesProperty(() => IsBusy);

            SaveSelectionCommand = new DelegateCommand(() =>
                {
                    IsBusy = true;
                    foreach (var audioFile in _selectedAudioFiles.Where(audioFile =>
                        audioFile.SaveCommand.CanExecute()))
                        audioFile.SaveCommand.Execute();
                    IsBusy = false;
                }, () => !IsBusy && _selectedAudioFiles.Count > 0)
                .ObservesProperty(() => IsBusy);

            SaveModifiedCommand = new DelegateCommand(() =>
                {
                    IsBusy = true;
                    foreach (var audioFile in audioFilesCollection.Where(audioFile =>
                        audioFile.Metadata.Modified && audioFile.SaveCommand.CanExecute()))
                        audioFile.SaveCommand.Execute();
                    IsBusy = false;
                }, () => !IsBusy && audioFilesCollection.Any(audioFile =>
                             audioFile.Metadata.Modified && audioFile.SaveCommand.CanExecute()))
                .ObservesProperty(() => IsBusy);

            SaveAllCommand = new DelegateCommand(async () =>
                {
                    IsBusy = true;
                    if (await metroDialogCoordinator.ShowMessageAsync(this, "Are You Sure?",
                            "All files will be re-written according to the current metadata encoder settings.",
                            MessageDialogStyle.AffirmativeAndNegative) == MessageDialogResult.Affirmative)
                        foreach (var audioFile in audioFilesCollection
                            .Where(audioFile => audioFile.SaveCommand.CanExecute()))
                            audioFile.SaveCommand.Execute();
                    IsBusy = false;
                }, () => !IsBusy && audioFilesCollection.Count > 0)
                .ObservesProperty(() => IsBusy);

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
                    audioFilesCollection.Remove(audioFile);
            }, () => _selectedAudioFiles.Count > 0);

            AnalyzeAllCommand = new DelegateCommand<string>(name =>
                        prismDialogService.ShowDialog("AnalysisControl",
                            new DialogParameters { { "Name", name }, { "AudioFiles", AudioFiles } }, result =>
                            {
                                foreach (var audioFile in audioFilesCollection)
                                    audioFile.Metadata.Refresh();
                            }),
                    name => !IsBusy && audioFilesCollection.Count > 0)
                .ObservesProperty(() => IsBusy);

            EncodeAllCommand = new DelegateCommand<string>(async name =>
                {
                    //TODO
                }, name => !IsBusy && audioFilesCollection.Count > 0)
                .ObservesProperty(() => IsBusy);

            ExitCommand = new DelegateCommand<CancelEventArgs>(e =>
            {
                var modifications = audioFilesCollection.Count(audioFile => audioFile.Metadata.Modified);
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

        void SetGroupDescriptions(GroupDescription? groupDescription)
        {
            AudioFiles.GroupDescriptions.Clear();
            if (groupDescription != null)
                AudioFiles.GroupDescriptions.Add(groupDescription);
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
                var audioFileCollection = (ObservableCollection<AudioFileViewModel>) AudioFiles.SourceCollection;
                var existingFiles = audioFileCollection.Select(audioFile => audioFile.Path);

                foreach (var newFile in filePaths.Where(file =>
                        validExtensions.Contains(new FileInfo(file).Extension, StringComparer.OrdinalIgnoreCase) &&
                        !existingFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
                    .Select(file => new AudioFileViewModel(new TaggedAudioFile(file))))
                    audioFileCollection.Add(newFile);
            });
    }
}
