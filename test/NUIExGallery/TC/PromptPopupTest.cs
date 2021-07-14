using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class PromptPopupTest : TestCaseBase
    {
        public override string TestName => "PromptPopup Test";

        public override string TestDescription => "PromptPopup Test";

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
            var btn3 = new Button
            {
                Text = "Open Prompt popup",
                FontSize = 10,
            };
            view.Add(btn3);
            btn3.Clicked += async (s, e) =>
            {
                try
                {
                    var result = await new PromptPopup("Prompt", "Your name", placeholder: "Name").Open();
                    await new MessagePopup("Result", $"Your name is {result}", "OK").Open();
                }
                catch (TaskCanceledException)
                {
                    _ = new MessagePopup("Result", $"Prompt is canceld", "OK").Open();
                }
            };


            return view;
        }
    }
}
