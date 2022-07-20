using Moq;
using Tizen.UIExtensions.Common.GraphicsView;
using ICanvas = Microsoft.Maui.Graphics.ICanvas;
using RectF = Microsoft.Maui.Graphics.RectF;

namespace UnitTests
{
    public class RefreshIconDrawableTests
    {
        [Theory]
        [InlineData(true, 0.1)]
        [InlineData(true, 0.5)]
        [InlineData(false, 0.1)]
        [InlineData(false, 0.5)]
        public void TestDraw(bool isRunning, float pullDistance)
        {
            var view = new Mock<IRefreshIcon>();
            view.Setup(x => x.IsRunning).Returns(isRunning);
            view.Setup(x => x.PullDistance).Returns(pullDistance);
            Assert.Equal(isRunning, view.Object.IsRunning);
            Assert.Equal(pullDistance, view.Object.PullDistance);

            var canvas = new Mock<ICanvas>();
            var drawable = new RefreshIconDrawable(view.Object);
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
            var view = new Mock<IRefreshIcon>();
            var drawable = new RefreshIconDrawable(view.Object);
            var size = drawable.Measure(availableWidth, availableHeight);
            Assert.Equal(new Size(DeviceInfo.ScalingFactor, DeviceInfo.ScalingFactor), size);
        }

        [Fact]
        public void TestDispose()
        {
            var view = new Mock<IRefreshIcon>();
            var drawable = new RefreshIconDrawable(view.Object);
            drawable.Dispose();
            drawable = null;
            Assert.Null(drawable);
        }
    }
}