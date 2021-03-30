using ElmSharp;
using System;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using Button = Tizen.UIExtensions.ElmSharp.Button;
using Rect = Tizen.UIExtensions.Common.Rect;

namespace ElmSharpExGallery.TC
{
    public class ScrollViewTest1 : TestCaseBase
    {
        public override string TestName => "ScrollView Test";

        public override string TestDescription => "Test ScrollView";

        public override void Run(ElmSharp.Box parent)
        {
            var scrollBox = new ElmSharp.Box(parent)
            {
                IsHorizontal = true,
                WeightX = 1,
                AlignmentX = -1,
                MinimumHeight = 100,
            };
            scrollBox.Show();
            var menuScroll = new ScrollView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Default,
            };
            menuScroll.Show();
            scrollBox.PackEnd(menuScroll);
            var menu = new ElmSharp.Box(parent)
            {
                IsHorizontal = true,
                WeightX = 1,
                AlignmentX = -1,
                MinimumHeight = 100,
            };
            menu.Show();
            menuScroll.SetScrollCanvas(menu);
            menuScroll.SetContentSize(200 * 5, 100);
            parent.PackEnd(scrollBox);

            var scrollview = new ScrollView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            scrollview.Show();
            scrollview.ScrollOrientation = ScrollOrientation.Both;
            scrollview.VerticalScrollBarVisibility = ScrollBarVisibility.Always;
            scrollview.HorizontalScrollBarVisibility = ScrollBarVisibility.Always;

            var scrollCanvas = new ElmSharp.Box(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            scrollCanvas.Show();
            scrollview.SetScrollCanvas(scrollCanvas);

            int itemWidth = 500;
            int itemHeight = 500;

            int itemCols = 5;
            int itemRows = 5;

            scrollview.SetContentSize(itemWidth * itemCols, itemHeight * itemRows);

            for (int i = 0; i < itemRows; i++)
            {
                var innerContainer = new ElmSharp.Box(parent)
                {
                    IsHorizontal = true,
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                };
                innerContainer.Show();
                for (int j = 0; j < itemCols; j++)
                {
                    var rnd = new Random();
                    var item = new Rectangle(parent)
                    {
                        MinimumHeight = itemHeight,
                        MinimumWidth = itemWidth,
                        Color = ElmSharp.Color.FromRgb(rnd.Next(10, 255), rnd.Next(10, 255), rnd.Next(10, 255))
                    };
                    item.Show();
                    innerContainer.PackEnd(item);
                }
                scrollCanvas.PackEnd(innerContainer);
            }
            parent.PackEnd(scrollview);

            // menu button
            var scrollToBtn = new Button(parent)
            {
                AlignmentY = -1,
                WeightX = 1,
                FontSize = 30,
                Text = "Scroll(rnd)",
                MinimumWidth = 200,
            };
            scrollToBtn.Show();
            scrollToBtn.Clicked += (s, e) =>
            {
                var rnd = new Random();
                scrollview.ScrollToAsync(new Rect(rnd.Next(0, itemCols * itemWidth), rnd.Next(0, itemRows * itemHeight), itemWidth, itemHeight), true);
            };
            menu.PackEnd(scrollToBtn);

            var vBar = new Button(parent)
            {
                AlignmentY = -1,
                WeightX = 1,
                FontSize = 30,
                Text = "V-bar(O)",
                MinimumWidth = 200,
            };
            vBar.Show();
            vBar.Clicked += (s, e) =>
            {
                if (scrollview.VerticalScrollBarVisibility == ScrollBarVisibility.Always)
                {
                    vBar.Text = "V-bar(X)";
                    scrollview.VerticalScrollBarVisibility = ScrollBarVisibility.Never;
                }
                else
                {
                    vBar.Text = "V-bar(O)";
                    scrollview.VerticalScrollBarVisibility = ScrollBarVisibility.Always;
                }
            };
            menu.PackEnd(vBar);

            var hBar = new Button(parent)
            {
                AlignmentY = -1,
                WeightX = 1,
                FontSize = 30,
                Text = "H-bar(O)",
                MinimumWidth = 200,
            };
            hBar.Show();
            hBar.Clicked += (s, e) =>
            {
                if (scrollview.HorizontalScrollBarVisibility == ScrollBarVisibility.Always)
                {
                    hBar.Text = "H-bar(X)";
                    scrollview.HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
                }
                else
                {
                    hBar.Text = "H-bar(O)";
                    scrollview.HorizontalScrollBarVisibility = ScrollBarVisibility.Always;
                }
            };
            menu.PackEnd(hBar);


            var vScroll = new Button(parent)
            {
                AlignmentY = -1,
                WeightX = 1,
                FontSize = 30,
                Text = "V-Scroll(O)",
                MinimumWidth = 200,
            };
            vScroll.Show();
            vScroll.Clicked += (s, e) =>
            {
                if (scrollview.ScrollOrientation == ScrollOrientation.Horizontal || scrollview.ScrollOrientation == ScrollOrientation.Neither)
                {
                    vScroll.Text = "V-Scroll(O)";
                    scrollview.ScrollOrientation = scrollview.ScrollOrientation == ScrollOrientation.Horizontal ? ScrollOrientation.Both : ScrollOrientation.Vertical;
                }
                else
                {
                    vScroll.Text = "V-Scroll(X)";
                    scrollview.ScrollOrientation = scrollview.ScrollOrientation == ScrollOrientation.Both ? ScrollOrientation.Horizontal : ScrollOrientation.Neither;
                }
            };
            menu.PackEnd(vScroll);

            var hScroll = new Button(parent)
            {
                AlignmentY = -1,
                WeightX = 1,
                FontSize = 30,
                Text = "H-Scroll(O)",
                MinimumWidth = 200,
            };
            hScroll.Show();
            hScroll.Clicked += (s, e) =>
            {
                if (scrollview.ScrollOrientation == ScrollOrientation.Vertical || scrollview.ScrollOrientation == ScrollOrientation.Neither)
                {
                    hScroll.Text = "H-Scroll(O)";
                    scrollview.ScrollOrientation = scrollview.ScrollOrientation == ScrollOrientation.Vertical ? ScrollOrientation.Both : ScrollOrientation.Horizontal;
                }
                else
                {
                    hScroll.Text = "H-Scroll(X)";
                    scrollview.ScrollOrientation = scrollview.ScrollOrientation == ScrollOrientation.Both ? ScrollOrientation.Vertical : ScrollOrientation.Neither;
                }
            };
            menu.PackEnd(hScroll);
        }
    }
}
