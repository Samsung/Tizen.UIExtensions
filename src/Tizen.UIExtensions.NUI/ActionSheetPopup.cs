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

            var content = new View
            {
                Layout = new LinearLayout
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                },
                SizeWidth = Window.Instance.WindowSize.Width * 0.8f,
                BackgroundColor = TColor.White.ToNative(),
            };
            content.Add(new Label
            {
                Text = _title,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Padding = new Extents(10, 10, 10, 10),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = TColor.White,
                FontSize = 6 * DeviceInfo.ScalingFactor,
                BackgroundColor = TColor.FromHex("#344955").ToNative()
            });

            if (_buttons != null)
            {
                var scrollview = new ScrollView
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Always,
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
                        HorizontalTextAlignment = TextAlignment.Center,
                        PixelSize = (int)(25 * DeviceInfo.ScalingFactor),
                        WidthSpecification = LayoutParamPolicies.MatchParent,
                        HeightSpecification = LayoutParamPolicies.WrapContent,
                        Margin = new Extents(0, 0, 10, 10),
                    };
                    itemLabel.TouchEvent += (s, e) =>
                    {
                        var state = e.Touch.GetState(0);
                        if (state == PointStateType.Up)
                        {
                            SendSubmit(item);
                            return true;
                        }
                        return false;
                    };
                    itemLabel.KeyEvent += (s, e) =>
                    {
                        if (e.Key.State == Key.StateType.Up && (e.Key.KeyPressedName == "Return" || e.Key.KeyPressedName == "Enter"))
                        {
                            SendSubmit(item);
                            return true;
                        }
                        return false;
                    };
                    scrollview.ContentContainer.Add(itemLabel);
                    scrollview.ContentContainer.Add(new View
                    {
                        BackgroundColor = TColor.Black.ToNative(),
                        SizeHeight = 2,
                        WidthSpecification = LayoutParamPolicies.MatchParent,
                    });
                }

                scrollview.SizeHeight = (float)((DeviceInfo.ScalingFactor * 45 + 2) * Math.Min(_buttons.Count(), 5));
            }

            var hlayout = new View
            {
                Layout = new LinearLayout
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
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
                    Margin = new Extents(20, 20, 10, 10),
                    Text = _destruction,
                    SizeWidth = content.SizeWidth * 0.4f,
                    HeightSpecification = LayoutParamPolicies.WrapContent,
                };
                destructionButton.Clicked += (s, e) => SendSubmit(_destruction);
                hlayout.Add(destructionButton);
            }

            var cancelButton = new Button
            {
                Margin = new Extents(20, 20, 10, 10),
                Text = _cancel,
                SizeWidth = content.SizeWidth * 0.4f,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };
            cancelButton.Clicked += (s, e) => SendCancel();
            hlayout.Add(cancelButton);

            content.Relayout += (s, e) =>
            {
                hlayout.Children[0].SizeWidth = content.SizeWidth * 0.4f;
                if (hlayout.Children.Count > 1)
                {
                    hlayout.Children[1].SizeWidth = content.SizeWidth * 0.4f;
                }
            };
            Relayout += (s, e) =>
            {
                content.SizeWidth = Window.Instance.WindowSize.Width * 0.8f;
            };

            return content;
        }
    }
}
