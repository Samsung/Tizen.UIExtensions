using Microsoft.Maui.Graphics;
using System;
using Tizen.UIExtensions.Common.Internal;
using GColor = Microsoft.Maui.Graphics.Color;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public class ActivityIndicatorDrawable : GraphicsViewDrawable, IAnimatable
    {
        public ActivityIndicatorDrawable(IActivityIndicator view)
        {
            View = view;
        }

        IActivityIndicator View { get; }

        float MaterialActivityIndicatorRotate { get; set; }

        float MaterialActivityIndicatorStartAngle { get; set; }

        float MaterialActivityIndicatorSweepAngle { get; set; }
        float MaterialActivityIndicatorLastStartAngle { get; set; }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawMaterialActivityIndicator(canvas, dirtyRect);
        }

        public void UpdateAnimation(bool animate)
        {
            if (!animate)
            {
                this.AbortAnimation("MaterialActivityIndicator");
                SendInvalidated();
                return;
            }

            var materialActivityIndicatorAngleAnimation = new Animation();

            MaterialActivityIndicatorStartAngle = 0;
            MaterialActivityIndicatorLastStartAngle  = 0;

            var rotateAnimation = new Animation(v =>
            {
                MaterialActivityIndicatorRotate = (int)v;
                SendInvalidated();
            }, 0, 360*3, easing: Easing.Linear);
            var sweepAnimationUp = new Animation(v =>
            {
                MaterialActivityIndicatorSweepAngle = 30 + (int)v;
                MaterialActivityIndicatorLastStartAngle  = MaterialActivityIndicatorSweepAngle;
            }, 0, 270, easing: Easing.Linear);
            var sweepAnimationDown = new Animation(v =>
            {
                MaterialActivityIndicatorSweepAngle = 30 + (int)v;
                MaterialActivityIndicatorStartAngle += (Math.Abs(MaterialActivityIndicatorLastStartAngle  - MaterialActivityIndicatorSweepAngle));
                MaterialActivityIndicatorLastStartAngle  = MaterialActivityIndicatorSweepAngle;
            }, 270, 0, easing: Easing.Linear);

            materialActivityIndicatorAngleAnimation.Add(0, 1, rotateAnimation);
            materialActivityIndicatorAngleAnimation.Add(0, 0.5, sweepAnimationUp);
            materialActivityIndicatorAngleAnimation.Add(0.5, 1, sweepAnimationDown);

            materialActivityIndicatorAngleAnimation.Commit(this, "MaterialActivityIndicator", length: 1400, repeat: () => true, finished: (l, c) => materialActivityIndicatorAngleAnimation = null);
        }

        void DrawMaterialActivityIndicator(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            float size = 40f;
            float strokeWidth = 4f;

            canvas.StrokeSize = strokeWidth;

            var x = dirtyRect.X;
            var y = dirtyRect.Y;

            if (View.IsRunning)
            {
                canvas.Rotate(MaterialActivityIndicatorRotate, x + strokeWidth + size / 2, y + strokeWidth + size / 2);
                canvas.StrokeColor = View.Color.ToGraphicsColor(Material.Color.Blue);
                canvas.DrawArc(x + strokeWidth, y + strokeWidth, size, size, MaterialActivityIndicatorStartAngle, MaterialActivityIndicatorStartAngle + MaterialActivityIndicatorSweepAngle, false, false);
            }
            else
            {
                canvas.Rotate(0, x + strokeWidth + size / 2, y + strokeWidth + size / 2);
                if (View.Color.IsDefault)
                {
                    canvas.StrokeColor = GColor.FromArgb(Material.Color.LightBlue);
                }
                else
                {
                    canvas.StrokeColor = View.Color.MultiplyAlpha(0.5).ToGraphicsColor(Material.Color.LightBlue);
                }
                canvas.DrawArc(x + strokeWidth, y + strokeWidth, size, size, 0, 360, false, false);
            }

            canvas.RestoreState();
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            return new TSize(46 * DeviceInfo.ScalingFactor, 46 * DeviceInfo.ScalingFactor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.AbortAnimation("MaterialActivityIndicator");
            }
            base.Dispose(disposing);
        }
    }
}
