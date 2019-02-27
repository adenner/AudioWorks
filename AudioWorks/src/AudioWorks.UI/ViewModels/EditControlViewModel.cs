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
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace AudioWorks.UI.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    public class EditControlViewModel : BindableBase, IInteractionRequestAware
    {
        INotification? _notification;
        List<AudioFileViewModel>? _audioFiles;
        bool _titleIsCommon;
        string _title = string.Empty;
        bool _artistIsCommon;
        string _artist = string.Empty;
        bool _albumIsCommon;
        string _album = string.Empty;
        bool _albumArtistIsCommon;
        string _albumArtist = string.Empty;
        bool _composerIsCommon;
        string _composer = string.Empty;
        bool _genreIsCommon;
        string _genre = string.Empty;

        public INotification? Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value);
                _audioFiles = (List<AudioFileViewModel>?) value?.Content;
                SetProperties();
            }
        }

        public Action? FinishInteraction { get; set; }

        public bool TitleIsCommon
        {
            get => _titleIsCommon;
            set => SetProperty(ref _titleIsCommon, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool ArtistIsCommon
        {
            get => _artistIsCommon;
            set => SetProperty(ref _artistIsCommon, value);
        }

        public string Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        public bool AlbumIsCommon
        {
            get => _albumIsCommon;
            set => SetProperty(ref _albumIsCommon, value);
        }

        public string Album
        {
            get => _album;
            set => SetProperty(ref _album, value);
        }

        public bool AlbumArtistIsCommon
        {
            get => _albumArtistIsCommon;
            set => SetProperty(ref _albumArtistIsCommon, value);
        }

        public string AlbumArtist
        {
            get => _albumArtist;
            set => SetProperty(ref _albumArtist, value);
        }

        public bool ComposerIsCommon
        {
            get => _composerIsCommon;
            set => SetProperty(ref _composerIsCommon, value);
        }

        public string Composer
        {
            get => _composer;
            set => SetProperty(ref _composer, value);
        }

        public bool GenreIsCommon
        {
            get => _genreIsCommon;
            set => SetProperty(ref _genreIsCommon, value);
        }

        public string Genre
        {
            get => _genre;
            set => SetProperty(ref _genre, value);
        }

        public DelegateCommand ApplyCommand { get; }

        public EditControlViewModel()
        {
            ApplyCommand = new DelegateCommand(() =>
            {
                if (_audioFiles != null)
                    foreach (var audioFile in _audioFiles)
                    {
                        if (TitleIsCommon)
                            audioFile.Metadata.Title = Title;
                        if (ArtistIsCommon)
                            audioFile.Metadata.Artist = Artist;
                        if (AlbumIsCommon)
                            audioFile.Metadata.Album = Album;
                        if (AlbumArtistIsCommon)
                            audioFile.Metadata.AlbumArtist = AlbumArtist;
                        if (ComposerIsCommon)
                            audioFile.Metadata.Composer = Composer;
                        if (GenreIsCommon)
                            audioFile.Metadata.Genre = Genre;
                    }

                FinishInteraction?.Invoke();
            });
        }

        void SetProperties()
        {
            if (_audioFiles!.TrueForAll(audioFile =>
                audioFile.Metadata.Title.Equals(_audioFiles![0].Metadata.Title, StringComparison.Ordinal)))
            {
                TitleIsCommon = true;
                Title = _audioFiles[0].Metadata.Title;
            }
            else
            {
                TitleIsCommon = false;
                Title = string.Empty;
            }

            if (_audioFiles.TrueForAll(audioFile =>
                audioFile.Metadata.Artist.Equals(_audioFiles![0].Metadata.Artist, StringComparison.Ordinal)))
            {
                ArtistIsCommon = true;
                Artist = _audioFiles[0].Metadata.Artist;
            }
            else
            {
                ArtistIsCommon = false;
                Artist = string.Empty;
            }

            if (_audioFiles.TrueForAll(audioFile =>
                audioFile.Metadata.Album.Equals(_audioFiles![0].Metadata.Album, StringComparison.Ordinal)))
            {
                AlbumIsCommon = true;
                Album = _audioFiles[0].Metadata.Album;
            }
            else
            {
                AlbumIsCommon = false;
                Album = string.Empty;
            }

            if (_audioFiles.TrueForAll(audioFile =>
                audioFile.Metadata.AlbumArtist.Equals(_audioFiles![0].Metadata.AlbumArtist, StringComparison.Ordinal)))
            {
                AlbumArtistIsCommon = true;
                AlbumArtist = _audioFiles[0].Metadata.AlbumArtist;
            }
            else
            {
                AlbumArtistIsCommon = false;
                AlbumArtist = string.Empty;
            }

            if (_audioFiles.TrueForAll(audioFile =>
                audioFile.Metadata.Composer.Equals(_audioFiles![0].Metadata.Composer, StringComparison.Ordinal)))
            {
                ComposerIsCommon = true;
                Composer = _audioFiles[0].Metadata.Composer;
            }
            else
            {
                ComposerIsCommon = false;
                Composer = string.Empty;
            }

            if (_audioFiles.TrueForAll(audioFile =>
                audioFile.Metadata.Genre.Equals(_audioFiles![0].Metadata.Genre, StringComparison.Ordinal)))
            {
                GenreIsCommon = true;
                Genre = _audioFiles[0].Metadata.Genre;
            }
            else
            {
                GenreIsCommon = false;
                Genre = string.Empty;
            }

        }
    }
}
