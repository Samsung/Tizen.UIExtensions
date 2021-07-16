using Microsoft.Maui.Graphics;
using Tizen.UIExtensions.Common.Internal;
using GColor = Microsoft.Maui.Graphics.Color;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public class EditorDrawable : GraphicsViewDrawable, IAnimatable
    {
        const float FocusedMaterialPlaceholderFontSize = 12f;
        const float UnfocusedMaterialPlaceholderFontSize = 16f;

        const float FocusedMaterialPlaceholderPosition = 6f;
        const float UnfocusedMaterialPlaceholderPosition = 22f;

        float _placeholderY;
        float _placeholderFontSize;


        public EditorDrawable(IEditor view)
        {
            View = view;
            AnimateMaterialPlaceholder(false);
        }

        IEditor View { get; }

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
            DrawMaterialEditorBackground(canvas, dirtyRect);
            DrawMaterialEditorBorder(canvas, dirtyRect);
            DrawMaterialEditorPlaceholder(canvas, dirtyRect);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            return new TSize(availableWidth, 114.95 * DeviceInfo.ScalingFactor);
        }

        public override void OnFocused()
        {
            AnimateMaterialPlaceholder(true);
        }

        public override void OnUnfocused()
        {
            AnimateMaterialPlaceholder(false);
        }

        void DrawMaterialEditorBackground(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FillColor = View.BackgroundColor.ToGraphicsColor(Material.Color.Gray5);

            //var width = dirtyRect.Width;

            //var vBuilder = new PathBuilder();
            //var path =
            //    vBuilder.BuildPath(
            //        $"M0 4C0 1.79086 1.79086 0 4 0H{width - 4}C{width - 2}.209 0 {width} 1.79086 {width} 4V114.95H0V4Z");

            //canvas.FillPath(path);

            canvas.FillRectangle(dirtyRect);

            canvas.RestoreState();
        }

        void DrawMaterialEditorBorder(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            var strokeWidth = 1.0f;
            canvas.FillColor = GColor.FromArgb(Material.Color.Black);

            if (View.IsFocused)
            {
                strokeWidth = 2.0f;
                canvas.FillColor = GColor.FromArgb(Material.Color.Blue);
            }

            var x = dirtyRect.X;

            //var y = 112.91f;
            var y = dirtyRect.Y + dirtyRect.Height - strokeWidth * 2;


            var width = dirtyRect.Width;
            var height = strokeWidth;

            canvas.FillRectangle(x, y, width, height);

            canvas.RestoreState();
        }

        void DrawMaterialEditorPlaceholder(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            canvas.FontColor = View.PlaceholderColor.ToGraphicsColor(Material.Color.Dark);
            canvas.FontSize = PlaceholderFontSize;

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
