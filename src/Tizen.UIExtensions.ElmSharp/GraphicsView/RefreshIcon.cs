using Tizen.UIExtensions.Common.GraphicsView;
using ElmSharp;
using DeviceInfo = Tizen.UIExtensions.Common.DeviceInfo;

namespace Tizen.UIExtensions.ElmSharp.GraphicsView
{
    /// <summary>
    /// A visual control used to indicate that refreshing is ongoing.
    /// </summary>
    public class RefreshIcon : GraphicsView<RefreshIconDrawable>, IRefreshIcon
    {
        float _iconSize = ThemeConstants.RefreshLayout.Resources.IconSize;
        float _strokeWidth = ThemeConstants.RefreshLayout.Resources.IconStrokeWidth;

        /// <summary>
        /// Initializes a new instance of the RefreshIcon.
        /// </summary>
        public RefreshIcon(EvasObject parent) : base(parent)
        {
            Drawable = new RefreshIconDrawable(this);

            var iconSize = (_iconSize + (_strokeWidth * 2)) * DeviceInfo.ScalingFactor;
            Resize((int)iconSize, (int)iconSize);
        }

        /// <summary>
        /// Gets or sets the background of RefreshIcon.
        /// </summary>
        public new Common.Color BackgroundColor
        {
            get
            {
                if (Drawable == null)
                    return Common.Color.Transparent;
                else
                    return Drawable.BackgroundColor;
            }
            set
            {
                if (Drawable == null)
                    return;

                Drawable.BackgroundColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the value indicating if the RefreshIcon is running.
        /// </summary>
        public bool IsRunning
        {
            get => GetProperty<bool>(nameof(IsRunning));
            set
            {
                SetProperty(nameof(IsRunning), value);
                Drawable?.UpdateRunningAnimation(value);
            }
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
            set
            {
                if (value > 1.0)
                    SetProperty(nameof(PullDistance), 1.0f);
                else if (value < 0)
                    SetProperty(nameof(PullDistance), 0f);
                else
                    SetProperty(nameof(PullDistance), value);
            }
        }
    }
}
