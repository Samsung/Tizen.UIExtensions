﻿namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// Interface defining properties of formattable text.
    /// </summary>
    public interface ITextable
    {
        /// <summary>
        /// Get or sets the formatted text.
        /// </summary>
        FormattedString? FormattedText { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the color for the text.
        /// </summary>
        Color TextColor { get; set; }

        /// <summary>
        /// Gets or sets the background color for the text.
        /// </summary>
        Color TextBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the font family for the text.
        /// </summary>
        string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font attributes for the text.
        /// See <see cref="FontAttributes"/> for information about FontAttributes.
        /// </summary>
        FontAttributes FontAttributes { get; set; }

        /// <summary>
        /// Gets or sets the font size for the text.
        /// </summary>
        double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment mode for the text.
        /// See <see cref="TextAlignment"/> for information about TextAlignment.
        /// </summary>
        TextAlignment HorizontalTextAlignment { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment mode for the text.
        /// See <see cref="TextAlignment"/> for information about TextAlignment.
        /// </summary>
        TextAlignment VerticalTextAlignment { get; set; }

        /// <summary>
        /// Gets or sets the TextDecorations applied to Text.
        /// See <see cref="TextDecorations"/> for information about TextAlignment.
        /// </summary>
        TextDecorations TextDecorations { get; set; }
    }
}