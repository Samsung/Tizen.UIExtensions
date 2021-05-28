using System;
using ElmSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    public interface INavigationDrawer
    {
        EvasObject TargetView { get; }

        EvasObject? NavigationView { get; set; }

        EvasObject? Main { get; set; }

        bool IsOpen { get; set; }

        bool IsSplit { get; set; }

        event EventHandler Toggled;
    }
}