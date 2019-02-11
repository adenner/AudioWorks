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
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace AudioWorks.Common
{
    /// <summary>
    /// Represents a dictionary of settings which you can pass to various methods.
    /// </summary>
    /// <seealso cref="Dictionary{String, Object}"/>
    [PublicAPI]
    public class SettingDictionary : IDictionary<string, object>
    {
        readonly IDictionary<string, object> _dictionary = new Dictionary<string, object>();

        /// <summary>
        /// Gets the string value associated with the specified key, if present.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value, or empty string.</param>
        /// <returns>true, if the value is present in the dictionary. Otherwise, false.</returns>
        [CollectionAccess(CollectionAccessType.Read)]
        [ContractAnnotation("=> false, value:null; => true, value:notnull")]
        public bool TryGetValue([NotNull] string key, out string value)
        {
            if (TryGetValue(key, out object objectValue) && objectValue is string typedValue)
            {
                value = typedValue;
                return true;
            }

            value = string.Empty;
            return false;
        }

        /// <summary>
        /// Gets the value associated with the specified key and of the specified type, if present.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>true, if the value is present in the dictionary. Otherwise, false.</returns>
        [CollectionAccess(CollectionAccessType.Read)]
        [ContractAnnotation("=> false, value:null; => true, value:notnull")]
        public bool TryGetValue<TValue>([NotNull] string key, out TValue value) where TValue : struct
        {
            if (TryGetValue(key, out object objectValue) && objectValue is TValue typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
#if NETCOREAPP2_1
        [CollectionAccess(CollectionAccessType.Read), NotNull]
#else
        [CollectionAccess(CollectionAccessType.Read)]
#endif
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

#if NETCOREAPP2_1
        [CollectionAccess(CollectionAccessType.Read), NotNull]
#else
        [CollectionAccess(CollectionAccessType.Read)]
#endif
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _dictionary).GetEnumerator();
        }

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.UpdatedContent)]
        public virtual void Add(KeyValuePair<string, object> item)
        {
            _dictionary.Add(item);
        }

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.Read)]
        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dictionary.Contains(item);
        }

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.Read)]
#if NETCOREAPP2_1
        public void CopyTo([NotNull] KeyValuePair<string, object>[] array, int arrayIndex)
#else
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
#endif
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
        public bool Remove(KeyValuePair<string, object> item)
        {
            return _dictionary.Remove(item);
        }

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.Read)]
        public int Count => _dictionary.Count;

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.Read)]
        public bool IsReadOnly => _dictionary.IsReadOnly;

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.UpdatedContent)]
#if NETCOREAPP2_1
        public virtual void Add([NotNull] string key, [NotNull] object value)
#else
        public virtual void Add(string key, [NotNull] object value)
#endif
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _dictionary.Add(key, value);
        }

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.Read)]
#if NETCOREAPP2_1
        public bool ContainsKey([NotNull] string key)
#else
        public bool ContainsKey(string key)
#endif
        {
            return _dictionary.ContainsKey(key);
        }

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
#if NETCOREAPP2_1
        public bool Remove([NotNull] string key)
#else
        public bool Remove(string key)
#endif
        {
            return _dictionary.Remove(key);
        }

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.Read)]
        [ContractAnnotation("=> false, value:null; => true, value:notnull")]
#if NETCOREAPP2_1
        public bool TryGetValue([NotNull] string key, out object value)
#else
        public bool TryGetValue(string key, out object value)
#endif
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <inheritdoc/>
        [CollectionAccess(CollectionAccessType.UpdatedContent), NotNull]
#if NETCOREAPP2_1
        public virtual object this[[NotNull] string key]
#else
        public virtual object this[string key]
#endif
        {
            get => _dictionary[key];
            set => _dictionary[key] = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <inheritdoc/>
#if NETCOREAPP2_1
        [CollectionAccess(CollectionAccessType.Read), NotNull, ItemNotNull]
#else
        [CollectionAccess(CollectionAccessType.Read), ItemNotNull]
#endif
        public ICollection<string> Keys => _dictionary.Keys;

        /// <inheritdoc/>
#if NETCOREAPP2_1
        [CollectionAccess(CollectionAccessType.Read), NotNull, ItemNotNull]
#else
        [CollectionAccess(CollectionAccessType.Read), ItemNotNull]
#endif
        public ICollection<object> Values => _dictionary.Values;
    }
}