
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    public class GradientStop
    {
        public GradientStop() { }

        public GradientStop(Color color, float offset)
        {
            Color = color;
            Offset = offset;
        }

        public Color Color;

        public float Offset;
    }
}
