using Microsoft.Maui.Graphics;
using Tizen.UIExtensions.Common.Internal;
using GColor = Microsoft.Maui.Graphics.Color;
using GPoint = Microsoft.Maui.Graphics.Point;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public class RippleEffectDrawable : GraphicsViewDrawable, IAnimatable
    {
        float _rippleEffectSize;

        public RippleEffectDrawable()
        {
            RippleColor = Colors.White;
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if ((ClipRectangle == RectF.Zero || ClipRectangle.Contains(TouchPoint)) && RippleEffectSize > 0)
            {
                canvas.SaveState();

                if (ClipRectangle == RectF.Zero)
                    ClipRectangle = dirtyRect;

                canvas.ClipRectangle(ClipRectangle);

                canvas.FillColor = RippleColor;
                canvas.Alpha = 0.25f;
                canvas.FillCircle((float)TouchPoint.X, (float)TouchPoint.Y, RippleEffectSize);

                canvas.RestoreState();
            }
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            return new TSize(availableWidth, availableHeight);
        }

        public GColor RippleColor { get; set; }

        public RectF ClipRectangle { get; set; }

        public PointF TouchPoint { get; set; }

        float RippleEffectSize
        {
            get { return _rippleEffectSize; }
            set
            {
                _rippleEffectSize = value;
                SendInvalidated();
            }
        }

        public override void OnTouchDown(GPoint point)
        {
            var touchDownPoint = new PointF((float)point.X, (float)point.Y);
            TouchPoint = touchDownPoint;
        }

        public override void OnTouchUp(GPoint point)
        {
            if (ClipRectangle == RectF.Zero || ClipRectangle.Contains(TouchPoint))
                AnimateDrawRipple();
        }

        void AnimateDrawRipple()
        {
            var from = 0;
            var to = ClipRectangle != RectF.Zero ? ClipRectangle.Width : 1000;

            var thumbSizeAnimation = new Animation(v => RippleEffectSize = (int)v, from, to, easing: Easing.SinInOut);
            thumbSizeAnimation.Commit(this, "RippleEffectAnimation", rate:32, length: 350, finished: (l, c) =>
            {
                _rippleEffectSize = 0;
                thumbSizeAnimation = null;
            });
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
