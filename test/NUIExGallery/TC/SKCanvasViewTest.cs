using SkiaSharp;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class SKCanvasViewTest : TestCaseBase
    {
        public override string TestName => "SKCanvasView Test1";

        public override string TestDescription => "SKCanvasView (image view) Test1";


        int startX = 100;

        public override View Run()
        {
            var view = new View
            {
                Layout = new LinearLayout
                {
                    LinearAlignment = LinearLayout.Alignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                }
            };
            view.UpdateBackgroundColor(Color.FromHex("#618833"));

            var canvas = new SKCanvasView()
            {
                SizeWidth = 300,
                SizeHeight = 300,
            };
            view.Add(new Label
            {
                Text = "ImageView"
            });
            view.Add(canvas);

            var glSurface = new SKGLSurfaceView()
            {
                SizeWidth = 300,
                SizeHeight = 300,
            };
            view.Add(new Label
            {
                Text = "GL Surface View"
            });
            view.Add(glSurface);

            canvas.PaintSurface += OnPaintSurface;
            glSurface.PaintSurface += OnPaintSurface;


            var left = new Button
            {
                Text = "<-"
            };
            view.Add(left);
            left.Clicked += (s, e) =>
            {
                startX -= 10;
                canvas.Invalidate();
                glSurface.Invalidate();
            };

            var right = new Button
            {
                Text = "->"
            };
            view.Add(right);
            right.Clicked += (s, e) =>
            {
                startX += 10;
                canvas.Invalidate();
                glSurface.Invalidate();
            };

            var add = new Button
            {
                Text = "+"
            };
            view.Add(add);
            add.Clicked += (s, e) =>
            {
                canvas.SizeWidth += 30;
                glSurface.SizeWidth += 30;
            };

            return view;
        }

        void OnPaintSurface(object sender, SkiaSharp.Views.Tizen.SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            // configure our brush

            using (var paint = new SKPaint
            {
                Color = SKColors.Green,
                StrokeWidth = 2,
                TextSize = 30,
            })
            {
                canvas.DrawRect(new SKRect(0, 0, 30, 30), paint);
                canvas.DrawRect(new SKRect(startX, 100, startX + 100, 200), paint);
            }
        }
    }
}
