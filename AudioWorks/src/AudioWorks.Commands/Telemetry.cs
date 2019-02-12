﻿/* Copyright © 2018 Jeremy Herbison

This file is part of AudioWorks.

AudioWorks is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public
License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
version.

AudioWorks is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
details.

You should have received a copy of the GNU Affero General Public License along with AudioWorks. If not, see
<https://www.gnu.org/licenses/>. */

using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Runtime.InteropServices;
using AudioWorks.Common;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;

namespace AudioWorks.Commands
{
    static class Telemetry
    {
        const string _instrumentationKey = "f36d131d-ef59-4b6a-8788-403d0ef14927";
        static TelemetryClient? _client;
        static readonly object _syncRoot = new object();

        static Telemetry()
        {
            TelemetryConfiguration.Active.InstrumentationKey = _instrumentationKey;
        }

        internal static void TrackFirstLaunch()
        {
            var store = IsolatedStorageFile.GetUserStoreForAssembly();
            lock (_syncRoot)
            {
                if (store.FileExists("FIRST_LAUNCH_TELEMETRY_GATHERED")) return;
                store.CreateFile("FIRST_LAUNCH_TELEMETRY_GATHERED");
            }

            TrackEvent("FirstLaunch");
        }

        static void TrackEvent(string eventName, IDictionary<string, string>? properties = null)
        {
            if (!ConfigurationManager.Configuration.GetValue("EnableTelemetry", true)) return;

            if (_client == null)
                _client = InitializeClient();

            _client!.TrackEvent(eventName, properties);
        }

        static TelemetryClient InitializeClient()
        {
            var result = new TelemetryClient();
            result.Context.Device.OperatingSystem = RuntimeInformation.OSDescription;
            result.Context.Component.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            result.Context.Properties["Framework"] = RuntimeInformation.FrameworkDescription;
            result.Context.Properties["Architecture"] = RuntimeInformation.ProcessArchitecture.ToString();
            return result;
        }
    }
}