using Tizen.Applications;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUIExGallery.TC
{
    public class PerformanceTest : TestCaseBase
    {
        public override string TestName => "# Scroll Performance Test";

        public override string TestDescription => "Hello World";

        public override View Run()
        {
            ScrollableBase scroller = new ScrollableBase
            {
                BackgroundColor = Color.White,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                HideScrollbar = false,
            };

            scroller.ContentContainer.Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical
            };

            var images = new string[] { "image.png", "image2.jpg" };

            for (int i = 0; i < 80; i++)
            {
                var innerScroller = new ScrollableBase
                {
                    ScrollingDirection = ScrollableBase.Direction.Horizontal,
                    Layout = new LinearLayout
                    {
                        LinearOrientation = LinearLayout.Orientation.Horizontal,
                    },
                    WidthResizePolicy = ResizePolicyType.FillToParent,
                    SizeHeight = 200,
                };
                for (int j = 0; j < 10; j++)
                {
                    var view = new View
                    {
                        Layout = new LinearLayout { LinearOrientation = LinearLayout.Orientation.Vertical },
                        HeightResizePolicy = ResizePolicyType.FillToParent,
                        SizeWidth = 200,
                    };
                    var img = new ImageView
                    {
                        WidthResizePolicy = ResizePolicyType.FillToParent,
                        SizeHeight = 150,
                        ResourceUrl = Application.Current.DirectoryInfo.Resource + images[(j + i) % 2]
                    };
                    view.Add(img);
                    view.Add(new TextLabel
                    {
                        Text = "Image..",
                        PixelSize = 30,
                        BackgroundColor = Color.White,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        WidthResizePolicy = ResizePolicyType.FillToParent,
                        SizeHeight = 50,
                    });
                    innerScroller.ContentContainer.Add(view);
                }
                scroller.ContentContainer.Add(innerScroller);
            }

            return scroller;
        }
    }
}
