using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;
using Size = Tizen.UIExtensions.Common.Size;
using NColor = Tizen.NUI.Color;

namespace Tizen.UIExtensions.NUI
{
    public class Label : TextLabel, IMeasurable, ITextable
    {
        FontAttributes _fontAttributes;
        LineBreakMode _lineBreakMode;
        TextDecorations _textDecorations;
        FormattedString _formattedText;

        float _defaultFontSize;
        NColor _defaultTextColor;

        public Label()
        {
            _defaultFontSize = PointSize;
            _defaultTextColor = base.TextColor;
        }

        /// <summary>
        /// Gets or sets the formatted text for the Label.
        /// </summary>
        public FormattedString FormattedText
        {
            get => _formattedText;
            set
            {
                _formattedText = value;
                Text = _formattedText?.ToMarkupText() ?? "";
                EnableMarkup = _formattedText != null;
            }
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
                this.SetFontAttributes(value);
            }
        }

        /// <summary>
        /// Gets or sets the TextDecorations applied to Text.
        /// </summary>
        public TextDecorations TextDecorations
        {
            get => _textDecorations;
            set
            {
                var hasUnderline = value.HasFlag(TextDecorations.Underline);
                bool updated = hasUnderline ^ _textDecorations.HasFlag(TextDecorations.Underline);

                if (updated)
                {
                    var line = new PropertyMap();
                    line.Add("enable", new PropertyValue(hasUnderline));
                    Underline = line;
                }

                _textDecorations = value;
            }

        }

        /// <summary>
        /// Gets or sets the LineBreakMode for the Label
        /// </summary>
        public LineBreakMode LineBreakMode
        {
            get => _lineBreakMode;
            set
            {
                _lineBreakMode = value;
                UpdateLineBreakMode(value);
            }
        }

        /// <summary>
        /// Gets or sets the background color for the text.
        /// </summary>
        /// <value>The color of the label's background.</value>
        public Color TextBackgroundColor 
        {
            get => BackgroundColor.ToCommon();
            set => BackgroundColor = value.ToNative();
        }

        /// <summary>
        /// Gets or sets the font size for the text.
        /// </summary>
        /// <value>The size of the font as point unit</value>
        public double FontSize 
        { 
            get
            {
                return PointSize;
            }
            set
            {
                PointSize = value == -1 ? _defaultFontSize : (float)value;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal text alignment.
        /// </summary>
        /// <value>The horizontal text alignment.</value>
        public TextAlignment HorizontalTextAlignment 
        {
            get => HorizontalAlignment.ToCommon();
            set => HorizontalAlignment = value.ToHorizontal();
        }


        public TextAlignment VerticalTextAlignment
        {
            get => VerticalAlignment.ToCommon();
            set => VerticalAlignment = value.ToVertical();
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

        Color ITextable.TextColor 
        {
            get => TextColor;
            set => TextColor = value;
        }

        /// <summary>
        /// Implements <see cref="IMeasurable"/> to provide a desired size of the label.
        /// </summary>
        /// <param name="availableWidth">Available width.</param>
        /// <param name="availableHeight">Available height.</param>
        /// <returns>Size of the control that fits the available area.</returns>
        public Size Measure(double availableWidth, double availableHeight)
        {
#pragma warning disable CS0618
            if (availableWidth < NaturalSize.Width)
            {
                return new Size(availableWidth, GetHeightForWidth((float)availableWidth));
            }
            else
            {
                return new Size(NaturalSize.Width, GetHeightForWidth(NaturalSize.Width));
            }
#pragma warning restore CS0618
        }

        void UpdateLineBreakMode(LineBreakMode mode)
        {
            switch (mode)
            {
                case LineBreakMode.NoWrap:
                    MultiLine = false;
                    Ellipsis = false;
                    break;
                case LineBreakMode.WordWrap:
                    MultiLine = true;
                    Ellipsis = false;
                    LineWrapMode = LineWrapMode.Word;
                    break;
                case LineBreakMode.CharacterWrap:
                    MultiLine = true;
                    Ellipsis = false;
                    LineWrapMode = LineWrapMode.Character;
                    break;
                case LineBreakMode.HeadTruncation:
                case LineBreakMode.TailTruncation:
                case LineBreakMode.MiddleTruncation:
                    MultiLine = false;
                    Ellipsis = true;
                    break;
            }
        }
    }
}
