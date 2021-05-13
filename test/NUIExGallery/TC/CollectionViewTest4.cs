using System;
using System.Collections;
using System.Collections.ObjectModel;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;
using Size = Tizen.UIExtensions.Common.Size;

namespace NUIExGallery.TC
{
    public class CollectionViewTest4 : TestCaseBase
    {
        public override string TestName => "CollectionView Add remove test";

        public override string TestDescription => "CollectionView Add remove test";

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
                Text = "Add",
                HeightResizePolicy = ResizePolicyType.FillToParent,
                SizeWidth = 200,
            };

            hlayout.Add(more);

            var less = new Button
            {
                Text = "Remove",
                HeightResizePolicy = ResizePolicyType.FillToParent,
                SizeWidth = 200,
            };
            hlayout.Add(less);


            var items = new ObservableCollection<int>();

            var adaptor = new MyAdaptor3(items);

            var collectionView = new CollectionView();

            collectionView.Adaptor = adaptor;
            var layoutmanager = new LinearLayoutManager(false);
            collectionView.LayoutManager = layoutmanager;

            more.Clicked += (s, e) =>
            {
                items.Add(0);
            };


            less.Clicked += (s, e) =>
            {
                items.RemoveAt(0);
            };
            layout.Add(collectionView);

            return layout;
        }


        public class MyAdaptor3 : MyAdaptor2
        {

            public MyAdaptor3(IEnumerable items) : base(items)
            {

            }

            public override View GetHeaderView()
            {
                return null;
            }
        }
    }
}
