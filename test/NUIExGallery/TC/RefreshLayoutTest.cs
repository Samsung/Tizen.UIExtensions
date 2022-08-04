using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.NUI.GraphicsView;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class RefreshLayoutTest : TestCaseBase
    {
        public override string TestName => "RefreshLayout Test";

        public override string TestDescription => "RefreshLayout test";

        public override View Run()
        {
            var main = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical
                },
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };

            main.Add(new Label
            {
                Text = "Refresh Layout Test",
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

            var scrollview = new Tizen.UIExtensions.NUI.ScrollView()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };

            scrollview.ContentContainer.Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
            };

            for (int i = 0; i < 3; i++)
            {
                var rnd = new Random();
                var child = new View
                {
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    SizeHeight = 50,
                };
                child.BackgroundColor = new Tizen.NUI.Color(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle(), 1);
                scrollview.Add(child);
            }

            var startRefresh = new Tizen.UIExtensions.NUI.Button()
            {
                Text = "StartRefresh"
            };
            scrollview.Add(startRefresh);

            var changeBg = new Tizen.UIExtensions.NUI.Button()
            {
                Text = "Change background"
            };
            scrollview.Add(changeBg);


            var changeColor = new Tizen.UIExtensions.NUI.Button()
            {
                Text = "Change Color"
            };
            scrollview.Add(changeColor);

            for (int i = 0; i < 100; i++)
            {
                var rnd = new Random();
                var child = new View
                {
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    SizeHeight = 50,
                };
                child.BackgroundColor = new Tizen.NUI.Color(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle(), 1);
                scrollview.Add(child);
            }



            var wrapperView = new View
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            wrapperView.Add(scrollview);

            var refreshView = new RefreshLayout()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Content = wrapperView,
            };
            refreshView.Refreshing += async (s, e) =>
            {
                await System.Threading.Tasks.Task.Delay(2000);
                refreshView.IsRefreshing = false;
            };
            main.Add(refreshView);


            startRefresh.Clicked += (s, e) => refreshView.IsRefreshing = true;
            changeBg.Clicked += (s, e) =>
            {
                if (refreshView.IconBackgroundColor == Color.Default)
                {
                    refreshView.IconBackgroundColor = Color.Green;
                }
                else
                {
                    refreshView.IconBackgroundColor = Color.Default;
                }
            };
            changeColor.Clicked += (s, e) =>
            {
                if (refreshView.IconColor == Color.Default)
                {
                    refreshView.IconColor = Color.Red;
                }
                else
                {
                    refreshView.IconColor = Color.Default;
                }
            };

            return main;
        }
    }
}
