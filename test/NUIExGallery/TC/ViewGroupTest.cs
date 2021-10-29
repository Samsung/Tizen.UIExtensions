using System;
using System.Linq;
using System.Threading;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;

namespace NUIExGallery.TC
{
    public class ViewGroupTest : TestCaseBase
    {
        public override string TestName => "ViewGroup test";

        public override string TestDescription => "ViewGroup test";

        public override View Run()
        {
            var viewgroup = new ViewGroup()
            {
                BackgroundColor = Tizen.NUI.Color.Red
            };
            viewgroup.LayoutUpdated += (s, e) =>
            {
                var blockSize = viewgroup.Size.Height / viewgroup.Children.Count;
                var currentTop = 0f;
                foreach (var child in viewgroup.Children)
                {
                    child.UpdateBounds(new Tizen.UIExtensions.Common.Rect(0, currentTop, viewgroup.Size.Width, blockSize));
                    Thread.Sleep(100);
                    currentTop += blockSize;
                }
            };

            var button = new Button
            {
                Text = "Add"
            };
            var removeButton = new Button
            {
                Text = "Remove"
            };
            viewgroup.Children.Add(button);
            viewgroup.Children.Add(removeButton);
            viewgroup.Children.Add(new View
            {
                BackgroundColor = Tizen.NUI.Color.Yellow
            });

            button.Clicked += (s, e) =>
            {
                var rnd = new Random();
                var view = new View();
                view.UpdateBackgroundColor(Tizen.UIExtensions.Common.Color.FromRgb(rnd.Next(10, 255), rnd.Next(10, 255), rnd.Next(10, 255)));
                viewgroup.Add(view);
            };

            removeButton.Clicked += (s, e) =>
            {
                var last = viewgroup.Children.Last();
                viewgroup.Remove(last);
                last.Dispose();
            };

            return viewgroup;
        }
    }
}
