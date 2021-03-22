using ElmSharp;
using Point = Tizen.UIExtensions.Common.Point;
using Rect = Tizen.UIExtensions.Common.Rect;
using Size = Tizen.UIExtensions.Common.Size;
using ERect = ElmSharp.Rect;

namespace Tizen.UIExtensions.ElmSharp
{
    public static class GeometryExtensions
    {
        public static void UpdateSize(this EvasObject view, Size size)
        {
            view.Resize((int)size.Width, (int)size.Height);
        }

        public static void UpdatePosition(this EvasObject view, Point position)
        {
            view.Move((int)position.X, (int)position.Y);
        }

        public static void UpdateBounds(this EvasObject view, Rect bounds)
        {
            view.Geometry = new ERect((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
        }
    }
}
