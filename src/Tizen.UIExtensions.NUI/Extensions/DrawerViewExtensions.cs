using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI
{
    public static class DrawerViewExtensions
    {
        public static bool IsDrawerOpened(this DrawerView drawerView)
        {
            return drawerView.DrawerBehavior != DrawerBehavior.Drawer || drawerView.IsOpened;
        }
    }
}
