using System;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// Provides data for the IndicatorView.SelectedPosition event.
    /// </summary>
    public class SelectedPositionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the selected position index.
        /// </summary>
        public int SelectedPosition { get; }

        /// <summary>
        /// Initializes a new instance of the SelectedPositionChangedEventArgs class.
        /// </summary>
        public SelectedPositionChangedEventArgs(int selectedPosition)
        {
            SelectedPosition = selectedPosition;
        }
    }
}