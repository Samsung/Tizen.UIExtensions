using System.Graphics;
using Tizen.UIExtensions.NUI.GraphicsView;
using Tizen.UIExtensions.NUI.Internal;
using GPoint = System.Graphics.Point;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.NUI
{
    public class SliderDrawable : GraphicsViewDrawable, IAnimatable
    {
        const float NormalMaterialThumbSize = 12f;
        const float SelectedMaterialThumbSize = 18f;

        bool _isThumbSelected;

        public SliderDrawable(ISlider view)
        {
            View = view;
        }

        ISlider View { get; }

        float MaterialFloatThumb { get; set; } = NormalMaterialThumbSize;

        public RectangleF ThumbRect { get; set; }
        public RectangleF TrackRect { get; set; }

        public double ValueRate
        {
            get
            {
                return View.Value / View.Maximum;
            }
            set
            {
                double start = View.Minimum;
                double diff = View.Maximum - View.Minimum;
                View.Value = start + diff * value;
            }
        }

        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            DrawMaterialSliderTrackBackground(canvas, dirtyRect);
            DrawMaterialSliderTrackProgress(canvas, dirtyRect);
            DrawMaterialSliderThumb(canvas, dirtyRect);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            return new TSize(availableWidth, DeviceInfo.ScalingFactor * SelectedMaterialThumbSize);
        }

        public override void OnTouchDown(GPoint point)
        {
            _isThumbSelected = ThumbRect.Contains(new PointF((float)point.X, (float)point.Y));

            if (_isThumbSelected)
                AnimateMaterialThumbSize(true);
            UpdateValue(point);
        }

        public override void OnTouchUp(GPoint point)
        {
            if (_isThumbSelected)
            {
                _isThumbSelected = false;
                AnimateMaterialThumbSize(false);
            }
        }

        public override void OnTouchMove(GPoint point)
        {
            UpdateValue(point);
        }


        void UpdateValue(GPoint point)
        {
            ValueRate = (point.X - TrackRect.X) / TrackRect.Width;
            SendInvalidated();
        }

        void DrawMaterialSliderTrackBackground(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FillColor = View.MaximumTrackColor.ToGraphicsColor(Material.Color.LightBlue);

            var x = dirtyRect.X;

            var width = dirtyRect.Width;
            var height = 2;

            var y = (float)((dirtyRect.Height - height) / 2);

            canvas.FillRoundedRectangle(x, y, width, height, 0);

            canvas.RestoreState();

            TrackRect = new RectangleF(x, y, width, height);
        }

        protected virtual void DrawMaterialSliderTrackProgress(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FillColor = View.MinimumTrackColor.ToGraphicsColor(Material.Color.Blue);

            var x = dirtyRect.X;

            var value = ((double)ValueRate).Clamp(0, 1);
            var width = (float)(dirtyRect.Width * value);

            var height = 2;

            var y = (float)((dirtyRect.Height - height) / 2);

            canvas.FillRoundedRectangle(x, y, width, height, 0);

            canvas.RestoreState();
        }

        protected virtual void DrawMaterialSliderThumb(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            var value = ((double)ValueRate).Clamp(0, 1);
            var x = (float)((dirtyRect.Width * value) - (MaterialFloatThumb / 2));

            if (x <= 0)
                x = 0;

            if (x >= dirtyRect.Width - MaterialFloatThumb)
                x = dirtyRect.Width - MaterialFloatThumb;

            var y = (float)((dirtyRect.Height - MaterialFloatThumb) / 2);

            canvas.FillColor = View.ThumbColor.ToGraphicsColor(Material.Color.Blue);

            canvas.FillEllipse(x, y, MaterialFloatThumb, MaterialFloatThumb);

            canvas.RestoreState();

            ThumbRect = new RectangleF(x, y, MaterialFloatThumb, MaterialFloatThumb);
        }

        public void AnimateMaterialThumbSize(bool increase)
        {
            float start = increase ? NormalMaterialThumbSize : SelectedMaterialThumbSize;
            float end = increase ? SelectedMaterialThumbSize : NormalMaterialThumbSize;

            var thumbSizeAnimation = new Animation(v =>
            {
                MaterialFloatThumb = (int)v;
                SendInvalidated();
            }, start, end, easing: Easing.SinInOut);
            thumbSizeAnimation.Commit(this, "ThumbSizeAnimation", length: 50, finished: (l, c) => thumbSizeAnimation = null);
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
