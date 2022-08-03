using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using TColor = Tizen.UIExtensions.Common.Color;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A custom popup with message and returning a bool value
    /// </summary>
    public class MessagePopup : Popup<bool>
    {
        string _title;
        string _message;
        string _cancel;
        string? _accept;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePopup"/> class.
        /// </summary>
        /// <param name="title">Title text</param>
        /// <param name="message">Message text</param>
        /// <param name="accept">Accept text</param>
        /// <param name="cancel">Cancel text</param>
        public MessagePopup(string title, string message, string accept, string cancel)
        {
            _title = title;
            _message = message;
            _accept = accept;
            _cancel = cancel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePopup"/> class.
        /// </summary>
        /// <param name="title">Title text</param>
        /// <param name="message">Message text</param>
        /// <param name="confirm">Confirm text</param>
        public MessagePopup(string title, string message, string confirm)
        {
            _title = title;
            _message = message;
            _cancel = confirm;
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
                BoxShadow = new Shadow(20d.ToPixel(), TColor.FromHex("#333333").ToNative()),
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

            // message
            content.Add(new Label
            {
                Text = _message,
                Margin = new Extents(margin1, margin1, 0, margin2),
                LineBreakMode = LineBreakMode.CharacterWrap,
                PixelSize = 16d.ToPixel(),
                WidthSpecification = LayoutParamPolicies.MatchParent,
            });

            var hlayout = new View
            {
                Margin = new Extents(margin1, margin1, 0, margin1),
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

            var cancelButton = new Button
            {
                Text = _cancel,
                Focusable = true,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                TextColor = TColor.Black,
                BackgroundColor = TColor.Transparent.ToNative(),
            };
            cancelButton.TextLabel.PixelSize = 15d.ToPixel();
            cancelButton.SizeWidth = cancelButton.TextLabel.NaturalSize.Width + 15d.ToPixel() * 2;
            cancelButton.Clicked += (s, e) => SendSubmit(false);
            hlayout.Add(cancelButton);

            if (_accept != null)
            {
                var acceptButton = new Button
                {
                    Margin = new Extents(margin2, 0, 0, 0),
                    Text = _accept,
                    Focusable = true,
                    HeightSpecification = LayoutParamPolicies.WrapContent,
                    TextColor = TColor.Black,
                    BackgroundColor = TColor.Transparent.ToNative(),
                };
                acceptButton.TextLabel.PixelSize = 15d.ToPixel();
                acceptButton.SizeWidth = acceptButton.TextLabel.NaturalSize.Width + 15d.ToPixel() * 2;
                acceptButton.Clicked += (s, e) => SendSubmit(true);
                hlayout.Add(acceptButton);
            }

            Relayout += (s, e) =>
            {
                var isHorizontal = Window.Instance.WindowSize.Width > Window.Instance.WindowSize.Height;
                content.SizeWidth = Window.Instance.WindowSize.Width * (isHorizontal ? 0.5f : 0.8f);
            };

            return content;
        }
    }
}
