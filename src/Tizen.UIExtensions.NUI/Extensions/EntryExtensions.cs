using Tizen.NUI;
using NEntry = Tizen.NUI.BaseComponents.TextField;
using NEditor = Tizen.NUI.BaseComponents.TextEditor;
using FontAttributes = Tizen.UIExtensions.Common.FontAttributes;

namespace Tizen.UIExtensions.NUI
{
    public static class EntryExtensions
    {
        public static void SetFontAttributes(this NEntry entry, FontAttributes attr)
        {
            bool isBold = attr.HasFlag(FontAttributes.Bold);
            bool isItalic = attr.HasFlag(FontAttributes.Italic);
            var style = new PropertyMap();
            style.Add("weight", isBold ? "bold" : "normal")
                 .Add("slant", isItalic ? "italic" : "normal");
            entry.FontStyle = style;
        }

        public static void SetFontAttributes(this NEditor editor, FontAttributes attr)
        {
            bool isBold = attr.HasFlag(FontAttributes.Bold);
            bool isItalic = attr.HasFlag(FontAttributes.Italic);
            var style = new PropertyMap();
            style.Add("weight", isBold ? "bold" : "normal")
                 .Add("slant", isItalic ? "italic" : "normal");
            editor.FontStyle = style;
        }
    }
}
