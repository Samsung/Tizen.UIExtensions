using ElmSharp;
using System;
using System.Collections.Generic;
using Tizen.Applications;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using EBox = ElmSharp.Box;
using EButton = ElmSharp.Button;
using EColor = ElmSharp.Color;
using ELabel = ElmSharp.Label;
using ExLabel = Tizen.UIExtensions.ElmSharp.Label;
using ExImage = Tizen.UIExtensions.ElmSharp.Image;

namespace ElmSharpExGallery.TC
{
    public class NavigationDrawerTest1 : TestCaseBase
    {
        public override string TestName => "Shell NavigationDrawer Test 1";

        public override string TestDescription => "Shell NavigationDrawer Test 1";

        public override void Run(ElmSharp.Box parent)
        {
            Console.WriteLine($" run !!!");
            var data = new List<string>();
            for (int i = 0; i < 50; i++)
            {
                data.Add($"item {i}");
            }

            var drawer = new NavigationDrawer(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };

            var naviView = new NavigationView(parent);

            var content = new EBox(parent)
            {
                BackgroundColor = EColor.White
            };

            var label = new ELabel(parent)
            {
                AlignmentX = 0.5,
                WeightX = 1,
                Text = "selected item index: / data: ",
            };
            label.Show();
            content.PackEnd(label);

            var bLabel = new ELabel(parent)
            {
                AlignmentX = 0.5,
                WeightX = 1,
                Text = "behavior: ",
            };
            bLabel.Show();
            content.PackEnd(bLabel);

            var openButton = new EButton(parent)
            {
                MinimumWidth = 300,
                Text = "open / close"
            };
            openButton.Show();
            content.PackEnd(openButton);

            var headerButton = new EButton(parent)
            {
                MinimumWidth = 300,
                Text = "Scroll / Fixed"
            };
            headerButton.Show();
            content.PackEnd(headerButton);

            openButton.Clicked += (s, e) =>
            {
                drawer.IsOpen = !drawer.IsOpen;
            };

            headerButton.Clicked += (s, e) =>
            {
                if (naviView.HeaderBehavior == DrawerHeaderBehavior.Scroll)
                    naviView.HeaderBehavior = DrawerHeaderBehavior.Fixed;
                else
                    naviView.HeaderBehavior = DrawerHeaderBehavior.Scroll;

                bLabel.Text = $"behavior: {naviView.HeaderBehavior}";
            };

            drawer.DimArea.BackgroundColor = EColor.Black;
            drawer.DimArea.Opacity = 30;

            naviView.BuildMenu(data);
            naviView.ItemSelected += (s, e) =>
            {
                label.Text = $"selected item index: {e.SelectedItemIndex} / data: {e.SelectedItem.ToString()}";
                drawer.IsOpen = false;
            };

            naviView.Header = CreateHeader(parent);
            naviView.BackgroundImage = CreateBackgroundImage(parent);
            naviView.BackgroundColor = EColor.Yellow;
            naviView.HeaderBehavior = DrawerHeaderBehavior.Fixed;

            drawer.Content = content;
            drawer.Drawer = naviView;

            drawer.Show();
            parent.PackEnd(drawer);
        }

        EvasObject CreateBackgroundImage(EvasObject parent)
        {
            var img = new ExImage(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            img.Show();
            img.Aspect = Aspect.Fill;
            img.LoadAsync(Application.Current.DirectoryInfo.Resource + "animated2.gif");

            return img;
        }

        EvasObject CreateHeader(EvasObject parent)
        {
            var header = new EBox(parent);
            header.BackgroundColor = EColor.Red;
            header.MinimumHeight = 200;

            var headerLabel = new ExLabel(parent)
            {
                FormattedText = new FormattedString
                {
                    Spans =
                    {
                        new Span
                        {
                            Text = "Header",
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment= TextAlignment.Center,
                            FontSize = 60,
                        }
                    }
                }
            };
            headerLabel.Show();
            header.PackEnd(headerLabel);

            return header;
        }
    }
}
