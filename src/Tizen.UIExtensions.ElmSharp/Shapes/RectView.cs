using ElmSharp;
using SkiaSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    public class RectView : ShapeView
    {
        float radiusX = 0f;
        float radiusY = 0f;

        public RectView(EvasObject parent) : base(parent)
        {
            UpdateShape();
        }

        public float RadiusX
        {
            get
            {
                return radiusX;
            }
            set
            {
                radiusX = value;
                UpdateShape();
            }
        }

        public float RadiusY
        {
            get
            {
                return radiusY;
            }
            set
            {
                radiusY = value;
                UpdateShape();
            }
        }
        void UpdateShape()
        {
            var path = new SKPath();
            path.AddRoundRect(new SKRect(0, 0, 1, 1), RadiusX, RadiusY, SKPathDirection.Clockwise);
            UpdateShape(path);
        }
    }
}
