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

using System;
using System.Xml.Linq;
using AudioWorks.Common;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AudioWorks.Api
{
    sealed class ExtensionProjectContext : INuGetProjectContext
    {
        readonly ILogger _logger = LoggerManager.LoggerFactory.CreateLogger<ExtensionProjectContext>();

        public void Log(MessageLevel level, string? message, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (level)
            {
                case MessageLevel.Debug:
                    _logger.LogTrace(message, args);
                    break;
                case MessageLevel.Info:
                    _logger.LogDebug(message, args);
                    break;
                case MessageLevel.Warning:
                    _logger.LogWarning(message, args);
                    break;
                case MessageLevel.Error:
                    _logger.LogError(message, args);
                    break;
            }
        }

        public void ReportError(string message)
        {
            _logger.LogError(message);
        }

        public FileConflictAction ResolveFileConflict(string? message) => FileConflictAction.Overwrite;

        public PackageExtractionContext? PackageExtractionContext { get; set; } = new PackageExtractionContext(
            PackageSaveMode.Defaultv3,
            XmlDocFileSaveMode.Skip,
            null,
            NullLogger.Instance);

        public ISourceControlManagerProvider? SourceControlManagerProvider => null;

        public ExecutionContext? ExecutionContext => null;

        public XDocument? OriginalPackagesConfig { get; set; }

        public NuGetActionType ActionType { get; set; }

        public Guid OperationId { get; set; }
    }
}