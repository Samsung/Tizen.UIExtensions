using Moq;
using Tizen.UIExtensions.Common.GraphicsView;
using ICanvas = Microsoft.Maui.Graphics.ICanvas;
using RectF = Microsoft.Maui.Graphics.RectF;

namespace UnitTests
{
    public class MaterialIconDrawableTests
    {
        [Fact]
        public void TestDraw()
        {
            var canvas = new Mock<ICanvas>();
            var drawable = new MaterialIconDrawable();
            
            var exception = Record.Exception(() =>
            {
                drawable.Draw(canvas.Object, new RectF(0, 0, 100, 100));
            });
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(100, 100)]
        public void TestMeasure(double availableWidth, double availableHeight)
        {
            var drawable = new MaterialIconDrawable();
            var size = drawable.Measure(availableWidth, availableHeight);
            Assert.Equal(new Size(DeviceInfo.ScalingFactor, DeviceInfo.ScalingFactor), size);
        }

        [Theory]
        [InlineData(MaterialIcons.Add)]
        [InlineData(MaterialIcons.App)]
        [InlineData(MaterialIcons.ArrowBack)]
        [InlineData(MaterialIcons.ArrowDownward)]
        [InlineData(MaterialIcons.ArrowForward)]
        [InlineData(MaterialIcons.ArrowUpward)]
        [InlineData(MaterialIcons.Check)]
        [InlineData(MaterialIcons.Close)]
        [InlineData(MaterialIcons.ExpandLess)]
        [InlineData(MaterialIcons.ExpandMore)]
        [InlineData(MaterialIcons.Link)]
        [InlineData(MaterialIcons.Menu)]
        [InlineData(MaterialIcons.MoreHoriz)]
        [InlineData(MaterialIcons.MoreVert)]
        [InlineData(MaterialIcons.Remove)]
        [InlineData(MaterialIcons.RemoveCircle)]
        [InlineData(MaterialIcons.Search)]
        [InlineData(MaterialIcons.Replay)]
        [InlineData(MaterialIcons.Account)]
        [InlineData(MaterialIcons.Settings)]
        [InlineData(MaterialIcons.ArrowLeft)]
        [InlineData(MaterialIcons.ArrowRight)]
        public void TestIcon(MaterialIcons icon)
        {
            var drawable = new MaterialIconDrawable();
            drawable.Icon = icon;
            Assert.Equal(icon, drawable.Icon);
        }

        [Fact]
        public void TestColor()
        {
            var drawable = new MaterialIconDrawable();
            drawable.Color = Color.Red;
            Assert.Equal(Color.Red, drawable.Color);
            drawable.Color = Color.Default;
            Assert.Equal(Color.Default, drawable.Color);
        }
    }
}