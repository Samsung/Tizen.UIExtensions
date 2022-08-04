using Tizen.UIExtensions.Common;
using NButton = Tizen.NUI.Components.Button;
using NColor = Tizen.NUI.Color;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// Extends the Tizen.NUI.Components.Button, providing Tizen.UIExtensions.Common data struct
    /// </summary>
    public class Button : NButton, IMeasurable
    {
        float _defaultFontSize = 0;
        NColor? _defaultTextColor = null;
        FontAttributes _fontAttributes;

        public Button()
        {
            _defaultFontSize = TextLabel.PointSize;
            _defaultTextColor = TextLabel.TextColor;
            TextLabel.MultiLine = true;
        }

        /// <summary>
        /// Gets or sets the color of the formatted text.
        /// </summary>
        /// <value>The color of the text.</value>
        public new Color TextColor
        {
            get => base.TextColor.ToCommon();
            set => base.TextColor = value.IsDefault ? _defaultTextColor : value.ToNative();
        }

        /// <summary>
        /// Gets a value that indicates whether the font for the label is bold, italic, or neither
        /// </summary>
        public FontAttributes FontAttributes
        {
            get => _fontAttributes;
            set
            {
                _fontAttributes = value;
                TextLabel.SetFontAttributes(value);
            }
        }

        /// <summary>
        /// Gets or sets the font size for the text.
        /// </summary>
        /// <value>The size of the font as point unit</value>
        public double FontSize
        {
            get
            {
                return PointSize / DeviceInfo.FontScale.Value;
            }
            set
            {
                PointSize = value == -1 ? _defaultFontSize : (float)value * DeviceInfo.FontScale.Value;
            }
        }

        public Size Measure(double availableWidth, double availableHeight)
        {
            // Issue : NaturalSize of Button is fixed when SizeWidth and SizeHight is set
            // so, Button's measured size never smaller than before
            var textNaturalSize = TextLabel.NaturalSize;
            float buttonPadding = 46;
            float horizontalPadding = buttonPadding;
            float verticalPadding = buttonPadding;

            if (Icon != null)
            {
                if (IconRelativeOrientation == IconOrientation.Bottom || IconRelativeOrientation == IconOrientation.Top)
                {
                    verticalPadding += Icon.NaturalSize.Height;
                }
                else
                {
                    horizontalPadding += Icon.NaturalSize.Width;
                }
                if (IconPadding != null)
                {
                    verticalPadding += IconPadding.Top + IconPadding.Bottom;
                    horizontalPadding += IconPadding.Start + IconPadding.End;
                }
            }

            return new Size(textNaturalSize.Width + horizontalPadding, textNaturalSize.Height + verticalPadding);
        }
    }
}
