using ElmSharp;
using EColor = ElmSharp.Color;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The native widget that is configured with an header and an list of items to be used in TVNavigationDrawer.
    /// </summary>
	public class TVNavigationView : NavigationView
    {
        EColor _backgroundColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tizen.UIExtensions.ElmSharp.TVNavigationView"/> class.
        /// </summary>
        /// <param name="parent">Parent evas object.</param>
        public TVNavigationView(EvasObject parent) : base(parent)
        {
            BackgroundColor = this.GetTvDefaultBackgroundColor();
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public override EColor BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                base.BackgroundColor = _backgroundColor.IsDefault ? this.GetTvDefaultBackgroundColor(): _backgroundColor;
            }
        }
    }
}
