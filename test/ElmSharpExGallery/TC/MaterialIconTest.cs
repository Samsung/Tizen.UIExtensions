using System;
using System.Linq;
using Tizen.UIExtensions.Common.GraphicsView;
using Tizen.UIExtensions.ElmSharp;
using Tizen.UIExtensions.ElmSharp.GraphicsView;

namespace ElmSharpExGallery.TC
{
    public class MaterialIconTest : TestCaseBase
    {
        public override string TestName => "MaterialIcon Test";

        public override string TestDescription => "Test MaterialIcon";

        public override void Run(ElmSharp.Box parent)
        {
            var page = new ElmSharp.Box(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            page.Show();
            parent.PackEnd(page);

            foreach (var iconType in Enum.GetValues(typeof(MaterialIcons)).Cast<MaterialIcons>())
            {
                var box = new ElmSharp.Box(parent)
                {
                    IsHorizontal = true,
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                };
                box.Show();
      
                var label = new Label(parent)
                {
                    AlignmentX = 0.5,
                    AlignmentY = -1,
                    WeightY = 1,
                    WeightX =1,
                    Text = iconType.ToString(),
                };
                label.Show();

                var icon = new MaterialIcon(parent)
                {
                    AlignmentX = 0,
                    AlignmentY = -1,
                    WeightY = 1,
                    WeightX = 1,
                    IconType = iconType,
                };
                icon.Show();

                box.PackEnd(label);
                box.PackEnd(icon);

                page.PackEnd(box);
            }    

        }
    }
}
