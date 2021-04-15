using System;
using System.Collections.Generic;
using System.Text;
using Tizen.NUI.BaseComponents;
using ScrollView = Tizen.UIExtensions.NUI.ScrollView;
using Image = Tizen.UIExtensions.NUI.Image;
using Tizen.NUI;
using Tizen.Applications;
using Tizen.UIExtensions.NUI;

namespace NUIExGallery.TC
{
    public class ImageTest : TestCaseBase
    {
        public override string TestName => "Image Test";

        public override string TestDescription => "Image Test";

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

            var scrollView = new ScrollView()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            view.Add(scrollView);

            scrollView.ContentContainer.Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
            };

            {
                var img = new Image
                {
                    WidthResizePolicy = ResizePolicyType.FillToParent,
                    SizeHeight = 300,
                    BackgroundColor = Color.Red,
                    ResourceUrl = Application.Current.DirectoryInfo.Resource + "image.png",
                };
                scrollView.ContentContainer.Add(img);
            }

            {
                var img = new Image
                {
                    SizeHeight = 300,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    ResourceUrl = "http://i.imgur.com/9f974SC.jpg",
                    // now it does not working, but NUI team will fix it
                    Aspect = Tizen.UIExtensions.Common.Aspect.AspectFill
                };
                img.ResourceReady += (s, e) =>
                {
                    Console.WriteLine($"Image Loaded - {img.LoadingStatus}");
                };
                scrollView.ContentContainer.Add(new Label { Text = "AspectFill" });
                scrollView.ContentContainer.Add(img);
            }

            {
                var img = new Image
                {
                    SizeHeight = 300,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    ResourceUrl = "http://i.imgur.com/9f974SC.jpg",
                    // now it does not working, but NUI team will fix it
                    Aspect = Tizen.UIExtensions.Common.Aspect.AspectFit
                };
                img.ResourcesLoaded += (s, e) =>
                {
                    Console.WriteLine($"Image Loaded - {img.LoadingStatus}");
                };
                scrollView.ContentContainer.Add(new Label { Text = "AspectFit" });
                scrollView.ContentContainer.Add(img);
            }

            {
                var img = new Image
                {
                    SizeHeight = 300,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    ResourceUrl = "http://i.imgur.com/9f974SC.jpg",
                    // now it does not working, but NUI team will fix it
                    Aspect = Tizen.UIExtensions.Common.Aspect.Fill
                };
                img.ResourcesLoaded += (s, e) =>
                {
                    Console.WriteLine($"Image Loaded - {img.LoadingStatus}");

                };
                scrollView.ContentContainer.Add(new Label { Text = "Fill" });
                scrollView.ContentContainer.Add(img);
            }

            {
                var img = new Image
                {
                    ResourceUrl = Application.Current.DirectoryInfo.Resource + "animated2.gif",
                };
                img.SetIsAnimationPlaying(true);
                img.ResourcesLoaded += (s, e) =>
                {
                    // Aspect not apply before loaded image
                    Console.WriteLine($"Image Loaded");
                };
                scrollView.ContentContainer.Add(new Label { Text = "Animated" });
                scrollView.ContentContainer.Add(img);
            }

            {
                var img = new Image
                {
                    ResourceUrl = Application.Current.DirectoryInfo.Resource + "animated2.gif",
                };
                img.SetIsAnimationPlaying(false);
                img.ResourcesLoaded += (s, e) =>
                {
                    // Aspect not apply before loaded image
                    Console.WriteLine($"Image Loaded");
                };
                scrollView.ContentContainer.Add(new Label { Text = "SetIsAnimationPlaying(false)" });
                scrollView.ContentContainer.Add(img);
            }

            return view;
        }
    }
}
