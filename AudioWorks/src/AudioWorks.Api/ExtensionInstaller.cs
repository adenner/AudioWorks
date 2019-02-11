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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AudioWorks.Common;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AudioWorks.Api
{
    static class ExtensionInstaller
    {
        static readonly string _projectRoot = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AudioWorks",
            "Extensions",
#if NETSTANDARD2_0
            "netstandard2.0"
#else
            "netcoreapp2.1"
#endif
            );

        static readonly SourceRepository _customRepository =
            new SourceRepository(new PackageSource(
                    ConfigurationManager.Configuration.GetValue("UsePreReleaseExtensions", false)
                        ? ConfigurationManager.Configuration.GetValue(
                            "PreReleaseExtensionRepository",
                            "https://www.myget.org/F/audioworks-extensions-prerelease/api/v3/index.json")
                        : ConfigurationManager.Configuration.GetValue(
                            "ExtensionRepository",
                            "https://www.myget.org/F/audioworks-extensions-v3/api/v3/index.json")),
                Repository.Provider.GetCoreV3());

        static readonly SourceRepository _defaultRepository =
            new SourceRepository(new PackageSource(
                    ConfigurationManager.Configuration.GetValue(
                        "DefaultRepository",
                        "https://api.nuget.org/v3/index.json")),
                Repository.Provider.GetCoreV3());

        static readonly List<string> _compatibleTargets = new List<string>(new[]
        {
#if NETCOREAPP
            "netcoreapp2.1",
            "netcoreapp2.0",
            "netcoreapp1.1",
            "netcoreapp1.0",
#endif
            "netstandard2.0",
            "netstandard1.6",
            "netstandard1.5",
            "netstandard1.4",
            "netstandard1.3",
            "netstandard1.2",
            "netstandard1.1",
            "netstandard1.0"
        });

        static readonly List<string> _fileTypesToInstall = new List<string>(new[]
        {
            ".dll",
            ".dylib",
            ".pdb",
            ".so"
        });

        internal static void Download()
        {
            var logger = LoggerManager.LoggerFactory.CreateLogger(typeof(ExtensionInstaller).FullName);

            if (!ConfigurationManager.Configuration.GetValue("AutomaticExtensionDownloads", true))
                logger.LogInformation("Automatic extension downloads are disabled.");
            else
            {
                logger.LogInformation("Beginning automatic extension updates.");

                Directory.CreateDirectory(_projectRoot);

                var settings = Settings.LoadDefaultSettings(_projectRoot);
                var packageManager = new NuGetPackageManager(
                    new SourceRepositoryProvider(settings, Repository.Provider.GetCoreV3()), settings, _projectRoot);

                try
                {
                    var publishedPackages = GetPublishedPackages(logger);

                    var packagesInstalled = false;

                    foreach (var packageMetadata in publishedPackages)
                        if (InstallPackage(packageManager, packageMetadata, logger))
                            packagesInstalled = true;

                    // Remove any extensions that aren't published
                    foreach (var obsoleteExtension in new DirectoryInfo(_projectRoot).GetDirectories()
                        .Select(dir => dir.Name)
                        .Except(publishedPackages.Select(package => package.Identity.ToString()),
                            StringComparer.OrdinalIgnoreCase))
                    {
                        Directory.Delete(Path.Combine(_projectRoot, obsoleteExtension), true);

                        logger.LogDebug("Deleted unlisted or obsolete extension in '{0}'.",
                            obsoleteExtension);
                    }

                    logger.LogInformation(!packagesInstalled
                        ? "Extensions are already up to date."
                        : "Extensions successfully updated.");
                }
                catch (Exception e)
                {
                    // Timeout on search throws a TaskCanceled inside an Aggregate
                    // Timeout on install throws an OperationCanceled inside an InvalidOperation inside an Aggregate
                    if (e is AggregateException aggregate)
                        foreach (var inner in aggregate.InnerExceptions)
                            if (inner is OperationCanceledException ||
                                inner is InvalidOperationException invalidInner &&
                                invalidInner.InnerException is OperationCanceledException)
                                logger.LogWarning("The configured timeout was exceeded.");
                            else
                                logger.LogError(inner, e.Message);
                    else
                        logger.LogError(e, e.Message);
                }
            }
        }

        static IPackageSearchMetadata[] GetPublishedPackages(ILogger logger)
        {
            // Search on the thread pool to avoid deadlocks
            // ReSharper disable once ImplicitlyCapturedClosure
            var result = Task.Run(async () =>
                {
                    var cancellationTokenSource = GetCancellationTokenSource();
                    return await (await _customRepository
                            .GetResourceAsync<PackageSearchResource>(cancellationTokenSource.Token)
                            .ConfigureAwait(false))
                        .SearchAsync("AudioWorks.Extensions", new SearchFilter(false), 0, 100,
                            NullLogger.Instance,
                            cancellationTokenSource.Token)
                        .ConfigureAwait(false);
                }).Result
#if NETSTANDARD2_0
                    .Where(package => package.Tags.Contains(GetOSTag()))
#else
                .Where(package => package.Tags.Contains(GetOSTag(), StringComparison.OrdinalIgnoreCase))
#endif
                .ToArray();

            logger.LogDebug("Discovered {0} extension packages published at '{1}'.",
                result.Length, _customRepository.PackageSource.SourceUri);

            return result;
        }

        static bool InstallPackage(
            NuGetPackageManager packageManager,
            IPackageSearchMetadata packageMetadata,
            ILogger logger)
        {
            var extensionDir =
                new DirectoryInfo(Path.Combine(_projectRoot, packageMetadata.Identity.ToString()));
            if (extensionDir.Exists)
            {
                logger.LogDebug("'{0}' version {1} is already installed. Skipping.",
                    packageMetadata.Identity.Id, packageMetadata.Identity.Version.ToString());

                return false;
            }

            logger.LogInformation("Installing '{0}' version {1}.",
                packageMetadata.Identity.Id, packageMetadata.Identity.Version.ToString());

            extensionDir.Create();
            var stagingDir = extensionDir.CreateSubdirectory("Staging");

            var project = new ExtensionNuGetProject(stagingDir.FullName);

            try
            {
                // Download on the thread pool to avoid deadlocks
                Task.Run(async () =>
                {
                    using (var cancellationTokenSource = GetCancellationTokenSource())
                        await packageManager.InstallPackageAsync(
                                project,
                                packageMetadata.Identity,
                                new ResolutionContext(DependencyBehavior.Lowest, true, false,
                                    VersionConstraints.None),
                                new ExtensionProjectContext(),
                                _customRepository,
                                new[] { _defaultRepository },
                                cancellationTokenSource.Token)
                            .ConfigureAwait(false);
                }).Wait();

                // Move newly installed packages into the extension folder
                foreach (var installedPackage in project
                    .GetInstalledPackagesAsync(CancellationToken.None)
                    .Result)
                {
                    var packageDir = new DirectoryInfo(
                        project.GetInstalledPath(installedPackage.PackageIdentity));

                    foreach (var subDir in packageDir.GetDirectories())
                        // ReSharper disable once SwitchStatementMissingSomeCases
                        switch (subDir.Name)
                        {
                            case "lib":
                                MoveContents(
                                    SelectDirectory(subDir.GetDirectories()),
                                    extensionDir,
                                    logger);
                                break;

                            case "contentFiles":
                                MoveContents(
                                    SelectDirectory(subDir.GetDirectories("any").FirstOrDefault()
                                        ?.GetDirectories()),
                                    extensionDir,
                                    logger);
                                break;
                        }
                }
            }
            finally
            {
                stagingDir.Delete(true);

                // If the download was cancelled, clean up an empty extension directory
                if (!extensionDir.EnumerateFileSystemInfos().Any())
                    extensionDir.Delete();
            }

            return true;
        }

        [Pure]
        static string GetOSTag() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "Windows"
            : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? "Linux"
                : "MacOS";

        [Pure]
        static CancellationTokenSource GetCancellationTokenSource() => new CancellationTokenSource(
            ConfigurationManager.Configuration.GetValue("AutomaticExtensionDownloadTimeout", 30) *
            1000);

        static DirectoryInfo? SelectDirectory(IEnumerable<DirectoryInfo>? directories)
        {
            // Select the first directory in the list of compatible TFMs
            return directories?
                .Where(dir => _compatibleTargets.Contains(dir.Name, StringComparer.OrdinalIgnoreCase))
                .OrderBy(dir => _compatibleTargets
                    .FindIndex(target => target.Equals(dir.Name, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault();
        }

        static void MoveContents(DirectoryInfo? source, DirectoryInfo destination, ILogger logger)
        {
            if (source == null || !source.Exists) return;

            foreach (var file in source.GetFiles()
                .Where(file => _fileTypesToInstall.Contains(file.Extension, StringComparer.OrdinalIgnoreCase)))
            {
                // Skip any 3rd party symbols
                if (file.Extension.Equals(".pdb", StringComparison.OrdinalIgnoreCase) &&
                    !file.Name.StartsWith("AudioWorks.Extensions", StringComparison.OrdinalIgnoreCase))
                    continue;

                logger.LogDebug("Moving '{0}' to '{1}'.",
                    file.FullName, destination.FullName);

                file.MoveTo(Path.Combine(destination.FullName, file.Name));
            }

            foreach (var subDir in source.GetDirectories())
                MoveContents(subDir, destination.CreateSubdirectory(subDir.Name), logger);
        }
    }
}