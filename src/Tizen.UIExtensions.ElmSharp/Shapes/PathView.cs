using ElmSharp;
using SkiaSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    public class PathView : ShapeView
    {
        public PathView(EvasObject parent) : base(parent)
        {
        }

        public void UpdateData(SKPath path)
        {
            UpdateShape(path);
        }

        public void UpdateTransform(SKMatrix transform)
        {
            UpdateShapeTransform(transform);
        }
    }
}
