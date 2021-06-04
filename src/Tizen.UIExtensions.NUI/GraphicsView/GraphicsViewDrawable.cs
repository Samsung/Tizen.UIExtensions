using System;
using System.Graphics;
using Tizen.UIExtensions.Common;
using GPoint = System.Graphics.Point;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    public abstract class GraphicsViewDrawable : IDrawable, IMeasurable
    {
        public event EventHandler? Invalidated;
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
    }
}
