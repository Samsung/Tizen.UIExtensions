using System.Collections.Generic;

namespace Tizen.UIExtensions.ElmSharp
{
    public class GradientBrush : ShapeBrush
    {
        public GradientBrush()
        {
            GradientStops = new List<GradientStop>();
        }

        public IList<GradientStop> GradientStops;
    }
}
