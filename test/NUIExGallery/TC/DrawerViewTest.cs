using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;

namespace NUIExGallery.TC
{
    public class FocusDisabledDrawerView : DrawerView
    {
        public FocusDisabledDrawerView() : base(true) {}

        protected override void OnDrawerFocusGained(object? sender, EventArgs args)
        {
        }

        protected override void OnContentFocusGained(object? sender, EventArgs args)
        {
        }
    }

    public class DrawerViewTest : TestCaseBase
    {
        public override string TestName => "DrawerViewTest test";

        public override string TestDescription => "DrawerViewTest test";

        public override View Run()
        {
            var drawerView = new FocusDisabledDrawerView();
            drawerView.DrawerShadow = new Shadow(30.0f, Tizen.NUI.Color.Red, new Vector2(10, 0));

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
                Text = "Open"
            };

            var closeButton = new Button
            {
                Text = "Close"
            };

            var modeButton = new Button
            {
                Text = $"IsPopover={drawerView.IsPopover}"
            };

            var behaviorButton = new Button
            {
                Text = $"Behavior : {drawerView.DrawerBehavior}"
            };

            openButton.Clicked += (s, e) =>
            {
                _ = drawerView.OpenAsync(false);
            };

            closeButton.Clicked += (s, e) =>
            {
                _ = drawerView.CloseAsync(false);
            };

            modeButton.Clicked += (s, e) =>
            {
                drawerView.IsPopover = !drawerView.IsPopover;
                modeButton.Text = $"IsPopover={drawerView.IsPopover}";
            };

            behaviorButton.Clicked += (s, e) =>
            {
                if (drawerView.DrawerBehavior == Tizen.UIExtensions.Common.DrawerBehavior.Drawer)
                {
                    drawerView.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Locked;   
                }
                else if (drawerView.DrawerBehavior == Tizen.UIExtensions.Common.DrawerBehavior.Locked)
                {
                    drawerView.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Disabled;
                }
                else
                {
                    drawerView.DrawerBehavior = Tizen.UIExtensions.Common.DrawerBehavior.Drawer;
                }

                behaviorButton.Text = $"hange behavior : {drawerView.DrawerBehavior}";
            };

            viewGroup.Children.Add(openButton);
            viewGroup.Children.Add(closeButton);
            viewGroup.Children.Add(modeButton);
            viewGroup.Children.Add(behaviorButton);

            var naviView = new ViewGroup
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            naviView.BackgroundColor = Tizen.NUI.Color.Cyan;

            var shadowButton = new Button
            {
                Text = $"shadow = {(drawerView.DrawerShadow != null)}",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };
            shadowButton.Clicked += (s, e) =>
            {
                var shadow = new Shadow(30.0f, Tizen.NUI.Color.Red, new Vector2(10, 0));
                drawerView.DrawerShadow = (drawerView.DrawerShadow == null) ? shadow : null;
                shadowButton.Text = $"shadow = {(drawerView.DrawerShadow != null)}";
            };

            naviView.Add(shadowButton);

            drawerView.Content = viewGroup;
            drawerView.Drawer = naviView;

            return drawerView;
        }
    }
}