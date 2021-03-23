using System;
using ElmSharp;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;
using EEntry = ElmSharp.Entry;
using ESize = ElmSharp.Size;

namespace Tizen.UIExtensions.ElmSharp
{
    public class Entry : EEntry, IMeasurable, IBatchable, IEntry
    {
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

        public event EventHandler TextBlockFocused;
        public event EventHandler TextBlockUnfocused;
        public event EventHandler EntryLayoutFocused;
        public event EventHandler EntryLayoutUnfocused;

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
                }
            }
        }

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

        public virtual ESize Measure(int availableWidth, int availableHeight)
        {
            var originalSize = Geometry;
            // resize the control using the whole available width
            Resize(availableWidth, originalSize.Height);

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
            if (rawSize.Width > availableWidth)
            {
                size.Width = availableWidth;
                size.Height = Math.Min(formattedSize.Height, Math.Max(rawSize.Height, availableHeight));
            }
            else
            {
                // width is fine, return the formatted text size
                size = formattedSize;
            }

            return size;

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