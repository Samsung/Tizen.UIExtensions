using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using TColor = Tizen.UIExtensions.Common.Color;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A custom popup with entry and returning a string value
    /// </summary>
    public class PromptPopup : Popup<string>
    {
        string _title;
        string _message;
        string _accept;
        string _cancel;
        string? _placeholder;
        int _maxLength;
        Keyboard _keyboard;
        string _initialValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PromptPopup"/> class.
        /// </summary>
        /// <param name="title">Title text</param>
        /// <param name="message">Message text</param>
        /// <param name="accept">Accept text</param>
        /// <param name="cancel">Cancel text</param>
        /// <param name="placeholder">Placeholder text</param>
        /// <param name="maxLength">A max length of entry</param>
        /// <param name="keyboard">A keyboard type</param>
        /// <param name="initialValue">A initial value of entry</param>
        public PromptPopup(string title, string message, string accept = "OK", string cancel = "Cancel", string? placeholder = null, int maxLength = -1, Keyboard keyboard = Keyboard.Normal, string initialValue = "")
        {
            _title = title;
            _message = message;
            _accept = accept;
            _cancel = cancel;
            _placeholder = placeholder;
            _maxLength = maxLength;
            _keyboard = keyboard;
            _initialValue = initialValue;
        }

        protected override View CreateContent()
        {
            Layout = new LinearLayout
            {
                LinearAlignment = LinearLayout.Alignment.Center
            };
            BackgroundColor = new TColor(0.1f, 0.1f, 0.1f, 0.5f).ToNative();

            var content = new View
            {
                Layout = new LinearLayout
                {
                    LinearAlignment = LinearLayout.Alignment.Center,
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

            var entry = new Entry
            {
                Text = _initialValue,
                Keyboard = _keyboard,
                Placeholder = _placeholder ?? "",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                PlaceholderColor = TColor.FromRgb(100, 100, 100),
                HeightSpecification = LayoutParamPolicies.WrapContent,
                PixelSize = (int)(25 * DeviceInfo.ScalingFactor),
                BackgroundColor = TColor.FromRgb(220,220,220).ToNative(),
                Margin = new Extents(20, 20, 0, 0),
            };
            if (_maxLength != -1)
            {
                entry.MaxLength = _maxLength;
            }

            content.Add(entry);

            var hlayout = new View
            {
                Layout = new LinearLayout
                {
                    LinearAlignment = LinearLayout.Alignment.Center,
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
            cancelButton.Clicked += (s, e) => SendCancel();
            hlayout.Add(cancelButton);
            var acceptButton = new Button
            {
                Margin = new Extents(20, 20, 10, 10),
                Text = _accept,
                SizeWidth = content.SizeWidth * 0.4f,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            };
            acceptButton.Clicked += (s, e) => SendSubmit(entry.Text);
            hlayout.Add(acceptButton);

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
