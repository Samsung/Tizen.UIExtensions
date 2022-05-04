using ElmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tizen.Applications;

namespace ElmSharpExGallery
{
    class App : CoreUIApplication
    {
        Window _window;
        SimpleViewStack _viewStack;

        public string Profile { get; } = Elementary.GetProfile();

        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
            CreateListPage(GetTestCases());
        }

        void CreateListPage(IEnumerable<TestCaseBase> tcs)
        {
            GenList list = new GenList(_window)
            {
                Homogeneous = true,
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1
            };

            GenItemClass defaultClass = new GenItemClass("default")
            {
                GetTextHandler = (data, part) =>
                {
                    TestCaseBase tc = data as TestCaseBase;
                    return tc == null ? "" : tc.TestName;
                }
            };

            foreach (var tc in tcs.Where<TestCaseBase>((tc) => tc.TargetProfile.HasFlag(GetTargetProfile())))
            {
                list.Append(defaultClass, tc);
            }

            if (Profile == "wearable")
            {
                list.Prepend(defaultClass, null);
                list.Append(defaultClass, null);
            }

            list.ItemSelected += (s, e) =>
            {
                TestCaseBase tc = e.Item.Data as TestCaseBase;
                RunTC(tc);
            };
            list.Show();
            _viewStack.Push(list);
        }

        void RunTC(TestCaseBase tc) 
        {
            var container = new Box(_window)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentX = -1,
                AlignmentY = -1,
            };
            tc.Run(container);
            _viewStack.Push(container);
        }

        void Initialize()
        {
            Window window = new Window("ElmSharpApp")
            {
                AvailableRotations = DisplayRotation.Degree_0 | DisplayRotation.Degree_180 | DisplayRotation.Degree_270 | DisplayRotation.Degree_90
            };
            _window = window;
            window.BackButtonPressed += (s, e) =>
            {
                if (_viewStack.Stack.Count > 1)
                {
                    _viewStack.Pop();
                }
                else
                {
                    Exit();
                }
            };
            window.Show();
            var conformant = new Conformant(window);
            conformant.Show();
            var bg = new Background(window)
            {
                Color = Color.White
            };
            bg.Show();
            conformant.SetContent(bg);

            _viewStack = new SimpleViewStack(window)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentX = -1,
                AlignmentY = -1,
            };
            _viewStack.Show();
            bg.SetContent(_viewStack);
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

        TargetProfile GetTargetProfile()
        {
            switch (Profile)
            {
                case "wearable":
                    return TargetProfile.Wearable;
                case "mobile":
                    return TargetProfile.Mobile;
                case "tv":
                    return TargetProfile.Tv;
            }
            return TargetProfile.Mobile;
        }

        static void Main(string[] args)
        {
            Elementary.Initialize();
            Elementary.ThemeOverlay();
            App app = new App();
            app.Run(args);
        }
    }
}
