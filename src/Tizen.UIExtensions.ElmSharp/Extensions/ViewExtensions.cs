using EvasObject = ElmSharp.EvasObject;
using Widget = ElmSharp.Widget;
using Color = Tizen.UIExtensions.Common.Color;

namespace Tizen.UIExtensions.ElmSharp
{
    public static class ViewExtensions
    {
        public static void UpdateBackgroundColor(this EvasObject view, Color color)
        {
            if (view is Widget widget)
            {
                widget.BackgroundColor = color.ToNative();
            }
        }

        public static void SetEnable(this EvasObject view, bool enable)
        {
            if (view is Widget widget)
            {
                widget.IsEnabled = enable;
            }
        }
    }
}
