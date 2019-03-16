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
using System.IO;
using System.Windows.Media.Imaging;
using AudioWorks.Common;
using Prism.Mvvm;

namespace AudioWorks.UI.ViewModels
{
    public sealed class AudioMetadataViewModel : BindableBase
    {
        AudioMetadata _metadata;
        Lazy<BitmapImage?> _coverImage;
        bool _modified;

        public string SongTitle
        {
            get => _metadata.Title;
            set
            {
                _metadata.Title = value;
                RaisePropertyChanged();
            }
        }

        public string Artist
        {
            get => _metadata.Artist;
            set
            {
                _metadata.Artist = value;
                RaisePropertyChanged();
            }
        }

        public string Album
        {
            get => _metadata.Album;
            set
            {
                _metadata.Album = value;
                RaisePropertyChanged();
            }
        }

        public string AlbumArtist
        {
            get => _metadata.AlbumArtist;
            set
            {
                _metadata.AlbumArtist = value;
                RaisePropertyChanged();
            }
        }

        public string Composer
        {
            get => _metadata.Composer;
            set
            {
                _metadata.Composer = value;
                RaisePropertyChanged();
            }
        }

        public string Genre
        {
            get => _metadata.Genre;
            set
            {
                _metadata.Genre = value;
                RaisePropertyChanged();
            }
        }

        public string Comment
        {
            get => _metadata.Comment;
            set
            {
                _metadata.Comment = value;
                RaisePropertyChanged();
            }
        }


        public string Day
        {
            get => _metadata.Day;
            set
            {
                _metadata.Day = value;
                RaisePropertyChanged();
            }
        }


        public string Month
        {
            get => _metadata.Month;
            set
            {
                _metadata.Month = value;
                RaisePropertyChanged();
            }
        }


        public string Year
        {
            get => _metadata.Year;
            set
            {
                _metadata.Year = value;
                RaisePropertyChanged();
            }
        }

        public string TrackNumber
        {
            get => _metadata.TrackNumber;
            set
            {
                _metadata.TrackNumber = value;
                RaisePropertyChanged();
            }
        }

        public string TrackCount
        {
            get => _metadata.TrackCount;
            set
            {
                _metadata.TrackCount = value;
                RaisePropertyChanged();
            }
        }

        public string TrackPeak => _metadata.TrackPeak;

        public string AlbumPeak => _metadata.AlbumPeak;

        public string TrackGain => _metadata.TrackGain;

        public string AlbumGain => _metadata.AlbumGain;

        public BitmapImage? CoverImage => _coverImage.Value;

        public bool Modified
        {
            get => _modified;
            private set => SetProperty(ref _modified, value);
        }

        public AudioMetadataViewModel(AudioMetadata metadata)
        {
            _metadata = metadata;
            PropertyChanged += (sender, e) =>
            {
                if (!e.PropertyName.Equals("Modified", StringComparison.Ordinal))
                    Modified = true;
            };

            _coverImage = new Lazy<BitmapImage?>(() => LoadImage(metadata.CoverArt));
        }

        internal void UpdateModel(AudioMetadata metadata)
        {
            _metadata = metadata;
            _coverImage = new Lazy<BitmapImage?>(() => LoadImage(metadata.CoverArt));
            RaisePropertyChanged(string.Empty);
            Modified = false;
        }

        static unsafe BitmapImage? LoadImage(ICoverArt? coverArt)
        {
            if (coverArt == null) return null;

            fixed (byte* dataAddress = coverArt.Data)
            {
                using (var stream = new UnmanagedMemoryStream(dataAddress, coverArt.Data.Length))
                {
                    var result = new BitmapImage();
                    result.BeginInit();
                    result.StreamSource = stream;
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    result.EndInit();
                    result.Freeze();
                    return result;
                }
            }
        }
    }
}