using Moq;
using Tizen.UIExtensions.Common.GraphicsView;
using ICanvas = Microsoft.Maui.Graphics.ICanvas;
using RectF = Microsoft.Maui.Graphics.RectF;

namespace UnitTests
{
    public class ActivityIndicatorDrawableTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void TestDraw(bool isRunning, bool isDefaultColor)
        {
            var view = new Mock<IActivityIndicator>();
            view.Setup(x => x.IsRunning).Returns(isRunning);
            view.Setup(x => x.Color).Returns(isDefaultColor ? Color.Default : Color.White);
            Assert.Equal(isRunning, view.Object.IsRunning);

            var canvas = new Mock<ICanvas>();
            var drawable = new ActivityIndicatorDrawable(view.Object);
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
            var view = new Mock<IActivityIndicator>();
            var drawable = new ActivityIndicatorDrawable(view.Object);
            var size = drawable.Measure(availableWidth, availableHeight);
            Assert.Equal(new Size(DeviceInfo.ScalingFactor, DeviceInfo.ScalingFactor), size);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestUpdateAnimation(bool animate)
        {
            var view = new Mock<IActivityIndicator>();
            var drawable = new ActivityIndicatorDrawable(view.Object);
            var exception = Record.Exception(() =>
            {
                drawable.UpdateAnimation(animate);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void TestDispose()
        {
            var view = new Mock<IActivityIndicator>();
            var drawable = new ActivityIndicatorDrawable(view.Object);
            drawable.Dispose();
            drawable = null;
            Assert.Null(drawable);
        }
    }
}