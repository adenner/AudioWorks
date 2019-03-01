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
using System.Windows.Interop;
using Prism.Services.Dialogs;

namespace AudioWorks.UI.Views
{
    public partial class CustomDialogWindow : IDialogWindow
    {
        const int _gwlStyle = -16;
        const uint _wsSysMenu = 0x80000;

        public IDialogResult? Result { get; set; }

        public CustomDialogWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            // Remove the icon and menu bar buttons
            var windowHandle = new WindowInteropHelper(this).Handle;
            var styleHandle = SafeNativeMethods.GetWindowLongPtr(windowHandle, _gwlStyle);
            SafeNativeMethods.SetWindowLongPtr(windowHandle, _gwlStyle,
                new IntPtr(styleHandle.ToInt32() & ~_wsSysMenu));

            base.OnSourceInitialized(e);
        }
    }
}
