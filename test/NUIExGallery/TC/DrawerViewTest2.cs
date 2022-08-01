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

        NavigationDrawer _navigationDrawer;

        public override View Run()
        {
            _navigationDrawer = new NavigationDrawer();
            _navigationDrawer.DrawerShadow = new Shadow(30.0f, Tizen.NUI.Color.Red, new Vector2(10, 0));
            _navigationDrawer.Toggled += (s, e) =>
            {
                Console.WriteLine($"drawerView toggled!!!");
            };

            _navigationDrawer.Content = CreateContent();
            _navigationDrawer.IsGestureEnabled = false;

            var backdrop = new ViewGroup();
            backdrop.BackgroundColor = Tizen.NUI.Color.Blue;
            //backdrop.Opacity = 0.5f;
            _navigationDrawer.Backdrop = backdrop;

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
                Text = $"shadow = {(_navigationDrawer.DrawerShadow != null)}",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };
            shadowButton.Clicked += (s, e) =>
            {
                var shadow = new Shadow(30.0f, Tizen.NUI.Color.Red, new Vector2(10, 0));
                _navigationDrawer.DrawerShadow = (_navigationDrawer.DrawerShadow == null) ? shadow : null;
                shadowButton.Text = $"shadow = {(_navigationDrawer.DrawerShadow != null)}";
            };


            var contentButon = new Button
            {
                Text = "content",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };

            contentButon.Clicked += (s, e) =>
            {
                _navigationDrawer.Content = CreateContent();
            };

            var newContentButon = new Button
            {
                Text = "new content",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };

            newContentButon.Clicked += (s, e) =>
            {
                _navigationDrawer.Content = new View
                {
                    BackgroundColor = Color.White,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                };
            };

            var scrollableContentButton = new Button
            {
                Text = "scroll content",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };

            scrollableContentButton.Clicked += (s, e) =>
            {
                var test = new ScrollViewTest1();
                var content = test.Run();
                content.WidthSpecification = LayoutParamPolicies.MatchParent;
                content.HeightSpecification = LayoutParamPolicies.MatchParent;
                _navigationDrawer.Content = content;
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
                _navigationDrawer.Backdrop = backdrop;
            };

            naviView.Add(shadowButton);
            naviView.Add(contentButon);
            naviView.Add(newContentButon);
            naviView.Add(scrollableContentButton);
            naviView.Add(backdropButton);

            _navigationDrawer.Drawer = naviView;

            return _navigationDrawer;
        }

        View CreateContent()
        {
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
            var closeButton = new Button
            {
                Text = "Close"
            };
            var gestureButton = new Button
            {
                Text = $"Gesture: {_navigationDrawer.IsGestureEnabled}"
            };

            openButton.Clicked += (s, e) =>
            {
                _ = _navigationDrawer.OpenAsync(true);
            };
            openButton2.Clicked += (s, e) =>
            {
                _ = _navigationDrawer.OpenAsync();
            };
            closeButton.Clicked += (s, e) =>
            {
                _ = _navigationDrawer.CloseAsync(true);
            };
            gestureButton.Clicked += (s, e) =>
            {
                _navigationDrawer.IsGestureEnabled = !_navigationDrawer.IsGestureEnabled;
                gestureButton.Text = $"GestureEnabled: {_navigationDrawer.IsGestureEnabled}";
            };

            var behaviorButton = new Button
            {
                Text = $"Behavior : {_navigationDrawer.DrawerBehavior}"
            };

            behaviorButton.Clicked += (s, e) =>
            {
                if (_navigationDrawer.DrawerBehavior == Tizen.UIExtensions.Common.DrawerBehavior.Drawer)
                {
                    _navigationDrawer.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Locked;
                }
                else if (_navigationDrawer.DrawerBehavior == Tizen.UIExtensions.Common.DrawerBehavior.Locked)
                {
                    _navigationDrawer.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Disabled;
                }
                else
                {
                    _navigationDrawer.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Drawer;
                }

                behaviorButton.Text = $"hange behavior : {_navigationDrawer.DrawerBehavior}";
            };

            viewGroup.Children.Add(openButton);
            viewGroup.Children.Add(openButton2);
            viewGroup.Children.Add(closeButton);
            viewGroup.Children.Add(gestureButton);
            viewGroup.Children.Add(behaviorButton);

            return viewGroup;
        }
    }
}