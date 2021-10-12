using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics.Skia.Views;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;
using EvasObject = ElmSharp.EvasObject;
using GestureLayer = ElmSharp.GestureLayer;
using GPoint = Microsoft.Maui.Graphics.Point;

namespace Tizen.UIExtensions.ElmSharp.GraphicsView
{
    /// <summary>
    /// A base class for Views that inherits `SkiaGraphicsView`.
    /// It helps Views covering `GraphicsViewDrawable` methods.
    /// </summary>
    public abstract class GraphicsView<TDrawable> : SkiaGraphicsView, IMeasurable where TDrawable : GraphicsViewDrawable
    {
        Dictionary<string, object> _propertyBag = new Dictionary<string, object>();
        TDrawable? _drawable;
        bool _isEnabled = true;
        GestureLayer _gestureLayer;

        protected GraphicsView(EvasObject parent) : base(parent)
        {
            _gestureLayer = new GestureLayer(parent);
            _gestureLayer.Attach(this);
            _gestureLayer.SetTapCallback(GestureLayer.GestureType.Tap, GestureLayer.GestureState.Start, OnTapStartCallback);
            _gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Move, OnTapMoveCallback);
            _gestureLayer.SetMomentumCallback(GestureLayer.GestureState.End, OnTapEndCallback);
            _gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Abort, OnTapEndCallback);

            Focused += OnFocused;
            Unfocused += OnUnfocused;
        }

        /// <summary>
        /// Gets or sets the state of the view, which might be enabled or disabled.
        /// </summary>
        public override bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value != _isEnabled)
                {
                    IsEnabled = _isEnabled = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Measures the size of the view based on a drawable.
        /// </summary>
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

        public new TDrawable? Drawable
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

        protected virtual void OnUnfocused(object? sender, EventArgs e)
        {
            Drawable?.OnUnfocused();
        }

        protected virtual void OnFocused(object? sender, EventArgs e)
        {
            Drawable?.OnFocused();
        }

        void OnTapStartCallback(GestureLayer.TapData e)
        {
            if (!IsEnabled)
                return;
            OnTapStart(GetScaledGraphicsPoint(e.X, e.Y));
        }

        void OnTapMoveCallback(GestureLayer.MomentumData e)
        {
            if (!IsEnabled)
                return;
            OnTapMove(GetScaledGraphicsPoint(e.X2, e.Y2));
        }

        void OnTapEndCallback(GestureLayer.MomentumData e)
        {
            if (!IsEnabled)
                return;
            OnTapEnd(GetScaledGraphicsPoint(e.X2, e.Y2));
        }

        protected virtual void OnTapStart(GPoint touchPoint)
        {
            Drawable?.OnTouchDown(touchPoint);
        }

        protected virtual void OnTapMove(GPoint touchPoint)
        {
            Drawable?.OnTouchMove(touchPoint);
        }

        protected virtual void OnTapEnd(GPoint touchPoint)
        {
            Drawable?.OnTouchUp(touchPoint);
        }

        GPoint GetScaledGraphicsPoint(int x, int y)
        {
            return new GPoint(x / DeviceInfo.ScalingFactor, y / DeviceInfo.ScalingFactor);
        }

        void OnInvalidated(object? sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
