using SkiaSharp;
using SkiaSharp.Views.Tizen;
using System;
using System.Diagnostics;
using Tizen.NUI;

namespace Tizen.UIExtensions.NUI
{

    public class SKCanvasView : CustomRenderingView
    {
        NativeImageSource? _nativeImageSource;
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

            UpdateSurface();

            var buffer = _nativeImageSource!.AcquireBuffer(ref _bufferWidth, ref _bufferHeight, ref _bufferStride);
            Debug.Assert(buffer != IntPtr.Zero, "AcquireBuffer is faild");
            var info = new SKImageInfo(_bufferWidth, _bufferHeight);

            using (var surface = SKSurface.Create(info, buffer, _bufferStride))
            {
                // draw using SkiaSharp
                SendPaintSurface(new SKPaintSurfaceEventArgs(surface, info));
                surface.Canvas.Flush();
            }
            _nativeImageSource.ReleaseBuffer();
            var url = _imageUrl?.ToString();
            SetImage(url);
        }

        protected override void OnResized()
        {
            if (Size.Width == 0 || Size.Height == 0)
                return;
            OnDrawFrame();
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
            _nativeImageSource = new NativeImageSource((uint)Size.Width, (uint)Size.Height, NativeImageSource.ColorDepth.Default);
            _imageUrl = _nativeImageSource.GenerateUrl();
        }
    }
}
