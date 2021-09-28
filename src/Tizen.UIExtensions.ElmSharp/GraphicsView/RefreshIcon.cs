using System;
using Microsoft.Maui.Graphics.Skia.Views;
using Tizen.UIExtensions.Common.GraphicsView;
using ElmSharp;

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
        bool _isPulling;
        float _pullDistance;

        /// <summary>
        /// Initializes a new instance of the RefreshIcon.
        /// </summary>
        public RefreshIcon(EvasObject parent) : base(parent)
        {
            _drawable = new RefreshIconDrawable(this);
            _drawable.Invalidated += OnRefreshIconInvalidated;
            Drawable = _drawable;

            Resize((int)_drawable.MeasuredWidth, (int)_drawable.MeasuredHeight);
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
                _drawable.UpdateRunningAnimation(value);
            }
        }

        /// <summary>
        /// Gets or sets the value indicating if the RefreshIcon is pulling.
        /// </summary>
        public bool IsPulling
        {
            get => _isPulling;
            set
            {
                _isPulling = value;
                Invalidate();
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

        /// <summary>
        /// Gets or sets the value(0.0 ~ 1.0) indicating the icon pulling rate.
        /// </summary>
        public float PullDistance
        {
            get => _pullDistance;
            set
            {
                if (value > 1.0)
                    _pullDistance = 1.0f;
                else
                    _pullDistance = value;
                _drawable.UpdateIconDistance(_pullDistance);
            }
        }

        void OnRefreshIconInvalidated(object? sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
