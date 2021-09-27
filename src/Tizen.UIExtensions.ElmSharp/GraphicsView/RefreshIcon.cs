using System;
using Microsoft.Maui.Graphics.Skia.Views;
using Tizen.UIExtensions.Common.GraphicsView;
using ElmSharp;
using DeviceInfo = Tizen.UIExtensions.Common.DeviceInfo;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// A visual control used to indicate that refreshing is ongoing.
    /// </summary>
    public class RefreshIcon : SkiaGraphicsView, IRefreshIcon
    {
        RefreshIconDrawable _drawable;
        bool _isRunning;
        Common.Color _color;

        /// <summary>
        /// Initializes a new instance of the RefreshIcon.
        /// </summary>
        public RefreshIcon(EvasObject parent) : base(parent)
        {
            _drawable = new RefreshIconDrawable(this);
            _drawable.Invalidated += OnRefreshIconInvalidated;
            Drawable = _drawable;

            var size = (RefreshIconDrawable.IconSize + (RefreshIconDrawable.StrokeWidth * 2)) * DeviceInfo.ScalingFactor;
            Resize((int)size, (int)size);
        }

        /// <summary>
        /// Gets or sets the value indicating if the RefreshIcon is running.
        /// </summary>
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                _drawable.UpdateAnimation(value);
            }
        }

        /// <summary>
        /// Gets or sets the Color of the RefreshIcon.
        /// </summary>
        public new Common.Color Color
        {
            get => _color;
            set
            {
                _color = value;
                Invalidate();
            }
        }

        void OnRefreshIconInvalidated(object? sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
