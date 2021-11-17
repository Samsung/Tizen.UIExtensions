using Microsoft.Maui.Graphics;
using GColor = Microsoft.Maui.Graphics.Color;
using GPoint = Microsoft.Maui.Graphics.Point;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public class ButtonDrawable : GraphicsViewDrawable
    {
        const float MaterialBackgroundHeight = 36f;
        const float MaterialShadowOffset = 3f;

        readonly RippleEffectDrawable _rippleEffect;
        RectangleF _backgroundRect;

        public ButtonDrawable(IButton view)
        {
            View = view;
            _rippleEffect = new RippleEffectDrawable();
            _rippleEffect.Invalidated += (s, e) => SendInvalidated();
        }

        IButton View { get; }

        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            DrawMaterialButtonBackground(canvas, dirtyRect);
            DrawMaterialButtonText(canvas, dirtyRect);
            _rippleEffect.Draw(canvas, dirtyRect);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {

            var fontSize = (float)(Material.Font.Button * DeviceInfo.ScalingFactor);
            float textLength = 0;

            using (var paint = new SkiaSharp.SKPaint
            {
                TextSize = fontSize
            })
            {
                textLength = paint.MeasureText(View?.Text ?? "");
            }

            return new TSize(DeviceInfo.ScalingFactor * MaterialBackgroundHeight + textLength, DeviceInfo.ScalingFactor * MaterialBackgroundHeight);
        }

        public override void OnTouchDown(GPoint point)
        {
            _rippleEffect.ClipRectangle = _backgroundRect;
            _rippleEffect.OnTouchDown(point);
        }

        public override void OnTouchUp(GPoint point)
        {
            _rippleEffect.OnTouchUp(point);
        }

        void DrawMaterialButtonBackground(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FillColor = IsEnabled ? View.BackgroundColor.ToGraphicsColor(Material.Color.Blue) : GColor.FromArgb(Material.Color.Gray1);

            var x = dirtyRect.X;
            var y = dirtyRect.Y;

            var width = dirtyRect.Width - MaterialShadowOffset;
            var height = MaterialBackgroundHeight - MaterialShadowOffset;
            canvas.SetShadow(new SizeF(0, 1), 3, GColor.FromArgb(Material.Color.Gray2));

            canvas.FillRoundedRectangle(x, y, width, height, (float)View.CornerRadius);

            canvas.RestoreState();

            _backgroundRect = new RectangleF(x, y, width, height);
        }

        void DrawMaterialButtonText(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FontName = "Roboto";
            canvas.FontColor = View.TextColor.ToGraphicsColor(Material.Color.White);

            var x = dirtyRect.X;
            var y = dirtyRect.Y;

            var width = dirtyRect.Width - MaterialShadowOffset;

            canvas.DrawString(View.Text, x, y, width, MaterialBackgroundHeight, HorizontalAlignment.Center, VerticalAlignment.Center);

            canvas.RestoreState();
        }

    }
}
