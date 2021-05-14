using ElmSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// Provides data for the extended CollectionView.Scrolled event.
    /// </summary>
    public class ItemsViewScrolledExtendedEventArgs : ItemsViewScrolledEventArgs
    {
        /// <summary>
        /// Gets or sets the scrolled canvas size.
        /// </summary>
        public Size CanvasSize { get; set; }
    }
}