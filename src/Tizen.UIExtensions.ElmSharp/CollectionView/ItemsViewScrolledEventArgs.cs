using System;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// Provides data for the CollectionView.Scrolled event.
    /// </summary>
    public class ItemsViewScrolledEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the first visible item index on the scrolled canvas.
        /// </summary>
        public int FirstVisibleItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the center item index on the scrolled canvas.
        /// </summary>
        public int CenterItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the last visible item index on the scrolled canvas.
        /// </summary>
        public int LastVisibleItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the scrolled horizontal offset.
        /// </summary>
        public double HorizontalOffset { get; set; }

        /// <summary>
        /// Gets or sets the scrolled horizontal delta.
        /// </summary>
        public double HorizontalDelta { get; set; }

        /// <summary>
        /// Gets or sets the scrolled vertical offset.
        /// </summary>
        public double VerticalOffset { get; set; }

        /// <summary>
        /// Gets or sets the scrolled vertical delta.
        /// </summary>
        public double VerticalDelta { get; set; }
    }
}