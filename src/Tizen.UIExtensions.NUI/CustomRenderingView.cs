using SkiaSharp.Views.Tizen;
using System;
using System.Threading;
using Tizen.NUI;
using NImageView = Tizen.NUI.BaseComponents.ImageView;

namespace Tizen.UIExtensions.NUI
{
    public abstract class CustomRenderingView : NImageView
    {
        bool _redrawRequest;
        PropertyNotification _resized;

        protected SynchronizationContext MainloopContext { get; }

        protected CustomRenderingView()
        {
            MainloopContext = SynchronizationContext.Current ?? throw new InvalidOperationException("Must create on main thread");
            _resized = AddPropertyNotification("Size", PropertyCondition.Step(0.1f));
            _resized.Notified += OnResized;
        }

        public event EventHandler<SKPaintSurfaceEventArgs>? PaintSurface;

        public void Invalidate()
        {
            if (!_redrawRequest)
            {
                _redrawRequest = true;
                MainloopContext.Post((s) =>
                {
                    _redrawRequest = false;
                    if (!Disposed)
                    {
                        OnDrawFrame();
                    }
                }, null);
            }
        }

        protected abstract void OnResized();

        protected abstract void OnDrawFrame();

        protected void SendPaintSurface(SKPaintSurfaceEventArgs e)
        {
            PaintSurface?.Invoke(this, e);
        }

        void OnResized(object source, PropertyNotification.NotifyEventArgs e)
        {
            OnResized();
        }
    }
}
