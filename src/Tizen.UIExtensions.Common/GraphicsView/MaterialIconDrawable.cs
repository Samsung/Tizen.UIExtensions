using Microsoft.Maui.Graphics;
using GPoint = Microsoft.Maui.Graphics.Point;
using TSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public enum MaterialIcons
    {
        Add,
        App,
        ArrowBack,
        ArrowDownward,
        ArrowForward,
        ArrowUpward,
        Check,
        Close,
        ExpandLess,
        ExpandMore,
        Link,
        Menu,
        MoreHoriz,
        MoreVert,
        Remove,
        RemoveCircle
    }

    public class MaterialIconDrawable : GraphicsViewDrawable
    {

        const float MaterialIconHeight = 24;
        const float MaterialIconWidth = 24;

        const string PathAdd = "M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z";
        const string PathApp = "M4 8h4V4H4v4zm6 12h4v-4h-4v4zm-6 0h4v-4H4v4zm0-6h4v-4H4v4zm6 0h4v-4h-4v4zm6-10v4h4V4h-4zm-6 4h4V4h-4v4zm6 6h4v-4h-4v4zm0 6h4v-4h-4v4z";
        const string PathArrowBack = "M20 11H7.83l5.59-5.59L12 4l-8 8 8 8 1.41-1.41L7.83 13H20v-2z";
        const string PathArrowDownward = "M20 12l-1.41-1.41L13 16.17V4h-2v12.17l-5.58-5.59L4 12l8 8 8-8z";
        const string PathArrowForward = "M12 4l-1.41 1.41L16.17 11H4v2h12.17l-5.58 5.59L12 20l8-8z";
        const string PathArrowUpward = "M4 12l1.41 1.41L11 7.83V20h2V7.83l5.58 5.59L20 12l-8-8-8 8z";
        const string PathCheck = "M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z";
        const string PathClose = "M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z";
        const string PathExpandLess = "M12 8l-6 6 1.41 1.41L12 10.83l4.59 4.58L18 14z";
        const string PathExpandMore = "M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z";
        const string PathLink = "M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z";
        const string PathMenu = "M3 18h18v-2H3v2zm0-5h18v-2H3v2zm0-7v2h18V6H3z";
        const string PathMoreHoriz = "M6 10c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm12 0c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm-6 0c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z";
        const string PathMoreVert = "M12 8c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z";
        const string PathRemove = "M19 13H5v-2h14v2z";
        const string PathRemoveCircle = "M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm5 11H7v-2h10v2z";

        static readonly string[] s_Paths = 
        {
            PathAdd, 
            PathApp,
            PathArrowBack,
            PathArrowDownward,
            PathArrowForward,
            PathArrowUpward,
            PathCheck,
            PathClose,
            PathExpandLess,
            PathExpandMore,
            PathLink,
            PathMenu,
            PathMoreHoriz,
            PathMoreVert,
            PathRemove,
            PathRemoveCircle,
        };

        public MaterialIconDrawable()
        {
        }

        public MaterialIcons Icon { get; set; }
        public Color Color { get; set; }

        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            DrawIcon(canvas, dirtyRect);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            return new TSize(DeviceInfo.ScalingFactor * MaterialIconWidth, DeviceInfo.ScalingFactor * MaterialIconHeight);
        }


        void DrawIcon(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.SaveState();

            // Note. SkiaGraphicsView use DP unit
            var width = dirtyRect.Width;
            var height = dirtyRect.Height;
            var imgWidth = MaterialIconWidth;
            var imgHeight = MaterialIconHeight;

            var transX = (width - imgWidth) / 2.0f;
            var transY = (height - imgHeight) / 2.0f;
            canvas.Translate(transX, transY);

            var vBuilder = new PathBuilder();
            var path = vBuilder.BuildPath(s_Paths[(int)Icon]);

            canvas.FillColor = Color.ToGraphicsColor(Material.Color.Black);
            canvas.FillPath(path);
            canvas.RestoreState();
        }

    }
}
