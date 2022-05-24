using System;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    /// <summary>
    /// A visual control used to indicate that refreshing is ongoing.
    /// </summary>
    public class RefreshIcon : GraphicsView<RefreshIconDrawable>, IRefreshIcon
    {
        /// <summary>
        /// Initializes a new instance of the RefreshIcon.
        /// </summary>
        public RefreshIcon()
        {
            Drawable = new RefreshIconDrawable(this);
        }

        /// <summary>
        /// Gets or sets the background of RefreshIcon.
        /// </summary>
        public new Color BackgroundColor
        {
            get => GetProperty<Color>(nameof(BackgroundColor));
            set => SetProperty(nameof(BackgroundColor), value);
        }

        Color IRefreshIcon.BackgroundColor => BackgroundColor;

        /// <summary>
        /// Gets or sets the value indicating if the RefreshIcon is running.
        /// </summary>
        public bool IsRunning
        {
            get => GetProperty<bool>(nameof(IsRunning));
            set => SetProperty(nameof(IsRunning), value);
        }

        bool IRefreshIcon.IsRunning => IsRunning;

        /// <summary>
        /// Gets or sets the value indicating if the RefreshIcon is pulling.
        /// </summary>
        public bool IsPulling
        {
            get => GetProperty<bool>(nameof(IsPulling));
            set => SetProperty(nameof(IsPulling), value);
        }

        bool IRefreshIcon.IsPulling => IsPulling;

        /// <summary>
        /// Gets or sets the Color of the RefreshIcon.
        /// </summary>
        public new Color Color
        {
            get => GetProperty<Color>(nameof(Color));
            set => SetProperty(nameof(Color), value);
        }
        Color IRefreshIcon.Color => Color;

        /// <summary>
        /// Gets or sets the value(0.0 ~ 1.0) indicating the icon pulling rate.
        /// </summary>
        public float PullDistance
        {
            get => GetProperty<float>(nameof(PullDistance));
            set => SetProperty(nameof(PullDistance), Math.Clamp(value, 0, 1));
        }

        float IRefreshIcon.PullDistance => PullDistance;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Drawable?.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
