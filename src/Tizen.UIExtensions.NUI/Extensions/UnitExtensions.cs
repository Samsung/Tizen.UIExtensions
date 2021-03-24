using System;
using System.Collections.Generic;
using System.Text;
using Tizen.UIExtensions.Common;
using Tizen.NUI;
using NColor = Tizen.NUI.Color;
using CColor = Tizen.UIExtensions.Common.Color;

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

        public static CColor ToCommon(this NColor c)
        {
            return new Common.Color(c.R, c.G, c.B, c.A);
        }

        public static NColor ToNative(this CColor c)
        {
            return new NColor((float)c.R, (float)c.B, (float)c.B, (float)c.A);
        }

    }
}
