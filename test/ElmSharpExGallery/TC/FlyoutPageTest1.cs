using ElmSharp;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using EColor = ElmSharp.Color;

namespace ElmSharpExGallery.TC
{
    public class FlyoutPageTest1 : TestCaseBase
    {
        public override string TestName => "FlyoutPage Test 1";

        public override string TestDescription => "FlyoutPage Test 1";

        public override void Run(ElmSharp.Box parent)
        {
            var flyoutBox = new ElmSharp.Box(parent);
            var detailBox = new ElmSharp.Box(parent);

            var flyoutPage = new FlyoutPage(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
                IsPresented = false,
                Flyout = flyoutBox,
                Detail = detailBox,
            };

            flyoutPage.DimArea.BackgroundColor = EColor.Black;
            flyoutPage.DimArea.Opacity = 30;

            var list = new GenList(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1
            };

            GenItemClass defaultClass = new GenItemClass("default")
            {
                GetTextHandler = (data, part) =>
                {
                    return data as string;
                }
            };

            for (int i = 0; i < 10; i++)
            {
                list.Append(defaultClass, $"Flyout List Item {i}");
            }

            list.Show();
            flyoutBox.PackEnd(list);

            var openLabel = new ElmSharp.Label(parent)
            {
                AlignmentX = 0.5,
                WeightX = 1,
                Text = $"IsPresented: {flyoutPage.IsPresented}"
            };

            var gestureLabel = new ElmSharp.Label(parent)
            {
                AlignmentX = 0.5,
                WeightX = 1,
                Text = $"IsGestureEnabled: {flyoutPage.IsGestureEnabled}"
            };

            var behaviorLabel = new ElmSharp.Label(parent)
            {
                AlignmentX = 0.5,
                WeightX = 1,
                Text = $"FlyoutLayoutBehavior: {flyoutPage.FlyoutLayoutBehavior}"
            };

            var openButton = new ElmSharp.Button(parent)
            {
                AlignmentX = -1,
                WeightX = 1,
                Text = "Open Flyout"
            };

            var gestureButton = new ElmSharp.Button(parent)
            {
                AlignmentX = -1,
                WeightX = 1,
                Text = "Enable/Disable Gesture"
            };

            var behaviorButton = new ElmSharp.Button(parent)
            {
                AlignmentX = -1,
                WeightX = 1,
                Text = "Default/Split Behavior"
            };

            var sRatioButton = new ElmSharp.Button(parent)
            {
                AlignmentX = -1,
                WeightX = 1,
                Text = "Split ratio + 10"
            };

            var dRatioButton = new ElmSharp.Button(parent)
            {
                AlignmentX = -1,
                WeightX = 1,
                Text = "popover ratio + 10"
            };

            var sRatioButton2 = new ElmSharp.Button(parent)
            {
                AlignmentX = -1,
                WeightX = 1,
                Text = "Split ratio - 10"
            };

            var dRatioButton2 = new ElmSharp.Button(parent)
            {
                AlignmentX = -1,
                WeightX = 1,
                Text = "popover ratio - 10"
            };

            list.ItemSelected += (s, e) =>
            {
                flyoutPage.IsPresented = false;
            };

            flyoutPage.IsPresentedChanged += (s, e) =>
            {
                openLabel.Text = $"IsPresented: {flyoutPage.IsPresented}";
            };

            openButton.Clicked += (s, e) =>
            {
                flyoutPage.IsPresented = !flyoutPage.IsPresented;
            };

            gestureButton.Clicked += (s, e) =>
            {
                flyoutPage.IsGestureEnabled = !flyoutPage.IsGestureEnabled;
                gestureLabel.Text = $"IsGestureEnabled: {flyoutPage.IsGestureEnabled}";
            };


            sRatioButton.Clicked += (s, e) =>
            {
                flyoutPage.SplitRatio += 0.1;
            };

            sRatioButton2.Clicked += (s, e) =>
            {
                flyoutPage.SplitRatio -= 0.1;
            };

            dRatioButton.Clicked += (s, e) =>
            {
                flyoutPage.PopoverRatio += 0.1;
            };

            dRatioButton2.Clicked += (s, e) =>
            {
                flyoutPage.PopoverRatio -= 0.1;
            };

            behaviorButton.Clicked += (s, e) =>
            {
                if (flyoutPage.FlyoutLayoutBehavior == FlyoutLayoutBehavior.Default)
                    flyoutPage.FlyoutLayoutBehavior = FlyoutLayoutBehavior.Split;
                else
                    flyoutPage.FlyoutLayoutBehavior = FlyoutLayoutBehavior.Default;

                behaviorLabel.Text = $"FlyoutLayoutBehavior: {flyoutPage.FlyoutLayoutBehavior}";
                openLabel.Text = $"IsPresented: {flyoutPage.IsPresented}";
            };


            openLabel.Show();
            gestureLabel.Show();
            behaviorLabel.Show();

            openButton.Show();
            gestureButton.Show();
            behaviorButton.Show();
            sRatioButton.Show();
            sRatioButton2.Show();
            dRatioButton.Show();
            dRatioButton2.Show();

            detailBox.PackEnd(openLabel);
            detailBox.PackEnd(gestureLabel);
            detailBox.PackEnd(behaviorLabel);

            detailBox.PackEnd(openButton);
            detailBox.PackEnd(gestureButton);
            detailBox.PackEnd(behaviorButton);
            detailBox.PackEnd(sRatioButton);
            detailBox.PackEnd(sRatioButton2);
            detailBox.PackEnd(dRatioButton);
            detailBox.PackEnd(dRatioButton2);

            flyoutBox.Show();
            detailBox.Show();
            flyoutPage.Show();
            parent.PackEnd(flyoutPage);
        }
    }
}
