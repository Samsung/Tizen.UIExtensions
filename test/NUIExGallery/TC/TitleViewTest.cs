using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.NUI.GraphicsView;
using TColor = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class TitleViewTest : TestCaseBase
    {
        public override string TestName => "TitleView Test";

        public override string TestDescription => "TitleView test1";

        public override View Run()
        {
            var scrollview = new Tizen.UIExtensions.NUI.ScrollView();

            scrollview.ContentContainer.Layout = new LinearLayout
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                LinearOrientation = LinearLayout.Orientation.Vertical,
            };

            var view = scrollview.ContentContainer;

            view.UpdateBackgroundColor(TColor.FromHex("#eeeeee"));

            view.Add(new TitleView
            {
                Title = "Title view 1",
                Icon = new MaterialIconButton
                {
                    Icon = MaterialIcons.ArrowBack,
                    SizeWidth = (float)(25 * DeviceInfo.ScalingFactor),
                    SizeHeight = (float)(25 * DeviceInfo.ScalingFactor),
                    Color = TColor.White
                },
                Actions =
                {
                    new MaterialIconButton
                    {
                        Icon = MaterialIcons.MoreVert,
                        SizeWidth = (float)(25 * DeviceInfo.ScalingFactor),
                        SizeHeight = (float)(25 * DeviceInfo.ScalingFactor),
                        Color = TColor.White
                    },
                }
            });

            view.Add(new View
            {
                SizeHeight = 20,
            });

            var title2 = new TitleView
            {
                Title = "Title view 2"
            };
            title2.UpdateBackgroundColor(TColor.White);
            title2.Label.TextColor = TColor.Black;
            title2.Icon = new MaterialIconButton
            {
                Icon = MaterialIcons.Menu,
                SizeWidth = (float)(25 * DeviceInfo.ScalingFactor),
                SizeHeight = (float)(25 * DeviceInfo.ScalingFactor),
            };
            title2.Actions.Add(new MaterialIconButton
            {
                Icon = MaterialIcons.Close,
                SizeWidth = (float)(25 * DeviceInfo.ScalingFactor),
                SizeHeight = (float)(25 * DeviceInfo.ScalingFactor),
            });


            view.Add(title2);

            view.Add(new View
            {
                SizeHeight = 20,
            });

            var title3 = new TitleView();
            title3.UpdateBackgroundColor(TColor.FromHex("6200EE"));
            title3.Icon = new MaterialIconButton
            {
                Icon = MaterialIcons.ArrowBack,
                Color = TColor.White,
                SizeWidth = (float)(25 * DeviceInfo.ScalingFactor),
                SizeHeight = (float)(25 * DeviceInfo.ScalingFactor),
            };
            title3.Content = new Tizen.UIExtensions.NUI.Entry
            {
                PlaceholderText = "Search",
                PlaceholderColor = TColor.FromHex("#222222"),
                VerticalTextAlignment = TextAlignment.Center,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Margin = new Extents(0, 10, 5, 5),
            };
            title3.Content.UpdateBackgroundColor(TColor.FromHex("#eeeeee"));
            title3.Icon = new MaterialIconButton
            {
                Icon = MaterialIcons.Check,
                SizeWidth = (float)(25 * DeviceInfo.ScalingFactor),
                SizeHeight = (float)(25 * DeviceInfo.ScalingFactor),
                Color = TColor.White,
            };
            view.Add(title3);


            view.Add(new View
            {
                SizeHeight = 20,
            });

            var title4 = new TitleView();
            title4.UpdateBackgroundColor(TColor.FromHex("6200EE"));
            title4.Icon = new MaterialIconButton
            {
                Icon = MaterialIcons.ArrowBack,
                Color = TColor.White,
                SizeWidth = (float)(25 * DeviceInfo.ScalingFactor),
                SizeHeight = (float)(25 * DeviceInfo.ScalingFactor),
            };
            title4.Content = new Tizen.UIExtensions.NUI.Entry
            {
                PlaceholderText = "Search",
                PlaceholderColor = TColor.FromHex("#222222"),
                VerticalTextAlignment = TextAlignment.Center,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Margin = new Extents(0, 10, 5, 5),
            };
            title4.Content.UpdateBackgroundColor(TColor.FromHex("#eeeeee"));
            title4.Icon = new MaterialIconButton
            {
                Icon = MaterialIcons.Menu,
                SizeWidth = (float)(25 * DeviceInfo.ScalingFactor),
                SizeHeight = (float)(25 * DeviceInfo.ScalingFactor),
                Color = TColor.White,
            };
            title4.Actions.Add(new MaterialIconButton
            {
                Icon = MaterialIcons.Check,
                SizeWidth = (float)(25 * DeviceInfo.ScalingFactor),
                SizeHeight = (float)(25 * DeviceInfo.ScalingFactor),
                Color = TColor.White,
            });
            title4.Actions.Add(new MaterialIconButton
            {
                Icon = MaterialIcons.Close,
                SizeWidth = (float)(25 * DeviceInfo.ScalingFactor),
                SizeHeight = (float)(25 * DeviceInfo.ScalingFactor),
                Color = TColor.White,
            });
            view.Add(title4);

            return scrollview;
        }
    }
}
