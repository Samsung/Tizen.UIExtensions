using Color = Tizen.UIExtensions.Common.Color;
using View = Tizen.NUI.BaseComponents.View;
using Vector2 = Tizen.NUI.Vector2;

namespace Tizen.UIExtensions.NUI
{
    public static class ViewExtensions
    {
        public static void UpdateBackgroundColor(this View view, Color color)
        {
            view.BackgroundColor = color.IsDefault ? Color.Transparent.ToNative() : color.ToNative();
        }

        public static void SetEnable(this View view, bool enable)
        {
            view.IsEnabled = enable;
        }

        public static bool IsInside(this View view, Vector2 position)
        {
            return position.X > 0 && position.X < view.SizeWidth && position.Y > 0 && position.Y < view.SizeHeight;
        }
    }
}
