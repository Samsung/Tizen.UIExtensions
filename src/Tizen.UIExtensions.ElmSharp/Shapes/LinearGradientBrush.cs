using System.Collections.Generic;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    public class LinearGradientBrush : GradientBrush
    {
        public LinearGradientBrush()
        {
            StartPoint = new Point(0, 0);
            EndPoint = new Point(1, 1);
        }

        public LinearGradientBrush(IList<GradientStop> gradientStops) : this()
        {
            GradientStops = gradientStops;
        }

        public LinearGradientBrush(IList<GradientStop> gradientStops, Point startPoint, Point endPoint)
        {
            GradientStops = gradientStops;
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public Point StartPoint;

        public Point EndPoint;
    }
}
