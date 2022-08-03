using SkiaSharp;
using SkiaSharp.Views.Tizen;
using Tizen.NUI;

namespace Tizen.UIExtensions.NUI
{
    public class SKCanvasView : CustomRenderingView
    {
        public SKCanvasView()
        {
            OnResized();
        }

        protected override void OnDrawFrame()
        {
            if (Size.Width <= 0 || Size.Height <= 0)
                return;

            int width = (int)Size.Width;
            int height = (int)Size.Height;
            int stride = 4 * (int)Size.Width;

            using var pixelBuffer = new PixelBuffer((uint)width, (uint)height, PixelFormat.BGRA8888);
            var buffer = pixelBuffer.GetBuffer();

            var info = new SKImageInfo(width, height, SKColorType.Bgra8888);
            using (var surface = SKSurface.Create(info, buffer, stride))
            {
                if (surface == null)
                {
                    Invalidate();
                    return;
                }

                // draw using SkiaSharp
                SendPaintSurface(new SKPaintSurfaceEventArgs(surface, info));
                surface.Canvas.Flush();
            }

            using var pixelData = PixelBuffer.Convert(pixelBuffer);
            using var url = pixelData.GenerateUrl();
            SetImage(url.ToString());
        }

        protected override void OnResized()
        {
            if (Size.Width <= 0 || Size.Height <= 0)
                return;

            UpdateSurface();
            OnDrawFrame();
            UpdateImageUrl();
        }
    }
}
