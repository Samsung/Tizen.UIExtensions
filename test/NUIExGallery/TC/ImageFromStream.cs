using System;
using System.Reflection;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Image = Tizen.UIExtensions.NUI.Image;

namespace NUIExGallery.TC
{
    public class ImageFromStream : TestCaseBase
    {
        public override string TestName => "Image Load from stream";

        public override string TestDescription => "Image Load from stream";

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

            LoadImage(view, 1000);
            LoadImage(view, 2000);
            LoadImage(view, 5000);

            return view;
        }

        async void LoadImage(View parent, int delay = 2000)
        {
            var img = new Image
            {
                SizeHeight = 300,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Aspect = Tizen.UIExtensions.Common.Aspect.AspectFill
            };
            img.ResourceReady += (s, e) =>
            {
                Console.WriteLine($"[{img.GetHashCode()}] Image Loaded - {img.LoadingStatus}");
            };
            parent.Add(img);
            bool run = true;
            img.RemovedFromWindow += (s, e) =>
            {
                run = false;
                (s as View).Dispose();
            };

            int count = 0;
            while (run)
            {
                var assembly = typeof(ImageFromStream).GetTypeInfo().Assembly;
                var assemblyName = assembly.GetName().Name;
                var resourcePath = assemblyName + ".Resource." + ((count++ % 2 == 0) ? "animated.gif" : "image2.jpg");
                var stream = assembly.GetManifestResourceStream(resourcePath);
                var result = await img.LoadAsync(stream);
                Console.WriteLine($"[{img.GetHashCode()}] Load result : {result}");
                await Task.Delay(delay);
            }

        }
    }
}
