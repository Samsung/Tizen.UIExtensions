using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;

namespace NUIExGallery
{
    class App : NUIApplication
    {
        SimpleViewStack Stack { get; set; }

        protected override void OnCreate()
        {
            base.OnCreate();

            Initialize();
            Stack.Push(CreateListPage(GetTestCases()));
        }

        void Initialize()
        {
            Window.Instance.KeyEvent += OnKeyEvent;

            Stack = new SimpleViewStack
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
        }

        void RunTC(TestCaseBase tc)
        {
            Stack.Push(tc.Run());
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
            if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape"))
            {
                if (Tizen.UIExtensions.NUI.Popup.HasOpenedPopup)
                {
                    Tizen.UIExtensions.NUI.Popup.CloseLast();
                    return;
                }

                if (Stack.Stack.Count > 1)
                {
                    Stack.Pop();
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
