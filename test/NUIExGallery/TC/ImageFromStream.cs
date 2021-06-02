using System;
using System.Collections.Generic;
using System.Text;
using Tizen.NUI.BaseComponents;
using ScrollView = Tizen.UIExtensions.NUI.ScrollView;
using Image = Tizen.UIExtensions.NUI.Image;
using Tizen.NUI;
using Tizen.Applications;
using Tizen.UIExtensions.NUI;
using System.Reflection;
using System.Threading.Tasks;

namespace NUIExGallery.TC
{
    public class ImageFromStream : TestCaseBase
    {
        public override string TestName => "Image Load from stream";

        public override string TestDescription => "Image Load from stream";

        Image _img;
        bool _stop;

        public override View Run()
        {
            _stop = false;
            var view = new View
            {
                Layout = new LinearLayout
                {
                    LinearAlignment = LinearLayout.Alignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                }
            };

            {
                var img = new Image
                {
                    SizeHeight = 300,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    Aspect = Tizen.UIExtensions.Common.Aspect.AspectFill
                };
                img.ResourceReady += (s, e) =>
                {
                    Console.WriteLine($"Image Loaded - {img.LoadingStatus}");
                };
                _img = img;
                LoadStream();
                view.Add(img);
            }
            view.RemovedFromWindow += (s, e) =>
            {
                _img.Dispose();
                _stop = true;
            };

            return view;
        }

        int count = 0;

        async void LoadStream()
        {
            var assembly = typeof(ImageFromStream).GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName().Name;
            var resourcePath = assemblyName + ".Resource." + ((count++ % 2 == 0) ? "animated.gif" : "image2.jpg");
            var stream = assembly.GetManifestResourceStream(resourcePath);
            await _img.LoadAsync(stream);

            _img.Layout.RequestLayout();
            await Task.Delay(1000);

            if (_stop)
            {
                return;
            }
            LoadStream();
        }
    }
}
