using System;
using System.Collections.Generic;
using ElmSharp;
using Tizen.UIExtensions.Common;
using EColor = ElmSharp.Color;

namespace Tizen.UIExtensions.ElmSharp
{
    public interface INavigationView
    {
        EvasObject TargetView { get; }

        EvasObject? Header { get; set; }

        DrawerHeaderBehavior HeaderBehavior { get; set; }

        EColor BackgroundColor { get; set; }

        EvasObject? BackgroundImage { get; set; }

        void BuildMenu(IEnumerable<object> items);

        void UpdateHeaderLayout();

        event EventHandler<ItemSelectedEventArgs> ItemSelected;
    }
}