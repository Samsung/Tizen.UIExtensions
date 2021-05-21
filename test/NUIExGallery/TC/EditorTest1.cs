using System.Diagnostics;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class EditorTest1 : TestCaseBase
    {
        public override string TestName => "Editor Test1";

        public override string TestDescription => "Editor test1";

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

            var editor1 = new Editor
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                SizeHeight = 300,
                HorizontalTextAlignment = TextAlignment.Center,
                Placeholder = "place holder",
                PlaceholderColor = Color.Gray,
                FontSize = 10,
            };
            editor1.UpdateBackgroundColor(Color.CornflowerBlue);
            view.Add(editor1);

            var editor2 = new Editor
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                IsReadOnly = true,
                Placeholder = "Copy",
                SizeHeight = 200,
            };
            view.Add(editor2);
            editor1.TextChanged += (s, e) => editor2.Text = editor1.Text;

            var editor3 = new Editor
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                FontAttributes = FontAttributes.Italic,
                Placeholder = "Italic",
                PlaceholderColor = Color.Red,
            };
            editor3.UpdateBackgroundColor(Color.Gray);
            view.Add(editor3);
            Debug.Assert(editor3.FontAttributes == FontAttributes.Italic, "FontAttributes");

            return view;
        }
    }
}
