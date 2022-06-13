using System;
using Microsoft.Maui.Graphics;
using Tizen.UIExtensions.Common.Internal;
using GColor = Microsoft.Maui.Graphics.Color;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    /// <summary>
    /// A Drawable class that is used to draw a refresh icon.
    /// This drawable draws a round circle stroke that can run in circle.
    /// </summary>
    public class RefreshIconDrawable : GraphicsViewDrawable, IAnimatable
    {
        const float Margin = 10;
        const float IconSize = 40;
        const float ArcSize = 20;
        const float StrokeWidth = 3f;

        /// <summary>
        /// Initializes a new instance of the RefreshIconDrawable.
        /// </summary>
        public RefreshIconDrawable(IRefreshIcon view)
        {
            View = view;
        }

        IRefreshIcon View { get; }

        GColor ForegroundColor => View.Color.ToGraphicsColor(Material.Color.Blue);
        GColor BackgroundColor => View.BackgroundColor.ToGraphicsColor(Material.Color.White);

        float MaterialRefreshIconRunningRotate { get; set; }

        float MaterialRefreshIconRunningStartAngle { get; set; }

        float MaterialRefreshIconRunningSweepAngle { get; set; }

        float MaterialRefreshIconRunningLastStartAngle { get; set; }

        Animation? MaterialRefreshIconRunningAnimation { get; set; }


        /// <summary>
        /// Implementation of the IDrawable.Draw() method.
        /// This method defines how to draw a refresh icon.
        /// </summary>
        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawRefreshIcon(canvas, dirtyRect);
        }

        void UpdateAnimation(bool animate)
        {
            if (!animate)
            {
                this.AbortAnimation("MaterialRefreshIcon");
                SendInvalidated();
                MaterialRefreshIconRunningAnimation = null;
                return;
            }

            MaterialRefreshIconRunningAnimation = new Animation();

            MaterialRefreshIconRunningStartAngle = 0;
            MaterialRefreshIconRunningLastStartAngle = 0;

            var rotateAnimation = new Animation(v =>
            {
                MaterialRefreshIconRunningRotate = (int)v;
                SendInvalidated();
            }, 0, 360 * 3, easing: Easing.Linear);
            var sweepAnimationUp = new Animation(v =>
            {
                MaterialRefreshIconRunningSweepAngle = 30 + (int)v;
                MaterialRefreshIconRunningLastStartAngle = MaterialRefreshIconRunningSweepAngle;
            }, 0, 270, easing: Easing.Linear);
            var sweepAnimationDown = new Animation(v =>
            {
                MaterialRefreshIconRunningSweepAngle = 30 + (int)v;
                MaterialRefreshIconRunningStartAngle += (Math.Abs(MaterialRefreshIconRunningLastStartAngle - MaterialRefreshIconRunningSweepAngle));
                MaterialRefreshIconRunningLastStartAngle = MaterialRefreshIconRunningSweepAngle;
            }, 270, 0, easing: Easing.Linear);

            MaterialRefreshIconRunningAnimation.Add(0, 1, rotateAnimation);
            MaterialRefreshIconRunningAnimation.Add(0, 0.5, sweepAnimationUp);
            MaterialRefreshIconRunningAnimation.Add(0.5, 1, sweepAnimationDown);

            MaterialRefreshIconRunningAnimation.Commit(this, "MaterialRefreshIcon", length: 1400, repeat: () => true);
        }


        void DrawRefreshIcon(ICanvas canvas, RectF dirtyRect)
        {
            DrawBackground(canvas, dirtyRect);
            if (View.IsRunning)
            {
                if (MaterialRefreshIconRunningAnimation == null)
                {
                    UpdateAnimation(true);
                }
                DrawRunningProgress(canvas, dirtyRect);
            }
            else
            {
                if (MaterialRefreshIconRunningAnimation != null)
                {
                    UpdateAnimation(false);
                }

                DrawProgress(canvas, dirtyRect);
                DrawArrowhead(canvas, dirtyRect);
            }
        }

        void DrawRunningProgress(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            var x = dirtyRect.Center.X - ArcSize / 2f;
            var y = dirtyRect.Center.Y - ArcSize / 2f;
            var width = ArcSize;
            var height = ArcSize;

            canvas.Rotate(MaterialRefreshIconRunningRotate, dirtyRect.Center.X, dirtyRect.Center.Y);
            canvas.StrokeColor = ForegroundColor;
            canvas.StrokeSize = StrokeWidth;
            canvas.DrawArc(x, y, width, height, MaterialRefreshIconRunningStartAngle, MaterialRefreshIconRunningStartAngle + MaterialRefreshIconRunningSweepAngle, false, false);

            canvas.RestoreState();
        }

        void DrawArrowhead(ICanvas canvas, RectF dirtyRect)
        {
            if (View.PullDistance < 0.3)
            {
                return;
            }

            float arrowheadScale = Math.Min(1f, View.PullDistance * 1.2f);

            canvas.SaveState();

            // translate center
            canvas.Translate(dirtyRect.Width / 2f, dirtyRect.Height / 2f);

            var startArc = 45 - 80 * Math.Min(1, View.PullDistance * 2f);

            var sweepArc = -startArc + View.PullDistance * 300;

            var radius = ArcSize / 2f;
            var arc = (float)((sweepArc) * Math.PI / 180f);
            var ux = (float)Math.Cos(arc);
            var uy = (float)Math.Sin(arc);


            float arrowheadRadius = StrokeWidth * 2.0f * arrowheadScale;
            float innerRadius = radius - arrowheadRadius;
            float outerRadius = radius + arrowheadRadius;

            float arrowheadPointX = ux * radius + -uy * StrokeWidth * 1.5f * arrowheadScale;
            float arrowheadPointY = uy * radius + ux * StrokeWidth * 1.5f * arrowheadScale;

            canvas.FillColor = ForegroundColor;

            using PathF path = new PathF();
            path.MoveTo(ux * innerRadius, uy * innerRadius);
            path.LineTo(ux * outerRadius, uy * outerRadius);
            path.LineTo(arrowheadPointX, arrowheadPointY);
            path.Close();
            canvas.FillPath(path);

            canvas.RestoreState();

        }

        void DrawProgress(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            canvas.StrokeColor = ForegroundColor;
            canvas.StrokeSize = StrokeWidth;

            var sweepArc = 360 - View.PullDistance * 300;

            var startArc = 45 - 80 * Math.Min(1, View.PullDistance * 2f);

            var x = dirtyRect.Center.X - ArcSize / 2f;
            var y = dirtyRect.Center.Y - ArcSize / 2f;
            var width = ArcSize;
            var height = ArcSize;

            canvas.DrawArc(x, y, width, height, startArc, startArc + sweepArc, true, false);

            canvas.RestoreState();
        }

        void DrawBackground(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            canvas.Antialias = true;
            canvas.FillColor = BackgroundColor;
            canvas.SetShadow(new SizeF(0, 0), 2, GColor.FromArgb(Material.Color.Gray6));
            canvas.FillCircle(dirtyRect.Center, IconSize / 2f);
            canvas.RestoreState();
        }

        /// <summary>
        /// Implementation of the IMeasurable.Measure() method. It returns the size of a drawn icon.
        /// </summary>
        public override TSize Measure(double availableWidth, double availableHeight)
        {
            var iconSize = (IconSize + Margin) * DeviceInfo.ScalingFactor;
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
