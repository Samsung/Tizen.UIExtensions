using Microsoft.Maui.Graphics;
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
        RemoveCircle,
        Search,
        Replay,
        Account,
        Settings,
        ArrowLeft,
        ArrowRight,
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
        const string PathSearch = "M9.5,3A6.5,6.5 0 0,1 16,9.5C16,11.11 15.41,12.59 14.44,13.73L14.71,14H15.5L20.5,19L19,20.5L14,15.5V14.71L13.73,14.44C12.59,15.41 11.11,16 9.5,16A6.5,6.5 0 0,1 3,9.5A6.5,6.5 0 0,1 9.5,3M9.5,5C7,5 5,7 5,9.5C5,12 7,14 9.5,14C12,14 14,12 14,9.5C14,7 12,5 9.5,5Z";
        const string PathReplay = "M12,5V1L7,6L12,11V7A6,6 0 0,1 18,13A6,6 0 0,1 12,19A6,6 0 0,1 6,13H4A8,8 0 0,0 12,21A8,8 0 0,0 20,13A8,8 0 0,0 12,5Z";
        const string PathAccount = "M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z";
        const string PathSettings = "M12,15.5A3.5,3.5 0 0,1 8.5,12A3.5,3.5 0 0,1 12,8.5A3.5,3.5 0 0,1 15.5,12A3.5,3.5 0 0,1 12,15.5M19.43,12.97C19.47,12.65 19.5,12.33 19.5,12C19.5,11.67 19.47,11.34 19.43,11L21.54,9.37C21.73,9.22 21.78,8.95 21.66,8.73L19.66,5.27C19.54,5.05 19.27,4.96 19.05,5.05L16.56,6.05C16.04,5.66 15.5,5.32 14.87,5.07L14.5,2.42C14.46,2.18 14.25,2 14,2H10C9.75,2 9.54,2.18 9.5,2.42L9.13,5.07C8.5,5.32 7.96,5.66 7.44,6.05L4.95,5.05C4.73,4.96 4.46,5.05 4.34,5.27L2.34,8.73C2.21,8.95 2.27,9.22 2.46,9.37L4.57,11C4.53,11.34 4.5,11.67 4.5,12C4.5,12.33 4.53,12.65 4.57,12.97L2.46,14.63C2.27,14.78 2.21,15.05 2.34,15.27L4.34,18.73C4.46,18.95 4.73,19.03 4.95,18.95L7.44,17.94C7.96,18.34 8.5,18.68 9.13,18.93L9.5,21.58C9.54,21.82 9.75,22 10,22H14C14.25,22 14.46,21.82 14.5,21.58L14.87,18.93C15.5,18.67 16.04,18.34 16.56,17.94L19.05,18.95C19.27,19.03 19.54,18.95 19.66,18.73L21.66,15.27C21.78,15.05 21.73,14.78 21.54,14.63L19.43,12.97Z";
        const string PathArrowLeft = "M15.41,16.58L10.83,12L15.41,7.41L14,6L8,12L14,18L15.41,16.58Z";
        const string PathArrowRight = "M8.59,16.58L13.17,12L8.59,7.41L10,6L16,12L10,18L8.59,16.58Z";

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
            PathSearch,
            PathReplay,
            PathAccount,
            PathSettings,
            PathArrowLeft,
            PathArrowRight,
        };

        public MaterialIconDrawable()
        {
        }

        public MaterialIcons Icon { get; set; }
        public Color Color { get; set; }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawIcon(canvas, dirtyRect);
        }

        public override TSize Measure(double availableWidth, double availableHeight)
        {
            return new TSize(DeviceInfo.ScalingFactor * MaterialIconWidth, DeviceInfo.ScalingFactor * MaterialIconHeight);
        }


        void DrawIcon(ICanvas canvas, RectF dirtyRect)
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
