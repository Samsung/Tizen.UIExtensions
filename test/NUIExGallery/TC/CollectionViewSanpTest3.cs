using System.Collections;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;

namespace NUIExGallery.TC
{

    public class CollectionViewSnapTest3 : TestCaseBase
    {

        class SnapAdaptor : MyAdaptor2
        {
            public SnapAdaptor(IEnumerable items) : base(items)
            {
            }

            public override View GetHeaderView()
            {
                return null;
            }

            public override Tizen.UIExtensions.Common.Size MeasureItem(int index, double widthConstraint, double heightConstraint)
            {
                return new Tizen.UIExtensions.Common.Size(300, 300);
            }
        }

        public override string TestName => "CollectionView Snap Test3";

        public override string TestDescription => "CollectionView Snap Test3";

        public override View Run()
        {
            var items = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add($"Items {i}");
            }

            var adaptor = new SnapAdaptor(items);

            var collectionView = new CollectionView()
            {
                Adaptor = adaptor,
                LayoutManager = new GridLayoutManager(false, 2),
                SnapPointsAlignment = SnapPointsAlignment.Center,
                SnapPointsType = SnapPointsType.Mandatory
            };

            var layout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                }
            };

            {
                var label = new Label
                {
                    FontSize = 10,
                    Text = "SnapPointType: Mandatory"
                };
                layout.Add(label);

                var horizontal = new View
                {
                    Layout = new LinearLayout
                    {
                        LinearOrientation = LinearLayout.Orientation.Horizontal
                    }
                };
                layout.Add(horizontal);
                var typeNone = new Button
                {
                    Text = "None",
                    SizeWidth = 200,
                };
                horizontal.Add(typeNone);
                typeNone.Clicked += (s, e) =>
                {
                    collectionView.SnapPointsType = SnapPointsType.None;
                    label.Text = "SnapPointType: None";
                };

                var typeMandatory = new Button
                {
                    Text = "Mandatory",
                    SizeWidth = 200,
                };
                horizontal.Add(typeMandatory);
                typeMandatory.Clicked += (s, e) =>
                {
                    collectionView.SnapPointsType = SnapPointsType.Mandatory;
                    label.Text = "SnapPointType: Mandatory";
                };

                var typeSingle = new Button
                {
                    Text = "MandatorySingle",
                    SizeWidth = 200,
                };
                horizontal.Add(typeSingle);
                typeSingle.Clicked += (s, e) =>
                {
                    collectionView.SnapPointsType = SnapPointsType.MandatorySingle;
                    label.Text = "SnapPointType: MandatorySingle";
                };

            }

            {
                var label = new Label
                {
                    FontSize = 10,
                    Text = "SnapPointsAlignment: Center"
                };
                layout.Add(label);

                var horizontal = new View
                {
                    Layout = new LinearLayout
                    {
                        LinearOrientation = LinearLayout.Orientation.Horizontal
                    }
                };
                layout.Add(horizontal);
                var start = new Button
                {
                    Text = "Start",
                    SizeWidth = 200,
                };
                horizontal.Add(start);
                start.Clicked += (s, e) =>
                {
                    collectionView.SnapPointsAlignment = SnapPointsAlignment.Start;
                    label.Text = "SnapPointsAlignment: Start";
                };

                var center = new Button
                {
                    Text = "Center",
                    SizeWidth = 200,
                };
                horizontal.Add(center);
                center.Clicked += (s, e) =>
                {
                    collectionView.SnapPointsAlignment = SnapPointsAlignment.Center;
                    label.Text = "SnapPointsAlignment: Center";
                };

                var end = new Button
                {
                    Text = "End",
                    SizeWidth = 200,
                };
                horizontal.Add(end);
                end.Clicked += (s, e) =>
                {
                    collectionView.SnapPointsAlignment = SnapPointsAlignment.End;
                    label.Text = "SnapPointsAlignment: End";
                };

            }

            layout.Add(collectionView);
            return layout;
        }
    }
}
