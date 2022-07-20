using System;
using System.Threading.Tasks;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI
{
    public interface INavigationDrawer
    {
        View? Drawer { get; set; }

        View? Content { get; set; }

        View? Backdrop { get; set; }

        bool IsOpened { get; }

        double DrawerWidth { get; set; }

        bool IsGestureEnabled { get; set; }

        DrawerBehavior DrawerBehavior { get; set; }

        event EventHandler Toggled;

        Task<bool> OpenAsync(bool animate);

        Task<bool> CloseAsync(bool animate);

    }
}
