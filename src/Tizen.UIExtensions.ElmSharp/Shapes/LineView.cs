using ElmSharp;
using SkiaSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    public class LineView : ShapeView
    {
        float _x1, _y1, _x2, _y2 = 0.0f;

        public LineView(EvasObject parent) : base(parent)
        {
        }

        public float X1
        {
            get
            {
                return _x1;
            }
            set
            {
                _x1 = value;
                UpdateShape();
            }
        }

        public float Y1
        {
            get
            {
                return _y1;
            }
            set
            {
                _y1 = value;
                UpdateShape();
            }
        }

        public float X2
        {
            get
            {
                return _x2;
            }
            set
            {
                _x2 = value;
                UpdateShape();
            }
        }

        public float Y2
        {
            get
            {
                return _y2;
            }
            set
            {
                _y2 = value;
                UpdateShape();
            }
        }

        void UpdateShape()
        {
            var path = new SKPath();
            path.MoveTo(_x1, _y1);
            path.LineTo(_x2, _y2);
            UpdateShape(path);
        }
    }
}
