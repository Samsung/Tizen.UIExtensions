using ElmSharp;
using System;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using Color = Tizen.UIExtensions.Common.Color;
using Label = Tizen.UIExtensions.ElmSharp.Label;

namespace ElmSharpExGallery.TC
{
    public class LabelTest1 : TestCaseBase
    {
        public override string TestName => "LabelTest1";

        public override string TestDescription => "Test Label";

        public override void Run(ElmSharp.Box parent)
        {
            var label = new Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = 0.5,
                AlignmentX = 0.5,
                FontSize = 30,
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
                            FontSize = 60,
                        }
                    }
                }
            };
            label.Show();
            parent.PackEnd(label);
        }
    }
}
