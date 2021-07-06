using System;

namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// Holds information about selected item.
    /// </summary>
    public class ItemSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tizen.UIExtensions.Common.ItemSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="selectedItem">the selected item object</param>
        /// <param name="selectedItemIndex">An index value of the selected item</param>
        public ItemSelectedEventArgs(object selectedItem, int selectedItemIndex)
        {
            SelectedItem = selectedItem;
            SelectedItemIndex = selectedItemIndex;
        }

        /// <summary>
        /// The selected item obejct.
        /// </summary>
        public object SelectedItem { get; private set; }

        /// <summary>
        /// The index value of the selected item.
        /// </summary>
        public int SelectedItemIndex { get; private set; }

    }
}
