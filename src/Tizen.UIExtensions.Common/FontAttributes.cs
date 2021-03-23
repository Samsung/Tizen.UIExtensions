using System;

namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// Enumerates values that describe attributes of text.
    /// </summary>
    [Flags]
    public enum FontAttributes
    {
        None = 0,
        Bold = 1 << 0,
        Italic = 1 << 1
    }
}
