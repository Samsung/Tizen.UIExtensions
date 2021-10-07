using System.Threading.Tasks;
using Tizen.UIExtensions.ElmSharp;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;

namespace ElmSharpExGallery.TC
{
    public class RefreshLayoutTest1 : TestCaseBase
    {
        public override string TestName => "RefreshLayoutTest1";

        public override string TestDescription => "Test RefreshLayout";

        public override void Run(ElmSharp.Box parent)
        {
            var refreshLayout = new RefreshLayout(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
            };

            var contentBox = new Box(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                BackgroundColor = ElmSharp.Color.Gray
            };
            var changeIconButton = new Button(parent)
            {
                Text = "Change Icon Color",
                WeightX = 1,
                WeightY = 1,
                AlignmentX = -1,
                AlignmentY = -1,
            };
            changeIconButton.Move(100, 300);
            changeIconButton.Resize(400, 100);
            changeIconButton.Clicked += (s, e) =>
            {
                if (refreshLayout.RefreshIconColor == Color.Default)
                    refreshLayout.RefreshIconColor = Color.Red;
                else
                    refreshLayout.RefreshIconColor = Color.Default;
            };
            changeIconButton.Show();
            var statusLabel = new Label(parent)
            {
                Text = "Idle",
                WeightX = 1,
                WeightY = 1,
                AlignmentX = -1,
                AlignmentY = -1,
                TextColor = Color.Beige,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 30
            };
            statusLabel.Move(100, 200);
            statusLabel.Resize(300, 100);
            statusLabel.Show();

            var changeRefreshingButton = new Button(parent)
            {
                Text = $"Start Refreshing",
                WeightX = 1,
                WeightY = 1,
                AlignmentX = -1,
                AlignmentY = -1,
            };
            changeRefreshingButton.Move(100, 400);
            changeRefreshingButton.Resize(400, 100);
            changeRefreshingButton.Clicked += (s, e) =>
            {
                refreshLayout.IsRefreshing = true;
            };
            changeRefreshingButton.Show();

            contentBox.PackEnd(statusLabel);
            contentBox.PackEnd(changeIconButton);
            contentBox.PackEnd(changeRefreshingButton);
            contentBox.Show();

            var scrollView = new ScrollView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Default,
            };

            scrollView.SetScrollCanvas(contentBox);
            scrollView.SetContentSize(720, 2000);
            scrollView.Show();

            refreshLayout.Content = scrollView;
            refreshLayout.IsRefreshEnabled = true;
            refreshLayout.Refreshing += async (s, e) =>
            {
                statusLabel.Text = "Refreshing";
                await Task.Delay(2000);
                refreshLayout.IsRefreshing = false;
                statusLabel.Text = "Refreshed";

            };
            refreshLayout.Show();
            parent.PackEnd(refreshLayout);
        }
    }
}
