using System;
using System.Threading;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;

namespace NUIExGallery.TC
{
    public class NavigationDrawerTest : TestCaseBase
    {
        public override string TestName => "DrawerView(NavigationDrawer) Test";

        public override string TestDescription => "DrawerView(NavigationDrawer) Test";

        public override View Run()
        {
            var navigationDrawer = new NavigationDrawer();
            navigationDrawer.DrawerShadow = new Shadow(30.0f, Tizen.NUI.Color.Red, new Vector2(10, 0));

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
                Text = "Open with animation"
            };
            var openButton2 = new Button
            {
                Text = "Open"
            };
            var gestureButton = new Button
            {
                Text = $"Gesture: {navigationDrawer.IsGestureEnabled}"
            };

            openButton.Clicked += (s, e) =>
            {
                navigationDrawer.OpenDrawer(true);
            };
            openButton2.Clicked += (s, e) =>
            {
                navigationDrawer.OpenDrawer();
            };

            gestureButton.Clicked += (s, e) =>
            {
                navigationDrawer.IsGestureEnabled = !navigationDrawer.IsGestureEnabled;
                gestureButton.Text = $"GestureEnabled: {navigationDrawer.IsGestureEnabled}";
            };

            var behaviorButton = new Button
            {
                Text = $"Behavior : {navigationDrawer.DrawerBehavior}"
            };

            behaviorButton.Clicked += (s, e) =>
            {
                if (navigationDrawer.DrawerBehavior == Tizen.UIExtensions.Common.DrawerBehavior.Drawer)
                {
                    navigationDrawer.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Locked;
                }
                else if (navigationDrawer.DrawerBehavior == Tizen.UIExtensions.Common.DrawerBehavior.Locked)
                {
                    navigationDrawer.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Disabled;
                }
                else
                {
                    navigationDrawer.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Drawer;
                }

                behaviorButton.Text = $"hange behavior : {navigationDrawer.DrawerBehavior}";
            };

            viewGroup.Children.Add(openButton);
            viewGroup.Children.Add(openButton2);
            viewGroup.Children.Add(gestureButton);
            viewGroup.Children.Add(behaviorButton);

            navigationDrawer.Content = viewGroup;
            navigationDrawer.IsGestureEnabled = false;

            var backdrop = new ViewGroup();
            backdrop.BackgroundColor = Tizen.NUI.Color.Blue;
            backdrop.Opacity = 0.5f;
            navigationDrawer.BackDrop = backdrop;

            var naviView = new ViewGroup
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
                Text = $"shadow = {(navigationDrawer.DrawerShadow != null)}",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };
            shadowButton.Clicked += (s, e) =>
            {
                var shadow = new Shadow(30.0f, Tizen.NUI.Color.Red, new Vector2(10, 0));
                navigationDrawer.DrawerShadow = (navigationDrawer.DrawerShadow == null) ? shadow : null;
                shadowButton.Text = $"shadow = {(navigationDrawer.DrawerShadow != null)}";
            };

            var newButon = new Button
            {
                Text = "new content",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };

            newButon.Clicked += (s, e) =>
            {
                navigationDrawer.Content = new View
                {
                    BackgroundColor = Color.Green,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                };
            };

            var backdropButton = new Button
            {
                Text = "backdrop",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };

            backdropButton.Clicked += (s, e) =>
            {
                var backdrop = new ViewGroup();
                backdrop.BackgroundColor = Tizen.NUI.Color.Red;
                backdrop.Opacity = 0.5f;
                navigationDrawer.BackDrop = backdrop;
            };

            naviView.Add(shadowButton);
            naviView.Add(newButon);
            naviView.Add(backdropButton);

            navigationDrawer.Drawer = naviView;

            return navigationDrawer;
        }
    }
}