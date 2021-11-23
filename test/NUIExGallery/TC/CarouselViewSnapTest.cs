using System;
using System.Collections;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{

    public class CarouselViewSnapTest : TestCaseBase
    {

        class CarouselViewAdaptor : MyAdaptor2
        {
            public CarouselViewAdaptor(IEnumerable items) : base(items)
            {
            }

            public override View GetHeaderView()
            {
                return null;
            }

            public override Tizen.UIExtensions.Common.Size MeasureItem(int index, double widthConstraint, double heightConstraint)
            {
                return new Tizen.UIExtensions.Common.Size(720, 300);
            }
        }

        class CarouselViewAdaptor2 : CarouselViewAdaptor
        {
            public CarouselViewAdaptor2(IEnumerable items) : base(items)
            {
            }

            public override Tizen.UIExtensions.Common.Size MeasureItem(int index, double widthConstraint, double heightConstraint)
            {
                return new Tizen.UIExtensions.Common.Size(300, 300);
            }
        }

        public override string TestName => "CarouselView snap test";

        public override string TestDescription => "CarouselView snap test";

        public override View Run()
        {
            var items = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add($"Items {i}");
            }

            var adaptor = new CarouselViewAdaptor(items);

            var collectionView = new CollectionView
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            collectionView.SelectionMode = CollectionViewSelectionMode.Single;
            collectionView.Adaptor = adaptor;
            collectionView.LayoutManager = new LinearLayoutManager(true);
            collectionView.SnapPointsAlignment = SnapPointsAlignment.Center;
            collectionView.SnapPointsType = SnapPointsType.MandatorySingle;



            var collectionView2 = new CollectionView
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Adaptor = new CarouselViewAdaptor2(items),
                LayoutManager = new LinearLayoutManager(true),
                SnapPointsAlignment = SnapPointsAlignment.Center,
                SnapPointsType = SnapPointsType.MandatorySingle
            };

            var layout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Top
                }
            };

            layout.Add(collectionView);
            layout.Add(collectionView2);
            return layout;
        }
    }
}
