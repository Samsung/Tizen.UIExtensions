using ElmSharp;
using SkiaSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    public class EllipseView : ShapeView
    {
        public EllipseView(EvasObject parent) : base(parent)
        {
            UpdateShape();
        }

        void UpdateShape()
        {
            var path = new SKPath();
            path.AddCircle(0, 0, 1);
            UpdateShape(path);
        }
    }
}
