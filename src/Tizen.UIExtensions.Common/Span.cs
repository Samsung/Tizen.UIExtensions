namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// Represent a text with attributes applied.
    /// </summary>
    public class Span
    {

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <remarks>
        /// Setting Text to a non-null value will set the FormattedText property to null.
        /// </remarks>
        public string Text { get; set; }


        /// <summary>
        /// Gets or sets the color for the text.
        /// </summary>
        public Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background color for the text.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the font family for the text.
        /// </summary>
        public string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font attributes for the text.
        /// See <see cref="FontAttributes"/> for information about FontAttributes.
        /// </summary>
        public FontAttributes FontAttributes { get; set; }

        /// <summary>
        /// Gets or sets the font size for the text.
        /// </summary>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the line height.
        /// </summary>
        public double LineHeight { get; set; }

        /// <summary>
        /// Gets or sets the line break mode for the text.
        /// See <see cref="LineBreakMode"/> for information about LineBreakMode.
        /// </summary>
        public LineBreakMode LineBreakMode { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment mode for the text.
        /// See <see cref="TextAlignment"/> for information about TextAlignment.
        /// </summary>
        public TextAlignment HorizontalTextAlignment { get; set; }

        /// <summary>
        /// Gets or sets the TextDecorations applied to Text.
        /// </summary>
        public TextDecorations TextDecorations { get; set; }

        /// <summary>
        /// Create a new Span instance with default attributes.
        /// </summary>
        public Span()
        {
            Text = "";
            FontFamily = "";
            FontSize = -1;
            FontAttributes = FontAttributes.None;
            ForegroundColor = Color.Default;
            BackgroundColor = Color.Default;
            HorizontalTextAlignment = TextAlignment.None;
            LineBreakMode = LineBreakMode.None;
            TextDecorations = TextDecorations.None;
            LineHeight = -1.0d;
        }

        /// <summary>
        /// Converts string value to Span.
        /// </summary>
        /// <param name="text">The string text</param>
        public static implicit operator Span(string text)
        {
            return new Span { Text = text };
        }
    }
}