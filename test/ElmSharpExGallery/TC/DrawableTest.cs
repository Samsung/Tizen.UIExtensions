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
                WeightY = 1
            };
            refreshIcon.Show();
            layout.PackEnd(refreshIcon);

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
