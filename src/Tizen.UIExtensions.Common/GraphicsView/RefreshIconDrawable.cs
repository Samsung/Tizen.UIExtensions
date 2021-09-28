using Microsoft.Maui.Graphics;
using Tizen.UIExtensions.Common.Internal;
using GColor = Microsoft.Maui.Graphics.Color;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public class RefreshIconDrawable : GraphicsViewDrawable, IAnimatable
    {
        public const float IconSize = 40f;
        public const float StrokeWidth = 4f;
        public const int RotationAngle = 360;

        public RefreshIconDrawable(IRefreshIcon view)
        {
            View = view;
        }

        IRefreshIcon View { get; }

        float MaterialRefreshViewIconRotate { get; set; }

        float MaterialRefreshViewIconStartAngle { get; set; }

        float MaterialRefreshViewIconEndAngle { get; set; }

        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            DrawRefreshIcon(canvas, dirtyRect);
        }

        public void UpdateRunningAnimation(bool animate)
        {
            if (animate)
            {
                StartRunningAnimation();
                return;
            }
            AbortRunningAnimation();
        }

        void StartRunningAnimation()
        {
            var startAngle = 90;
            var endAngle = 360;
            uint animationLength = 1400;

            var materialRefreshViewIconAngleAnimation = new Animation();
            var rotateAnimation = new Animation(v =>
            {
                MaterialRefreshViewIconRotate = (int)v;
                SendInvalidated();
            }, 0, RotationAngle, easing: Easing.Linear);
            var startAngleAnimation = new Animation(v => MaterialRefreshViewIconStartAngle = (int)v, startAngle, startAngle - RotationAngle, easing: Easing.Linear);
            var endAngleAnimation = new Animation(v => MaterialRefreshViewIconEndAngle = (int)v, endAngle, endAngle - RotationAngle, easing: Easing.Linear);

            materialRefreshViewIconAngleAnimation.Add(0, 1, rotateAnimation);
            materialRefreshViewIconAngleAnimation.Add(0, 1, startAngleAnimation);
            materialRefreshViewIconAngleAnimation.Add(0, 1, endAngleAnimation);

            materialRefreshViewIconAngleAnimation.Commit(this, "MaterialRefreshIcon", length: animationLength, repeat: () => true, finished: (l, c) => materialRefreshViewIconAngleAnimation = null);
        }

        void AbortRunningAnimation()
        {
            this.AbortAnimation("MaterialRefreshIcon");
            SendInvalidated();
        }

        void DrawRefreshIcon(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();
            canvas.StrokeSize = StrokeWidth;

            var x = dirtyRect.X + StrokeWidth;
            var y = dirtyRect.Y + StrokeWidth;
            if (View.IsRunning)
            {
                DrawRunningIcon(canvas, x, y);
            }
            else
            {
                DrawIdleIcon(canvas, x, y);
            }
            canvas.RestoreState();
        }

        void DrawRunningIcon(ICanvas canvas, float x, float y)
        {
            canvas.Rotate(MaterialRefreshViewIconRotate, x + IconSize / 2, y + IconSize / 2);
            canvas.StrokeColor = View.Color.ToGraphicsColor(Material.Color.Blue);
            canvas.DrawArc(x, y, IconSize, IconSize, MaterialRefreshViewIconStartAngle, MaterialRefreshViewIconEndAngle, false, false);
        }

        void DrawIdleIcon(ICanvas canvas, float x, float y)
        {
            canvas.Rotate(0, x + IconSize / 2, y + IconSize / 2);
            canvas.StrokeColor = View.Color.IsDefault ? GColor.FromArgb(Material.Color.LightBlue) : View.Color.MultiplyAlpha(0.5).ToGraphicsColor(Material.Color.LightBlue);
            canvas.DrawArc(x, y, IconSize, IconSize, 0, RotationAngle, false, false);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            var iconSize = (IconSize + (StrokeWidth * 2)) * DeviceInfo.ScalingFactor;
            return new TSize(iconSize, iconSize);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.AbortAnimation("MaterialRefreshIcon");
            }
            base.Dispose(disposing);
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
