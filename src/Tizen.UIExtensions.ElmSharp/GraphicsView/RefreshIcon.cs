using System;
using ElmSharp;
using Tizen.UIExtensions.Common.GraphicsView;

namespace Tizen.UIExtensions.ElmSharp.GraphicsView
{
    /// <summary>
    /// A visual control used to indicate that refreshing is ongoing.
    /// </summary>
    public class RefreshIcon : GraphicsView<RefreshIconDrawable>, IRefreshIcon
    {
        /// <summary>
        /// Initializes a new instance of the RefreshIcon.
        /// </summary>
        public RefreshIcon(EvasObject parent) : base(parent)
        {
            Drawable = new RefreshIconDrawable(this);

            var size = Drawable.Measure(double.PositiveInfinity, double.PositiveInfinity);
            Resize((int)size.Width, (int)size.Height);
        }

        /// <summary>
        /// Gets or sets the background of RefreshIcon.
        /// </summary>
        public new Common.Color BackgroundColor
        {
            get => GetProperty<Common.Color>(nameof(BackgroundColor));
            set => SetProperty(nameof(BackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the value indicating if the RefreshIcon is running.
        /// </summary>
        public bool IsRunning
        {
            get => GetProperty<bool>(nameof(IsRunning));
            set => SetProperty(nameof(IsRunning), value);
        }

        /// <summary>
        /// Gets or sets the value indicating if the RefreshIcon is pulling.
        /// </summary>
        public bool IsPulling
        {
            get => GetProperty<bool>(nameof(IsPulling));
            set => SetProperty(nameof(IsPulling), value);
        }

        /// <summary>
        /// Gets or sets the Color of the RefreshIcon.
        /// </summary>
        public new Common.Color Color
        {
            get => GetProperty<Common.Color>(nameof(Color));
            set => SetProperty(nameof(Color), value);
        }

        /// <summary>
        /// Gets or sets the value(0.0 ~ 1.0) indicating the icon pulling rate.
        /// </summary>
        public float PullDistance
        {
            get => GetProperty<float>(nameof(PullDistance));
            set => SetProperty(nameof(PullDistance), Math.Clamp(value, 0, 1));
        }
    }
}
