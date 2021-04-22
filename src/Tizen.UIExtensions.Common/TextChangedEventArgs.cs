using System;

namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// TextChangedEventArgs event arguments.
    /// </summary>
    public class TextChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new <see cref="TextChangedEventArgs"/> object that represents a change from <paramref name="oldTextValue"/> to <paramref name="newTextValue"/>.
        /// </summary>
        /// <param name="oldTextValue">Old text value of <see cref="Entry"/>.</param>
        /// <param name="newTextValue">New text value of <see cref="Entry"/>.</param>
        public TextChangedEventArgs(string oldTextValue, string newTextValue)
        {
            OldTextValue = oldTextValue;
            NewTextValue = newTextValue;
        }

        /// <summary>
        /// The new text
        /// </summary>
        public string NewTextValue { get; private set; }

        /// <summary>
        /// The old text
        /// </summary>
        public string OldTextValue { get; private set; }
    }
}
