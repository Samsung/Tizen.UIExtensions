using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.Common;

namespace NUIExGallery.TC
{
    public class ButtonTest1 : TestCaseBase
    {
        public override string TestName => "Button Test1";

        public override string TestDescription => "Button test1";

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
            var btn1 = new Button
            {
                Text = "A... looooooooooong text",
                FontSize = 10,
            };
            view.Add(btn1);
            var fitButton = new Button
            {
                Text = "Fit"
            };
            view.Add(fitButton);

            fitButton.Clicked += (s, e) =>
            {
                btn1.UpdateSize(btn1.Measure(600, 500));
            };
            btn1.Clicked += (s, e) =>
            {
                Console.WriteLine($"btn1 clicked - Measure{btn1.Measure(600, 500)}");
                Console.WriteLine($"btn1 clicked - NaturalSize {btn1.NaturalSize2D.ToCommon()}");
            };


            var btn2 = new Button
            {
                Text = "Click",
                FontAttributes = FontAttributes.Bold,
            };
            int count = 0;
            btn2.Clicked += (s, e) =>
            {
                btn2.Text = $"Clicked {++count}";
            };
            view.Add(btn2);

            var btn3 = new Button
            {
                Text = "Icon Btn",
            };
            btn3.Icon.ResourceUrl = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "image.png";
            view.Add(btn3);
            btn3.Clicked += (s, e) => 
            {
                Console.WriteLine($"btn1 clicked - NaturalSize {btn3.NaturalSize2D.ToCommon()}");
            };

            return view;
        }
    }
}
