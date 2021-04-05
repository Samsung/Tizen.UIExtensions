using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using Button = Tizen.UIExtensions.ElmSharp.Button;
using Color = Tizen.UIExtensions.Common.Color;

namespace ElmSharpExGallery.TC
{
    public class ShapeEllipseTest : TestCaseBase
    {
        public override string TestName => "Shape Ellipse Test";

        public override string TestDescription => "Test EllipseView";

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

            var ellipseText1 = new ElmSharp.Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                Text = "EllipseView with SolidBrush"
            };
            ellipseText1.Show();
            var ellipse1 = new EllipseView(parent)
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
            ellipse1.Show();

            content.PackEnd(ellipseText1);
            content.PackEnd(ellipse1);

            var ellipseText2 = new ElmSharp.Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                Text = "EllipseView with GradientBrush"
            };
            ellipseText2.Show();
            var ellipse2 = new EllipseView(parent)
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
            ellipse2.Show();
            content.PackEnd(ellipseText2);
            content.PackEnd(ellipse2);

            var ellipseText3 = new ElmSharp.Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                Text = "EllipseView Aspect Test"
            };
            ellipseText3.Show();
            var ellipse3 = new EllipseView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                Stroke = new SolidColorBrush(Color.PaleGoldenrod),
                StrokeThickness = 5,
                Fill = new SolidColorBrush(Color.MediumPurple),
                Aspect = Stretch.Fill
            };
            ellipse3.Show();
            var aspectButton = new Button(parent)
            {
                Text = "Change Aspect",
                WeightX = 0.5,
                WeightY = 0.5,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            aspectButton.Show();
            aspectButton.Clicked += (s, e) =>
            {
                switch (ellipse3.Aspect)
                {
                    case (Stretch.Fill):
                        ellipse3.Aspect = Stretch.Uniform;
                        break;
                    case (Stretch.Uniform):
                        ellipse3.Aspect = Stretch.UniformToFill;
                        break;
                    case (Stretch.UniformToFill):
                        ellipse3.Aspect = Stretch.Fill;
                        break;
                }
            };

            content.PackEnd(ellipseText3);
            content.PackEnd(ellipse3);
            content.PackEnd(aspectButton);
        }
    }
}
