using ElmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElmSharpExGallery.TC
{
    public class HelloWorld : TestCaseBase
    {
        public override string TestName => "Hello World";

        public override string TestDescription => "Hello world..";

        public override void Run(Box parent)
        {
            var eflLabel = new Label(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = 0.5,
                AlignmentX = 0.5,
                Text = "Hello World"
            };
            eflLabel.Show();
            parent.PackEnd(eflLabel);

        }
    }
}
