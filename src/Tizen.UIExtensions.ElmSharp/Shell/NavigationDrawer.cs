using ElmSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The native widget to be used in Xamarin.Forms Shell.
    /// </summary>
    public class NavigationDrawer : DrawerBox, INavigationDrawer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tizen.UIExtensions.ElmSharp.NavigationDrawer"/> class.
        /// </summary>
        /// <param name="parent">Parent evas object.</param>
        public NavigationDrawer(EvasObject parent) : base(parent)
        {
        }

        public EvasObject? NavigationView
        {
            get
            {
                return Drawer;
            }
            set
            {
                Drawer = value;
            }
        }

        public EvasObject? Main
        {
            get
            {
                return Content;
            }
            set
            {
                Content = value;
            }
        }

        public EvasObject TargetView => this;
    }
}
