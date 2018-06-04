﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;

namespace AudioWorks.Extensions
{
    sealed class ExtensionAssemblyResolver
    {
        [NotNull]
        internal Assembly Assembly { get; }

        internal ExtensionAssemblyResolver([NotNull] string path)
        {
            Assembly = Assembly.LoadFrom(path);

            if (RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework",
                StringComparison.Ordinal))
                ResolveFullFramework();
            else
                ResolveWithLoader();
        }

        void ResolveFullFramework()
        {
            // .NET Framework should look for dependencies in the root directory and the extension's directory

            // ReSharper disable AssignNullToNotNullAttribute
            var assemblyFiles = Directory.GetFiles(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.dll")
                .Concat(Directory.GetFiles(
                    Path.GetDirectoryName(Path.GetDirectoryName(Assembly.Location)), "*.dll"));
            // ReSharper restore AssignNullToNotNullAttribute

            AppDomain.CurrentDomain.AssemblyResolve += (context, args) => assemblyFiles
                .Where(assemblyFile => AssemblyName.ReferenceMatchesDefinition(
                    AssemblyName.GetAssemblyName(assemblyFile),
                    new AssemblyName(args.Name)))
                .Select(Assembly.LoadFrom).FirstOrDefault();
        }

        void ResolveWithLoader()
        {
            // .NET Core can additionally look for dependencies in each extension's deps.json file
            var dependencyContext = DependencyContext.Load(Assembly);
            var dependencyResolver = new CompositeCompilationAssemblyResolver(new ICompilationAssemblyResolver[]
            {
                new AppBaseCompilationAssemblyResolver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)),
                new AppBaseCompilationAssemblyResolver(Path.GetDirectoryName(Assembly.Location)),
                new PackageCompilationAssemblyResolver()
            });

            AssemblyLoadContext.Default.Resolving += (context, name) =>
            {
                var runtimeLibrary = dependencyContext.RuntimeLibraries.FirstOrDefault(library =>
                    library.RuntimeAssemblyGroups.SelectMany(group => group.AssetPaths)
                        .Select(Path.GetFileNameWithoutExtension)
                        .Contains(name.Name, StringComparer.OrdinalIgnoreCase));

                if (runtimeLibrary != null)
                {
                    var assemblies = new List<string>(1);
                    dependencyResolver.TryResolveAssemblyPaths(new CompilationLibrary(
                        runtimeLibrary.Type,
                        runtimeLibrary.Name,
                        runtimeLibrary.Version,
                        runtimeLibrary.Hash,
                        runtimeLibrary.RuntimeAssemblyGroups.SelectMany(group => group.AssetPaths),
                        runtimeLibrary.Dependencies,
                        runtimeLibrary.Serviceable), assemblies);
                    if (assemblies.Count > 0)
                        return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblies[0]);
                }

                return null;
            };
        }
    }
}
