using System.Graphics;
using Tizen.UIExtensions.Common.Internal;
using GColor = System.Graphics.Color;
using GPoint = System.Graphics.Point;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public class EntryDrawable : GraphicsViewDrawable, IAnimatable
    {
        const string MaterialEntryIndicatorIcon = "M9.295 7.885C9.68436 8.27436 9.68436 8.90564 9.295 9.295C8.90564 9.68436 8.27436 9.68436 7.885 9.295L5 6.41L2.115 9.295C1.72564 9.68436 1.09436 9.68436 0.705 9.295C0.315639 8.90564 0.315639 8.27436 0.705 7.885L3.59 5L0.705 2.115C0.315639 1.72564 0.31564 1.09436 0.705 0.705C1.09436 0.315639 1.72564 0.315639 2.115 0.705L5 3.59L7.885 0.705C8.27436 0.315639 8.90564 0.31564 9.295 0.705C9.68436 1.09436 9.68436 1.72564 9.295 2.115L6.41 5L9.295 7.885Z";

        const float FocusedMaterialPlaceholderFontSize = 12f;
        const float UnfocusedMaterialPlaceholderFontSize = 16f;

        const float FocusedMaterialPlaceholderPosition = 6f;
        const float UnfocusedMaterialPlaceholderPosition = 22f;

        float _placeholderY;
        float _placeholderFontSize;

        RectangleF _indicatorRect;

        public EntryDrawable(IEntry view)
        {
            View = view;

            AnimateMaterialPlaceholder(false);
        }

        IEntry View { get; }


        float PlaceholderY
        {
            get { return _placeholderY; }
            set
            {
                _placeholderY = value;
                SendInvalidated();
            }
        }

        float PlaceholderFontSize
        {
            get { return _placeholderFontSize; }
            set
            {
                _placeholderFontSize = value;
                SendInvalidated();
            }
        }

        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            DrawMaterialEntryBackground(canvas, dirtyRect);
            DrawMaterialEntryBorder(canvas, dirtyRect);
            DrawMaterialEntryPlaceholder(canvas, dirtyRect);
            DrawMaterialEntryIndicators(canvas, dirtyRect);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            return new TSize(availableWidth, 56 * DeviceInfo.ScalingFactor);
        }

        public override void OnFocused()
        {
            AnimateMaterialPlaceholder(true);
        }

        public override void OnUnfocused()
        {
            AnimateMaterialPlaceholder(false);
        }

        public override void OnTouchDown(GPoint point)
        {
            PointF touchPoint = new PointF((float)point.X, (float)point.Y);

            if (_indicatorRect.Contains(touchPoint))
                View.Text = string.Empty;
        }

        void DrawMaterialEntryBackground(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FillColor = View.BackgroundColor.ToGraphicsColor(Material.Color.Gray5);

            var width = dirtyRect.Width;

            var vBuilder = new PathBuilder();
            var path =
                vBuilder.BuildPath(
                    $"M0 4C0 1.79086 1.79086 0 4 0H{width - 4}C{width - 2}.209 0 {width} 1.79086 {width} 4V56H0V4Z");

            canvas.FillPath(path);

            canvas.RestoreState();
        }

        void DrawMaterialEntryBorder(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            var strokeWidth = 1.0f;
            canvas.FillColor = new GColor(Material.Color.Black);

            if (View.IsFocused)
            {
                strokeWidth = 2.0f;
                canvas.FillColor = new GColor(Material.Color.Blue);
            }

            var x = dirtyRect.X;
            var y = 53.91f;

            var width = dirtyRect.Width;
            var height = strokeWidth;

            canvas.FillRectangle(x, y, width, height);

            canvas.RestoreState();
        }

        void DrawMaterialEntryPlaceholder(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FontColor = View.PlaceholderColor.ToGraphicsColor(Material.Color.Dark);
            canvas.FontSize = (float)(PlaceholderFontSize * DeviceInfo.ScalingFactor);

            float margin = 12f;

            var horizontalAlignment = HorizontalAlignment.Left;

            var x = dirtyRect.X + margin;

            //if (FlowDirection == FlowDirection.RightToLeft)
            //{
            //    x = dirtyRect.X;
            //    horizontalAlignment = HorizontalAlignment.Right;
            //}

            var height = dirtyRect.Height;
            var width = dirtyRect.Width;

            canvas.DrawString(View.Placeholder, x, PlaceholderY, width - margin, height, horizontalAlignment, VerticalAlignment.Top);

            canvas.RestoreState();
        }

        void DrawMaterialEntryIndicators(ICanvas canvas, RectangleF dirtyRect)
        {
            if (!string.IsNullOrEmpty(View.Text))
            {
                canvas.SaveState();

                float radius = 12f;

                var backgroundMarginX = 24;
                var backgroundMarginY = 28;

                var x = dirtyRect.Width - backgroundMarginX;
                var y = dirtyRect.Y + backgroundMarginY;

                //if (FlowDirection == FlowDirection.RightToLeft)
                //    x = backgroundMarginX;

                canvas.FillColor = View.BackgroundColor.ToGraphicsColor(Material.Color.Black);
                canvas.Alpha = 0.12f;

                canvas.FillCircle(x, y, radius);

                canvas.RestoreState();

                _indicatorRect = new RectangleF(x - radius, y - radius, radius * 2, radius * 2);

                canvas.SaveState();

                var iconMarginX = 29;
                var iconMarginY = 23;

                var tX = dirtyRect.Width - iconMarginX;
                var tY = dirtyRect.Y + iconMarginY;

                //if (FlowDirection == FlowDirection.RightToLeft)
                //{
                //    iconMarginX = 19;
                //    tX = iconMarginX;
                //}

                canvas.Translate((float)(tX * DeviceInfo.ScalingFactor), (float)(tY * DeviceInfo.ScalingFactor));

                var vBuilder = new PathBuilder();
                var path = vBuilder.BuildPath(MaterialEntryIndicatorIcon);

                canvas.FillColor = View.BackgroundColor.ToGraphicsColor(Material.Color.Black);
                canvas.FillPath(path);

                canvas.RestoreState();
            }
        }

        void AnimateMaterialPlaceholder(bool isFocused)
        {
            if (View.IsFocused && !string.IsNullOrEmpty(View.Text))
                return;

            var materialPlaceholderAnimation = new Animation();

            float startFontSize = isFocused ? UnfocusedMaterialPlaceholderFontSize : FocusedMaterialPlaceholderFontSize;
            float endFontSize = (isFocused || !string.IsNullOrEmpty(View.Text)) ? FocusedMaterialPlaceholderFontSize : UnfocusedMaterialPlaceholderFontSize;
            var fontSizeAnimation = new Animation(v => PlaceholderFontSize = (int)v, startFontSize, endFontSize, easing: Easing.Linear);

            float startPosition = isFocused ? UnfocusedMaterialPlaceholderPosition : FocusedMaterialPlaceholderPosition;
            float endPosition = (isFocused || !string.IsNullOrEmpty(View.Text)) ? FocusedMaterialPlaceholderPosition : UnfocusedMaterialPlaceholderPosition;
            var placeholderPositionAnimation = new Animation(v => PlaceholderY = (int)v, startPosition, endPosition, easing: Easing.Linear);

            materialPlaceholderAnimation.Add(0, 1, fontSizeAnimation);
            materialPlaceholderAnimation.Add(0, 1, placeholderPositionAnimation);

            materialPlaceholderAnimation.Commit(this, "MaterialPlaceholderAnimation", length: 100, finished: (l, c) => materialPlaceholderAnimation = null);
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
