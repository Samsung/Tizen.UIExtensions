using System.Collections;
using System.Collections.Generic;
using Tizen.Applications;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;
using Size = Tizen.UIExtensions.Common.Size;

namespace NUIExGallery.TC
{

    public class MyHeaderAdaptor : ItemAdaptor
    {

        public MyHeaderAdaptor(IEnumerable items) : base(items)
        {
        }

        public override View CreateNativeView()
        {
            var view = new View();
            view.UpdateBackgroundColor(Color.Gray);

            view.Add(new TextLabel
            {
                Text = "Default text"
            });
            view.Add(new ImageView
            {
                ResourceUrl = Application.Current.DirectoryInfo.Resource + "image2.jpg",
            });
            return view;
        }

        public override View CreateNativeView(int index)
        {
            var view = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                }
            };
            view.UpdateBackgroundColor(Color.Yellow);

            view.Add(new TextLabel
            {
                Text = $"Text for{index}"
            });
            view.Add(new ImageView
            {
                ResourceUrl = Application.Current.DirectoryInfo.Resource + (index % 2 == 0 ? "image2.jpg" : "image.png"),
            });
            return view;
        }

        public override View GetFooterView()
        {
            var layout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Center,
                },
                BackgroundColor = Color.SkyBlue.ToNative()
            };
            layout.Add(new Label
            {
                Text = "Footer!"
            });

            return layout;
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
            return new Size(200, 200);
        }

        public override Size MeasureHeader(double widthConstraint, double heightConstraint)
        {
            return new Size(200, 300);
        }

        public override Size MeasureItem(double widthConstraint, double heightConstraint)
        {
            return new Size(100, 300);
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

    public class CVHeaderTest : TestCaseBase
    {
        public override string TestName => "CollectionView Header Test";

        public override string TestDescription => "CollectionView Header test";

        public override View Run()
        {
            var items = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                items.Add($"Items {i}");
            }

            var adaptor = new MyHeaderAdaptor(items);

            var collectionView = new CollectionView();

            collectionView.Adaptor = adaptor;
            collectionView.LayoutManager = new LinearLayoutManager(false);

            return collectionView;
        }
    }
}
