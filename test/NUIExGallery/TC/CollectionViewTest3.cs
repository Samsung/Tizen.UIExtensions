using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;

namespace NUIExGallery.TC
{
    public class CollectionViewTest3 : TestCaseBase
    {
        public override string TestName => "CollectionView Grid layout test2";

        public override string TestDescription => "CollectionView grid test2";

        public override View Run()
        {
            var layout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                }
            };

            var hlayout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                },
                WidthResizePolicy = ResizePolicyType.FillToParent,
                SizeHeight = 100,
            };

            layout.Add(hlayout);

            var more = new Button
            {
                Text = "More",
                HeightResizePolicy = ResizePolicyType.FillToParent,
                SizeWidth = 200,
            };

            hlayout.Add(more);

            var less = new Button
            {
                Text = "Less",
                HeightResizePolicy = ResizePolicyType.FillToParent,
                SizeWidth = 200,
            };
            hlayout.Add(less);

            var items = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add($"Items {i}");
            }

            var adaptor = new MyAdaptor2(items);

            var collectionView = new CollectionView();

            collectionView.Adaptor = adaptor;
            var layoutmanager = new GridLayoutManager(false, 3);
            collectionView.LayoutManager = layoutmanager;

            more.Clicked += (s, e) =>
            {
                layoutmanager.UpdateSpan(layoutmanager.Span + 1);
            };


            less.Clicked += (s, e) =>
            {
                layoutmanager.UpdateSpan(layoutmanager.Span - 1);
            };
            layout.Add(collectionView);

            return layout;
        }
    }
}
