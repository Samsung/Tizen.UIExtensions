using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.Common;
using System.Collections;
using Color = Tizen.UIExtensions.Common.Color;
using Size = Tizen.UIExtensions.Common.Size;
using System.Collections.Generic;

namespace NUIExGallery.TC
{

    public class MyAdaptor2 : ItemAdaptor
    {

        public MyAdaptor2(IEnumerable items) : base(items)
        {

        }

        public override View CreateNativeView()
        {
            Console.WriteLine($"CreateNativeView...");
            var view = new View();
            view.UpdateBackgroundColor(Color.Gray);

            view.Add(new TextLabel
            {
                Text = "Default text"
            });
            return view;
        }

        public override View CreateNativeView(int index)
        {
            Console.WriteLine($"CreateNativeView... for {index}");

            var rnd = new Random();
            var view = new View();

            view.UpdateBackgroundColor(Color.FromRgb(rnd.Next(10, 255), rnd.Next(10, 255), rnd.Next(10, 255)));

            view.Add(new TextLabel
            {
                Text = $"Text for{index}"
            });
            return view;
        }

        public override View GetFooterView()
        {
            return null;
        }

        public override View GetHeaderView()
        {
            var layout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Center,
                },
                BackgroundColor = Color.BlueViolet.ToNative()
            };
            layout.Add(new Label
            {
                Text = "Header!"
            });

            return layout;
        }

        public override Size MeasureFooter(double widthConstraint, double heightConstraint)
        {
            return new Size(0, 0);
        }

        public override Size MeasureHeader(double widthConstraint, double heightConstraint)
        {
            return new Size(100, 300);
        }

        public override Size MeasureItem(double widthConstraint, double heightConstraint)
        {
            return MeasureItem(0, widthConstraint, heightConstraint);
        }

        public override Size MeasureItem(int index, double widthConstraint, double heightConstraint)
        {
            return new Size(100, 300);
        }

        public override void RemoveNativeView(View native)
        {
            native.Unparent();
            native.Dispose();
        }

        public override void SetBinding(View view, int index)
        {
            (view.Children[0] as TextLabel).Text = $"(U) - View for {index}";
        }

        public override void UnBinding(View view)
        {
            (view.Children[0] as TextLabel).Text = $"default!!";
        }
    }

    public class CollectionViewTest2 : TestCaseBase
    {
        public override string TestName => "CollectionView Grid layout test";

        public override string TestDescription => "CollectionView grid test";

        public override View Run()
        {
            var items = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add($"Items {i}");
            }

            var adaptor = new MyAdaptor2(items);

            var collectionView = new CollectionView();

            collectionView.Adaptor = adaptor;
            collectionView.LayoutManager = new GridLayoutManager(false, 3);

            return collectionView;
        }
    }
}
