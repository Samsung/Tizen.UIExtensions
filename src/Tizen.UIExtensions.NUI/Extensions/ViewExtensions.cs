using Color = Tizen.UIExtensions.Common.Color;
using View = Tizen.NUI.BaseComponents.View;

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
    }
}
