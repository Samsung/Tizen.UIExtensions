using View = Tizen.NUI.BaseComponents.View;
using Point = Tizen.UIExtensions.Common.Point;
using Rect = Tizen.UIExtensions.Common.Rect;
using Size = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.NUI
{
    public static class GeometryExtensions
    {
        public static void UpdateSize(this View view, Size size)
        {
            view.Size = size.ToNative();
        }

        public static void UpdatePosition(this View view, Point position)
        {
            view.Position = position.ToNative();
        }

        public static void UpdateBounds(this View view, Rect bounds)
        {
            view.Size = bounds.Size.ToNative();
            view.Position = bounds.Location.ToNative();
        }

        public static Rect GetBounds(this View view)
        {
            return new Rect(view.Position.ToCommon(), view.Size.ToCommon());
        }
    }
}
