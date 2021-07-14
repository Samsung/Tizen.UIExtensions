using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.NUI.GraphicsView;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class ActivityIndicatorTest : TestCaseBase
    {
        public override string TestName => "ActivityIndicator Test";

        public override string TestDescription => "ActivityIndicator test1";

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
            var ai = new ActivityIndicator();
            ai.SizeHeight = (float)ai.Measure(300, 300).Height;
            ai.SizeWidth = (float)ai.Measure(300, 300).Width;
            view.Add(ai);

            var ai2 = new ActivityIndicator
            {
                Color = Color.Red,
            };
            ai2.SizeHeight = (float)ai2.Measure(300, 300).Height;
            ai2.SizeWidth = (float)ai2.Measure(300, 300).Width;
            view.Add(ai2);

            var start = new Tizen.UIExtensions.NUI.Button()
            {
                Text = "start"
            };
            view.Add(start);
            start.Clicked += (s, e) =>
            {
                ai.IsRunning = true;
                ai2.IsRunning = true;
            };

            var stop = new Tizen.UIExtensions.NUI.Button()
            {
                Text = "stop"
            };
            view.Add(stop);
            stop.Clicked += (s, e) =>
            {
                ai.IsRunning = false;
                ai2.IsRunning = false;
            };

            return view;
        }
    }
}
