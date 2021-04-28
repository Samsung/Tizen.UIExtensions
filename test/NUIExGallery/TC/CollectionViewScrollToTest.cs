using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;

namespace NUIExGallery.TC
{

    public class CollectionViewScrollToTest : TestCaseBase
    {
        public override string TestName => "CollectionView ScrollTo Test1";

        public override string TestDescription => "CollectionView Scrolled event test1";

        public override View Run()
        {
            var items = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add($"Items {i}");
            }

            var adaptor = new MyAdaptor(items);

            var collectionView = new CollectionView()
            {
                SizeHeight = 800,
            };
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

            var hlayout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                }
            };
            layout.Add(hlayout);

            var first = new Button
            {
                SizeWidth = 150,
                Text = "Go(1)"
            };
            first.Clicked += (s, e) => collectionView.ScrollTo(1, Tizen.UIExtensions.Common.ScrollToPosition.Start, true);
            hlayout.Add(first);

            var go50 = new Button
            {
                SizeWidth = 150,
                Text = "Go(50)"
            };
            go50.Clicked += (s, e) => collectionView.ScrollTo(50, Tizen.UIExtensions.Common.ScrollToPosition.Start, true);
            hlayout.Add(go50);

            var go100 = new Button
            {
                SizeWidth = 150,
                Text = "Go(100)"
            };
            go100.Clicked += (s, e) => collectionView.ScrollTo(100, Tizen.UIExtensions.Common.ScrollToPosition.End, true);
            hlayout.Add(go100);

            var go150 = new Button
            {
                SizeWidth = 150,
                Text = "Go(150)"
            };
            go150.Clicked += (s, e) => collectionView.ScrollTo(150, Tizen.UIExtensions.Common.ScrollToPosition.End, true);
            hlayout.Add(go150);


            layout.Add(collectionView);
            return layout;
        }
    }
}
