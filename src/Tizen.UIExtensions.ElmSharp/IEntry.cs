using System;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    public interface IEntry
    {
        double FontSize { get; set; }

        FontAttributes FontAttributes { get; set; }

        string FontFamily { get; set; }

        Color TextColor { get; set; }

        TextAlignment HorizontalTextAlignment { get; set; }

        string Placeholder { get; set; }

        Color PlaceholderColor { get; set; }

        Keyboard Keyboard { get; set; }

        event EventHandler<TextChangedEventArgs> TextChanged;

        event EventHandler TextBlockFocused;

        event EventHandler TextBlockUnfocused;

        event EventHandler EntryLayoutFocused;

        event EventHandler EntryLayoutUnfocused;
    }
}
