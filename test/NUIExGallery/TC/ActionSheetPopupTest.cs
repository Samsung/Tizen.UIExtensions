using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class ActionSheetPopupTest : TestCaseBase
    {
        public override string TestName => "ActionSheetPopup Test";

        public override string TestDescription => "ActionSheetPopup Test";

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

            {
                var btn3 = new Button
                {
                    Text = "Open1",
                    FontSize = 10,
                };
                view.Add(btn3);
                btn3.Clicked += async (s, e) =>
                {
                    try
                    {
                        var result = await new ActionSheetPopup("Choose", "Cancel", "Delete", new []{
                            "Apple",
                            "Banana",
                            "Test",
                            "Test2",
                            "test3",
                            "bbbb"
                        }).Open();
                        _ = new MessagePopup("Result", $"You choose {result}", "OK").Open();
                    }
                    catch (TaskCanceledException)
                    {
                        _ = new MessagePopup("Result", $"Canceled", "OK").Open();
                    }

                };
            }

            {
                var btn3 = new Button
                {
                    Text = "Open2",
                    FontSize = 10,
                };
                view.Add(btn3);
                btn3.Clicked += async (s, e) =>
                {
                    try
                    {
                        var result = await new ActionSheetPopup("Choose", "Cancel", buttons: new string[]{
                            "Item1",
                            "Item2",
                            "Item3",
                        }).Open();
                        _ = new MessagePopup("Result", $"You choose {result}", "OK").Open();
                    }
                    catch (TaskCanceledException)
                    {
                        _ = new MessagePopup("Result", $"Canceled", "OK").Open();
                    }
                };
            }



            return view;
        }
    }
}
