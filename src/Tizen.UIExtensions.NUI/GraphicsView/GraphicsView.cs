using System;
using System.Collections.Generic;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;
using GPoint = Microsoft.Maui.Graphics.Point;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    public abstract class GraphicsView<TDrawable> : SkiaGraphicsView, IMeasurable where TDrawable : GraphicsViewDrawable
    {
        Dictionary<string, object> _propertyBag = new Dictionary<string, object>();
        TDrawable? _drawable;
        bool _isEnabled = true;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                EnableControlState = _isEnabled = value;
                Invalidate();
            }
        }

        public virtual Size Measure(double availableWidth, double availableHeight)
        {
            return Drawable?.Measure(availableWidth, availableHeight) ?? new Size(availableWidth, availableHeight);
        }

        protected void SetProperty<T>(string name, T value)
        {
            _propertyBag[name] = value!;
            Invalidate();
        }

#nullable disable
        protected T GetProperty<T>(string name)
        {
            if (_propertyBag.TryGetValue(name, out object value))
            {
                return (T)value;
            }
            return default;
        }
#nullable enable

        protected new TDrawable? Drawable
        {
            get => _drawable;
            set
            {
                if (_drawable != value)
                {
                    if (_drawable != null)
                    {
                        _drawable.Invalidated -= OnInvalidated;
                    }

                    base.Drawable = _drawable = value;

                    if (value != null)
                    {
                        value.Invalidated += OnInvalidated;
                    }
                }
            }
        }

        protected GraphicsView()
        {
            TouchEvent += OnTouch;
        }

        protected virtual void OnUnfocused(object? sender, EventArgs e)
        {
            Drawable?.OnUnfocused();
        }

        protected virtual void OnFocused(object? sender, EventArgs e)
        {
            Drawable?.OnFocused();
        }

        protected virtual bool OnTouch(object source, TouchEventArgs e)
        {
            if (!IsEnabled)
                return false;

            var pos = e.Touch.GetLocalPosition(0);
            var state = e.Touch.GetState(0);
            var point = new GPoint(pos.X / DeviceInfo.ScalingFactor, pos.Y / DeviceInfo.ScalingFactor);

            if (state == Tizen.NUI.PointStateType.Down)
            {
                Drawable?.OnTouchDown(point);
            }
            else if (state == Tizen.NUI.PointStateType.Up)
            {
                Drawable?.OnTouchUp(point);
            }
            else if (state == Tizen.NUI.PointStateType.Motion)
            {
                Drawable?.OnTouchMove(point);
            }

            return true;
        }

        void OnInvalidated(object? sender, global::System.EventArgs e)
        {
            Invalidate();
        }
    }
}
