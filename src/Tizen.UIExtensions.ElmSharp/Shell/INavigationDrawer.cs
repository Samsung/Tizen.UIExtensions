using System;
using ElmSharp;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    public interface INavigationDrawer
    {
        EvasObject TargetView { get; }

        EvasObject? NavigationView { get; set; }

        EvasObject? Main { get; set; }

        bool IsOpen { get; set; }

        bool IsSplit { get; set; }

        double DrawerWidth { get; set; }

        bool IsGestureEnabled { get; set; }

        DrawerBehavior DrawerBehavior { get; set; }

        event EventHandler Toggled;
    }
}