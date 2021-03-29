using Tizen.UIExtensions.Common;
using HorizontalAlignment = Tizen.NUI.HorizontalAlignment;
using VerticalAlignment = Tizen.NUI.VerticalAlignment;
using NColor = Tizen.NUI.Color;
using NSize = Tizen.NUI.Size;
using NPoint = Tizen.NUI.Position;
using NSize2D = Tizen.NUI.Size2D;
using System;

namespace Tizen.UIExtensions.NUI
{
    public static class UnitExtensions
    {
        public static TextAlignment ToCommon(this HorizontalAlignment horizontal) =>
            horizontal switch
            {
                HorizontalAlignment.Begin => TextAlignment.Start,
                HorizontalAlignment.Center => TextAlignment.Center,
                HorizontalAlignment.End => TextAlignment.End,
                _ => TextAlignment.None,
            };

        public static TextAlignment ToCommon(this VerticalAlignment vertical) =>
            vertical switch
            {
                VerticalAlignment.Top => TextAlignment.Start,
                VerticalAlignment.Center => TextAlignment.Center,
                VerticalAlignment.Bottom => TextAlignment.End,
                _ => TextAlignment.None,
            };

        public static HorizontalAlignment ToHorizontal(this TextAlignment alignment) =>
            alignment switch
            {
                TextAlignment.Start => HorizontalAlignment.Begin,
                TextAlignment.Center => HorizontalAlignment.Center,
                TextAlignment.End => HorizontalAlignment.End,
                _ => HorizontalAlignment.Begin
            };

        public static VerticalAlignment ToVertical(this TextAlignment alignment) =>
            alignment switch
            {
                TextAlignment.Start => VerticalAlignment.Top,
                TextAlignment.Center => VerticalAlignment.Center,
                TextAlignment.End => VerticalAlignment.Bottom,
                _ => VerticalAlignment.Top
            };

        public static Color ToCommon(this NColor c)
        {
            return new Color(c.R, c.G, c.B, c.A);
        }

        public static NColor ToNative(this Color c)
        {
            return new NColor((float)c.R, (float)c.G, (float)c.B, (float)c.A);
        }

        public static NSize ToNative(this Size size)
        {
            return new NSize((float)size.Width, (float)size.Height);
        }

        public static NPoint ToNative(this Point point)
        {
            return new NPoint((float)point.X, (float)point.Y);
        }

        public static Size ToCommon(this NSize2D size)
        {
            return new Size(size.Width, size.Height);
        }

        public static Size ToCommon(this NSize size)
        {
            return new Size(size.Width, size.Height);
        }

        public static Point ToCommon(this NPoint point)
        {
            return new Point(point.X, point.Y);
        }


    }
}
