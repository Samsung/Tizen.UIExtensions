using SkiaSharp;
using SkiaSharp.Views.Tizen;
using System;
using System.Diagnostics;
using Tizen.NUI;

namespace Tizen.UIExtensions.NUI
{

    public class SKCanvasView : CustomRenderingView
    {
        NativeImageQueue? _nativeImageSource;
        ImageUrl? _imageUrl;

        int _bufferWidth = 0;
        int _bufferHeight = 0;
        int _bufferStride = 0;

        public SKCanvasView()
        {
            OnResized();
        }

        protected override void OnDrawFrame()
        {
            if (Size.Width == 0 || Size.Height == 0)
                return;

            if (_nativeImageSource?.CanDequeueBuffer() ?? false)
            {
                var buffer = _nativeImageSource!.DequeueBuffer(ref _bufferWidth, ref _bufferHeight, ref _bufferStride);
                Debug.Assert(buffer != IntPtr.Zero, "AcquireBuffer is faild");
                var info = new SKImageInfo(_bufferWidth, _bufferHeight);

                using (var surface = SKSurface.Create(info, buffer, _bufferStride))
                {
                    // draw using SkiaSharp
                    SendPaintSurface(new SKPaintSurfaceEventArgs(surface, info));
                    surface.Canvas.Flush();
                }
                _nativeImageSource.EnqueueBuffer(buffer);
                Window.Instance.KeepRendering(0);
            }
            else
            {
                Invalidate();
            }

        }

        protected override void OnResized()
        {
            if (Size.Width == 0 || Size.Height == 0)
                return;

            UpdateSurface();
            OnDrawFrame();
            UpdateImageUrl();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _nativeImageSource?.Dispose();
                _nativeImageSource = null;
                _imageUrl?.Dispose();
                _imageUrl = null;
            }
            base.Dispose(disposing);
        }

        void UpdateSurface()
        {
            _nativeImageSource?.Dispose();
            _nativeImageSource = new NativeImageQueue((uint)Size.Width, (uint)Size.Height, NativeImageQueue.ColorFormat.RGBA8888);
        }

        void UpdateImageUrl()
        {
            _imageUrl?.Dispose();
            _imageUrl = _nativeImageSource!.GenerateUrl();
            SetImage(_imageUrl.ToString());
        }
    }
}
