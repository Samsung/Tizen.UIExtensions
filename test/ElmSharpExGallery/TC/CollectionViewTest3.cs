using ElmSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using Ext = Tizen.UIExtensions.ElmSharp;

namespace ElmSharpExGallery.TC
{
    public class CollectionViewTest3 : TestCaseBase
    {
        public override string TestName => "CollectionViewTest3";

        public override string TestDescription => "CollectionViewTest3";

        public override void Run(Box parent)
        {
            var collectionView = new Ext.CollectionView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            collectionView.Show();

            var items = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                items.Add($"Items {i}");

            }
            var adaptor = new MyAdaptor(items);
            collectionView.Adaptor = adaptor;
            collectionView.LayoutManager = new Ext.LinearLayoutManager(true, Ext.ItemSizingStrategy.MeasureAllItems, 10);

            parent.PackEnd(collectionView);
        }

        public class MyAdaptor : Ext.ItemAdaptor
        {

            public MyAdaptor(IEnumerable items) : base(items)
            {

            }

            public override EvasObject CreateNativeView(EvasObject parent)
            {
                var label = new Label(parent)
                {
                    Text = "Default label",
                    BackgroundColor = Color.Gray,
                };
                label.Show();
                return label;
            }

            public override EvasObject CreateNativeView(int index, EvasObject parent)
            {
                var label = new Label(parent)
                {
                    Text = $"Created [{index}] label",
                    BackgroundColor = Color.Yellow,
                };
                label.Show();
                return label;
            }


            public override EvasObject GetFooterView(EvasObject parent)
            {
                var label = new Label(parent)
                {
                    Text = $"footer",
                    BackgroundColor = Color.Green,
                };
                label.Show();
                return label;
            }

            public override EvasObject GetHeaderView(EvasObject parent)
            {
                var label = new Label(parent)
                {
                    Text = $"header",
                    BackgroundColor = Color.Blue,
                };
                label.Show();
                return label;
            }

            public override Size MeasureFooter(int widthConstraint, int heightConstraint)
            {
                return new Size(100, 100);
            }

            public override Size MeasureHeader(int widthConstraint, int heightConstraint)
            {
                return new Size(100, 100);
            }

            public override Size MeasureItem(int widthConstraint, int heightConstraint)
            {
                return new Size(150, 150);
            }

            public override Size MeasureItem(int index, int widthConstraint, int heightConstraint)
            {
                if (index % 2 == 0)
                    return new Size(150, 150);
                else
                    return new Size(200, 200);
            }

            public override void RemoveNativeView(EvasObject native)
            {
                native.Unrealize();
            }

            public override void SetBinding(EvasObject view, int index)
            {
                (view as Label).Text = $"Binding {index}";
            }

            public override void UnBinding(EvasObject view)
            {
                (view as Label).Text = $"UnBinding";
            }
        }
    }
}
