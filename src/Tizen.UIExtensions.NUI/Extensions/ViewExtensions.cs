using View = Tizen.NUI.BaseComponents.View;
using Color = Tizen.UIExtensions.Common.Color;

namespace Tizen.UIExtensions.NUI
{
    public static class ViewExtensions
    {
        public static void UpdateBackgroundColor(this View view, Color color)
        {
            view.BackgroundColor = color.ToNative();
        }
    }
}
