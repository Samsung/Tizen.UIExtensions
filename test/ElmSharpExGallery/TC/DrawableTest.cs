using System.Threading.Tasks;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.Internal;
using Tizen.UIExtensions.ElmSharp;
using Button = Tizen.UIExtensions.ElmSharp.Button;
using Label = Tizen.UIExtensions.ElmSharp.Label;

namespace ElmSharpExGallery.TC
{
    public class DrawableTest : TestCaseBase, IAnimatable
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

            var simulateButton = new Button(parent)
            {
                Text = "Simulate Animation",
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            simulateButton.Clicked += async (s, e) =>
            {
                if (refreshIcon.IsPulling)
                {
                    var iconMoveLength = 150;
                    var iconGeometry = refreshIcon.Geometry;
                    var _refreshIconStartAnimation = new Animation(v => {
                        refreshIcon.PullDistance = (float)(v / iconMoveLength);
                        refreshIcon.Move(iconGeometry.X, iconGeometry.Y + (int)v);
                    }, 0, iconMoveLength, Easing.Linear);
                    _refreshIconStartAnimation.Commit(this, "RefreshStart");
                    await Task.Delay(2000);
                    var _refreshIconEndAnimation = new Animation(v => refreshIcon.Move(iconGeometry.X, iconGeometry.Y + iconMoveLength - (int)v), 0, iconMoveLength, Easing.Linear);
                    _refreshIconEndAnimation.Commit(this, "RefreshEnd");
                }
            };
            simulateButton.Show();
            layout.PackEnd(simulateButton);
        }

        public void BatchBegin() { }

        public void BatchCommit() { }
    }
}
