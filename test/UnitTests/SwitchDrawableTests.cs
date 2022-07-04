using Moq;
using Tizen.UIExtensions.Common.GraphicsView;
using ICanvas = Microsoft.Maui.Graphics.ICanvas;
using RectF = Microsoft.Maui.Graphics.RectF;
using GPoint = Microsoft.Maui.Graphics.Point;

namespace UnitTests
{
    public class SwitchDrawableTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void TestDraw(bool isEnabled, bool isToggled)
        {
            var view = new Mock<ISwitch>();
            view.Setup(x => x.IsToggled).Returns(isToggled);
            Assert.Equal(isToggled, view.Object.IsToggled);
            var canvas = new Mock<ICanvas>();
            var drawable = new SwitchDrawable(view.Object);
            drawable.IsEnabled = isEnabled;
            Assert.Equal(isEnabled, drawable.IsEnabled);

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
            var view = new Mock<ISwitch>();
            var drawable = new SwitchDrawable(view.Object);
            var size = drawable.Measure(availableWidth, availableHeight);
            Assert.Equal(new Size(DeviceInfo.ScalingFactor, DeviceInfo.ScalingFactor), size);
        }

        [Fact]
        public void TestOnTouchDown()
        {
            var view = new Mock<ISwitch>();
            var drawable = new SwitchDrawable(view.Object);
            var exception = Record.Exception(() =>
            {
                var point = new GPoint(10, 10);
                drawable.OnTouchDown(point);
            });
            Assert.Null(exception);
        }
    }
}