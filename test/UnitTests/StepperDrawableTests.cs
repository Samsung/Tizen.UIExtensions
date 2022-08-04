using Moq;
using Tizen.UIExtensions.Common.GraphicsView;
using ICanvas = Microsoft.Maui.Graphics.ICanvas;
using RectF = Microsoft.Maui.Graphics.RectF;
using GPoint = Microsoft.Maui.Graphics.Point;

namespace UnitTests
{
    public class StepperDrawableTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestDraw(bool isEnabled)
        {
            var view = new Mock<IStepper>();
            var canvas = new Mock<ICanvas>();
            var drawable = new StepperDrawable(view.Object);
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
            var view = new Mock<IStepper>();
            var drawable = new StepperDrawable(view.Object);
            var size = drawable.Measure(availableWidth, availableHeight);
            Assert.Equal(new Size(DeviceInfo.ScalingFactor, DeviceInfo.ScalingFactor), size);
        }

        [Fact]
        public void TestOnTouchUpMoveDown()
        {
            var view = new Mock<IStepper>();
            var drawable = new StepperDrawable(view.Object);
            var exception = Record.Exception(() =>
            {
                var point = new GPoint(10, 10);
                drawable.OnTouchDown(point);
                drawable.OnTouchMove(point);
                drawable.OnTouchUp(point);
            });
            Assert.Null(exception);
        }
    }
}