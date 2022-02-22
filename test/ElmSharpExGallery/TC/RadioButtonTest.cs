using ElmSharp;
using System;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using Color = Tizen.UIExtensions.Common.Color;
using Label = Tizen.UIExtensions.ElmSharp.Label;

namespace ElmSharpExGallery.TC
{
    public class RadioButtonTest : TestCaseBase
    {
        public override string TestName => "RadioButton Test";

        public override string TestDescription => "Test RadioButton";

        public override void Run(ElmSharp.Box parent)
        {
            var scrollview = new ScrollView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            scrollview.Show();
            scrollview.ScrollOrientation = ScrollOrientation.Vertical;
            parent.PackEnd(scrollview);

            var scrollCanvas = new ElmSharp.Box(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            scrollCanvas.Show();
            scrollview.SetScrollCanvas(scrollCanvas);

            RadioButton rd1 = new RadioButton(parent)
            {
                StateValue = 1,
                Text = "Value #1",
                TextColor = Color.Violet,
                TextBackgroundColor = Color.Yellow,
                AlignmentX = -1,
                AlignmentY = 0,
                WeightX = 1,
                WeightY = 1
            };
            rd1.Show();

            Radio rd2 = new Radio(parent)
            {
                StateValue = 2,
                Text = "Value #2",
                AlignmentX = -1,
                AlignmentY = 0,
                WeightX = 1,
                WeightY = 1
            };
            rd2.Show();

            Radio rd3 = new Radio(parent)
            {
                StateValue = 3,
                Text = "Value #3",
                AlignmentX = -1,
                AlignmentY = 0,
                WeightX = 1,
                WeightY = 1
            };
            rd3.Show();

            rd2.SetGroup(rd1);
            rd3.SetGroup(rd2);
            
            var label = new Label(parent)
            {
                AlignmentX = -1,
                AlignmentY = 0,
                WeightX = 1,
                WeightY = 1
            };
            label.Show();

            rd1.ValueChanged += OnRadioValueChanged;
            rd2.ValueChanged += OnRadioValueChanged;
            rd3.ValueChanged += OnRadioValueChanged;

            void OnRadioValueChanged(object sender, EventArgs e)
            {
                label.Text = string.Format("Value Changed: {0}", ((Radio)sender).GroupValue);
            }

            scrollCanvas.PackEnd(label);
            scrollCanvas.PackEnd(rd1);
            scrollCanvas.PackEnd(rd2);
            scrollCanvas.PackEnd(rd3);

            scrollview.SetContentSize(720, 3000);

        }
    }
}
