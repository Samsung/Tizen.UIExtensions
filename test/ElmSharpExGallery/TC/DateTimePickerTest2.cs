using ElmSharp;
using System;
using Tizen.UIExtensions.ElmSharp;
using Label = Tizen.UIExtensions.ElmSharp.Label;

namespace ElmSharpExGallery.TC
{
    public class DateTimePickerTest2 : TestCaseBase
    {
        public override string TestName => "DateTimePickerTest2";

        public override string TestDescription => "To test basic operation of TimePicker";

        public override void Run(ElmSharp.Box parent)
        {
            DateTimePicker dateTime = new DateTimePicker(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                DateTime = DateTime.Today,
                Style = "time_layout",
                Format = "%d/%b/%Y %I:%M %p"
            };

            Label label1 = new Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1
            };

            Label label2 = new Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1
            };

            Label label3 = new Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                Text = string.Format("Current DateTime={0}", dateTime.DateTime),
            };

            dateTime.DateTimeChanged += (object sender, DateChangedEventArgs e) =>
            {
                label1.Text = string.Format("Old DateTime={0}", e.OldDate);
                label2.Text = string.Format("New DateTime={0}", e.NewDate);
                label3.Text = string.Format("Current DateTime={0}", dateTime.DateTime);
            };

            dateTime.Show();
            label1.Show();
            label2.Show();
            label3.Show();

            parent.PackEnd(dateTime);
            parent.PackEnd(label1);
            parent.PackEnd(label2);
            parent.PackEnd(label3);
        }
    }
}
