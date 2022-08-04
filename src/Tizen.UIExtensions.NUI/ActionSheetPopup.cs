using System;
using System.Collections.Generic;
using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using TColor = Tizen.UIExtensions.Common.Color;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A custom popup with list and returning a string value
    /// </summary>
    public class ActionSheetPopup : Popup<string>
    {
        string _title;
        string _cancel;
        string? _destruction;
        IEnumerable<string>? _buttons;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionSheetPopup"/> class.
        /// </summary>
        /// <param name="title">Title text</param>
        /// <param name="cancel">Cancel text</param>
        /// <param name="destruction">Destruction text</param>
        /// <param name="buttons">A value list to choose</param>
        public ActionSheetPopup(string title, string cancel, string? destruction = null, IEnumerable<string>? buttons = null)
        {
            _title = title;
            _cancel = cancel;
            _destruction = destruction;
            _buttons = buttons;
        }

        protected override View CreateContent()
        {
            Layout = new LinearLayout
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            BackgroundColor = new TColor(0.1f, 0.1f, 0.1f, 0.5f).ToNative();
            var margin1 = (ushort)20d.ToPixel();
            var margin2 = (ushort)10d.ToPixel();
            var radius = 8d.ToPixel();

            var isHorizontal = Window.Instance.WindowSize.Width > Window.Instance.WindowSize.Height;
            // container
            var content = new View
            {
                CornerRadius = radius,
                BoxShadow = new Shadow(20d.ToPixel(), TColor.Black.ToNative()),
                Layout = new LinearLayout
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                },
                SizeWidth = Window.Instance.WindowSize.Width * (isHorizontal ? 0.5f : 0.8f),
                BackgroundColor = TColor.White.ToNative(),
            };

            // title
            content.Add(new Label
            {
                Text = _title,
                Margin = new Extents(margin1, margin1, margin1, margin2),
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = TColor.FromHex("#000000"),
                PixelSize = 21d.ToPixel(),
            });

            if (_buttons != null)
            {
                // separator
                content.Add(new View
                {
                    BackgroundColor = TColor.FromHex("#cccccc").ToNative(),
                    SizeHeight = 1.5d.ToPixel(),
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                });

                var scrollview = new ScrollView
                {
                    Margin = new Extents(margin1, margin1, 0, 0),
                    VerticalScrollBarVisibility = ScrollBarVisibility.Default,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                };
                scrollview.ContentContainer.Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                };
                content.Add(scrollview);

                foreach (var item in _buttons)
                {
                    var itemLabel = new Label
                    {
                        Text = item,
                        Focusable = true,
                        HorizontalTextAlignment = TextAlignment.Start,
                        PixelSize = 16d.ToPixel(),
                        WidthSpecification = LayoutParamPolicies.MatchParent,
                        HeightSpecification = LayoutParamPolicies.WrapContent,
                        Margin = new Extents(0, 0, (ushort)5d.ToPixel(), (ushort)5d.ToPixel()),
                    };
                    itemLabel.TouchEvent += (s, e) =>
                    {
                        var state = e.Touch.GetState(0);
                        if (state == PointStateType.Up && itemLabel.IsInside(e.Touch.GetLocalPosition(0)))
                        {
                            SendSubmit(item);
                            return true;
                        }
                        return false;
                    };
                    itemLabel.KeyEvent += (s, e) =>
                    {
                        if (e.Key.IsAcceptKeyEvent())
                        {
                            SendSubmit(item);
                            return true;
                        }
                        return false;
                    };
                    scrollview.ContentContainer.Add(itemLabel);
                }
                scrollview.SizeHeight = 30d.ToPixel() * Math.Min(_buttons.Count(), 5);

                // separator
                content.Add(new View
                {
                    BackgroundColor = TColor.FromHex("#cccccc").ToNative(),
                    SizeHeight = 1.5d.ToPixel(),
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                });
            }

            var hlayout = new View
            {
                Margin = new Extents(margin1, margin1, margin2, margin1),
                Layout = new LinearLayout
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.End,
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                },
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };
            content.Add(hlayout);

            if (_destruction != null)
            {
                var destructionButton = new Button
                {
                    Focusable = true,
                    Text = _destruction,
                    Margin = new Extents(0, margin2, 0, 0),
                    TextColor = TColor.Black,
                    BackgroundColor = TColor.Transparent.ToNative(),
                };
                destructionButton.TextLabel.PixelSize = 15d.ToPixel();
                destructionButton.SizeWidth = destructionButton.TextLabel.NaturalSize.Width + 15d.ToPixel() * 2;
                destructionButton.Clicked += (s, e) => SendSubmit(_destruction);
                hlayout.Add(destructionButton);
            }

            var cancelButton = new Button
            {
                Focusable = true,
                Text = _cancel,
                TextColor = TColor.Black,
                BackgroundColor = TColor.Transparent.ToNative(),
            };
            cancelButton.TextLabel.PixelSize = 15d.ToPixel();
            cancelButton.SizeWidth = cancelButton.TextLabel.NaturalSize.Width + 15d.ToPixel() * 2;
            cancelButton.Clicked += (s, e) => SendCancel();
            hlayout.Add(cancelButton);

            Relayout += (s, e) =>
            {
                var isHorizontal = Window.Instance.WindowSize.Width > Window.Instance.WindowSize.Height;
                content.SizeWidth = Window.Instance.WindowSize.Width * (isHorizontal ? 0.5f : 0.8f);
            };

            return content;
        }
    }
}
