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
using System.Windows.Input;

namespace AudioWorks.UI.Views
{
    public static class DropBehavior
    {
        public static readonly DependencyProperty PreviewDropCommandProperty =
            DependencyProperty.RegisterAttached("PreviewDropCommand", typeof(ICommand), typeof(DropBehavior),
                new PropertyMetadata(PreviewDropCommandPropertyChanged));

        public static void SetPreviewDropCommand(this UIElement uiElement, ICommand inCommand) =>
            uiElement.SetValue(PreviewDropCommandProperty, inCommand);

        static void PreviewDropCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = (UIElement) sender;
            if (null == uiElement) return;
 
            uiElement.Drop += (obj, args) =>
            {
                ((ICommand) uiElement.GetValue(PreviewDropCommandProperty)).Execute(args.Data);
                args.Handled = true;
            };
        }
    }
}
