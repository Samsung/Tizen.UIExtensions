using System;
using Tizen.NUI.Components;
using Tizen.UIExtensions.Common;
using ScrollOrientation = Tizen.UIExtensions.Common.ScrollOrientation;
using CRect = Tizen.UIExtensions.Common.Rect;

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
            //TODO : fix it, NUI not support ScrollBar visiblility
            get
            {
                if (HideScrollbar)
                    return ScrollBarVisibility.Never;
                else
                    return ScrollBarVisibility.Always;
            }
            set
            {
                if (value == ScrollBarVisibility.Always)
                {
                    HideScrollbar = false;
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
            //TODO : fix it, NUI not support ScrollBar visiblility
            get
            {
                if (HideScrollbar)
                    return ScrollBarVisibility.Never;
                else
                    return ScrollBarVisibility.Always;
            }
            set
            {
                if (value == ScrollBarVisibility.Always)
                {
                    HideScrollbar = false;
                }
                else
                {
                    HideScrollbar = true;
                }
            }
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
