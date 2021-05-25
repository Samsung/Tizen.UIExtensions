using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;
using System.Diagnostics;

namespace NUIExGallery.TC
{
    public class EntryTest2 : TestCaseBase
    {
        public override string TestName => "Entry Test2";

        public override string TestDescription => "Entry test2";

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
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 30,
                SizeHeight = 200,
            };
            view.Add(entry1);
            entry1.UpdateBackgroundColor(Color.GreenYellow);
            entry1.Text = "Entry1";
            entry1.PrimaryCursorPosition = 0;

            var entry2 = new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                IsReadOnly = true,
                Placeholder = "Copy"
            };
            entry1.TextChanged += (s, e) => entry2.Text = entry1.Text;
            entry2.UpdateBackgroundColor(Color.Aqua);

            view.Add(entry2);

            var entry3 = new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
            };
            entry3.Text = "Entry3";

            entry3.UpdateBackgroundColor(Color.Aqua);

            view.Add(entry3);
            view.Add(new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                Keyboard = Keyboard.Numeric,
            });

            view.Add(new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
            });


            return view;
        }
    }
}
