using System;
using ElmSharp;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;
using EEntry = ElmSharp.Entry;
using ESize = ElmSharp.Size;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// Extends the Entry control, providing basic formatting features,
    /// i.e. font color, size, placeholder.
    /// </summary>
    public class Entry : EEntry, IMeasurable, IBatchable, IEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class.
        /// </summary>
        /// <param name="parent">Parent evas object.</param>
        public Entry(EvasObject parent) : base(parent)
        {
            Initialize();
        }

        const int VariationNormal = 0;
        const int VariationSignedAndDecimal = 3;
        readonly Span _span = new Span();
        readonly Span _placeholderSpan = new Span();
        int _changedByUserCallbackDepth;
        Keyboard _keyboard;

        /// <summary>
        /// Occurs when the text block get focused.
        /// </summary>
        public event EventHandler TextBlockFocused;

        /// <summary>
        /// Occurs when the text block loses focus
        /// </summary>
        public event EventHandler TextBlockUnfocused;

        /// <summary>
        /// Occurs when the layout of entry get focused.
        /// </summary>
        public event EventHandler EntryLayoutFocused;

        /// <summary>
        /// Occurs when the layout of entry loses focus
        /// </summary>
        public event EventHandler EntryLayoutUnfocused;

        /// <summary>
        /// Occurs when the text has changed.
        /// </summary>
        public event EventHandler<TextChangedEventArgs> TextChanged;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public override string Text
        {
            get
            {
                return _span.Text;
            }

            set
            {

                if (value != _span.Text)
                {
                    var old = _span.Text;
                    _span.Text = value;
                    ApplyTextAndStyle();
                    //TODO: Adds BeginInvokeOnMainThread later
                    EcoreMainloop.AddTimer(TimeSpan.FromTicks(1).TotalSeconds, () =>
                    {
                        OnTextChanged(old, value);
                        return false;
                    });
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public Color TextColor
        {
            get
            {
                return _span.ForegroundColor;
            }

            set
            {
                if (!_span.ForegroundColor.Equals(value))
                {
                    _span.ForegroundColor = value;
                    ApplyTextAndStyle();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public string FontFamily
        {
            get
            {
                return _span.FontFamily;
            }

            set
            {
                if (value != _span.FontFamily)
                {
                    _span.FontFamily = value;
                    ApplyTextAndStyle();

                    _placeholderSpan.FontFamily = value;
                    ApplyPlaceholderAndStyle();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font family of the text and the placeholder.
        /// </summary>
        /// <value>The font family of the text and the placeholder.</value>
        public FontAttributes FontAttributes
        {
            get
            {
                return _span.FontAttributes;
            }

            set
            {
                if (value != _span.FontAttributes)
                {
                    _span.FontAttributes = value;
                    ApplyTextAndStyle();

                    _placeholderSpan.FontAttributes = value;
                    ApplyPlaceholderAndStyle();
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the font of both text and placeholder.
        /// </summary>
        /// <value>The size of the font of both text and placeholder.</value>
        public double FontSize
        {
            get
            {
                return _span.FontSize;
            }

            set
            {
                if (value != _span.FontSize)
                {
                    _span.FontSize = value;
                    ApplyTextAndStyle();

                    _placeholderSpan.FontSize = value;
                    ApplyPlaceholderAndStyle();
                }
            }
        }

        /// <summary>
        /// Gets or sets the horizontal text alignment of both text and placeholder.
        /// </summary>
        /// <value>The horizontal text alignment of both text and placeholder.</value>
        public TextAlignment HorizontalTextAlignment
        {
            get
            {
                return _span.HorizontalTextAlignment;
            }

            set
            {
                if (value != _span.HorizontalTextAlignment)
                {
                    _span.HorizontalTextAlignment = value;
                    ApplyTextAndStyle();

                    _placeholderSpan.HorizontalTextAlignment = value;
                    ApplyPlaceholderAndStyle();
                }
            }
        }

        /// <summary>
        /// Gets or sets the keyboard type used by the entry.
        /// </summary>
        /// <value>The keyboard type.</value>
        public Keyboard Keyboard
        {
            get
            {
                return _keyboard;
            }

            set
            {
                if (value != _keyboard)
                {
                    ApplyKeyboard(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the placeholder's text.
        /// </summary>
        /// <value>The placeholder's text.</value>
        public string Placeholder
        {
            get
            {
                return _placeholderSpan.Text;
            }

            set
            {
                if (value != _placeholderSpan.Text)
                {
                    _placeholderSpan.Text = value;
                    ApplyPlaceholderAndStyle();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the placeholder's text.
        /// </summary>
        /// <value>The color of the placeholder's text.</value>
        public Color PlaceholderColor
        {
            get
            {
                return _placeholderSpan.ForegroundColor;
            }

            set
            {
                if (!_placeholderSpan.ForegroundColor.Equals(value))
                {
                    _placeholderSpan.ForegroundColor = value;
                    ApplyPlaceholderAndStyle();
                }
            }
        }

        /// <summary>
        /// Implementation of the IMeasurable.Measure() method.
        /// </summary>
        public virtual Common.Size Measure(double availableWidth, double availableHeight)
        {
            var originalSize = Geometry;
            // resize the control using the whole available width
            Resize((int)availableWidth, originalSize.Height);

            ESize rawSize;
            ESize formattedSize;

            // if there's no text, but there's a placeholder, use it for measurements
            if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Placeholder))
            {
                rawSize = this.GetPlaceHolderTextBlockNativeSize();
                formattedSize = this.GetPlaceHolderTextBlockFormattedSize();
            }
            else
            {
                // there's text in the entry, use it instead
                rawSize = this.GetTextBlockNativeSize();
                formattedSize = this.GetTextBlockFormattedSize();
            }

            // restore the original size
            Resize(originalSize.Width, originalSize.Height);

            // Set bottom padding for lower case letters that have segments below the bottom line of text (g, j, p, q, y).
            var verticalPadding = (int)Math.Ceiling(0.05 * FontSize);
            var horizontalPadding = (int)Math.Ceiling(0.2 * FontSize);
            rawSize.Height += verticalPadding;
            formattedSize.Height += verticalPadding;
            formattedSize.Width += horizontalPadding;

            ESize size;

            // if the raw text width is larger than available width, we use the available width,
            // while height is set to the smallest height value
            if (rawSize.Width > (int)availableWidth)
            {
                size.Width = (int)availableWidth;
                size.Height = Math.Min(formattedSize.Height, Math.Max(rawSize.Height, (int)availableHeight));
            }
            else
            {
                // width is fine, return the formatted text size
                size = formattedSize;
            }

            return size.ToCommon();

        }

        protected virtual void OnTextBlockFocused()
        {
            TextBlockFocused?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnTextBlcokUnfocused()
        {
            TextBlockUnfocused?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnEntryLayoutFocused()
        {
            EntryLayoutFocused?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnEntryLayoutUnfocused()
        {
            EntryLayoutUnfocused?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnTextChanged(string oldValue, string newValue)
        {
            TextChanged?.Invoke(this, new TextChangedEventArgs(oldValue, newValue));
        }

        void Initialize()
        {
            Scrollable = true;
            ChangedByUser += (s, e) =>
            {
                _changedByUserCallbackDepth++;

                Text = GetInternalText();

                _changedByUserCallbackDepth--;
            };

            ApplyKeyboard(Keyboard.Normal);
        }

        void IBatchable.OnBatchCommitted()
        {
            ApplyTextAndStyle();
        }

        void ApplyTextAndStyle()
        {
            if (!this.IsBatched())
            {
                SetInternalTextAndStyle(_span.GetDecoratedText(), _span.GetStyle());
            }
        }

        void SetInternalTextAndStyle(string formattedText, string textStyle)
        {
            if (_changedByUserCallbackDepth == 0)
            {
                base.Text = formattedText;
                base.TextStyle = textStyle;
            }
        }

        string GetInternalText()
        {
            return EEntry.ConvertMarkupToUtf8(base.Text);
        }

        void ApplyKeyboard(Keyboard keyboard)
        {
            _keyboard = keyboard;
            SetInternalKeyboard(keyboard);
        }

        void SetInternalKeyboard(Keyboard keyboard)
        {
            if (keyboard == Keyboard.None)
            {
                SetInputPanelEnabled(false);
            }
            else if (Keyboard == Keyboard.Numeric)
            {
                SetInputPanelEnabled(true);
                SetInputPanelLayout(InputPanelLayout.NumberOnly);
                // InputPanelVariation is used to allow using deciaml point.
                InputPanelVariation = VariationSignedAndDecimal;
            }
            else
            {
                SetInputPanelEnabled(true);
                SetInputPanelLayout((InputPanelLayout)keyboard);
                InputPanelVariation = VariationNormal;
            }
        }

        void ApplyPlaceholderAndStyle()
        {
            SetInternalPlaceholderAndStyle(_placeholderSpan.GetMarkupText());
        }

        protected virtual void SetInternalPlaceholderAndStyle(string markupText)
        {
            this.SetPlaceHolderTextPart(markupText ?? "");
        }
    }
}