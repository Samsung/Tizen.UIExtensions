using ScrollableBase = Tizen.NUI.Components.ScrollableBase;
using Rect = Tizen.UIExtensions.Common.Rect;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI
{
    public static class ScrollViewExtensions
    {
        public static Rect GetScrollBound(this ScrollableBase view)
        {
            return new Rect(-view.ContentContainer.Position.X, -view.ContentContainer.Position.Y, view.Size.Width, view.Size.Height);
        }

        public static void SetScrollOrientation(this ScrollableBase view, ScrollOrientation orientation)
        {
            view.ScrollingDirection = orientation.ToNative();
        }

    }
}
