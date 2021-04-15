using Tizen.NUI;
using NLabel = Tizen.NUI.BaseComponents.TextLabel;
using FontAttributes = Tizen.UIExtensions.Common.FontAttributes;

namespace Tizen.UIExtensions.NUI
{
    public static class LabelExtensions
    {
        public static void SetFontAttributes(this NLabel label, FontAttributes attr)
        {
            bool isBold = attr.HasFlag(FontAttributes.Bold);
            bool isItalic = attr.HasFlag(FontAttributes.Italic);
            var style = new PropertyMap();
            style.Add("weight", isBold ? "bold" : "normal")
                 .Add("slant", isItalic ? "italic" : "normal");
            label.FontStyle = style;
        }
    }
}
