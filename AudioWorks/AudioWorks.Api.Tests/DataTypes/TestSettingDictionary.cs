﻿using AudioWorks.Common;
using JetBrains.Annotations;
using System;
using System.Linq;
using Xunit.Abstractions;

namespace AudioWorks.Api.Tests.DataTypes
{
    sealed class TestSettingDictionary : SettingDictionary, IXunitSerializable
    {
        public void Deserialize([NotNull] IXunitSerializationInfo info)
        {
            foreach (var item in info.GetValue<string[]>("Items"))
            {
                var splitItem = item.Split('|');
                Add(splitItem[0], Convert.ChangeType(splitItem[1], Type.GetType(splitItem[2])));
            }
        }

        public void Serialize([NotNull] IXunitSerializationInfo info)
        {
            info.AddValue("Items", this.Select(item =>
                $"{item.Key}|{item.Value}|{item.Value.GetType().AssemblyQualifiedName}"
            ).ToArray());
        }
    }
}