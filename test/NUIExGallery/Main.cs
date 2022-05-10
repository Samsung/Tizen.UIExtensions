using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;
using Tizen.UIExtensions.NUI;

namespace NUIExGallery
{
    class App : NUIApplication
    {
        public static NavigationStack Stack { get; set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            FocusManager.Instance.EnableDefaultAlgorithm(true);
            Initialize();
            _ = Stack.Push(CreateListPage(GetTestCases()), true);
        }

        void Initialize()
        {
            View.SetDefaultGrabTouchAfterLeave(true);
            Window.Instance.KeyEvent += OnKeyEvent;

            Stack = new NavigationStack
            {
                BackgroundColor = Color.White
            };
            Window.Instance.GetDefaultLayer().Add(Stack);

            Window.Instance.AddAvailableOrientation(Window.WindowOrientation.Landscape);
            Window.Instance.AddAvailableOrientation(Window.WindowOrientation.LandscapeInverse);
            Window.Instance.AddAvailableOrientation(Window.WindowOrientation.Portrait);
            Window.Instance.AddAvailableOrientation(Window.WindowOrientation.PortraitInverse);
            Console.WriteLine($"-----------------------------------------------");
            Console.WriteLine($"-- DPI : {Window.Instance.Dpi.X}, {Window.Instance.Dpi.Y}");
            Console.WriteLine($"-- Orientation : {Window.Instance.GetCurrentOrientation()}");
            Console.WriteLine($"-----------------------------------------------");

            Window.Instance.Resized += (s, e) =>
            {
                Console.WriteLine($"---------- Window Resized --------------------");
                Console.WriteLine($"-- Orientation : {Window.Instance.GetCurrentOrientation()}");
            };

            PerformanceMonitor.Attach();

        }

        void RunTC(TestCaseBase tc)
        {
            _ = Stack.Push(tc.Run(), true);
        }

        View CreateListPage(IEnumerable<TestCaseBase> tests)
        {
            var layout = new View
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                }
            };

            EventHandler<ClickedEventArgs> clicked = (object sender, ClickedEventArgs args) =>
            {
                var testCase = ((sender as View).BindingContext as TestCaseBase);
                RunTC(testCase);
            };


            var collectionview = new ScrollableBase
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical
                }
            };

            foreach (var item in tests)
            {
                var itemView = new DefaultLinearItem();

                itemView.Focusable = true;
                itemView.BindingContext = item;
                itemView.Clicked += clicked;

                var label = new TextLabel
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                };

                label.PixelSize = 30;
                label.SetBinding(TextLabel.TextProperty, new Binding("TestName"));

                itemView.Add(label);
                collectionview.Add(itemView);
            }

            // NUI bug - on target NUI CollectionView was crashed
            /*
            var collectionview = new CollectionView
            {
                SelectionMode = ItemSelectionMode.None,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                ItemsLayouter = new LinearLayouter(),
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                ItemsSource = tests,
                ItemTemplate = new DataTemplate(() =>
                {
                    //var itemView = new RecyclerViewItem
                    //{
                    //    Layout = new LinearLayout
                    //    {
                    //        LinearOrientation = LinearLayout.Orientation.Vertical,
                    //        LinearAlignment = LinearLayout.Alignment.Center
                    //    },
                    //    WidthResizePolicy = ResizePolicyType.FillToParent,
                    //    SizeHeight = 100,
                    //};
                    //var border = new BorderVisual
                    //{
                    //    Opacity = 0.8f,
                    //    CornerRadius = 0,
                    //    BorderSize = 1,
                    //    Color = Color.Black,
                    //    AntiAliasing = true,
                    //};
                    //itemView.AddVisual("border", border);

                    var itemView = new DefaultLinearItem();

                    itemView.Clicked += clicked;

                    var label = new TextLabel
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        WidthSpecification = LayoutParamPolicies.MatchParent,
                        HeightSpecification = LayoutParamPolicies.MatchParent,
                    };

                    label.PixelSize = 30;
                    label.SetBinding(TextLabel.TextProperty, new Binding("TestName"));
                    itemView.Add(label);
                    return itemView;
                    //var itemView = new DefaultLinearItem
                    //{
                    //    WidthResizePolicy = ResizePolicyType.FillToParent,
                    //};

                    //itemView.Clicked += clicked;
                    //itemView.Label.PixelSize = 30;
                    //itemView.Label.SetBinding(TextLabel.TextProperty, new Binding("TestName"));
                    //return itemView;
                })
            };
            */

            layout.Add(collectionview);

            return layout;
        }


        IEnumerable<TestCaseBase> GetTestCases()
        {
            Assembly asm = typeof(App).GetTypeInfo().Assembly;
            Type testCaseType = typeof(TestCaseBase);

            var tests = from test in asm.GetTypes()
                        where testCaseType.IsAssignableFrom(test) && !test.GetTypeInfo().IsInterface && !test.GetTypeInfo().IsAbstract
                        select Activator.CreateInstance(test) as TestCaseBase;

            return from test in tests
                   orderby test.TestName
                   select test;
        }

        void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.IsDeclineKeyEvent())
            {
                if (Tizen.UIExtensions.NUI.Popup.HasOpenedPopup)
                {
                    Tizen.UIExtensions.NUI.Popup.CloseLast();
                    return;
                }

                if (Stack.Stack.Count > 1)
                {
                    Stack.Pop(true);
                }
                else
                {
                    Exit();
                }
            }
        }

        static void Main(string[] args)
        {
            App app = new App();
            app.Run(args);
        }
    }
}
