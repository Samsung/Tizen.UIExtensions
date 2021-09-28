using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using Button = Tizen.UIExtensions.ElmSharp.Button;
using Label = Tizen.UIExtensions.ElmSharp.Label;

namespace ElmSharpExGallery.TC
{
    public class DrawableTest : TestCaseBase
    {
        public override string TestName => "Drawable Test";

        public override string TestDescription => "Test Drawable";

        public override void Run(ElmSharp.Box parent)
        {
            int _maximumPullDistance = 100;

            var layout = new ElmSharp.Box(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            layout.Show();
            parent.PackEnd(layout);

            var label = new Label(parent)
            {
                Text = "refreshIcon",
                FontSize = 20
            };
            label.Show();
            layout.PackEnd(label);
            var refreshIcon = new RefreshIcon(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
                MaximumPullDistance = _maximumPullDistance
            };
            refreshIcon.Show();
            layout.PackEnd(refreshIcon);

            var distanceLabel = new Label(parent)
            {
                Text = "Pull Distance",
                FontSize = 20
            };
            distanceLabel.Show();
            layout.PackEnd(distanceLabel);
            var distanceSlider = new ElmSharp.Slider(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            distanceSlider.ValueChanged += (s, e) =>
            {
                refreshIcon.PullDistance = (float)distanceSlider.Value;
            };
            distanceSlider.Show();
            layout.PackEnd(distanceSlider);

            var isPullingButton = new Button(parent)
            {
                Text = $"IsPulling : {refreshIcon.IsPulling}",
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            isPullingButton.Clicked += (s, e) =>
            {
                refreshIcon.IsPulling = refreshIcon.IsPulling ? false : true;
                isPullingButton.Text = $"IsPulling : {refreshIcon.IsPulling}";
            };
            isPullingButton.Show();
            layout.PackEnd(isPullingButton);

            var MaxpullDistanceButton = new Button(parent)
            {
                Text = $"Max PullDistance: {refreshIcon.MaximumPullDistance}",
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            MaxpullDistanceButton.Clicked += (s, e) =>
            {
                if (refreshIcon.MaximumPullDistance == _maximumPullDistance)
                {
                    refreshIcon.MaximumPullDistance = _maximumPullDistance * 2;
                    MaxpullDistanceButton.Text = $"Max PullDistance: {refreshIcon.MaximumPullDistance}";
                }
                else
                {
                    refreshIcon.MaximumPullDistance = _maximumPullDistance;
                    MaxpullDistanceButton.Text = $"Max PullDistance: {refreshIcon.MaximumPullDistance}";
                }
            };
            MaxpullDistanceButton.Show();
            layout.PackEnd(MaxpullDistanceButton);
            var isRunningButton = new Button(parent)
            {
                Text = "IsRefreshing",
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            isRunningButton.Clicked += (s, e) =>
            {
                refreshIcon.IsRunning = refreshIcon.IsRunning ? false : true;
            };
            isRunningButton.Show();
            layout.PackEnd(isRunningButton);

            var colorChangeButton = new Button(parent)
            {
                Text = "Change Color",
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            colorChangeButton.Clicked += (s, e) =>
            {
                refreshIcon.Color = refreshIcon.Color == Color.Default ? Color.Red : Color.Default;
            };
            colorChangeButton.Show();
            layout.PackEnd(colorChangeButton);
        }
    }
}
