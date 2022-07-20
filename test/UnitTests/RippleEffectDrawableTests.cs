using Moq;
using Tizen.UIExtensions.Common.GraphicsView;
using ICanvas = Microsoft.Maui.Graphics.ICanvas;
using RectF = Microsoft.Maui.Graphics.RectF;
using PointF = Microsoft.Maui.Graphics.PointF;
using GColor = Microsoft.Maui.Graphics.Color;
using GPoint = Microsoft.Maui.Graphics.Point;

namespace UnitTests
{
    public class RippleEffectDrawableTests
    {
        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(0, 0, 0, 0, 10, 10)]
        [InlineData(10, 10, 50, 50, 10, 10)]
        public void TestDraw(float x, float y, float w, float h, float pointX, float pointY)
        {
            var canvas = new Mock<ICanvas>();
            var rect = new RectF(x, y, w, h);
            var point = new PointF(pointX, pointY);
            var drawable = new RippleEffectDrawable
            {
                ClipRectangle = rect,
                TouchPoint = point
            };

            var exception = Record.Exception(() =>
            {
                drawable.Draw(canvas.Object, new RectF(x, y, w, h));
            });
            Assert.Null(exception);
        }

        [Fact]        
        
        public void TestDraw2()
        {
            var canvas = new Mock<ICanvas>();
            var point = new PointF(0, 0);
            var drawable = new RippleEffectDrawable
            {
                ClipRectangle = RectF.Zero,
                TouchPoint = new PointF(0, 0)
            };

            var exception = Record.Exception(() =>
            {
                drawable.Draw(canvas.Object, new RectF(0, 0, 10, 10));
            });
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(100, 100)]
        public void TestMeasure(double availableWidth, double availableHeight)
        {
            var drawable = new RippleEffectDrawable();
            var size = drawable.Measure(availableWidth, availableHeight);
            Assert.Equal(new Size(availableWidth, availableHeight), size);
        }

        [Fact]
        public void TestColor()
        {
            var drawable = new RippleEffectDrawable();
            var red = GColor.FromRgb(255, 0, 0);
            drawable.RippleColor = red;
            Assert.Equal(red, drawable.RippleColor);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(10, 0, 10, 0)]
        [InlineData(0, 10, 0, 10)]
        [InlineData(10, 10, 10, 10)]
        public void TestClipRectangle(float x, float y, float w, float h)
        {
            var rect = new RectF(x, y, w, h);
            var drawable = new RippleEffectDrawable();
            drawable.ClipRectangle = rect;
            Assert.Equal(rect, drawable.ClipRectangle);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(10, 0)]
        [InlineData(0, 10)]
        [InlineData(10, 10)]
        public void TestTouchPoint(float x, float y)
        {
            var point = new PointF(x, y);
            var drawable = new RippleEffectDrawable();
            drawable.TouchPoint = point;
            Assert.Equal(point, drawable.TouchPoint);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(0, 0, 0, 0, 10, 10)]
        [InlineData(10, 10, 50, 50, 10, 10)]
        public void TestOnTouchUpDown(float x, float y, float w, float h, float pointX, float pointY)
        {
            var rect = new RectF(x, y, w, h);
            var point = new PointF(pointX, pointY);
            var drawable = new RippleEffectDrawable
            {
                ClipRectangle = rect,
                TouchPoint = point
            };
            var exception = Record.Exception(() =>
            {
                var point = new GPoint(10, 10);
                drawable.OnTouchDown(point);
                drawable.OnTouchUp(point);
            });
            Assert.Null(exception);
        }
    }
}