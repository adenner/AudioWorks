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
using CommonServiceLocator;
using MahApps.Metro.Controls.Dialogs;
using Prism.Ioc;
using Prism.Modularity;
using Metro = MahApps.Metro;

namespace AudioWorks.UI
{
    public sealed partial class App
    {
        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IFileSelectionService, FileSelectionService>();
            containerRegistry.RegisterSingleton<IMetadataSettingService, MetadataSettingService>();
            containerRegistry.RegisterSingleton<IAnalysisSettingService, AnalysisSettingService>();
            containerRegistry.RegisterSingleton<IEncoderSettingService, EncoderSettingService>();
            containerRegistry.RegisterDialog<EditControl, EditControlViewModel>();
            containerRegistry.RegisterDialog<AnalysisControl, AnalysisControlViewModel>();
            containerRegistry.RegisterDialogWindow<CustomDialogWindow>();

            // MahApps.Metro dialog service
            containerRegistry.RegisterInstance(DialogCoordinator.Instance);
        }

        protected override IModuleCatalog CreateModuleCatalog() => new ConfigurationModuleCatalog();

        protected override void OnStartup(StartupEventArgs e)
        {
            Metro.ThemeManager.IsAutomaticWindowsAppModeSettingSyncEnabled = true;
            Metro.ThemeManager.SyncThemeWithWindowsAppModeSetting();

            Fluent.ThemeManager.IsAutomaticWindowsAppModeSettingSyncEnabled = true;
            Fluent.ThemeManager.SyncThemeWithWindowsAppModeSetting();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ServiceLocator.Current.GetInstance<IMetadataSettingService>().Save();
            ServiceLocator.Current.GetInstance<IAnalysisSettingService>().Save();
            ServiceLocator.Current.GetInstance<IEncoderSettingService>().Save();

            base.OnExit(e);
        }
    }
}
