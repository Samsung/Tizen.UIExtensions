using System;
using System.Text;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    public static class SpanExtensions
    {
        public static string GetMarkupText(this Span span)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span ");
            sb = span.PrepareFormattingString(sb);
            sb.Append(">");
            sb.Append(span.GetDecoratedText());
            sb.Append("</span>");
            return sb.ToString();
        }

        public static string GetStyle(this Span span)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("DEFAULT='");

            span.PrepareFormattingString(sb);

            sb.Append("'");

            return sb.ToString();
        }

        public static string GetDecoratedText(this Span span)
        {
            return ConvertTags(span.Text);
        }

        public static StringBuilder PrepareFormattingString(this Span span, StringBuilder strBuffer)
        {
            if (!span.ForegroundColor.IsDefault)
            {
                strBuffer.AppendFormat("color={0} ", span.ForegroundColor.ToNative().ToHex());
            }

            if (!span.BackgroundColor.IsDefault)
            {
                strBuffer.AppendFormat("backing_color={0} backing=on ", span.BackgroundColor.ToNative().ToHex());
            }

            if (!string.IsNullOrEmpty(span.FontFamily))
            {
                strBuffer.AppendFormat("font={0} ", span.FontFamily);
            }

            if (span.FontSize != -1)
            {
                strBuffer.AppendFormat("font_size={0} ", span.FontSize.ToEflFontPoint());
            }

            if ((span.FontAttributes & FontAttributes.Bold) != 0)
            {
                strBuffer.Append("font_weight=Bold ");
            }

            if ((span.FontAttributes & FontAttributes.Italic) != 0)
            {
                strBuffer.Append("font_style=italic ");
            }

            if (span.TextDecorations.HasFlag(TextDecorations.Underline))
            {
                strBuffer.AppendFormat("underline=on underline_color={0} ",
                    span.ForegroundColor.IsDefault ? ThemeConstants.Span.ColorClass.DefaultUnderLineColor.ToHex() : span.ForegroundColor.ToNative().ToHex());
            }

            if (span.TextDecorations.HasFlag(TextDecorations.Strikethrough))
            {
                strBuffer.AppendFormat("strikethrough=on strikethrough_color={0} ",
                    span.ForegroundColor.IsDefault ? ThemeConstants.Span.ColorClass.DefaultUnderLineColor.ToHex() : span.ForegroundColor.ToNative().ToHex());
            }

            switch (span.HorizontalTextAlignment)
            {
                case TextAlignment.Auto:
                    strBuffer.Append("align=auto ");
                    break;

                case TextAlignment.Start:
                    strBuffer.Append("align=left ");
                    break;

                case TextAlignment.End:
                    strBuffer.Append("align=right ");
                    break;

                case TextAlignment.Center:
                    strBuffer.Append("align=center ");
                    break;

                case TextAlignment.None:
                    break;
            }

            if (span.LineHeight != -1.0d)
            {
                strBuffer.Append($"linerelsize={(int)(span.LineHeight * 100)}%");
            }

            switch (span.LineBreakMode)
            {
                case LineBreakMode.HeadTruncation:
                    strBuffer.Append("ellipsis=0.0");
                    break;

                case LineBreakMode.MiddleTruncation:
                    strBuffer.Append("ellipsis=0.5");
                    break;

                case LineBreakMode.TailTruncation:
                    strBuffer.Append("ellipsis=1.0");
                    break;
                case LineBreakMode.None:
                    break;
            }
            return strBuffer;
        }


        static string ConvertTags(string text)
        {
            return text.Replace("&", "&amp;")
                       .Replace("<", "&lt;")
                       .Replace(">", "&gt;")
                       .Replace(Environment.NewLine, "<br>");
        }
    }
}
