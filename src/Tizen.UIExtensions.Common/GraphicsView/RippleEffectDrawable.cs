using System.Graphics;
using Tizen.UIExtensions.Common.Internal;
using GColor = System.Graphics.Color;
using GPoint = System.Graphics.Point;
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

        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            if (ClipRectangle == RectangleF.Zero || ClipRectangle.Contains(TouchPoint))
            {
                canvas.SaveState();

                if (ClipRectangle == RectangleF.Zero)
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

        public RectangleF ClipRectangle { get; set; }

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
            if (ClipRectangle == RectangleF.Zero || ClipRectangle.Contains(TouchPoint))
                AnimateDrawRipple();
        }

        void AnimateDrawRipple()
        {
            var from = 0;
            var to = ClipRectangle != RectangleF.Zero ? ClipRectangle.Width : 1000;

            var thumbSizeAnimation = new Animation(v => RippleEffectSize = (int)v, from, to, easing: Easing.SinInOut);
            thumbSizeAnimation.Commit(this, "RippleEffectAnimation", length: 350, finished: (l, c) =>
            {
                _rippleEffectSize = 0;
                thumbSizeAnimation = null;
            });
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
