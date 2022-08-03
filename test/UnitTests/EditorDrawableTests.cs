using Moq;
using Tizen.UIExtensions.Common.GraphicsView;
using ICanvas = Microsoft.Maui.Graphics.ICanvas;
using RectF = Microsoft.Maui.Graphics.RectF;

namespace UnitTests
{
    public class EditorDrawableTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void TestDraw(bool isFocused, bool isTextNullOrEmpty)
        {
            var view = new Mock<IEditor>();
            view.Setup(x => x.IsFocused).Returns(isFocused);
            view.Setup(x => x.Text).Returns(isTextNullOrEmpty ? "" : "Text");
            Assert.Equal(isFocused, view.Object.IsFocused);

            var canvas = new Mock<ICanvas>();
            var drawable = new EditorDrawable(view.Object);
            var exception = Record.Exception(() =>
            {
                drawable.Draw(canvas.Object, new RectF(0, 0, 100, 100));
            });
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(100, 100)]
        public void TestMeasure(double availableWidth, double availableHeight)
        {
            var view = new Mock<IEditor>();
            var drawable = new EditorDrawable(view.Object);
            var size = drawable.Measure(availableWidth, availableHeight);
            Assert.Equal(new Size(availableWidth, DeviceInfo.ScalingFactor), size);
        }

        [Fact]
        public void TestOnFocusedUnfocused()
        {
            var view = new Mock<IEditor>();
            var drawable = new EditorDrawable(view.Object);
            var exception = Record.Exception(() =>
            {
                drawable.OnFocused();
                drawable.OnUnfocused();
            });
            Assert.Null(exception);
        }
    }
}