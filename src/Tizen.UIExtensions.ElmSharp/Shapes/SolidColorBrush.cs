using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    public class SolidColorBrush : ShapeBrush
    {
        public SolidColorBrush(Color color)
        {
            Color = color;
        }

        public Color Color;
    }
}
