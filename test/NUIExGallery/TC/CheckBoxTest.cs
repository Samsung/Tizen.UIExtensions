using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.NUI.GraphicsView;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class CheckBoxTest : TestCaseBase
    {
        public override string TestName => "CheckBox Test";

        public override string TestDescription => "CheckBox test1";

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
            var checkbox1 = new CheckBox
            {
                Text = "CheckBox1",
            };
            checkbox1.SizeHeight = (float)checkbox1.Measure(300, 300).Height;
            checkbox1.SizeWidth = (float)checkbox1.Measure(300, 300).Width;


            view.Add(checkbox1);

            var checkbox2 = new CheckBox
            {
                Text = "CheckBox2",
                Color = Color.Red,
                BackgroundColor = global::Tizen.NUI.Color.Green,
            };
            checkbox2.SizeHeight = (float)checkbox2.Measure(300, 300).Height + 20;
            checkbox2.SizeWidth = (float)checkbox2.Measure(300, 300).Width;

            view.Add(checkbox2);


            var switch1 = new Switch();
            switch1.SizeHeight = (float)switch1.Measure(300, 300).Height;
            switch1.SizeWidth = (float)switch1.Measure(300, 300).Width;
            view.Add(switch1);

            var switch2 = new Switch
            {
                OnColor = Color.Red
            };
            switch2.SizeHeight = (float)switch2.Measure(300, 300).Height;
            switch2.SizeWidth = (float)switch2.Measure(300, 300).Width;
            view.Add(switch2);

            return view;
        }
    }
}
