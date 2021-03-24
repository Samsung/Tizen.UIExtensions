using System;

namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// Flagging enumeration defining text decorations.
    /// This enumeration has a FlagsAttribute attribute that allows a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum TextDecorations
    {
        /// <summary>
        /// No text decoration.
        /// </summary>
        None = 0,
        /// <summary>
        /// A text underline.
        /// </summary>
        Underline = 1 << 0,
        /// <summary>
        /// A single-line strikethrough.
        /// </summary>
        Strikethrough = 1 << 1,
    }
}
