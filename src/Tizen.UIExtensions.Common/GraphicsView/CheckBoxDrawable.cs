using Microsoft.Maui.Graphics;
using GColor = Microsoft.Maui.Graphics.Color;
using GPoint = Microsoft.Maui.Graphics.Point;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public class CheckBoxDrawable : GraphicsViewDrawable
    {
        const string MaterialCheckBoxMark = "M13.3516 1.35156L5 9.71094L0.648438 5.35156L1.35156 4.64844L5 8.28906L12.6484 0.648438L13.3516 1.35156Z";

        public CheckBoxDrawable(ICheckBox view)
        {
            View = view;
        }

        ICheckBox View { get; }

        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            DrawMaterialCheckBoxBackground(canvas, dirtyRect);
            DrawMaterialCheckBoxMark(canvas, dirtyRect);
            DrawMaterialCheckBoxText(canvas, dirtyRect);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            var fontSize = (float)(14 * DeviceInfo.ScalingFactor);
            float textLength = 0;

            using (var paint = new SkiaSharp.SKPaint
            {
                TextSize = fontSize
            })
            {
                textLength = paint.MeasureText(View?.Text ?? "");
            }

            return new TSize(DeviceInfo.ScalingFactor * 28 + textLength, DeviceInfo.ScalingFactor * 20);
        }

        public override void OnTouchDown(GPoint point)
        {
            View.IsChecked = !View.IsChecked;
        }

        void DrawMaterialCheckBoxBackground(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            float size = 18f;

            var x = dirtyRect.X;
            var y = dirtyRect.Y;

            if (dirtyRect.Height > size)
            {
                y += (dirtyRect.Height - size) / 2;
            }

            if (IsEnabled)
            {
                if (View.IsChecked)
                {
                    canvas.FillColor = View.Color.ToGraphicsColor(Material.Color.Blue);
                    canvas.FillRoundedRectangle(x, y, size, size, 2);
                }
                else
                {
                    var strokeWidth = 2;

                    canvas.StrokeSize = strokeWidth;
                    canvas.StrokeColor = View.Color.ToGraphicsColor(Material.Color.Gray1);
                    canvas.DrawRoundedRectangle(x + strokeWidth / 2, y + strokeWidth / 2, size - strokeWidth, size - strokeWidth, 2);
                }
            }
            else
            {
                var strokeWidth = 2;
                canvas.FillColor = GColor.FromArgb(Material.Color.Gray2);
                canvas.FillRoundedRectangle(x, y, size, size, 2);
                canvas.StrokeSize = strokeWidth;
                canvas.StrokeColor = GColor.FromArgb(Material.Color.Gray1);
                canvas.DrawRoundedRectangle(x + strokeWidth / 2, y + strokeWidth / 2, size - strokeWidth, size - strokeWidth, 2);
            }

            canvas.RestoreState();
        }

        void DrawMaterialCheckBoxMark(ICanvas canvas, RectangleF dirtyRect)
        {
            if (View.IsChecked)
            {
                canvas.SaveState();

                float size = 18f;

                float x = 2;
                float y = 4;

                if (dirtyRect.Height > size)
                {
                    y += (dirtyRect.Height - size) / 2;
                }

                canvas.Translate(x, y);

                var vBuilder = new PathBuilder();
                var path = vBuilder.BuildPath(MaterialCheckBoxMark);

                canvas.StrokeColor = Colors.White;
                canvas.DrawPath(path);

                canvas.RestoreState();
            }
        }

        void DrawMaterialCheckBoxText(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FontColor = View.TextColor.ToGraphicsColor(Cupertino.Color.Label.Light.Primary);
            canvas.FontSize = 14;

            float size = 20f;
            float margin = 8f;

            var height = dirtyRect.Height;
            var width = dirtyRect.Width;

            canvas.DrawString(View.Text, size + margin, dirtyRect.Y, width - (size + margin), height, HorizontalAlignment.Left, VerticalAlignment.Center);

            canvas.RestoreState();
        }
    }
}
