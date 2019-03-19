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

using AudioWorks.Common;
using Prism.Commands;
using Prism.Mvvm;
using IO = System.IO;

namespace AudioWorks.UI.ViewModels
{
    public sealed class AudioFileViewModel : BindableBase
    {
        readonly ITaggedAudioFile _audioFile;

        public string Path => _audioFile.Path;

        public AudioInfo Info => _audioFile.Info;

        public AudioMetadataViewModel Metadata { get; }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand RevertCommand { get; }

        public AudioFileViewModel(ITaggedAudioFile audioFile)
        {
            _audioFile = audioFile;
            Metadata = new AudioMetadataViewModel(audioFile.Metadata);
            Metadata.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("Modified"))
                    RevertCommand.RaiseCanExecuteChanged();
            };

            SaveCommand = new DelegateCommand(() =>
            {
                SettingManager.MetadataSettings.TryGetValue(IO.Path.GetExtension(Path), out var settings);
                _audioFile.SaveMetadata(settings);
                Metadata.UpdateModel(_audioFile.Metadata);
            });

            RevertCommand = new DelegateCommand(() =>
            {
                _audioFile.LoadMetadata();
                Metadata.UpdateModel(_audioFile.Metadata);
            }, () => Metadata.Modified);
        }
    }
}
