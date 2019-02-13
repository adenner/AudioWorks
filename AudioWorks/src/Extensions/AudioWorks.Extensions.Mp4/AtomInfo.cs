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

namespace AudioWorks.Extensions.Mp4
{
    sealed class AtomInfo
    {
        internal uint Start { get; }

        internal uint Size { get; }

        internal uint End => Start + Size;

        internal string FourCc { get; }

        internal AtomInfo(uint start, uint size, string fourCc)
        {
            Start = start;
            Size = size;
            FourCc = fourCc;
        }
    }
}