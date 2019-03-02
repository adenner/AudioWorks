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

using System.Windows;
using AudioWorks.UI.Services;
using AudioWorks.UI.ViewModels;
using AudioWorks.UI.Views;
using MahApps.Metro;
using Prism.Ioc;

namespace AudioWorks.UI
{
    public sealed partial class App
    {
        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IFileSelectionService, WpfFileSelectionService>();
            containerRegistry.RegisterSingleton<IAppShutdownService, WpfAppShutdownService>();
            containerRegistry.RegisterDialog<EditControl, EditControlViewModel>();
            containerRegistry.RegisterDialogWindow<CustomDialogWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.IsAutomaticWindowsAppModeSettingSyncEnabled = true;
            ThemeManager.SyncThemeWithWindowsAppModeSetting();

            base.OnStartup(e);
        }
    }
}
