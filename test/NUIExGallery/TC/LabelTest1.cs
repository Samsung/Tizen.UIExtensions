using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;


namespace NUIExGallery.TC
{
    public class LabelTest1 : TestCaseBase
    {
        public override string TestName => "Label Test1";

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

            var label = new Label()
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap,
                FontSize = 10,
                FormattedText = new FormattedString
                {
                    Spans =
                    {
                        new Span
                        {
                            Text = "Span1-bold ",
                            FontAttributes = FontAttributes.Bold,
                        },
                        new Span
                        {
                            Text = "Span2-red ",
                            ForegroundColor = Color.Red
                        },
                        new Span
                        {
                            Text = "Span3-Large",
                            FontSize = 15,
                        },
                        new Span
                        {
                            Text = "Span4-underline",
                            TextDecorations = TextDecorations.Underline
                        },
                        new Span
                        {
                            Text = "Span5-Strikethrough",
                            TextDecorations = TextDecorations.Strikethrough
                        },
                        new Span
                        {
                            LineHeight = 3,
                            Text = "Span6-Long text Long text Long text Long text Long text Long text Long text Long text Long text Long text Long text Long text Long text"
                        },
                        new Span
                        {
                            HorizontalTextAlignment = TextAlignment.Start,
                            LineHeight = 3,
                            Text = "Span7-align start"
                        },
                        new Span
                        {
                            BackgroundColor = Color.Yellow,
                            Text = "Span7-BackgroundColor"
                        }
                    }
                }
            };

            view.Add(label);

            return view;
        }
    }
}
