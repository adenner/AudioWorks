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
using System.IO;
using AudioWorks.Common;
using Newtonsoft.Json;

namespace AudioWorks.UI
{
    public static class SettingManager
    {
        static readonly string _encoderSettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AudioWorks",
            "UI",
            "Settings",
            "Encoder");
        static readonly string _metadataSettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AudioWorks",
            "UI",
            "Settings",
            "Metadata");

        public static IDictionary<string, SettingDictionary> EncoderSettings { get; } =
            LoadFromDisk(_encoderSettingsPath);

        public static IDictionary<string, SettingDictionary> MetadataSettings { get; } =
            LoadFromDisk(_metadataSettingsPath);

        internal static void SaveToDisk()
        {
            Directory.CreateDirectory(_encoderSettingsPath);
            foreach (var (extension, settings) in EncoderSettings)
                using (var writer = new StreamWriter(Path.Combine(_encoderSettingsPath, $"{extension.TrimStart('.')}.json")))
                    writer.Write(JsonConvert.SerializeObject(settings));

            Directory.CreateDirectory(_metadataSettingsPath);
            foreach (var (extension, settings) in MetadataSettings)
                using (var writer = new StreamWriter(Path.Combine(_metadataSettingsPath, $"{extension.TrimStart('.')}.json")))
                    writer.Write(JsonConvert.SerializeObject(settings));
        }

        static Dictionary<string, SettingDictionary> LoadFromDisk(string path)
        {
            var result = new Dictionary<string, SettingDictionary>();
            if (Directory.Exists(path))
                foreach (var file in Directory.EnumerateFiles(path, "*.json"))
                    using (var reader = new StreamReader(file))
                        result[$".{Path.GetFileNameWithoutExtension(file)}"] =
                            JsonConvert.DeserializeObject<SettingDictionary>(reader.ReadToEnd(),
                                new SettingDictionaryConverter());
            return result;
        }
    }
}
