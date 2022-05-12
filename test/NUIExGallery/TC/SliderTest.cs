using System.Collections.Generic;
using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.NUI.GraphicsView;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class SliderTest : TestCaseBase
    {
        public override string TestName => "Slider Test";

        public override string TestDescription => "Slider test1";

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

            var slider1 = new Slider
            {
                Value = 0,
                Minimum = 0,
                Maximum = 1,
                SizeHeight = 50,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };
            view.Add(slider1);

            var label1 = new Label();
            slider1.ValueChanged += (s, e) =>
            {
                label1.Text = $"{slider1.Value}";
            };
            view.Add(label1);

            var slider2 = new Slider
            {
                Value = 0,
                Minimum = 0,
                Maximum = 100,
                SizeHeight = 50,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                MinimumTrackColor = Color.Blue,
                MaximumTrackColor = Color.Red,
                ThumbColor = Color.Yellow,
            };
            view.Add(slider2);

            List<Label> labels = new List<Label>();
            for (int i =0; i < 10; i++)
            {
                labels.Add(new Label());
                view.Add(labels.Last());
            }

            slider2.ValueChanged += (s, e) =>
            {
                foreach (var label in labels)
                    label.Text = $"{slider2.Value}";
            };

            return view;
        }
    }
}
