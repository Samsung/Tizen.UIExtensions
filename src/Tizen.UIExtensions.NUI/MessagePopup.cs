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
            content.Add(new Label
            {
                LineBreakMode = LineBreakMode.CharacterWrap,
                Margin = new Extents(10, 10, 10, 10),
                Text = _message,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            });

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

            var cancelButton = new Button
            {
                Margin = new Extents(20, 20, 10, 10),
                Text = _cancel,
                SizeWidth = content.SizeWidth * 0.4f,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };
            cancelButton.Clicked += (s, e) => SendSubmit(false);
            hlayout.Add(cancelButton);

            if (_accept != null)
            {
                var acceptButton = new Button
                {
                    Margin = new Extents(20, 20, 10, 10),
                    Text = _accept,
                    SizeWidth = content.SizeWidth * 0.4f,
                    HeightSpecification = LayoutParamPolicies.WrapContent,
                };
                acceptButton.Clicked += (s, e) => SendSubmit(true);
                hlayout.Add(acceptButton);
            }

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
