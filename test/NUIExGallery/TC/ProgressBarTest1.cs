using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI.GraphicsView;

namespace NUIExGallery.TC
{
    public class ProgressBarTest1 : TestCaseBase
    {
        public override string TestName => "ProgressBarTest1";

        public override string TestDescription => "ProgressBarTest1";

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
            var progressBar1 = new ProgressBar
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };
            progressBar1.SizeHeight = (float)progressBar1.Measure(300, 300).Height;

            view.Add(progressBar1);

            view.Add(new View
            {
                SizeHeight = 100,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            });

            var slider1 = new Slider
            {
                Value = 0,
                Minimum = 0,
                Maximum = 1,
                SizeHeight = 50,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };
            slider1.ValueChanged += (s, e) =>
            {
                progressBar1.Progress = slider1.Value;
            };
            view.Add(slider1);

            var btn1 = new Tizen.UIExtensions.NUI.Button()
            {
                Text = "To zero"
            };

            btn1.Clicked += (s, e) =>
            {
                progressBar1.ProgressTo(0);
            };
            view.Add(btn1);

            var btn2 = new Tizen.UIExtensions.NUI.Button()
            {
                Text = "To Max"
            };

            btn2.Clicked += (s, e) =>
            {
                progressBar1.ProgressTo(1);
            };
            view.Add(btn2);


            return view;
        }
    }
}
