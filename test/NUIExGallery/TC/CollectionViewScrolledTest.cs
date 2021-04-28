using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using System.Collections.Generic;

namespace NUIExGallery.TC
{

    public class CollectionViewScrolledTest : TestCaseBase
    {
        public override string TestName => "CollectionView Scrolled event Test1";

        public override string TestDescription => "CollectionView Scrolled event test1";

        public override View Run()
        {
            var items = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add($"Items {i}");
            }

            var adaptor = new MyAdaptor(items);

            var collectionView = new CollectionView();
            collectionView.Adaptor = adaptor;
            collectionView.LayoutManager = new LinearLayoutManager(false);

            var layout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Top
                }
            };

            var label = new Label
            {
                FontSize = 10,
                Text = "Scrolled:"
            };

            collectionView.Scrolled += (s, e) =>
            {
                label.Text = $"Scrolled F[{e.FirstVisibleItemIndex}], C[{e.CenterItemIndex}], L[{e.LastVisibleItemIndex}] - {e.VerticalOffset}";
            };

            layout.Add(label);
            layout.Add(collectionView);
            return layout;
        }
    }
}
