using System.Graphics;
using Tizen.UIExtensions.Common.Internal;
using GPoint = System.Graphics.Point;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public class SwitchDrawable : GraphicsViewDrawable, IAnimatable
    {
        const float MaterialThumbOffPosition = 12f;
        const float MaterialThumbOnPosition = 34f;
        const float MaterialSwitchBackgroundWidth = 34;
        const float MaterialSwitchBackgroundMargin = 5;

        public SwitchDrawable(ISwitch view)
        {
            View = view;
        }

        ISwitch View { get; }

        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            DrawMaterialSwitchBackground(canvas, dirtyRect);
            DrawMaterialSwitchThumb(canvas, dirtyRect);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            return new TSize((MaterialSwitchBackgroundWidth + 10) * DeviceInfo.ScalingFactor, 24 * DeviceInfo.ScalingFactor);
        }


        public override void OnTouchDown(GPoint point)
        {
            View.IsToggled = !View.IsToggled;
            AnimateMaterialSwitchThumb(View.IsToggled);
        }

        float _materialSwitchThumbPosition = MaterialThumbOffPosition;

        float MaterialSwitchThumbPosition
        {
            get { return _materialSwitchThumbPosition; }
            set
            {
                _materialSwitchThumbPosition = value;
                SendInvalidated();
            }
        }

        void DrawMaterialSwitchBackground(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            if (View.IsToggled)
            {
                canvas.FillColor = View.OnColor.ToGraphicsColor(Material.Color.LightBlue);
                canvas.Alpha = 0.5f;
            }
            else
            {
                canvas.FillColor = View.BackgroundColor.ToGraphicsColor(Material.Color.Gray2);
                canvas.Alpha = 1.0f;
            }

            var margin = MaterialSwitchBackgroundMargin;

            var x = dirtyRect.X + margin;
            var y = dirtyRect.Y + margin;

            var height = 14;
            var width = MaterialSwitchBackgroundWidth;

            canvas.FillRoundedRectangle(x, y, width, height, 10);

            canvas.RestoreState();
        }

        void DrawMaterialSwitchThumb(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            if (View.IsToggled)
                canvas.FillColor = View.ThumbColor.ToGraphicsColor(Material.Color.Blue);
            else
                canvas.FillColor = View.ThumbColor.ToGraphicsColor(Fluent.Color.Foreground.White);

            var margin = 2;
            var radius = 10;

            var y = dirtyRect.Y + margin + radius;

            canvas.SetShadow(new SizeF(0, 1), 2, CanvasDefaults.DefaultShadowColor);
            canvas.FillCircle(MaterialSwitchThumbPosition, y, radius);

            canvas.RestoreState();
        }

        void AnimateMaterialSwitchThumb(bool on)
        {
            float start = on ? MaterialThumbOffPosition : MaterialThumbOnPosition;
            float end = on ? MaterialThumbOnPosition : MaterialThumbOffPosition;

            var thumbPositionAnimation = new Animation(v => MaterialSwitchThumbPosition = (int)v, start, end, easing: Easing.Linear);
            thumbPositionAnimation.Commit(this, "MaterialSwitchThumbAnimation", length: 100, finished: (l, c) => thumbPositionAnimation = null);
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
