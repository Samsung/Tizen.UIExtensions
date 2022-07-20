using System;
using System.Threading;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;

namespace NUIExGallery.TC
{
    public class TVNavigationDrawerTest : TestCaseBase
    {
        public override string TestName => "DrawerView(TVNavigationDrawer) Test";

        public override string TestDescription => "DrawerView(TVNavigationDrawer) Test";

        public override View Run()
        {
            var tvNaviDrawer = new TVNavigationDrawer();
            tvNaviDrawer.DrawerShadow = new Shadow(30.0f, Tizen.NUI.Color.Red, new Vector2(10, 0));

            var viewGroup = new ViewGroup();
            viewGroup.BackgroundColor = Tizen.NUI.Color.Yellow;
            viewGroup.LayoutUpdated += (s, e) =>
            {
                var blockSize = viewGroup.Size.Height / viewGroup.Children.Count;
                var currentTop = 0f;
                foreach (var child in viewGroup.Children)
                {
                    child.UpdateBounds(new Tizen.UIExtensions.Common.Rect(0, currentTop, viewGroup.Size.Width, blockSize));
                    currentTop += blockSize;
                }
            };
            var openButton = new Button
            {
                Text = "Open with animation",
            };
            var openButton2 = new Button
            {
                Text = "Open",
            };

            openButton.Clicked += (s, e) =>
            {
                tvNaviDrawer.OpenAsync(true);
            };
            openButton2.Clicked +=  (s, e) =>
            {
                tvNaviDrawer.OpenAsync();
            };

            var behaviorButton = new Button
            {
                Text = $"Behavior : {tvNaviDrawer.DrawerBehavior}"
            };

            behaviorButton.Clicked += (s, e) =>
            {
                if (tvNaviDrawer.DrawerBehavior == Tizen.UIExtensions.Common.DrawerBehavior.Drawer)
                {
                    tvNaviDrawer.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Locked;   
                }
                else if (tvNaviDrawer.DrawerBehavior == Tizen.UIExtensions.Common.DrawerBehavior.Locked)
                {
                    tvNaviDrawer.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Disabled;
                }
                else
                {
                    tvNaviDrawer.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Drawer;
                }

                behaviorButton.Text = $"hange behavior : {tvNaviDrawer.DrawerBehavior}";
            };

            viewGroup.Children.Add(openButton);
            viewGroup.Children.Add(openButton2);
            viewGroup.Children.Add(behaviorButton);

            tvNaviDrawer.Content = viewGroup;

            var naviView = new ViewGroup()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            naviView.BackgroundColor = Tizen.NUI.Color.Cyan;
            naviView.LayoutUpdated += (s, e) =>
            {
                var blockSize = naviView.Size.Height / naviView.Children.Count;
                var currentTop = 0f;
                foreach (var child in naviView.Children)
                {
                    child.UpdateBounds(new Tizen.UIExtensions.Common.Rect(0, currentTop, naviView.Size.Width, blockSize));
                    currentTop += blockSize;
                }
            };

            var shadowButton = new Button
            {
                Text = $"shadow = {(tvNaviDrawer.DrawerShadow != null)}",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };
            shadowButton.Clicked += (s, e) =>
            {
                var shadow = new Shadow(30.0f, Tizen.NUI.Color.Red, new Vector2(10, 0));
                tvNaviDrawer.DrawerShadow = (tvNaviDrawer.DrawerShadow == null) ? shadow : null;
                shadowButton.Text = $"shadow = {(tvNaviDrawer.DrawerShadow != null)}";
            };

            var newButton = new Button
            {
                Text = "new content"
            };

            newButton.Clicked += (s, e) =>
            {
                tvNaviDrawer.Content = new View
                {
                    BackgroundColor = Color.Green,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                };
            };

            naviView.Children.Add(shadowButton);
            naviView.Children.Add(newButton);

            tvNaviDrawer.Drawer = naviView;

            return tvNaviDrawer;
        }
    }
}