using System;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// Provides data for the CollectionView.ItemSelected event.
    /// </summary>
    public class SelectedItemChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the selected item.
        /// </summary>
        public object SelectedItem { get; }

        /// <summary>
        /// Gets the selected item index
        /// </summary>
        public int SelectedItemIndex { get; }

        /// <summary>
        /// Initializes a new instance of the SelectedItemChangedEventArgs class.
        /// </summary>
        public SelectedItemChangedEventArgs(object selectedItem, int selectedItemIndex)
        {
            SelectedItem = selectedItem;
            SelectedItemIndex = selectedItemIndex;
        }
    }
}