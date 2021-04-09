using ElmSharp;
using System;
using Tizen.Applications;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using Image = Tizen.UIExtensions.ElmSharp.Image;
using Label = Tizen.UIExtensions.ElmSharp.Label;

namespace ElmSharpExGallery.TC
{
    public class ImageTest : TestCaseBase
    {
        public override string TestName => "Image Test";

        public override string TestDescription => "Test Image";

        public override void Run(ElmSharp.Box parent)
        {
            var scrollview = new ScrollView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            scrollview.Show();
            scrollview.ScrollOrientation = ScrollOrientation.Vertical;
            parent.PackEnd(scrollview);

            var scrollCanvas = new ElmSharp.Box(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            scrollCanvas.Show();
            scrollview.SetScrollCanvas(scrollCanvas);


            var img1 = new Image(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
                MinimumHeight = 300,
            };
            img1.Show();
            img1.LoadAsync(Application.Current.DirectoryInfo.Resource + "image.png");
            scrollCanvas.PackEnd(img1);

            {
                var label = new Label(parent)
                {
                    Text = "AspectFill"
                };
                label.Show();
                scrollCanvas.PackEnd(label);
                var img2 = new Image(parent)
                {
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                    MinimumHeight = 300,
                };
                img2.Show();
                img2.Aspect = Aspect.AspectFill;
                img2.LoadAsync("http://i.imgur.com/9f974SC.jpg");
                img2.LoadingCompleted += (s, e) => Console.WriteLine($"Loading completed");
                scrollCanvas.PackEnd(img2);
            }

            {
                var label = new Label(parent)
                {
                    Text = "AppectFit"
                };
                label.Show();
                scrollCanvas.PackEnd(label);

                var img2 = new Image(parent)
                {
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                    MinimumHeight = 300,
                };
                img2.Show();
                img2.Aspect = Aspect.AspectFit;
                img2.LoadAsync("http://i.imgur.com/9f974SC.jpg");
                img2.LoadingCompleted += (s, e) => Console.WriteLine($"Loading completed");
                scrollCanvas.PackEnd(img2);
            }

            {
                var label = new Label(parent)
                {
                    Text = "Fill"
                };
                label.Show();
                scrollCanvas.PackEnd(label);

                var img2 = new Image(parent)
                {
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                    MinimumHeight = 300,
                };
                img2.Show();
                img2.Aspect = Aspect.Fill;
                img2.LoadAsync("http://i.imgur.com/9f974SC.jpg");
                img2.LoadingCompleted += (s, e) => Console.WriteLine($"Loading completed");
                scrollCanvas.PackEnd(img2);
            }

            {
                var label = new Label(parent)
                {
                    Text = "Animated"
                };
                label.Show();
                scrollCanvas.PackEnd(label);

                var animated = new Image(parent)
                {
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                    MinimumHeight = 300,
                };
                animated.Show();
                animated.Aspect = Aspect.AspectFit;
                animated.LoadAsync(Application.Current.DirectoryInfo.Resource + "animated.gif");

                animated.LoadingCompleted += (s, e) =>
                {
                    Console.WriteLine($"Loading completed");
                    animated.SetIsAnimationPlaying(true);
                };
                scrollCanvas.PackEnd(animated);
            }

            {
                var label = new Label(parent)
                {
                    Text = "IsAnimationPlaying = false"
                };
                label.Show();
                scrollCanvas.PackEnd(label);

                var img2 = new Image(parent)
                {
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                    MinimumHeight = 300,
                };
                img2.Show();
                img2.Aspect = Aspect.AspectFit;
                img2.LoadAsync(Application.Current.DirectoryInfo.Resource + "animated2.gif");
                img2.LoadingCompleted += (s, e) =>
                {
                    Console.WriteLine($"Loading completed");
                    (s as Image).SetIsAnimationPlaying(false);
                };
                scrollCanvas.PackEnd(img2);
            }

            scrollview.SetContentSize(720, 3000);

        }
    }
}
