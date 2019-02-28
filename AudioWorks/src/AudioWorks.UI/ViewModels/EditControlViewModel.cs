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
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AudioWorks.UI.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    public class EditControlViewModel : DialogViewModelBase, INotifyDataErrorInfo
    {
        readonly ErrorsContainer<ValidationResult> _errors;
        List<AudioFileViewModel>? _audioFiles;
        bool _isMultiple;
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
        bool _commentIsCommon;
        string _comment = string.Empty;
        bool _dayIsCommon;
        string _day = string.Empty;
        bool _monthIsCommon;
        string _month = string.Empty;
        bool _yearIsCommon;
        string _year = string.Empty;
        bool _trackNumberIsCommon;
        string _trackNumber = string.Empty;
        bool _trackCountIsCommon;
        string _trackCount = string.Empty;

        public bool IsMultiple
        {
            get => _isMultiple;
            set => SetProperty(ref _isMultiple, value);
        }

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

        public bool CommentIsCommon
        {
            get => _commentIsCommon;
            set => SetProperty(ref _commentIsCommon, value);
        }

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public bool DayIsCommon
        {
            get => _dayIsCommon;
            set => SetProperty(ref _dayIsCommon, value);
        }

        public string Day
        {
            get => _day;
            set
            {
                if (string.IsNullOrEmpty(value) || int.TryParse(value, out var intValue) && intValue >= 1 && intValue <= 31)
                    _errors.ClearErrors(() => Day);
                else
                    _errors.SetErrors(() => Day,
                        new[] { new ValidationResult(false, "Day must be between 1 and 31.") });
                SetProperty(ref _day, value);
            }
        }

        public bool MonthIsCommon
        {
            get => _monthIsCommon;
            set => SetProperty(ref _monthIsCommon, value);
        }

        public string Month
        {
            get => _month;
            set
            {
                if (string.IsNullOrEmpty(value) || int.TryParse(value, out var intValue) && intValue >= 1 && intValue <= 12)
                    _errors.ClearErrors(() => Month);
                else
                    _errors.SetErrors(() => Month,
                        new[] { new ValidationResult(false, "Month must be between 1 and 12.") });
                SetProperty(ref _month, value);
            }
        }

        public bool YearIsCommon
        {
            get => _yearIsCommon;
            set => SetProperty(ref _yearIsCommon, value);
        }

        public string Year
        {
            get => _year;
            set
            {
                if (string.IsNullOrEmpty(value) || Regex.IsMatch(value, "^[1-9][0-9]{3}$"))
                    _errors.ClearErrors(() => Year);
                else
                    _errors.SetErrors(() => Year,
                        new[] { new ValidationResult(false, "Year must be between 1000 and 9999.") });
                SetProperty(ref _year, value);
            }
        }

        public bool TrackNumberIsCommon
        {
            get => _trackNumberIsCommon;
            set => SetProperty(ref _trackNumberIsCommon, value);
        }

        public string TrackNumber
        {
            get => _trackNumber;
            set
            {
                if (string.IsNullOrEmpty(value) || int.TryParse(value, out var intValue) && intValue >= 1 && intValue <= 99)
                    _errors.ClearErrors(() => TrackNumber);
                else
                    _errors.SetErrors(() => TrackNumber,
                        new[] { new ValidationResult(false, "Track # must be between 1 and 99.") });
                SetProperty(ref _trackNumber, value);
            }
        }

        public bool TrackCountIsCommon
        {
            get => _trackCountIsCommon;
            set => SetProperty(ref _trackCountIsCommon, value);
        }

        public string TrackCount
        {
            get => _trackCount;
            set
            {
                if (string.IsNullOrEmpty(value) || int.TryParse(value, out var intValue) && intValue >= 1 && intValue <= 99)
                    _errors.ClearErrors(() => TrackCount);
                else
                    _errors.SetErrors(() => TrackCount,
                        new[] { new ValidationResult(false, "Track count must be between 1 and 99.") });
                SetProperty(ref _trackCount, value);
            }
        }

        public DelegateCommand ApplyCommand { get; }

        public bool HasErrors => _errors.HasErrors;

        public EditControlViewModel()
        {
            _errors = new ErrorsContainer<ValidationResult>(RaiseErrorsChanged);

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
                        if (CommentIsCommon)
                            audioFile.Metadata.Comment = Comment;
                        if (DayIsCommon)
                            audioFile.Metadata.Day = Day;
                        if (MonthIsCommon)
                            audioFile.Metadata.Month = Month;
                        if (YearIsCommon)
                            audioFile.Metadata.Year = Year;
                        if (TrackNumberIsCommon)
                            audioFile.Metadata.TrackNumber = TrackNumber;
                        if (TrackCountIsCommon)
                            audioFile.Metadata.TrackCount = TrackCount;
                    }

                RaiseRequestClose(new DialogResult(true));
            });
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName) => _errors.GetErrors(propertyName);

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            _audioFiles = parameters.GetValue<List<AudioFileViewModel>>("AudioFiles");
            SetProperties();
        }

        void SetProperties()
        {
            IsMultiple = _audioFiles!.Count > 1;

            var thisType = GetType();

            foreach (var propertyName in thisType.GetProperties()
                .Where(prop => prop.PropertyType == typeof(string))
                .Select(prop => prop.Name)
                .Except(new[] { "IconSource" }))
            {
                var propertyInfo = typeof(AudioMetadataViewModel).GetProperty(propertyName);
                var firstValue = (string) propertyInfo.GetValue(_audioFiles[0].Metadata);

                if (_audioFiles.TrueForAll(audioFile =>
                    ((string) propertyInfo.GetValue(audioFile.Metadata)).Equals(firstValue, StringComparison.Ordinal)))
                {
                    thisType.GetProperty($"{propertyName}IsCommon").SetValue(this, true);
                    thisType.GetProperty(propertyName).SetValue(this, firstValue);
                }
                else
                {
                    thisType.GetProperty($"{propertyName}IsCommon").SetValue(this, false);
                    thisType.GetProperty(propertyName).SetValue(this, string.Empty);
                }
            }
        }

        void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged("HasErrors");
        }
    }
}
