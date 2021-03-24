using ElmSharp;
using Tizen.Applications;

namespace ElmSharpExGallery.TC
{
    public class PerformanceTest : TestCaseBase
    {
        public override string TestName => "# Scroll Performance Test";

        public override string TestDescription => "Hello world..";

        public override void Run(Box parent)
        {
            var scroller = new Scroller(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentX = -1,
                AlignmentY = -1,
                ScrollBlock = ScrollBlock.Horizontal,
            };
            scroller.Show();

            var box = new Box(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            box.Show();

            scroller.SetContent(box);

            var images = new string[] { "image.png", "image2.jpg" };

            int numRows = 80;
            box.MinimumHeight = 200 * numRows;
            for (int i = 0; i < numRows; i++)
            {
                var innerScroller = new Scroller(parent)
                {
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                    MinimumHeight = 200,
                    MinimumWidth = 720,
                    ScrollBlock = ScrollBlock.Vertical,
                };

                innerScroller.Show();
                box.PackEnd(innerScroller);

                var innerBox = new Box(parent)
                {
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                    IsHorizontal = true,
                };
                innerBox.Show();

                int numCols = 10;
                innerBox.MinimumHeight = 200;
                innerBox.MinimumWidth = 200 * numCols;

                for (int j = 0; j < numCols; j++)
                {
                    var img = new Image(parent)
                    {
                        AlignmentX = -1,
                        AlignmentY = -1,
                        WeightX = 1,
                        WeightY = 1,
                        MinimumHeight = 200,
                        MinimumWidth = 200,
                    };
                    img.Load(Application.Current.DirectoryInfo.Resource + images[(i + j) % 2]);
                    img.Show();

                    img.Show();
                    innerBox.PackEnd(img);
                }
                innerScroller.SetContent(innerBox);
            }

            parent.PackEnd(scroller);

        }
    }
}
