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

        /// <summary>
        /// Gets or sets the navigation view to be shown on the drawer.
        /// </summary>
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

        /// <summary>
        /// Gets of sets the main content of the NavigationDrawer.
        /// </summary>
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

        /// <summary>
        /// Gets the target view of the NaviagtionDrawer
        /// </summary>
        public EvasObject TargetView => this;
    }
}
