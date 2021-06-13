using SkiaSharp;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;
using ScrollView = Tizen.UIExtensions.NUI.ScrollView;
using Tizen.NUI;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace NUIExGallery.TC
{
    public class SKCanvasAnimationTest : TestCaseBase
    {
        public override string TestName => "SKCanvasView Animation Test1";

        public override string TestDescription => TestName;


        int startx = 0;
        int starty = 0;
        Timer timer;

        public override View Run()
        {
            var scrollView = new ScrollView();
            scrollView.UpdateBackgroundColor(Color.FromHex("#618833"));
            scrollView.ContentContainer.Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical
            };

            List<CustomRenderingView> viewList = new List<CustomRenderingView>();

            for (int i = 0; i < 5; i++)
            {
                var canvas = new SKCanvasView()
                {
                    Margin = new Extents(5, 5, 5, 5),
                    SizeWidth = 300,
                    SizeHeight = 300,
                };
                var glSurface = new SKGLSurfaceView()
                {
                    Margin = new Extents(5, 5, 5, 5),
                    SizeWidth = 300,
                    SizeHeight = 300,
                };
                var hlayout = new View
                {
                    SizeHeight = 310,
                    Layout = new LinearLayout
                    {
                        LinearOrientation = LinearLayout.Orientation.Horizontal,
                        LinearAlignment = LinearLayout.Alignment.Center,
                    }
                };

                canvas.PaintSurface += Draw;
                glSurface.PaintSurface += Draw;

                hlayout.Add(canvas);
                hlayout.Add(glSurface);

                viewList.Add(canvas);
                viewList.Add(glSurface);

                scrollView.Add(hlayout);
            }


            timer = new Timer(10);

            timer.Tick += (s, e) =>
            {
                startx += 10;
                starty += 1;
                if (startx + 100 >= 300)
                    startx = 0;
                if (starty + 100 >= 300)
                    starty = 0;

                foreach (var view in viewList)
                {
                    view.Invalidate();
                }

                return true;
            };

            scrollView.RemovedFromWindow += (s, e) =>
            {
                timer.Stop();
            };
            timer.Start();

            return scrollView;
        }

        void Draw(object s, SkiaSharp.Views.Tizen.SKPaintSurfaceEventArgs e)
        {
            Debug.Assert(e != null, "event args is null");
            Debug.Assert(e.Surface != null, "Surface is null");
            Debug.Assert(e.Surface.Canvas != null, "Canvas is null");

            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            using (var paint = new SKPaint
            {
                Color = SKColors.Green,
                Style = SKPaintStyle.Fill,
            })
            {
                canvas.DrawRect(new SKRect(startx, starty, startx + 100, starty + 100), paint);
            }
        }
    }
}
