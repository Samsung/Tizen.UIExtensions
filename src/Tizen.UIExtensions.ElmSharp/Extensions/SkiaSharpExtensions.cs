using SkiaSharp;
using System;
using System.Linq;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    public static class SkiaSharpExtensions
    {
        public static SKStrokeCap ToSkia(this PenLineCap penLineCap)
        {
            SKStrokeCap skStrokeCap = SKStrokeCap.Butt;
            switch (penLineCap)
            {
                case PenLineCap.Flat:
                    skStrokeCap = SKStrokeCap.Butt;
                    break;
                case PenLineCap.Square:
                    skStrokeCap = SKStrokeCap.Square;
                    break;
                case PenLineCap.Round:
                    skStrokeCap = SKStrokeCap.Round;
                    break;
            }
            return skStrokeCap;
        }

        public static PenLineCap ToCommon(this SKStrokeCap skStrokeCap)
        {
            PenLineCap penLineCap = PenLineCap.Flat;
            switch (skStrokeCap)
            {
                case SKStrokeCap.Butt:
                    penLineCap = PenLineCap.Flat;
                    break;
                case SKStrokeCap.Square:
                    penLineCap = PenLineCap.Square;
                    break;
                case SKStrokeCap.Round:
                    penLineCap = PenLineCap.Round;
                    break;
            }
            return penLineCap;
        }

        public static SKStrokeJoin ToSkia(this PenLineJoin penLineCap)
        {
            SKStrokeJoin skStrokeJoin = SKStrokeJoin.Miter;
            switch (penLineCap)
            {
                case PenLineJoin.Miter:
                    skStrokeJoin = SKStrokeJoin.Miter;
                    break;
                case PenLineJoin.Bevel:
                    skStrokeJoin = SKStrokeJoin.Bevel;
                    break;
                case PenLineJoin.Round:
                    skStrokeJoin = SKStrokeJoin.Round;
                    break;
            }
            return skStrokeJoin;
        }

        public static PenLineJoin ToCommon(this SKStrokeJoin skStrokeCap)
        {
            PenLineJoin penLineCap = PenLineJoin.Miter;
            switch (skStrokeCap)
            {
                case SKStrokeJoin.Miter:
                    penLineCap = PenLineJoin.Miter;
                    break;
                case SKStrokeJoin.Bevel:
                    penLineCap = PenLineJoin.Bevel;
                    break;
                case SKStrokeJoin.Round:
                    penLineCap = PenLineJoin.Round;
                    break;
            }
            return penLineCap;
        }

        public static Color ToCommon(this SKColor skColor)
        {
            return Color.FromRgba(skColor.Red, skColor.Green, skColor.Blue, skColor.Alpha);
        }

        public static SKColor ToSkia(this Color color)
        {
            var eColor = color.ToNative();
            return new SKColor((byte)eColor.R, (byte)eColor.G, (byte)eColor.B, (byte)eColor.A);
        }

        public static SKColor ToSolidColor(this SolidColorBrush solidColorBrush)
        {
            return solidColorBrush.Color != Color.Default ? solidColorBrush.Color.ToSkia() : SKColor.Empty;
        }

        public static SKShader CreateShader(this GradientBrush gradientBrush, SKRect bounds)
        {
            SKShader shader = null;

            if (gradientBrush is LinearGradientBrush linearGradientBrush)
            {
                shader = CreateLinearGradient(linearGradientBrush, bounds);
            }

            if (gradientBrush is RadialGradientBrush radialGradientBrush)
            {
                shader = CreateRadialGradient(radialGradientBrush, bounds);
            }

            return shader;
        }
        static SKShader CreateLinearGradient(LinearGradientBrush linearGradientBrush, SKRect pathBounds)
        {
            var startPoint = new SKPoint(pathBounds.Left + (float)linearGradientBrush.StartPoint.X * pathBounds.Width, pathBounds.Top + (float)linearGradientBrush.StartPoint.Y * pathBounds.Height);
            var endPoint = new SKPoint(pathBounds.Left + (float)linearGradientBrush.EndPoint.X * pathBounds.Width, pathBounds.Top + (float)linearGradientBrush.EndPoint.Y * pathBounds.Height);
            var orderedGradientStops = linearGradientBrush.GradientStops.OrderBy(x => x.Offset).ToList();
            var gradientColors = orderedGradientStops.Select(x => x.Color.ToSkia()).ToArray();
            var gradientColorPos = orderedGradientStops.Select(x => x.Offset).ToArray();
            return SKShader.CreateLinearGradient(startPoint, endPoint, gradientColors, gradientColorPos, SKShaderTileMode.Clamp);
        }

        static SKShader CreateRadialGradient(RadialGradientBrush radialGradientBrush, SKRect pathBounds)
        {
            var center = new SKPoint((float)radialGradientBrush.Center.X * pathBounds.Width + pathBounds.Left, (float)radialGradientBrush.Center.Y * pathBounds.Height + pathBounds.Top);
            var radius = (float)radialGradientBrush.Radius * Math.Max(pathBounds.Height, pathBounds.Width);
            var orderedGradientStops = radialGradientBrush.GradientStops.OrderBy(x => x.Offset).ToList();
            var gradientColors = orderedGradientStops.Select(x => x.Color.ToSkia()).ToArray();
            var gradientColorPos = orderedGradientStops.Select(x => x.Offset).ToArray();
            return SKShader.CreateRadialGradient(center, radius, gradientColors, gradientColorPos, SKShaderTileMode.Clamp);
        }
    }
}
