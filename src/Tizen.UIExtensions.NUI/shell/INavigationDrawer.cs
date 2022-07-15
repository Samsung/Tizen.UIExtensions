using System;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI
{
    public interface INavigationDrawer
    {
        View? Drawer { get; set; }

        View? Content { get; set; }

        View? BackDrop { get; set; }

        bool IsOpened { get; }

        double DrawerWidth { get; set; }

        bool IsGestureEnabled { get; set; }

        DrawerBehavior DrawerBehavior { get; set; }

        event EventHandler Toggled;
    }
}
