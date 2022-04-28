using Microsoft.Maui.Graphics;
using GColor = Microsoft.Maui.Graphics.Color;
using GPoint = Microsoft.Maui.Graphics.Point;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public class StepperDrawable : GraphicsViewDrawable
    {
        const string MaterialStepperMinusIcon = "M0.990234 1.96143H13.0098C13.5161 1.96143 13.9478 1.53809 13.9478 1.01514C13.9478 0.500488 13.5161 0.0688477 13.0098 0.0688477H0.990234C0.500488 0.0688477 0.0522461 0.500488 0.0522461 1.01514C0.0522461 1.53809 0.500488 1.96143 0.990234 1.96143Z";
        const string MaterialStepperPlusIcon = "M0.990234 7.95312H6.05371V13.0166C6.05371 13.5312 6.47705 13.9629 7 13.9629C7.52295 13.9629 7.94629 13.5312 7.94629 13.0166V7.95312H13.0098C13.5244 7.95312 13.9561 7.52979 13.9561 7.00684C13.9561 6.48389 13.5244 6.06055 13.0098 6.06055H7.94629V0.99707C7.94629 0.482422 7.52295 0.0507812 7 0.0507812C6.47705 0.0507812 6.05371 0.482422 6.05371 0.99707V6.06055H0.990234C0.475586 6.06055 0.0439453 6.48389 0.0439453 7.00684C0.0439453 7.52979 0.475586 7.95312 0.990234 7.95312Z";

        const float MaterialStepperHeight = 40.0f;
        const float MaterialStepperWidth = 110.0f;
        const float MaterialButtonMargin = 6.0f;

        RectF _minusRect;
        RectF _plusRect;
        bool _pressed;

        readonly RippleEffectDrawable _minusRippleEffect;
        readonly RippleEffectDrawable _plusRippleEffect;

        public StepperDrawable(IStepper view)
        {
            _minusRippleEffect = new RippleEffectDrawable
            {
                RippleColor = GColor.FromArgb(Material.Color.Gray6)
            };
            _plusRippleEffect = new RippleEffectDrawable
            {
                RippleColor = GColor.FromArgb(Material.Color.Gray6)
            };

            _minusRippleEffect.Invalidated += (s, e) => SendInvalidated();
            _plusRippleEffect.Invalidated += (s, e) => SendInvalidated();

            View = view;
        }

        IStepper View { get; }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawMaterialStepperMinus(canvas, dirtyRect);
            DrawMaterialStepperPlus(canvas, dirtyRect);
            _minusRippleEffect.Draw(canvas, dirtyRect);
            _plusRippleEffect.Draw(canvas, dirtyRect);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            return new TSize((MaterialStepperWidth + MaterialButtonMargin) * DeviceInfo.ScalingFactor, MaterialStepperHeight * DeviceInfo.ScalingFactor);
        }

        public override void OnTouchDown(GPoint point)
        {
            _minusRippleEffect.ClipRectangle = _minusRect;
            _plusRippleEffect.ClipRectangle = _plusRect;

            _plusRippleEffect.OnTouchDown(point);
            _minusRippleEffect.OnTouchDown(point);

            _pressed = true;
        }

        public override void OnTouchUp(GPoint point)
        {
            var touchDownPoint = new PointF((float)point.X, (float)point.Y);

            if (_minusRect.Contains(touchDownPoint) && _pressed)
                View.Value -= View.Increment;

            if (_plusRect.Contains(touchDownPoint) && _pressed)
                View.Value += View.Increment;

            _minusRippleEffect.OnTouchUp(point);
            _plusRippleEffect.OnTouchUp(point);
            _pressed = false;
        }

        public override void OnTouchMove(GPoint point)
        {
            _pressed = false;
        }


        void DrawMaterialStepperMinus(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            canvas.StrokeSize = 1;
            canvas.StrokeColor = GColor.FromArgb(Material.Color.Gray6);

            var x = dirtyRect.X+1;
            var y = dirtyRect.Y;

            var height = MaterialStepperHeight;
            var width = MaterialStepperWidth / 2;

            canvas.DrawRoundedRectangle(x, y, width, height, 6);

            if (!IsEnabled)
            {
                canvas.FillColor = GColor.FromArgb(Material.Color.Gray1);
                canvas.FillRoundedRectangle(x, y, width, height, 6);
            }

            canvas.Translate(20, 20);

            var vBuilder = new PathBuilder();
            var path = vBuilder.BuildPath(MaterialStepperMinusIcon);

            canvas.FillColor = GColor.FromArgb(Material.Color.Black);
            canvas.FillPath(path);

            canvas.RestoreState();

            _minusRect = new RectF(x, y, width, height);
        }

        void DrawMaterialStepperPlus(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            canvas.StrokeSize = 1;
            canvas.StrokeColor = GColor.FromArgb(Material.Color.Gray6);

            var x = MaterialStepperWidth / 2 + MaterialButtonMargin;
            var y = dirtyRect.Y;

            var height = MaterialStepperHeight;
            var width = MaterialStepperWidth / 2;

            canvas.DrawRoundedRectangle(x, y, width, height, 6);

            if (!IsEnabled)
            {
                canvas.FillColor = GColor.FromArgb(Material.Color.Gray1);
                canvas.FillRoundedRectangle(x, y, width, height, 6);
            }

            canvas.Translate(80, 14);

            var vBuilder = new PathBuilder();
            var path = vBuilder.BuildPath(MaterialStepperPlusIcon);

            canvas.FillColor = GColor.FromArgb(Material.Color.Black);
            canvas.FillPath(path);

            canvas.RestoreState();

            _plusRect = new RectF(x, y, width, height);
        }
    }
}
