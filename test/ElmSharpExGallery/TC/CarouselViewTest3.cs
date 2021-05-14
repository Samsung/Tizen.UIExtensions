using ElmSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using Ext = Tizen.UIExtensions.ElmSharp;

namespace ElmSharpExGallery.TC
{
    public class CarouselViewTest3 : TestCaseBase
    {
        public override string TestName => "CarouselViewTest3";

        public override string TestDescription => "CarouselViewTest3";

        public override void Run(Box parent)
        {
            var carouselView = new Ext.CarouselView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            carouselView.Show();

            var items = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                items.Add($"Items {i}");
            }
            var adaptor = new MyAdaptor(items);
            carouselView.Adaptor = adaptor;
            carouselView.LayoutManager = new Ext.LinearLayoutManager(true, Ext.ItemSizingStrategy.MeasureFirstItem, 10);

            var indicatorView = new Ext.IndicatorView(parent)
            {
                WeightX = 1,
                WeightY = 0.05,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            indicatorView.Show();
            indicatorView.ClearIndex();
            indicatorView.AppendIndex(items.Count);
            indicatorView.Update(0);

            var flag1 = false;
            var flag2 = false;
            indicatorView.SelectedPosition += (s, e) =>
            {
                if (!flag2)
                {
                    flag1 = true;
                    carouselView.ScrollTo(e.SelectedPosition);
                    flag1 = false;
                }
            };
            carouselView.Scrolled += (s, e) =>
            {
                if (!flag1)
                {
                    flag2 = true;
                    indicatorView.UpdateSelectedIndex(e.CenterItemIndex);
                    flag2 = false;
                }
            };

            parent.PackEnd(carouselView);
            parent.PackEnd(indicatorView);
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
                return new Size(0, 0);
            }

            public override Size MeasureHeader(int widthConstraint, int heightConstraint)
            {
                return new Size(0, 0);
            }

            public override Size MeasureItem(int widthConstraint, int heightConstraint)
            {
                return new Size(720, 1200);
            }

            public override Size MeasureItem(int index, int widthConstraint, int heightConstraint)
            {
                return new Size(720, 1200);
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
