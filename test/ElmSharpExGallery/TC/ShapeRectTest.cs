using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using Button = Tizen.UIExtensions.ElmSharp.Button;
using Color = Tizen.UIExtensions.Common.Color;

namespace ElmSharpExGallery.TC
{
    public class ShapeRectTest : TestCaseBase
    {
        public override string TestName => "Shape RectView Test";

        public override string TestDescription => "Test RectView";

        public override void Run(ElmSharp.Box parent)
        {
            var scrollView = new ScrollView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always,
            };
            scrollView.Show();

            var content = new ElmSharp.Box(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            scrollView.SetScrollCanvas(content);
            parent.PackEnd(scrollView);

            var rectText1 = new ElmSharp.Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                Text = "RectView with SolidBrush"
            };
            rectText1.Show();
            var rect1 = new RectView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                Stroke = new SolidColorBrush(Color.Maroon),
                StrokeThickness = 5,
                Fill = new SolidColorBrush(Color.Lavender),
                Aspect = Stretch.Uniform,
            };
            rect1.Show();

            content.PackEnd(rectText1);
            content.PackEnd(rect1);

            var rectText2 = new ElmSharp.Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                Text = "RectView with GradientBrush"
            };
            rectText2.Show();
            var rect2 = new RectView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                Stroke = new LinearGradientBrush(new System.Collections.Generic.List<GradientStop>()
                {
                    new GradientStop(Color.Lavender, 0.2f),
                    new GradientStop(Color.LightSkyBlue, 0.4f),
                    new GradientStop(Color.LightCyan, 0.6f),
                    new GradientStop(Color.LightPink, 0.8f),
                    new GradientStop(Color.YellowGreen, 1.0f),
                }),
                Fill = new RadialGradientBrush(new System.Collections.Generic.List<GradientStop>()
                {
                    new GradientStop(Color.YellowGreen, 0.2f),
                    new GradientStop(Color.LightPink, 0.4f),
                    new GradientStop(Color.LightCyan, 0.6f),
                    new GradientStop(Color.LightSkyBlue, 0.8f),
                    new GradientStop(Color.Lavender, 1.0f),
                }),
                StrokeThickness = 15,
                Aspect = Stretch.Fill,
            };
            rect2.Show();
            content.PackEnd(rectText2);
            content.PackEnd(rect2);

            var rectText3 = new ElmSharp.Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                Text = "RectView Aspect Test"
            };
            rectText3.Show();
            var rect3 = new RectView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                Stroke = new SolidColorBrush(Color.PaleGoldenrod),
                StrokeThickness = 5,
                Fill = new SolidColorBrush(Color.MediumPurple),
                Aspect = Stretch.Fill,
                RadiusX = 120,
                RadiusY = 120
            };
            rect3.Show();

            content.PackEnd(rectText3);
            content.PackEnd(rect3);
        }
    }
}
