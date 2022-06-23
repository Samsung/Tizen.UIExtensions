using Tizen.NUI.Components;
using Tizen.UIExtensions.Common;
using CRect = Tizen.UIExtensions.Common.Rect;
using ScrollOrientation = Tizen.UIExtensions.Common.ScrollOrientation;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// The ScrollView is a container that holds and clips a single object and allows you to scroll across it.
    /// </summary>
    public class ScrollView : ScrollableBase
    {

        /// <summary>
        /// Gets or sets the scrolling direction of the ScrollView.
        /// </summary>
        public ScrollOrientation ScrollOrientation
        {
            get => ScrollingDirection.ToCommon();
            set => ScrollingDirection = value.ToNative();
        }

        /// <summary>
        /// Gets or sets a value that controls when the vertical scroll bar is visible.
        /// </summary>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get
            {
                if (HideScrollbar)
                    return ScrollBarVisibility.Never;
                else
                    return FadeScrollbar ? ScrollBarVisibility.Default : ScrollBarVisibility.Always;
            }
            set
            {
                if (ScrollBarVisibility.Always == value)
                {
                    HideScrollbar = false;
                    FadeScrollbar = false;
                }
                else if (ScrollBarVisibility.Default == value)
                {
                    HideScrollbar = false;
                    FadeScrollbar = true;
                }
                else
                {
                    HideScrollbar = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that controls when the horizontal scroll bar is visible.
        /// </summary>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            // NUI not support individually setting of scrollbar visibility
            get => VerticalScrollBarVisibility;
            set => VerticalScrollBarVisibility = value;
        }

        /// <summary>
        /// Gets the current scroll bound.
        /// </summary>
        public CRect ScrollBound
        {
            get => new CRect(-ContentContainer.Position.X, -ContentContainer.Position.Y, Size.Width, Size.Height);
        }

    }
}
