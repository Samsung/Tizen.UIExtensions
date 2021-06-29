using ElmSharp;
using SkiaSharp;
using System;
using Tizen.Applications;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using Image = Tizen.UIExtensions.ElmSharp.Image;
using Label = Tizen.UIExtensions.ElmSharp.Label;

namespace ElmSharpExGallery.TC
{
    public class ClippingTest : TestCaseBase
    {
        public override string TestName => "Clipping Test";

        public override string TestDescription => "Clipping Test";

        public override void Run(ElmSharp.Box parent)
        {
            var container = new Canvas(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
                MinimumHeight = 300,
            };
            container.Show();
            parent.PackEnd(container);

            container.LayoutUpdated += (s, e) =>
            {
                foreach (var child in container.Children)
                {
                    child.Geometry = container.Geometry;
                    if (child is SKClipperView clip)
                    {
                        clip.Invalidate();
                    }
                }
            };

            var img1 = new Image(parent);
            img1.Show();
            img1.Load(Application.Current.DirectoryInfo.Resource + "animated.gif");
            img1.SetIsAnimationPlaying(true);

            var clipper = new SKClipperView(parent);
            clipper.Show();
            clipper.PaintSurface += (s, e) =>
            {
                var canvas = e.Surface.Canvas;
                var width = e.Info.Width;
                var height = e.Info.Height;

                using (var paint = new SKPaint
                {
                    IsAntialias = true,
                    Color = SKColors.White,
                    Style = SKPaintStyle.Fill,
                })
                {
                    canvas.Clear();
                    canvas.DrawCircle(width / 2.0f, height / 2.0f, Math.Min(width / 2.0f, height / 2.0f) / 2.0f, paint);
                }

                img1.SetClipperCanvas(clipper);
            };
            clipper.Lower();
            container.Children.Add(img1);
            container.Children.Add(clipper);
        }
    }
}
