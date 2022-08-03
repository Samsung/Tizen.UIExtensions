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
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            BackgroundColor = new TColor(0.1f, 0.1f, 0.1f, 0.5f).ToNative();
            var margin1 = (ushort)20d.ToPixel();
            var margin2 = (ushort)10d.ToPixel();
            var radius = 8d.ToPixel();

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
                SizeWidth = Window.Instance.WindowSize.Width * 0.8f,
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

            var entry = new Entry
            {
                Margin = new Extents(margin1, margin1, 0, margin2),
                Text = _initialValue,
                Keyboard = _keyboard,
                Placeholder = _placeholder ?? "",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                PlaceholderColor = TColor.FromRgb(100, 100, 100),
                VerticalTextAlignment = TextAlignment.Center,
                SizeHeight = 40d.ToPixel(),
                PixelSize = 16d.ToPixel(),
                BackgroundColor = TColor.FromRgb(220,220,220).ToNative(),
            };

            if (_maxLength != -1)
            {
                entry.MaxLength = _maxLength;
            }
            content.Add(entry);

            var hlayout = new View
            {
                Margin = new Extents(margin1, margin1, 0, margin1),
                Layout = new LinearLayout
                {
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

                SizeWidth = (_cancel.Length + 1) * 15d.ToPixel(),
                HeightSpecification = LayoutParamPolicies.WrapContent,

                TextColor = TColor.Black,
                BackgroundColor = TColor.Transparent.ToNative(),
            };
            cancelButton.TextLabel.PixelSize = 15d.ToPixel();
            cancelButton.SizeWidth = cancelButton.TextLabel.NaturalSize.Width + 15d.ToPixel() * 2;

            cancelButton.Clicked += (s, e) => SendCancel();
            hlayout.Add(cancelButton);

            var acceptButton = new Button
            {
                Text = _accept,
                Focusable = true,

                SizeWidth = (_accept.Length + 1) * 15d.ToPixel(),
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Margin = new Extents(40, 0, 0, 0),

                TextColor = TColor.Black,
                BackgroundColor = TColor.Transparent.ToNative(),
            };
            acceptButton.TextLabel.PixelSize = 15d.ToPixel();
            acceptButton.SizeWidth = acceptButton.TextLabel.NaturalSize.Width + 15d.ToPixel() * 2;

            acceptButton.Clicked += (s, e) => SendSubmit(entry.Text);
            hlayout.Add(acceptButton);

            Relayout += (s, e) =>
            {
                content.SizeWidth = Window.Instance.WindowSize.Width * 0.8f;
            };

            return content;
        }
    }
}
