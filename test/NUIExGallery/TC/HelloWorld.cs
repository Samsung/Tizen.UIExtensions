using System;
using System.Collections.Generic;
using System.Text;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;

namespace NUIExGallery.TC
{
    public class HelloWorld : TestCaseBase
    {
        public override string TestName => "Hello World";

        public override string TestDescription => "Hello World";

        public override View Run()
        {
            var view = new View
            {
                Layout = new LinearLayout { 
                    LinearAlignment = LinearLayout.Alignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                }
            };

            view.Add(new TextLabel
            {
                Text = "Hello World",
                PixelSize = 30,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            return view;
        }
    }
}
