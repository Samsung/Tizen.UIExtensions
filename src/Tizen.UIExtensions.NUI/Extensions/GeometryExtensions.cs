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
            if (view.Size.Width != bounds.Width || view.Size.Height != bounds.Height)
                view.Size = bounds.Size.ToNative();

            if (view.Position.X != bounds.X || view.Position.Y != bounds.Y)
                view.Position = bounds.Location.ToNative();
        }

        public static Rect GetBounds(this View view)
        {
            return new Rect(view.Position.ToCommon(), view.Size.ToCommon());
        }
    }
}
