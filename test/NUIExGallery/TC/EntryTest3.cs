using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;
using System.Diagnostics;

namespace NUIExGallery.TC
{
    public class EntryTest3 : TestCaseBase
    {
        public override string TestName => "Entry Test3";

        public override string TestDescription => "Entry test3";

        public override View Run()
        {
            var view = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                }
            };

            var entry1 = new Entry
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                SizeHeight = 100,
                BackgroundColor = Tizen.NUI.Color.Yellow
            };
            view.Add(entry1);

            entry1.Text = "Update from code";
            entry1.TextColor = Color.Blue;


            var entry2 = new Entry
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                IsReadOnly = true
            };
            entry2.Text = entry1.Text;
            entry1.TextChanged += (s, e) =>
            {
                entry2.Text = entry1.Text;
            };
            view.Add(entry2);

            return view;
        }
    }
}
