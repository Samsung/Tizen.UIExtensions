using ElmSharp;
using Ext = Tizen.UIExtensions.ElmSharp;

namespace ElmSharpExGallery.TC
{
    public class CarouselPageTest : TestCaseBase
    {
        public override string TestName => "CarouselPage test";

        public override string TestDescription => "CarouselPage test";

        public override void Run(Box parent)
        {
            var carouselPage = new Ext.CarouselPage(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = -1,
                AlignmentX = -1,
            };
            carouselPage.Show();

            carouselPage.ResetChildrenList();

            var box1 = new Box(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = 0.5,
                AlignmentX = 0.5,
                BackgroundColor = Color.Red,
            };
            box1.Show();
            carouselPage.AddChildrenList(box1);

            var box2 = new Box(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = 0.5,
                AlignmentX = 0.5,
                BackgroundColor = Color.Green,
            };
            box2.Show();
            carouselPage.AddChildrenList(box2);

            var box3 = new Box(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = 0.5,
                AlignmentX = 0.5,
                BackgroundColor = Color.Blue,
            };
            box3.Show();
            carouselPage.AddChildrenList(box3);

            carouselPage.UpdateIndexItem();

            parent.PackEnd(carouselPage);
        }
    }
}
