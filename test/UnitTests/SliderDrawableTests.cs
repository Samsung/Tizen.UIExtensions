using Moq;
using Tizen.UIExtensions.Common.GraphicsView;
using ICanvas = Microsoft.Maui.Graphics.ICanvas;
using RectF = Microsoft.Maui.Graphics.RectF;
using GPoint = Microsoft.Maui.Graphics.Point;

namespace UnitTests
{
    public class SliderDrawableTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestDraw(bool isEnabled)
        {
            var view = new Mock<ISlider>();
            var canvas = new Mock<ICanvas>();
            var drawable = new SliderDrawable(view.Object);
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
            var view = new Mock<ISlider>();
            var drawable = new SliderDrawable(view.Object);
            var size = drawable.Measure(availableWidth, availableHeight);
            Assert.Equal(new Size(availableWidth, DeviceInfo.ScalingFactor), size);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestUpdateAnimation(bool increase)
        {
            var view = new Mock<ISlider>();
            var drawable = new SliderDrawable(view.Object);
            var exception = Record.Exception(() =>
            {
                drawable.AnimateMaterialThumbSize(increase);
            });
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(0, 0, 100, 100)]
        [InlineData(10, 100, 10, 100)]
        [InlineData(100, 10, 100, 10)]
        public void TestThumbRect(float x, float y, float w, float h)
        {
            var rect = new RectF(x, y, w, h);
            var view = new Mock<ISlider>();
            var drawable = new SliderDrawable(view.Object);
            drawable.ThumbRect = rect;
            Assert.Equal(rect, drawable.ThumbRect);
        }

        [Theory]
        [InlineData(0, 0, 100, 100)]
        [InlineData(10, 100, 10, 100)]
        [InlineData(100, 10, 100, 10)]
        public void TestTrackRect(float x, float y, float w, float h)
        {
            var rect = new RectF(x, y, w, h);
            var view = new Mock<ISlider>();
            var drawable = new SliderDrawable(view.Object);
            drawable.TrackRect = rect;
            Assert.Equal(rect, drawable.TrackRect);
        }

        [Theory]
        [InlineData(0, 10, 1)]
        [InlineData(0, 1.0, .1)]
        public void TestValueRate(double minimum, double maximum, double valueRate)
        {
            var view = new Mock<ISlider>();
            view.Setup(x => x.Minimum).Returns(minimum);
            view.Setup(x => x.Maximum).Returns(maximum);
            Assert.Equal(minimum, view.Object.Minimum);
            Assert.Equal(maximum, view.Object.Maximum);
            var drawable = new SliderDrawable(view.Object);
            drawable.ValueRate = valueRate;
            Assert.Equal(0, drawable.ValueRate);
        }


        [Theory]
        [InlineData(0, 0, 100, 100, 10, 10)]
        [InlineData(10, 100, 10, 100, 200, 200)]
        [InlineData(100, 10, 100, 10, 100, 10)]
        public void TestOnTouchUpMoveDown(float x, float y, float w, float h, float pointX, float pointY)
        {
            var view = new Mock<ISlider>();
            var drawable = new SliderDrawable(view.Object);
            var rect = new RectF(x, y, w, h);
            drawable.ThumbRect = rect;
            Assert.Equal(rect, drawable.ThumbRect);

            var exception = Record.Exception(() =>
            {
                var point = new GPoint(pointX, pointY);
                drawable.OnTouchDown(point);
                drawable.OnTouchMove(point);
                drawable.OnTouchUp(point);
            });
            Assert.Null(exception);
        }
    }
}