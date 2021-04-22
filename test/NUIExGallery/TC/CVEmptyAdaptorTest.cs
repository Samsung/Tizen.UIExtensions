using System;
using System.Collections;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;
using Size = Tizen.UIExtensions.Common.Size;

namespace NUIExGallery.TC
{

    public class MyEmptyAdaptor : ItemAdaptor
    {

        public MyEmptyAdaptor(IEnumerable items) : base(items)
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
            var view = new View();
            view.UpdateBackgroundColor(Color.Yellow);

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
                BackgroundColor = Color.Green.ToNative()
            };
            layout.Add(new Label
            {
                Text = "Empty Item!"
            });

            return layout;
        }

        public override Size MeasureFooter(double widthConstraint, double heightConstraint)
        {
            return new Size(0, 0);
        }

        public override Size MeasureHeader(double widthConstraint, double heightConstraint)
        {
            return new Size(widthConstraint, heightConstraint);
        }

        public override Size MeasureItem(double widthConstraint, double heightConstraint)
        {
            return new Size(100, 100);
        }

        public override Size MeasureItem(int index, double widthConstraint, double heightConstraint)
        {
            return new Size(100, 100);
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

    public class CVEmptyAdaptorTest : TestCaseBase
    {
        public override string TestName => "CollectionView EmptyAdaptor Test";

        public override string TestDescription => "CollectionView EmptyAdaptor test";

        public override View Run()
        {
            var items = new List<string>();

            var adaptor = new MyEmptyAdaptor(items);

            var collectionView = new CollectionView();

            collectionView.Adaptor = adaptor;
            collectionView.LayoutManager = new LinearLayoutManager(false);

            return collectionView;
        }
    }
}
