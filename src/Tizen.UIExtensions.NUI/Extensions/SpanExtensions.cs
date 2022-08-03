using System;
using System.Collections.Generic;
using System.Text;
using Tizen.NUI;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI
{
    public static class SpanExtensions
    {
        public static string ToMarkupText(this Span span)
        {
            StringBuilder sb = new StringBuilder();
            Stack<string> opened = new Stack<string>();

            sb.Append("<p");
            if (span.LineHeight > 0)
            {
                sb.Append($" rel-line-height='{span.LineHeight}f'");
            }

            if (span.HorizontalTextAlignment == TextAlignment.Start)
                sb.Append($" align='begin'");
            else if (span.HorizontalTextAlignment == TextAlignment.Center)
                sb.Append(" align='center'");
            else if (span.HorizontalTextAlignment == TextAlignment.End)
                sb.Append(" align='end'");

            sb.Append(">");
            opened.Push("</p>");

            // Font
            sb.Append("<font");
            if (span.FontSize != -1)
            {
                sb.Append($" size={span.FontSize}");
            }

            if (!string.IsNullOrEmpty(span.FontFamily))
            {
                sb.Append($" family={span.FontFamily}");
            }

            if (span.FontAttributes.HasFlag(FontAttributes.Bold))
            {
                sb.Append(" weight=bold");
            }

            if (span.FontAttributes.HasFlag(FontAttributes.Italic))
            {
                sb.Append(" slant=italic");
            }
            sb.Append(">");
            opened.Push("</font>");

            // color
            if (!span.ForegroundColor.IsDefault)
            {
                sb.Append($"<color value={span.ForegroundColor.ToHex()}>");
                opened.Push("</color>");
            }

            if (!span.BackgroundColor.IsDefault)
            {
                sb.Append($"<background color='{span.BackgroundColor.ToHex()}'>");
                opened.Push("</background>");
            }

            if (span.TextDecorations.HasFlag(TextDecorations.Underline))
            {
                sb.Append("<u>");
                opened.Push("</u>");
            }

            if (span.TextDecorations.HasFlag(TextDecorations.Strikethrough))
            {
                sb.Append("<s>");
                opened.Push("</s>");
            }

            // Text
            sb.Append(span.Text);

            // close all opened tags
            foreach (var closed in opened)
            {
                sb.Append(closed);
            }

            return sb.ToString();
        }
    }
}
