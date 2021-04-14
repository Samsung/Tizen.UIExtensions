using ElmSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The native widget to be used in Xamarin.Forms Shell.
    /// </summary>
    public class NavigationDrawer : DrawerBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tizen.UIExtensions.ElmSharp.NavigationDrawer"/> class.
        /// </summary>
        /// <param name="parent">Parent evas object.</param>
        public NavigationDrawer(EvasObject parent) : base(parent)
        {
        }
    }
}
