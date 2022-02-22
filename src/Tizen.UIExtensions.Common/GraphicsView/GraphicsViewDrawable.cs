using System;
using Microsoft.Maui.Graphics;
using GPoint = Microsoft.Maui.Graphics.Point;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public abstract class GraphicsViewDrawable : IDrawable, IMeasurable, IDisposable
    {
        public event EventHandler? Invalidated;

        public bool IsEnabled { get; set; } = true;

        public abstract void Draw(ICanvas canvas, RectangleF dirtyRect);

        public virtual void OnTouchDown(GPoint point) { }

        public virtual void OnTouchUp(GPoint point) { }

        public virtual void OnTouchMove(GPoint point) { }

        public virtual void OnFocused() { }

        public virtual void OnUnfocused() { }

        protected void SendInvalidated()
        {
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public abstract TSize Measure(double availableWidth, double availableHeight);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
