﻿using Tizen.UIExtensions.Common;
using HorizontalAlignment = Tizen.NUI.HorizontalAlignment;
using VerticalAlignment = Tizen.NUI.VerticalAlignment;
using NColor = Tizen.NUI.Color;
using NSize = Tizen.NUI.Size;
using NPoint = Tizen.NUI.Position;
using NSize2D = Tizen.NUI.Size2D;
using NVector4 = Tizen.NUI.Vector4;
using ScrollableDirection = Tizen.NUI.Components.ScrollableBase.Direction;
using NPanelLayoutType = Tizen.NUI.InputMethod.PanelLayoutType;
using NActionButtonTitleType = Tizen.NUI.InputMethod.ActionButtonTitleType;

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

        public static Color ToColor(this NVector4 c)
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

        public static ScrollOrientation ToCommon(this ScrollableDirection direction)
        {
            if (direction == ScrollableDirection.Horizontal)
                return ScrollOrientation.Horizontal;
            else if (direction == ScrollableDirection.Vertical)
                return ScrollOrientation.Vertical;
            else
                return ScrollOrientation.Both;
        }

        public static ScrollableDirection ToNative(this ScrollOrientation orientation)
        {
            if (orientation == ScrollOrientation.Horizontal)
                return ScrollableDirection.Horizontal;
            else
                return ScrollableDirection.Vertical;
        }

        public static NPanelLayoutType ToPanelLayoutType(this Keyboard keyboard)
        {
            switch (keyboard)
            {
                case Keyboard.Normal:
                    return NPanelLayoutType.Normal;
                case Keyboard.Number:
                    return NPanelLayoutType.Number;
                case Keyboard.Email:
                    return NPanelLayoutType.Email;
                case Keyboard.Url:
                    return NPanelLayoutType.URL;
                case Keyboard.PhoneNumber:
                    return NPanelLayoutType.PhoneNumber;
                case Keyboard.Ip:
                    return NPanelLayoutType.IP;
                case Keyboard.Month:
                    return NPanelLayoutType.Month;
                case Keyboard.NumberOnly:
                case Keyboard.Numeric:
                    return NPanelLayoutType.NumberOnly;
                case Keyboard.Hex:
                    return NPanelLayoutType.HEX;
                case Keyboard.Terminal:
                    return NPanelLayoutType.Terminal;
                case Keyboard.Password:
                    return NPanelLayoutType.Password;
                case Keyboard.DateTime:
                    return NPanelLayoutType.Datetime;
                case Keyboard.Emoticon:
                    return NPanelLayoutType.Emoticon;
                default:
                    return NPanelLayoutType.Normal;
            }
        }

        public static NActionButtonTitleType ToActionButtonType(this ReturnType returnType)
        {
            switch (returnType)
            {
                case ReturnType.Go:
                    return NActionButtonTitleType.Go;
                case ReturnType.Next:
                    return NActionButtonTitleType.Next;
                case ReturnType.Send:
                    return NActionButtonTitleType.Send;
                case ReturnType.Search:
                    return NActionButtonTitleType.Search;
                case ReturnType.Done:
                    return NActionButtonTitleType.Done;
                case ReturnType.Default:
                    return NActionButtonTitleType.Default;
                default:
                    return NActionButtonTitleType.Default;
            }
        }

        internal static int ToPixel(this double dp)
        {
            return (int)(dp * DeviceInfo.ScalingFactor);
        }

    }
}
