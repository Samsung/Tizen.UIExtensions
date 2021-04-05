using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.ElmSharp;
using Color = Tizen.UIExtensions.Common.Color;

namespace ElmSharpExGallery.TC
{
    public class ShapeLineTest : TestCaseBase
    {
        public override string TestName => "Shape LineView Test";

        public override string TestDescription => "Test LineView";

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

            var lineText1 = new ElmSharp.Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                Text = "LineView with SolidBrush"
            };
            lineText1.Show();
            var line1 = new LineView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                Stroke = new SolidColorBrush(Color.Maroon),
                StrokeThickness = 5,
                Aspect = Stretch.Uniform,
                X1 = 100,
                Y1 = 0,
                X2 = 500,
                Y2 = 300
            };
            line1.Show();

            content.PackEnd(lineText1);
            content.PackEnd(line1);

            var lineText2 = new ElmSharp.Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                Text = "LineView with GradientBrush"
            };
            lineText2.Show();
            var line2 = new LineView(parent)
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
                StrokeThickness = 15,
                Aspect = Stretch.Uniform,
                X1 = 100,
                Y1 = 300,
                X2 = 500,
                Y2 = 100
            };
            line2.Show();
            content.PackEnd(lineText2);
            content.PackEnd(line2);

            var lineText3 = new ElmSharp.Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                Text = "LineView StrokeDashArray"
            };
            lineText3.Show();
            var line3 = new LineView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                Stroke = new SolidColorBrush(Color.PaleGoldenrod),
                StrokeThickness = 5,
                Aspect = Stretch.Uniform,
                X1 = 100,
                Y1 = 300,
                X2 = 500,
                Y2 = 100,
                StrokeDashArray = new float[] { 9, 3, 5, 3},
                StrokeDashOffset = 5
            };
            line3.Show();

            content.PackEnd(lineText3);
            content.PackEnd(line3);

            var lineText4 = new ElmSharp.Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                Text = "LineView PenLineCap Test"
            };
            lineText4.Show();
            var line4 = new LineView(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
                Stroke = new SolidColorBrush(Color.Red),
                StrokeThickness = 15,
                Aspect = Stretch.Uniform,
                X1 = 100,
                Y1 = 300,
                X2 = 500,
                Y2 = 100
            };
            line4.Show();

            content.PackEnd(lineText4);
            content.PackEnd(line4);

            var lineCapButton = new Button(parent)
            {
                Text = "Change PenLineCap",
                WeightX = 0.5,
                WeightY = 0.3,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            lineCapButton.Show();
            lineCapButton.Clicked += (s, e) =>
            {
                switch (line4.StrokeLineCap)
                {
                    case (PenLineCap.Flat):
                        line4.StrokeLineCap = PenLineCap.Round;
                        break;
                    case (PenLineCap.Round):
                        line4.StrokeLineCap = PenLineCap.Square;
                        break;
                    case (PenLineCap.Square):
                        line4.StrokeLineCap = PenLineCap.Flat;
                        break;
                }
            };
            content.PackEnd(lineCapButton);
        }
    }
}
