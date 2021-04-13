using System;
using Tizen.UIExtensions.Common;
using NButton = Tizen.NUI.Components.Button;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// Extends the Tizen.NUI.Components.Button, providing Tizen.UIExtensions.Common data struct
    /// </summary>
    public class Button : NButton, IMeasurable
    {
        FontAttributes _fontAttributes;

        public Button()
        {
            TextLabel.MultiLine = true;
        }

        /// <summary>
        /// Gets or sets the color of the formatted text.
        /// </summary>
        /// <value>The color of the text.</value>
        public new Color TextColor
        {
            get => base.TextColor.ToCommon();
            set => base.TextColor = value.ToNative();
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
            get => PointSize;
            set => PointSize = (float)value;
        }

        public Size Measure(double availableWidth, double availableHeight)
        {
            // Issue : NaturalSize of Button is fixed when SizeWidth and SizeHight is set
            // so, Button's measured size never smaller than before
            var buttonNaturalSize = NaturalSize;
            var textNaturalSize = TextLabel.NaturalSize;
            float buttonPadding = 46;

#pragma warning disable CS0618
            // select bigger size between button and label
            var requiredWidth = Math.Max(buttonNaturalSize.Width, textNaturalSize.Width + buttonPadding);

            if (availableWidth < requiredWidth)
            {
                return new Size(availableWidth, Math.Max(GetHeightForWidth((float)availableWidth), TextLabel.GetHeightForWidth((float)availableWidth)));
            }
            else
            {
                return new Size(requiredWidth, Math.Max(GetHeightForWidth(requiredWidth), TextLabel.GetHeightForWidth(requiredWidth)));
            }
#pragma warning restore CS0618
        }
    }
}
