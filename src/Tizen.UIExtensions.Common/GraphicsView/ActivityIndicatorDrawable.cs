using System.Graphics;
using Tizen.UIExtensions.Common.Internal;
using GColor = System.Graphics.Color;
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

        float MaterialActivityIndicatorEndAngle { get; set; }


        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
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


            var startAngle = 90;
            var endAngle = 360;

            var rotateAnimation = new Animation(v =>
            {
                MaterialActivityIndicatorRotate = (int)v;
                SendInvalidated();
            }, 0, 360, easing: Easing.Linear);
            var startAngleAnimation = new Animation(v => MaterialActivityIndicatorStartAngle = (int)v, startAngle, startAngle - 360, easing: Easing.Linear);
            var endAngleAnimation = new Animation(v => MaterialActivityIndicatorEndAngle = (int)v, endAngle, endAngle - 360, easing: Easing.Linear);

            materialActivityIndicatorAngleAnimation.Add(0, 1, rotateAnimation);
            materialActivityIndicatorAngleAnimation.Add(0, 1, startAngleAnimation);
            materialActivityIndicatorAngleAnimation.Add(0, 1, endAngleAnimation);

            materialActivityIndicatorAngleAnimation.Commit(this, "MaterialActivityIndicator", length: 1400, repeat: () => true, finished: (l, c) => materialActivityIndicatorAngleAnimation = null);
        }

        void DrawMaterialActivityIndicator(ICanvas canvas, RectangleF dirtyRect)
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
                canvas.DrawArc(x + strokeWidth, y + strokeWidth, size, size, MaterialActivityIndicatorStartAngle, MaterialActivityIndicatorEndAngle, false, false);
            }
            else
            {
                canvas.Rotate(0, x + strokeWidth + size / 2, y + strokeWidth + size / 2);
                if (View.Color.IsDefault)
                {
                    canvas.StrokeColor = new GColor(Material.Color.LightBlue);
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
            return new TSize(45 * DeviceInfo.ScalingFactor, 45 * DeviceInfo.ScalingFactor);
        }
    }
}
