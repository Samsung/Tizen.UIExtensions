using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp.Views.Tizen;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    public class SkiaGraphicsView : SKCanvasView
    {
        private IDrawable? _drawable;
        private SkiaCanvas _canvas;
        private ScalingCanvas _scalingCanvas;

        public SkiaGraphicsView(IDrawable? drawable = null)
        {
            _canvas = new SkiaCanvas();
            _scalingCanvas = new ScalingCanvas(_canvas);
            Drawable = drawable;
            PaintSurface += OnPaintSurface;
        }

        public IDrawable? Drawable
        {
            get => _drawable;
            set
            {
                _drawable = value;
                Invalidate();
            }
        }

        protected virtual void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            if (_drawable == null) return;
            var skiaCanvas = e.Surface.Canvas;
            skiaCanvas.Clear();

            var width = (float)(e.Info.Width / DeviceInfo.ScalingFactor);
            var height = (float)(e.Info.Height / DeviceInfo.ScalingFactor);

            _canvas.Canvas = skiaCanvas;
            _scalingCanvas.SaveState();
            _scalingCanvas.Scale((float)DeviceInfo.ScalingFactor, (float)DeviceInfo.ScalingFactor);
            _drawable.Draw(_scalingCanvas, new RectF(0, 0, width, height));
            _scalingCanvas.RestoreState();
        }
    }
}
