using System;
using Tizen.UIExtensions.ElmSharp;
using Label = Tizen.UIExtensions.ElmSharp.Label;

namespace ElmSharpExGallery.TC
{
    public class DateTimePickerDialogTest1 : TestCaseBase
    {
        public override string TestName => "DateTimePickerDialogTest1";

        public override string TestDescription => "To test basic operation of DatePickerDialog";

        public override void Run(ElmSharp.Box parent)
        {
            Button btn = new Button(parent)
            {
                AlignmentX = -1,
                WeightX = 1,
                Text = "Open Dialog"
            };
            btn.Show();

            Label label1 = new Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1
            };
            label1.Show();

            Label label2 = new Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1
            };
            label2.Show();

            parent.PackEnd(btn);
            parent.PackEnd(label1);
            parent.PackEnd(label2);

            DateTimePickerDialog dialog = new DateTimePickerDialog(parent)
            {
                Mode = DateTimePickerMode.Date,
                MinimumDateTime = new DateTime(2021, 1, 1),
                MaximumDateTime = DateTime.Now,
                DateTime = DateTime.Now
            };

            dialog.DateTimeChanged += (s, e) =>
            {
                label1.Text = e.NewDate.ToString();
            };

            dialog.PickerClosed += (s, e) =>
            {
                label2.Text = "PickerClosed";
            };

            dialog.PickerOpened += (s, e) =>
            {
                label2.Text = "PickerOpened";
            };

            btn.Clicked += (s, e) =>
            {
                dialog.Show();
            };
        }
    }
}
