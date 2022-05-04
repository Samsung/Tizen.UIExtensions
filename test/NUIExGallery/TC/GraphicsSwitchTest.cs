using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.NUI.GraphicsView;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class GraphcisSwitchTest : TestCaseBase
    {
        public override string TestName => "GraphcisSwitch Test";

        public override string TestDescription => "GraphcisSwitch test1";

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
                Text = "Switch Test",
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
                Padding = 10,
            };

            Switch switchtest = null;

            {
                var switch1 = new Switch
                {
                    Margin = 5,
                };
                switch1.SizeHeight = (float)switch1.Measure(300, 300).Height;
                switch1.SizeWidth = (float)switch1.Measure(300, 300).Width;
                hlayout.Add(switch1);
            }
            {
                var switch1 = new Switch
                {
                    IsEnabled = false,
                    Margin = 5,
                    ThumbColor = Color.Red,
                    OnColor = Color.Yellow
                };
                switch1.SizeHeight = (float)switch1.Measure(300, 300).Height;
                switch1.SizeWidth = (float)switch1.Measure(300, 300).Width;
                hlayout.Add(switch1);
            }
            {
                var switch1 = new Switch
                {
                    Margin = 5,
                    ThumbColor = Color.BlueViolet,
                    OnColor = Color.Red,
                    IsToggled = true,
                };
                switch1.SizeHeight = (float)switch1.Measure(300, 300).Height;
                switch1.SizeWidth = (float)switch1.Measure(300, 300).Width;
                hlayout.Add(switch1);
                switchtest = switch1;
            }
            view.Add(hlayout);


            var btn = new Tizen.UIExtensions.NUI.Button
            {
                Text = "Toggle"
            };
            btn.Clicked += (s, e) => switchtest.IsToggled = !switchtest.IsToggled;
            scrollview.ContentContainer.Add(btn);

            return scrollview;
        }
    }
}
