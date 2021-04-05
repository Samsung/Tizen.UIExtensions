using System.Collections.Generic;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    public class RadialGradientBrush : GradientBrush
    {
        public RadialGradientBrush()
        {
            Center = new Point(0.5, 0.5);
            Radius = 0.5d;
        }

        public RadialGradientBrush(List<GradientStop> gradientStops) : this()
        {
            GradientStops = gradientStops;
        }

        public RadialGradientBrush(List<GradientStop> gradientStops, double radius) : this()
        {
            GradientStops = gradientStops;
            Radius = radius;
        }

        public RadialGradientBrush(List<GradientStop> gradientStops, Point center, double radius)
        {
            GradientStops = gradientStops;
            Center = center;
            Radius = radius;
        }

        public Point Center;

        public double Radius;
    }
}
