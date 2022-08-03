using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.NUI.GraphicsView;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class RefreshIconTest : TestCaseBase
    {
        public override string TestName => "RefreshIcon Test";

        public override string TestDescription => "RefreshIcon test1";

        public override View Run()
        {
            var scrollview = new Tizen.UIExtensions.NUI.ScrollView();

            scrollview.ContentContainer.Layout = new LinearLayout
            {
                LinearAlignment = LinearLayout.Alignment.Center,
                LinearOrientation = LinearLayout.Orientation.Vertical,
            };

            var view = scrollview.ContentContainer;


            view.Add(new Label
            {
                Text = "RefreshIcon Test",
                TextColor = Color.White,
                FontSize = 9,
                FontAttributes = Tizen.UIExtensions.Common.FontAttributes.Bold,
                VerticalTextAlignment = Tizen.UIExtensions.Common.TextAlignment.Center,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                SizeHeight = 100,
                Padding = new Extents(20, 10, 10, 10),
                BackgroundColor = Color.FromHex("#2196f3").ToNative(),
                BoxShadow = new Shadow(5, Color.FromHex("#bbbbbb").ToNative(), new Vector2(0, 5))
            });

            view.Add(new View
            {
                SizeHeight = 20,
            });


            var hlayout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                },
                //BackgroundColor = Tizen.NUI.Color.Gray,
                Padding = 10,
            };


            var refreshIcon = new RefreshIcon
            {
                BorderlineColor = Tizen.NUI.Color.Red,
                BorderlineWidth = 2f,
            };
            refreshIcon.SizeHeight = (float)refreshIcon.Measure(300, 300).Height;
            refreshIcon.SizeWidth = (float)refreshIcon.Measure(300, 300).Width;
            hlayout.Add(refreshIcon);

            var refreshIcon2 = new RefreshIcon
            {
                Color = Color.Red,
                BackgroundColor = Color.Yellow,
            };
            refreshIcon2.SizeHeight = (float)refreshIcon2.Measure(300, 300).Height;
            refreshIcon2.SizeWidth = (float)refreshIcon2.Measure(300, 300).Width;
            hlayout.Add(refreshIcon2);

            view.Add(hlayout);

            var slider1 = new Slider
            {
                Value = 0,
                Minimum = 0,
                Maximum = 1,
                SizeHeight = 50,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };
            view.Add(slider1);

            slider1.ValueChanged += (s, e) =>
            {
                refreshIcon.PullDistance = (float)slider1.Value;
                refreshIcon2.PullDistance = (float)slider1.Value;
            };
            var switch1 = new Switch()
            {
                SizeHeight = 50,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };
            view.Add(switch1);

            switch1.Toggled += (s, e) =>
            {
                refreshIcon.IsRunning = switch1.IsToggled;
                refreshIcon2.IsRunning = switch1.IsToggled;
            };


            return scrollview;
        }
    }
}
