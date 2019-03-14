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
        static readonly string _settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AudioWorks",
            "UI",
            "Settings");

        public static IDictionary<string, SettingDictionary> MetadataEncoderSettings { get; } = LoadFromDisk();

        internal static void SaveToDisk()
        {
            Directory.CreateDirectory(_settingsPath);

            foreach (var (extension, settings) in MetadataEncoderSettings)
                using (var writer = new StreamWriter(Path.Combine(_settingsPath, $"{extension.TrimStart('.')}.json")))
                    writer.Write(JsonConvert.SerializeObject(settings));
        }

        static Dictionary<string, SettingDictionary> LoadFromDisk()
        {
            var result = new Dictionary<string, SettingDictionary>();
            if (Directory.Exists(_settingsPath))
                foreach (var file in Directory.EnumerateFiles(_settingsPath, "*.json"))
                    using (var reader = new StreamReader(file))
                        result[$".{Path.GetFileNameWithoutExtension(file)}"] =
                            JsonConvert.DeserializeObject<SettingDictionary>(reader.ReadToEnd(),
                                new SettingDictionaryConverter());
            return result;
        }
    }
}
