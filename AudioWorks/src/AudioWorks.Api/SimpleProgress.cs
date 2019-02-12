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

namespace AudioWorks.Api
{
    /// <summary>
    /// An <see cref="IProgress{T}"/> implementation that simply invokes the callbacks on the same thread, rather than
    /// attempting to capture the synchronization context like <see cref="Progress{T}"/>.
    /// </summary>
    /// <typeparam name="T">Specifies the type of the progress report value.</typeparam>
    /// <seealso cref="IProgress{T}"/>
    public sealed class SimpleProgress<T> : IProgress<T>
    {
        readonly Action<T> _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleProgress{T}"/> class.
        /// </summary>
        /// <param name="handler">The handler to invoke for each reported progress value.</param>
        public SimpleProgress(Action<T> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <inheritdoc/>
        public void Report(T value)
        {
            _handler(value);
        }
    }
}
