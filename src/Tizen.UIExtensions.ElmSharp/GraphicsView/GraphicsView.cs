using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics.Skia.Views;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;
using GestureLayer = ElmSharp.GestureLayer;
using GPoint = Microsoft.Maui.Graphics.Point;

namespace Tizen.UIExtensions.ElmSharp.GraphicsView
{
    public abstract class GraphicsView<TDrawable> : SkiaGraphicsView, IMeasurable where TDrawable : GraphicsViewDrawable
    {
        Dictionary<string, object> _propertyBag = new Dictionary<string, object>();
        TDrawable? _drawable;
        bool _isEnabled = true;
        GestureLayer _gestureLayer;

        protected GraphicsView(global::ElmSharp.EvasObject parent) : base(parent)
        {
            _gestureLayer = new GestureLayer(parent);
            _gestureLayer.Attach(this);
            _gestureLayer.SetTapCallback(GestureLayer.GestureType.Tap, GestureLayer.GestureState.Start, OnTapStart);
            _gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Move, OnTapMove);
            _gestureLayer.SetMomentumCallback(GestureLayer.GestureState.End, OnTapEnd);
            _gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Abort, OnTapEnd);

            Focused += OnFocused;
            Unfocused += OnUnfocused;
        }

        public override bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                IsEnabled = _isEnabled = value;
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

        protected virtual void OnUnfocused(object? sender, EventArgs e)
        {
            Drawable?.OnUnfocused();
        }

        protected virtual void OnFocused(object? sender, EventArgs e)
        {
            Drawable?.OnFocused();
        }

        protected virtual void OnTapStart(GestureLayer.TapData e)
        {
            if (!IsEnabled)
                return;

            Drawable?.OnTouchDown(GetScaledGraphicsPoint(e.X, e.Y));
        }

        protected virtual void OnTapMove(GestureLayer.MomentumData e)
        {
            if (!IsEnabled)
                return;

            Drawable?.OnTouchMove(GetScaledGraphicsPoint(e.X2, e.Y2));
        }

        protected virtual void OnTapEnd(GestureLayer.MomentumData e)
        {
            if (!IsEnabled)
                return;

            Drawable?.OnTouchUp(GetScaledGraphicsPoint(e.X2, e.Y2));
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
