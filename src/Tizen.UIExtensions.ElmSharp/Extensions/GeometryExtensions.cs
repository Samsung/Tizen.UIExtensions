using ElmSharp;
using CPoint = Tizen.UIExtensions.Common.Point;
using CRect = Tizen.UIExtensions.Common.Rect;
using CSize = Tizen.UIExtensions.Common.Size;
using ERect = ElmSharp.Rect;

namespace Tizen.UIExtensions.ElmSharp
{
    public static class GeometryExtensions
    {
        public static void UpdateSize(this EvasObject view, CSize size)
        {
            view.Resize((int)size.Width, (int)size.Height);
        }

        public static void UpdatePosition(this EvasObject view, CPoint position)
        {
            view.Move((int)position.X, (int)position.Y);
        }

        public static void UpdateBounds(this EvasObject view, CRect bounds)
        {
            view.Geometry = new ERect((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
        }
    }
}
