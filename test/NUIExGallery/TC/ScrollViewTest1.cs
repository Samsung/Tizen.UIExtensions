﻿using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;
using ScrollView = Tizen.UIExtensions.NUI.ScrollView;
using Button = Tizen.NUI.Components.Button;

namespace NUIExGallery.TC
{
    public class ScrollViewTest1 : TestCaseBase
    {
        public override string TestName => "ScrollView Test1";

        public override string TestDescription => "Hello World";

        public override View Run()
        {
            var view = new View
            {
                Layout = new LinearLayout
                {
                    LinearAlignment = LinearLayout.Alignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                }
            };
            var menu = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                },
                SizeHeight = 200,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                BackgroundColor = Color.Green.ToNative()
            };
            view.Add(menu);


            var scrollView = new ScrollView()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };

            int itemWidth = 300;
            int itemHeight = 300;
            int itemCols = 10;
            int itemRows = 10;

            var outterLayout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                },
                SizeWidth = itemWidth * itemCols,
                SizeHeight = itemHeight * itemRows
            };

            scrollView.Add(outterLayout);

            for (int i = 0; i < itemRows; i++)
            {
                var innerLayout = new View
                {
                    Layout = new LinearLayout
                    {
                        LinearOrientation = LinearLayout.Orientation.Horizontal,
                    },
                    SizeHeight = itemHeight,
                    SizeWidth = itemWidth * itemCols,
                };
                for (int j = 0; j < itemCols; j++)
                {
                    var rnd = new Random();
                    var item = new View
                    {
                        SizeWidth = itemWidth,
                        SizeHeight = itemHeight,
                        BackgroundColor = Color.FromRgb(rnd.Next(10, 255), rnd.Next(10, 255), rnd.Next(10, 255)).ToNative()
                    };
                    innerLayout.Add(item);
                }
                outterLayout.Add(innerLayout);
            }
            scrollView.ScrollingEventThreshold = 1;
            scrollView.ScrollDragStarted += (s, e) => Console.WriteLine($"DragStarted");
            scrollView.ScrollDragEnded += (s, e) => Console.WriteLine("DragEnd");
            scrollView.ScrollAnimationStarted += (s, e) => Console.WriteLine($"ScrollAnimation started");
            scrollView.ScrollAnimationEnded += (s, e) => Console.WriteLine("$ScrollAnimation ended");
            scrollView.Scrolling += (s, e) =>
            {
                Console.WriteLine($"OnScrolling : Bound : {scrollView.ScrollBound} - {e.Position.X}x{e.Position.Y}");
            };


            var direction = new Button()
            {
                Text = "direction(V)",
                SizeHeight = 300,
                SizeWidth = 300,
            };
            direction.Clicked += (s, e) =>
            {
                if (scrollView.ScrollOrientation == ScrollOrientation.Horizontal)
                {
                    scrollView.ScrollOrientation = ScrollOrientation.Vertical;
                    direction.Text = "direction(V)";
                }
                else
                {
                    scrollView.ScrollOrientation = ScrollOrientation.Horizontal;
                    direction.Text = "direction(H)";
                }
            };
            menu.Add(direction);

            var scrollBar = new Button()
            {
                Text = "Bar(Never)",
                SizeHeight = 300,
                SizeWidth = 300,
            };
            scrollBar.Clicked += (s, e) =>
            {
                if (scrollView.VerticalScrollBarVisibility == ScrollBarVisibility.Never)
                {
                    scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Default;
                    scrollBar.Text = "Bar(Default)";
                }
                else if (scrollView.VerticalScrollBarVisibility == ScrollBarVisibility.Default)
                {
                    scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Always;
                    scrollBar.Text = "Bar(Always)";
                }
                else if (scrollView.VerticalScrollBarVisibility == ScrollBarVisibility.Always)
                {
                    scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Never;
                    scrollBar.Text = "Bar(Never)";
                }
            };
            menu.Add(scrollBar);


            view.Add(scrollView);
            return view;
        }
    }
}
