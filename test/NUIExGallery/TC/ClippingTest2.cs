using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.Common;
using SkiaSharp;
using Tizen.Applications;
using Color = Tizen.NUI.Color;

namespace NUIExGallery.TC
{
    public class ClippingTest2 : TestCaseBase
    {
        public override string TestName => "Clipping Test2";

        public override string TestDescription => "Clipping test1";

        bool isCircle = true;

        public override View Run()
        {
            var scrollView = new Tizen.UIExtensions.NUI.ScrollView
            {
                BackgroundColor = Color.Blue,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent
            };

            scrollView.ContentContainer.Layout = new LinearLayout
            {
                LinearAlignment = LinearLayout.Alignment.Center,
                LinearOrientation = LinearLayout.Orientation.Vertical,
            };

            scrollView.ContentContainer.WidthSpecification = LayoutParamPolicies.MatchParent;
            scrollView.ContentContainer.HeightSpecification = LayoutParamPolicies.WrapContent;

            {
                var clipper = new SKClipperView
                {
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    SizeHeight = 300,
                };
                clipper.DrawClippingArea += OnDrawCircle;
                clipper.Add(new Image
                {
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                    ResourceUrl = Application.Current.DirectoryInfo.Resource + "dotnet_bot.png",
                });

                scrollView.ContentContainer.Add(clipper);

                var btn = new Button
                {
                    Text = "Change"
                };
                scrollView.ContentContainer.Add(btn);

                btn.Clicked += (s, e) =>
                {
                    if (isCircle)
                    {
                        clipper.DrawClippingArea -= OnDrawCircle;
                        clipper.DrawClippingArea += OnDrawRoundRect;
                    }
                    else
                    {
                        clipper.DrawClippingArea -= OnDrawRoundRect;
                        clipper.DrawClippingArea += OnDrawCircle;
                    }
                    isCircle = !isCircle;
                    clipper.Invalidate();
                };
            }

            for (int i = 0; i < 10; i++)
            {
                var clipper = new SKClipperView
                {
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    SizeHeight = 300,
                };
                clipper.DrawClippingArea += (s, e) =>
                {
                    var canvas = e.Surface.Canvas;
                    var width = e.Info.Width;
                    var height = e.Info.Height;

                    using (var paint = new SKPaint
                    {
                        IsAntialias = true,
                        Color = SKColors.White,
                        Style = SKPaintStyle.Fill,
                    })
                    {
                        canvas.Clear();
                        canvas.DrawCircle(width / 2.0f, height / 2.0f, Math.Min(width, height) / 2.0f, paint);
                    }
                };

                clipper.Add(new Image
                {
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                    ResourceUrl = Application.Current.DirectoryInfo.Resource + "dotnet_bot.png",
                });

                clipper.Add(new Label
                {
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                    Text = "Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text",
                    LineBreakMode = LineBreakMode.CharacterWrap,

                });
                scrollView.ContentContainer.Add(clipper);
            }

            return scrollView;
        }

        private void OnDrawRoundRect(object sender, SkiaSharp.Views.Tizen.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var width = e.Info.Width;
            var height = e.Info.Height;

            using (var paint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColors.White,
                Style = SKPaintStyle.Fill,
            })
            {
                canvas.Clear();
                canvas.DrawRoundRect(100, 10, width - 200, height - 20, 40, 40, paint);
            }
        }

        private void OnDrawCircle(object sender, SkiaSharp.Views.Tizen.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var width = e.Info.Width;
            var height = e.Info.Height;

            using (var paint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColors.White,
                Style = SKPaintStyle.Fill,
            })
            {
                canvas.Clear();
                canvas.DrawCircle(width / 2.0f, height / 2.0f, Math.Min(width, height) / 2.0f, paint);
            }
        }
    }
}
