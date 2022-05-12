using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;

namespace NUIExGallery.TC
{
    public class PerformanceMonitorTest : TestCaseBase
    {
        public override string TestName => "!Performance Monitor";

        public override string TestDescription => "!Performance Monitor";

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

            var on = new Button()
            {
                Text = "On"
            };
            view.Add(on);

            var off = new Button()
            {
                Text = "Off"
            };
            view.Add(off);

            on.Clicked += (s, e) => PerformanceMonitor.Attach();
            off.Clicked += (s, e) => PerformanceMonitor.Detach();

            return view;
        }
    }
}
