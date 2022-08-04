using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class MessagePopupTest : TestCaseBase
    {
        public override string TestName => "MessagePopup Test";

        public override string TestDescription => "MessagePopup Test";

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
            };
            view.Add(btn1);
            btn1.Clicked += async (s, e) =>
            {
                _ = new MessagePopup("Popup1", "Show your popup", "OK").Open();
            };

            var btn2 = new Button
            {
                Text = "Open popup2",
            };
            view.Add(btn2);
            btn2.Clicked += async (s, e) =>
            {
                try
                {
                    var result = await new MessagePopup("Popup2", "Show your popup", "Yes", "No").Open();
                    await new MessagePopup("Result", $"Result is {result}", "OK").Open();
                }
                catch
                {
                    _ = new MessagePopup("Result", "Popup was closed", "OK").Open();
                }
            };

            return view;
        }
    }
}
