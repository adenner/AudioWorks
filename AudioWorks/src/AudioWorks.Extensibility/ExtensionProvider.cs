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
using System.Composition;
using System.Linq;
using JetBrains.Annotations;

namespace AudioWorks.Extensibility
{
    /// <summary>
    /// Provides methods for accessing extensions of various types.
    /// </summary>
    [PublicAPI]
    public static class ExtensionProvider
    {
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of extensions wrapped in <see cref="ExportFactory{T}"/> objects to
        /// control their lifetime.
        /// </summary>
        /// <typeparam name="T">The type of extension.</typeparam>
        /// <returns>The extension factories.</returns>
        [NotNull]
        public static IEnumerable<ExportFactory<T, IDictionary<string, object>>> GetFactories<T>()
            where T : class
        {
            return ExtensionContainer<T>.Instance.Factories;
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of extensions wrapped in <see cref="ExportFactory{T}"/> objects to
        /// control their lifetime, filtered by metadata.
        /// </summary>
        /// <typeparam name="T">The type of extension.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The extension factories.</returns>
        [NotNull]
        public static IEnumerable<ExportFactory<T>> GetFactories<T>([NotNull] string key, [NotNull] string value)
            where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "The key cannot be null or empty.");
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value), "The value cannot be null or empty.");

            return ExtensionContainer<T>.Instance.Factories.Where(factory =>
                value.Equals((string) factory.Metadata[key], StringComparison.OrdinalIgnoreCase));
        }
    }
}
