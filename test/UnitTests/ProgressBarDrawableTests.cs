using Moq;
using Tizen.UIExtensions.Common.GraphicsView;
using ICanvas = Microsoft.Maui.Graphics.ICanvas;
using RectF = Microsoft.Maui.Graphics.RectF;

namespace UnitTests
{
    public class ProgressBarDrawableTests
    {
        [Fact]
        public void TestDraw()
        {
            var view = new Mock<IProgressBar>();
            var canvas = new Mock<ICanvas>();
            var drawable = new ProgressBarDrawable(view.Object);
            
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
            var view = new Mock<IProgressBar>();
            var drawable = new ProgressBarDrawable(view.Object);
            var size = drawable.Measure(availableWidth, availableHeight);
            Assert.Equal(new Size(availableWidth, DeviceInfo.ScalingFactor), size);
        }
    }
}