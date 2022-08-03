using SkiaSharp;
using System.Diagnostics;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class BufferCreatePerformanceTest : TestCaseBase
    {
        public override string TestName => "BufferCreatePerformanceTest";

        public override string TestDescription => "BufferCreatePerformanceTest";


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

            var test1 = new Button
            {
                Text = "PixelBuffer Create"
            };
            view.Add(test1);

            var test2 = new Button
            {
                Text = "NativeImageQueue Create"
            };
            view.Add(test2);

            var label = new Label();
            view.Add(label);


            test1.Clicked += (s, e) =>
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                for (int i = 0; i < 1000; i++)
                {
                    using var buffer = new PixelBuffer(500, 500, PixelFormat.BGRA8888);
                    buffer.GetBuffer();
                    using var pixelData = PixelBuffer.Convert(buffer);
                    using var url = pixelData.GenerateUrl();
                }
                stopwatch.Stop();
                label.Text = $"PixelBuffer create time : {stopwatch.ElapsedMilliseconds} ms";
            };

            test2.Clicked += (s, e) =>
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                for (int i = 0; i < 1000; i++)
                {
                    using var queue = new NativeImageQueue(500, 500, NativeImageQueue.ColorFormat.RGBA8888);
                    int width = 0;
                    int height = 0;
                    int stride = 0;
                    var buffer = queue.DequeueBuffer(ref width, ref height, ref stride);
                    queue.EnqueueBuffer(buffer);
                }
                stopwatch.Stop();
                label.Text = $"NativeImageQueue create time : {stopwatch.ElapsedMilliseconds} ms";
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
