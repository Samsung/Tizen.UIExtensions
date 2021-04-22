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

    public class MyAdaptor : ItemAdaptor
    {

        public MyAdaptor(IEnumerable items) : base(items)
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
            return null;
        }

        public override Size MeasureFooter(double widthConstraint, double heightConstraint)
        {
            return new Size(0, 0);
        }

        public override Size MeasureHeader(double widthConstraint, double heightConstraint)
        {
            return new Size(0, 0);
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

    public class CollectionViewTest1 : TestCaseBase
    {
        public override string TestName => "CollectionView Test1";

        public override string TestDescription => "CollectionView test1";

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

            return collectionView;
        }
    }
}
