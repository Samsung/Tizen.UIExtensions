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
    public class CollectionViewFocusTest : TestCaseBase
    {
        public override string TestName => "CollectionView Focus Test";

        public override string TestDescription => "CollectionView Focus Test";

        public override View Run()
        {
            var items = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add($"Items {i}");
            }

            var adaptor = new FocusableMyAdaptor(items);

            var collectionView = new CollectionView();
            collectionView.Adaptor = adaptor;
            collectionView.LayoutManager = new LinearLayoutManager(false);

            return collectionView;
        }

        public class FocusableMyAdaptor : ItemAdaptor
        {

            public FocusableMyAdaptor(IEnumerable items) : base(items)
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

                var wrapper = new MyWrapper
                {
                    Focusable = true,
                    FocusableInTouch = true,
                    Layout = new AbsoluteLayout(),

                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                };
                wrapper.UpdateBackgroundColor(Color.Yellow);
                var item = new TextLabel
                {
                    Text = $"Text for{index}",
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                    HeightResizePolicy = ResizePolicyType.FillToParent,
                    WidthResizePolicy = ResizePolicyType.FillToParent,
                };
                wrapper.Add(item);
                return wrapper;
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

        class MyWrapper : View
        {

        }
    }
}
