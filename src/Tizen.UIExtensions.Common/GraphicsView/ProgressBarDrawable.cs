using System.Graphics;
using GColor = System.Graphics.Color;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public class ProgressBarDrawable : GraphicsViewDrawable
    {
        const float MaterialTrackHeight = 4.0f;

        public ProgressBarDrawable(IProgressBar view)
        {
            View = view;
        }

        IProgressBar View { get; }

        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            DrawMaterialProgressTrack(canvas, dirtyRect);
            DrawMaterialProgressBar(canvas, dirtyRect);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            return new TSize(availableWidth, MaterialTrackHeight * DeviceInfo.ScalingFactor);
        }

        void DrawMaterialProgressTrack(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FillColor = new GColor(Fluent.Color.Background.NeutralLight);

            var x = dirtyRect.X;
            var y = (float)((dirtyRect.Height - MaterialTrackHeight) / 2);

            var width = dirtyRect.Width;

            canvas.FillRectangle(x, y, width, MaterialTrackHeight);

            canvas.RestoreState();
        }

        void DrawMaterialProgressBar(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FillColor = View.ProgressColor.ToGraphicsColor(Material.Color.Blue);

            var x = dirtyRect.X;
            var y = (float)((dirtyRect.Height - MaterialTrackHeight) / 2);

            var width = dirtyRect.Width;

            canvas.FillRectangle(x, y, (float)(width * View.Progress), MaterialTrackHeight);

            canvas.RestoreState();
        }
    }
}
