using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class PoupTest : TestCaseBase
    {
        public override string TestName => "PoupTest";

        public override string TestDescription => "PoupTest";

        public override View Run()
        {
            var view = new View
            {
                BackgroundColor = Color.FromHex("#F9AA33").ToNative(),
                Layout = new LinearLayout
                {
                    LinearAlignment = LinearLayout.Alignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                }
            };
            var btn1 = new Button
            {
                Text = "Open popup",
                FontSize = 10,
            };
            view.Add(btn1);
            btn1.Clicked += (s, e) =>
            {
                Popup popup = MakeSimplePopup();
                popup.Open();

                popup.OutsideClicked += (ss, ee) =>
                {
                    Console.WriteLine($"Popup outside clicked");
                };
                popup.Closed += (_, __) =>
                {
                    Console.WriteLine($"Popup is clsoed");
                };
            };

            var btn2 = new Button
            {
                Text = "Open popup 2"
            };
            view.Add(btn2);

            btn2.Clicked += (s, e) =>
            {
                Popup popup = new Popup
                {
                    BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f).ToNative(),
                    Layout = new LinearLayout
                    {
                        LinearAlignment = LinearLayout.Alignment.Center
                    },
                };
                var content = new View
                {
                    Layout = new LinearLayout
                    {
                        LinearOrientation = LinearLayout.Orientation.Vertical,
                    },
                    BackgroundColor = Color.White.ToNative(),
                    SizeWidth = 500,
                    HeightSpecification = LayoutParamPolicies.WrapContent
                };

                var title = new Label
                {
                    Text = "Title",
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.White,
                    FontSize = 12,
                    SizeHeight = 80,
                };
                title.UpdateBackgroundColor(Color.FromHex("#344955"));
                content.Add(title);
                var text = new Label
                {
                    LineBreakMode = LineBreakMode.CharacterWrap,
                    Margin = new Extents(10, 10, 10, 10),
                    Text = "message... message.... ddddd dddccc dccccc",
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                };
                content.Add(text);
                var btn = new Button
                {
                    Margin = new Extents(10, 10, 10, 10),
                    Text = "Close",
                    HeightSpecification = LayoutParamPolicies.WrapContent,

                };
                btn.Clicked += (ss, ee) =>
                {
                    popup.Close();
                };
                var hlayout = new View
                {
                    Layout = new LinearLayout
                    {
                        LinearAlignment = LinearLayout.Alignment.Center,
                        LinearOrientation = LinearLayout.Orientation.Horizontal,
                    },
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.WrapContent
                };
                hlayout.Add(btn);
                content.Add(hlayout);

                popup.Content = content;
                popup.Open();

                popup.OutsideClicked += (ss, ee) =>
                {
                    Console.WriteLine($"Popup outside clicked");
                };
            };


            var btn3 = new Button
            {
                Text = "Yes/No"
            };
            view.Add(btn3);

            btn3.Clicked += (s, e) =>
            {
                Popup popup = new Popup
                {
                    BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f).ToNative(),
                    Layout = new LinearLayout
                    {
                        LinearAlignment = LinearLayout.Alignment.Center
                    },
                };
                var content = new View
                {
                    Layout = new LinearLayout
                    {
                        LinearOrientation = LinearLayout.Orientation.Vertical,
                    },
                    BackgroundColor = Color.White.ToNative(),
                    WidthSpecification = LayoutParamPolicies.WrapContent,
                    HeightSpecification = LayoutParamPolicies.WrapContent
                };

                var title = new Label
                {
                    Text = "Choose",
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.White,
                    FontSize = 12,
                    SizeHeight = 80,
                };
                title.UpdateBackgroundColor(Color.FromHex("#344955"));
                content.Add(title);
                var text = new Label
                {
                    LineBreakMode = LineBreakMode.CharacterWrap,
                    Margin = new Extents(10, 10, 10, 10),
                    Text = "Do you want exit?",
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                };
                content.Add(text);
                var btn = new Button
                {
                    Margin = new Extents(10, 10, 10, 10),
                    Text = "Yes",
                    HeightSpecification = LayoutParamPolicies.WrapContent,
                    SizeWidth = 200,
                };
                btn.Clicked += (ss, ee) =>
                {
                    popup.Close();
                };
                var hlayout = new View
                {
                    Layout = new LinearLayout
                    {
                        LinearAlignment = LinearLayout.Alignment.Center,
                        LinearOrientation = LinearLayout.Orientation.Horizontal,
                    },
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.WrapContent
                };
                hlayout.Add(btn);

                var no = new Button
                {
                    Margin = new Extents(10, 10, 10, 10),
                    Text = "No",
                    HeightSpecification = LayoutParamPolicies.WrapContent,
                    SizeWidth = 200,
                };
                no.Clicked += (ss, ee) =>
                {
                    popup.Close();
                };
                hlayout.Add(no);

                content.Add(hlayout);

                popup.Content = content;
                popup.Open();

                popup.OutsideClicked += (ss, ee) =>
                {
                    Console.WriteLine($"Popup outside clicked");
                };
            };



            var btn4 = new Button
            {
                Text = "Nested popup"
            };
            view.Add(btn4);

            btn4.Clicked += (s, e) =>
            {
                Popup popup = new Popup
                {
                    BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f).ToNative(),
                    Layout = new LinearLayout
                    {
                        LinearAlignment = LinearLayout.Alignment.Center
                    },
                };
                var content = new View
                {
                    Layout = new LinearLayout
                    {
                        LinearOrientation = LinearLayout.Orientation.Vertical,
                    },
                    BackgroundColor = Color.White.ToNative(),
                    SizeWidth = 500,
                    HeightSpecification = LayoutParamPolicies.WrapContent
                };

                var title = new Label
                {
                    Text = "Nested",
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.White,
                    FontSize = 12,
                    SizeHeight = 80,
                };
                title.UpdateBackgroundColor(Color.FromHex("#344955"));
                content.Add(title);
                var text = new Label
                {
                    LineBreakMode = LineBreakMode.CharacterWrap,
                    Margin = new Extents(10, 10, 10, 10),
                    Text = "first popup ---------------- asfdasfd-asfd- asdfa",
                    SizeHeight = 600,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                };
                content.Add(text);
                var btn = new Button
                {
                    Margin = new Extents(10, 10, 10, 10),
                    Text = "Open second",
                    HeightSpecification = LayoutParamPolicies.WrapContent,

                };
                btn.Clicked += (ss, ee) =>
                {
                    MakeSimplePopup().Open();
                };
                var hlayout = new View
                {
                    Layout = new LinearLayout
                    {
                        LinearAlignment = LinearLayout.Alignment.Center,
                        LinearOrientation = LinearLayout.Orientation.Horizontal,
                    },
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.WrapContent
                };
                hlayout.Add(btn);
                content.Add(hlayout);

                popup.Content = content;
                popup.Open();

                popup.OutsideClicked += (ss, ee) =>
                {
                    Console.WriteLine($"Popup outside clicked");
                };
            };


            var btn5 = new Button
            {
                Text = "Unclosed pupup"
            };
            view.Add(btn5);
            btn5.Clicked += (s, e) =>
            {
                var popup = new NotClosedPopup();

                popup.Open();
            };
            return view;
        }


        Popup MakeSimplePopup()
        {
            Popup popup = new Popup
            {
                BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.7f).ToNative(),
                Layout = new LinearLayout
                {
                    LinearAlignment = LinearLayout.Alignment.Center
                },
            };
            var content = new View
            {
                Layout = new LinearLayout
                {
                    LinearAlignment = LinearLayout.Alignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                },
                BackgroundColor = Color.White.ToNative(),
                SizeHeight = 500,
                SizeWidth = 500,
            };
            content.Add(new Label
            {
                Text = "Popup Content!",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HorizontalTextAlignment = TextAlignment.Center,
            });

            var btn = new Button
            {
                Text = "Close",
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };
            btn.Clicked += (ss, ee) =>
            {
                popup.Close();
            };
            content.Add(btn);

            popup.Content = content;
            return popup;
        }

    }

    class NotClosedPopup : Popup
    {
        public NotClosedPopup()
        {
            BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.7f).ToNative();
            Layout = new LinearLayout
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            Content = new Label
            {
                TextColor = Tizen.UIExtensions.Common.Color.White,
                Text = "This Popup is not closed until 5 seconds"
            };
        }

        public new void Open()
        {
            base.Open();
            var timer = new Timer(5000);
            timer.Start();
            timer.Tick += (s, e) =>
            {
                Close();
                return false;
            };
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
