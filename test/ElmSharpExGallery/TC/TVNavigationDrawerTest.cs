using System;
using System.Collections;
using System.Collections.Generic;
using Tizen.Applications;
using ElmSharp;
using Common = Tizen.UIExtensions.Common;
using Ext = Tizen.UIExtensions.ElmSharp;

namespace ElmSharpExGallery.TC
{
    public class TVNavigationDrawerTest : TestCaseBase
    {
        public override string TestName => "Shell TVNavigationDrawer Test 1";

        public override string TestDescription => "Shell TVNavigationDrawer Test 1";

        Box _header;

        public override void Run(ElmSharp.Box parent)
        {
            Console.WriteLine($" run !!!");
            var data = new List<string>();
            for (int i = 0; i < 50; i++)
            {
                data.Add($"item {i}");
            }

            var drawer = new Ext.TVNavigationDrawer(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };

            var naviView = new Ext.TVNavigationView(parent);

            var content = new Box(parent)
            {
                BackgroundColor = Color.White
            };

            var label = new Label(parent)
            {
                AlignmentX = 0.5,
                WeightX = 1,
                Text = "selected item index: / data: ",
            };
            label.Show();
            content.PackEnd(label);

            var bLabel = new Label(parent)
            {
                AlignmentX = 0.5,
                WeightX = 1,
                Text = "behavior: ",
            };
            bLabel.Show();
            content.PackEnd(bLabel);

            var openButton = new Button(parent)
            {
                MinimumWidth = 500,
                Text = "open / close"
            };
            openButton.Show();
            content.PackEnd(openButton);

            var headerButton = new Button(parent)
            {
                MinimumWidth = 500,
                Text = "add / remove header"
            };
            headerButton.Show();
            content.PackEnd(headerButton);

            var footerButton = new Button(parent)
            {
                MinimumWidth = 500,
                Text = "add / remove footer"
            };
            footerButton.Show();
            content.PackEnd(footerButton);


            var contentButton = new Button(parent)
            {
                MinimumWidth = 500,
                Text = "add / remove content"
            };
            contentButton.Show();
            content.PackEnd(contentButton);



            var headerButton2 = new Button(parent)
            {
                MinimumHeight = 100,
                MinimumWidth = 300,
                Text = "header"
            };
            headerButton2.Show();
            content.PackEnd(headerButton2);

            headerButton2.Clicked += (s, e) =>
            {
                if (_header.MinimumHeight < 200)
                    _header.MinimumHeight = 300;
                else
                    _header.MinimumHeight = 150;
            };

            openButton.Clicked += (s, e) =>
            {
                drawer.IsOpen = !drawer.IsOpen;
            };

            headerButton.Clicked += (s, e) =>
            {
                if (naviView.Header != null)
                {
                    naviView.Header = null;
                }
                else
                {
                    naviView.Header = CreateHeader(parent);
                }
            };

            footerButton.Clicked += (s, e) =>
            {
                if (naviView.Footer != null)
                {
                    naviView.Footer = null;
                }
                else
                {
                    naviView.Footer = CreateFooter(parent);
                }
            };

            contentButton.Clicked += (s, e) =>
            {
                if (naviView.Content != null)
                {
                    naviView.Content = null;
                }
                else
                {
                    naviView.Content = CreateContent(parent);
                }
            };

            naviView.Header = CreateHeader(parent);
            naviView.Footer = CreateFooter(parent);
            naviView.Content = CreateContent(parent);
            naviView.BackgroundImage = CreateBackgroundImage(parent);
            //naviView.BackgroundColor = Color.Yellow;

            drawer.Main = content;
            drawer.NavigationView = naviView;

            drawer.Show();
            parent.PackEnd(drawer);
        }

        EvasObject CreateBackgroundImage(EvasObject parent)
        {
            var img = new Ext.Image(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            img.Show();
            img.Aspect = Common.Aspect.Fill;
            img.LoadAsync(Application.Current.DirectoryInfo.Resource + "animated2.gif");

            return img;
        }

        EvasObject CreateHeader(EvasObject parent)
        {
            _header = new Box(parent);
            _header.BackgroundColor = Color.Red;
            _header.MinimumHeight = 150;

            var headerLabel = new Ext.Label(parent)
            {
                FormattedText = new Common.FormattedString
                {
                    Spans =
                    {
                        new Common.Span
                        {
                            Text = "Header",
                            FontAttributes = Common.FontAttributes.Bold,
                            HorizontalTextAlignment= Common.TextAlignment.Center,
                            FontSize = 60,
                        }
                    }
                }
            };
            headerLabel.Show();
            _header.PackEnd(headerLabel);

            return _header;
        }

        EvasObject CreateFooter(EvasObject parent)
        {
            var footer = new Box(parent);
            footer.BackgroundColor = Color.Blue;
            footer.MinimumHeight = 200;

            var footerLabel = new Ext.Label(parent)
            {
                FormattedText = new Common.FormattedString
                {
                    Spans =
                    {
                        new Common.Span
                        {
                            Text = "Footer",
                            FontAttributes = Common.FontAttributes.Bold,
                            HorizontalTextAlignment= Common.TextAlignment.Center,
                            FontSize = 60,
                        }
                    }
                }
            };
            footerLabel.Show();
            footer.PackEnd(footerLabel);

            return footer;
        }

        EvasObject CreateContent(EvasObject parent)
        {
            var collectionView = new Ext.CollectionView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            collectionView.Show();

            var items = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                items.Add($"Items {i}");

            }
            var adaptor = new MyAdaptor(items);
            collectionView.Adaptor = adaptor;
            collectionView.LayoutManager = new Ext.LinearLayoutManager(false, Ext.ItemSizingStrategy.MeasureAllItems, 10);

            return collectionView;
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
                return null;
            }

            public override EvasObject GetHeaderView(EvasObject parent)
            {
                return null;
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
                return new Size(150, 150);
            }

            public override Size MeasureItem(int index, int widthConstraint, int heightConstraint)
            {
                if (index % 2 == 0)
                    return new Size(150, 150);
                else
                    return new Size(200, 200);
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
