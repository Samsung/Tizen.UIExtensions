using System;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// Provides data for the CarouselPage.PageScrolled event.
    /// </summary>
    public class PageScrolledEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the previous page index.
        /// </summary>
        public int PreviousPageIndex { get; }

        /// <summary>
        /// Gets the current page index.
        /// </summary>
        public int CurrentPageIndex { get; }

        /// <summary>
        /// Initializes a new instance of the PageScrolledEventArgs class.
        /// </summary>
        /// <param name="previousPageIndex">Previous page index.</param>
        /// <param name="currentPageIndex">Current page index.</param>
        public PageScrolledEventArgs(int previousPageIndex, int currentPageIndex)
        {
            PreviousPageIndex = previousPageIndex;
            CurrentPageIndex = currentPageIndex;
        }
    }
}