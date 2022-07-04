using Moq;
using Tizen.UIExtensions.Common.GraphicsView;
using ICanvas = Microsoft.Maui.Graphics.ICanvas;
using RectF = Microsoft.Maui.Graphics.RectF;
using GPoint = Microsoft.Maui.Graphics.Point;

namespace UnitTests
{
    public class CheckBoxDrawableTests
    {
        [Theory]
        [InlineData(true, "Text")]
        [InlineData(true, null)]
        [InlineData(false, "Text")]
        [InlineData(false, null)]
        public void TestDraw(bool isEnabled, string text)
        {
            var view = new Mock<ICheckBox>();
            view.Setup(x => x.Text).Returns(text);
            var canvas = new Mock<ICanvas>();
            var drawable = new CheckBoxDrawable(view.Object);
            drawable.IsEnabled = isEnabled;
            Assert.Equal(isEnabled, drawable.IsEnabled);

            var exception = Record.Exception(() =>
            {
                drawable.Draw(canvas.Object, new RectF(0, 0, 100, 100));
            });
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestDraw2(bool isChecked)
        {
            var view = new Mock<ICheckBox>();
            view.Setup(x => x.IsChecked).Returns(isChecked);
            var canvas = new Mock<ICanvas>();
            var drawable = new CheckBoxDrawable(view.Object);
            Assert.Equal(isChecked, view.Object.IsChecked);

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
            var view = new Mock<ICheckBox>();
            var drawable = new CheckBoxDrawable(view.Object);
            var size = drawable.Measure(availableWidth, availableHeight);
            Assert.Equal(new Size(DeviceInfo.ScalingFactor, DeviceInfo.ScalingFactor), size);
        }

        [Fact]
        public void TestOnTouchDown()
        {
            var view = new Mock<ICheckBox>();
            var drawable = new CheckBoxDrawable(view.Object);
            var exception = Record.Exception(() =>
            {
                var point = new GPoint(10, 10);
                drawable.OnTouchDown(point);
            });
            Assert.Null(exception);
        }
    }
}