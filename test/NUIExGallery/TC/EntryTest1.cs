using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;
using System.Diagnostics;

namespace NUIExGallery.TC
{
    public class EntryTest1 : TestCaseBase
    {
        public override string TestName => "Entry Test1";

        public override string TestDescription => "Entry test1";

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

            var entry1 = new Entry
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Placeholder = "place holder",
                PlaceholderColor = Color.Gray,
                FontSize = 30,
            };
            view.Add(entry1);

            Debug.Assert(entry1.HorizontalTextAlignment == TextAlignment.Center, "HorizontalTextAlignment");
            Debug.Assert(entry1.VerticalTextAlignment == TextAlignment.Center, "VerticalTextAlignment");
            Debug.Assert(entry1.PlaceholderColor == Color.Gray, "PlaceholderColor");
            Debug.Assert(entry1.Placeholder == "place holder", "Placeholder");

            var entry2 = new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                IsReadOnly = true,
                Placeholder = "Copy"
            };
            entry1.TextChanged += (s, e) => entry2.Text = entry1.Text;

            view.Add(entry2);

            var entry3 = new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                Keyboard = Keyboard.NumberOnly,
                Placeholder = "Number only"
            };
            view.Add(entry3);
            view.Add(new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                Keyboard = Keyboard.Numeric,
                Placeholder = "Numeric"
            });

            view.Add(new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                Keyboard = Keyboard.Email,
                Placeholder = "Email"
            });


            view.Add(new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                FontAttributes = FontAttributes.Italic,
                Placeholder = "Italic",
                PlaceholderColor = Color.Red,
            });

            view.Add(new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                IsPassword = true,
                FontAttributes = FontAttributes.Bold,
                Placeholder = "Password",
                PlaceholderColor = Color.Red,
            });

            var searchkey = new Entry
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                FontAttributes = FontAttributes.Bold,
                Placeholder = "Search",
                PlaceholderColor = Color.Red,
                ReturnType = ReturnType.Search,
            };
            view.Add(searchkey);
            Debug.Assert(searchkey.ReturnType == ReturnType.Search, "ReturnType");

            return view;
        }
    }
}
