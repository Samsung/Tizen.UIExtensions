using View = Tizen.NUI.BaseComponents.View;
using Color = Tizen.UIExtensions.Common.Color;
using Rect = Tizen.UIExtensions.Common.Rect;
using NButton = Tizen.NUI.Components.Button;
namespace Tizen.UIExtensions.NUI
{
    public static class ViewExtensions
    {
        public static void UpdateBackgroundColor(this View view, Color color)
        {
            view.BackgroundColor = color.ToNative();
        }

        public static Rect GetBounds(this View view)
        {
            return new Rect(view.Position.ToCommon(), view.Size.ToCommon());
        }

        public static void SetEnable(this View view, bool enable)
        {
            if (view is NButton button)
            {
                button.IsEnabled = enable;
            }
            else
            {
                view.EnableControlState = enable;
            }
        }
    }
}
