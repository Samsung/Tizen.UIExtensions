using System;
using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.NUI.GraphicsView;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class MaterialIconButtonTest : TestCaseBase
    {
        public override string TestName => "MaterialIconButton Test";

        public override string TestDescription => "MaterialIconButton test1";

        public override View Run()
        {
            var scrollview = new Tizen.UIExtensions.NUI.ScrollView();

            scrollview.ContentContainer.Layout = new LinearLayout
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                LinearOrientation = LinearLayout.Orientation.Vertical,
            };

            var view = scrollview.ContentContainer;

            view.Add(new Label
            {
                Text = "MaterialIconButton",
                TextColor = Color.White,
                FontSize = 9,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Center,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                SizeHeight = 100,
                Padding = new Extents(20, 10, 10, 10),
                BackgroundColor = Color.FromHex("#2196f3").ToNative(),
                BoxShadow = new Shadow(5, Color.FromHex("#bbbbbb").ToNative(), new Vector2(0, 5))
            });

            view.Add(new View
            {
                SizeHeight = 20,
            });

            foreach (var icon in Enum.GetValues(typeof(MaterialIcons)))
            {
                view.Add(new Label
                {
                    Padding = new Extents(10, 0, 0, 0),
                    Text = icon.ToString(),
                    FontSize = 7,
                    HorizontalTextAlignment = TextAlignment.Start,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.WrapContent,
                });

                {
                    var button = new MaterialIconButton()
                    {
                        Icon = (MaterialIcons)icon,
                    };

                    button.SizeHeight = (float)button.Measure(300, 300).Height;
                    button.SizeWidth = (float)button.Measure(300, 300).Width;
                    view.Add(button);
                }
            }


            foreach (var icon in Enum.GetValues(typeof(MaterialIcons)).Cast<MaterialIcons>().Take(3))
            {
                view.Add(new Label
                {
                    Padding = new Extents(10, 0, 0, 0),
                    Text = icon.ToString(),
                    FontSize = 7,
                    HorizontalTextAlignment = TextAlignment.Start,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.WrapContent,
                });

                {
                    var button = new MaterialIconButton()
                    {
                        Icon = icon,
                    };

                    button.SizeHeight = 100;
                    button.SizeWidth = 100;
                    button.UpdateBackgroundColor(Color.Yellow);
                    view.Add(button);
                }
            }

            foreach (var icon in Enum.GetValues(typeof(MaterialIcons)).Cast<MaterialIcons>().Take(3))
            {
                view.Add(new Label
                {
                    Padding = new Extents(10, 0, 0, 0),
                    Text = icon.ToString(),
                    FontSize = 7,
                    HorizontalTextAlignment = TextAlignment.Start,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.WrapContent,
                });

                {
                    var button = new MaterialIconButton()
                    {
                        Icon = icon,
                    };

                    button.SizeHeight = (float)DeviceInfo.ScalingFactor * 10;
                    button.SizeWidth = (float)DeviceInfo.ScalingFactor * 10;
                    button.UpdateBackgroundColor(Color.Yellow);
                    view.Add(button);
                }
            }

            return scrollview;
        }
    }
}
