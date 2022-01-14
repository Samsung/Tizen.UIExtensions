using System;
using Tizen.UIExtensions.Common;
using EvasObject = ElmSharp.EvasObject;
using Radio = ElmSharp.Radio;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// Extends the ElmSharp.Radio control, providing basic formatting features,
    /// i.e. font color, size, text color.
    /// </summary>
    public class RadioButton : Radio, IMeasurable, IBatchable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="parent">Parent evas object.</param>
        public RadioButton(EvasObject parent) : base(parent)
        {
        }

        /// <summary>
        /// Holds the formatted text of the radio button.
        /// </summary>
        readonly Span _span = new Span();

        /// <summary>
        /// Gets or sets the radio button's text.
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
                    _span.Text = value;
                    ApplyTextAndStyle();
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
        /// Gets or sets the color of the text background.
        /// </summary>
        /// <value>The color of the text background.</value>
        public Color TextBackgroundColor
        {
            get
            {
                return _span.BackgroundColor;
            }

            set
            {
                if (!_span.BackgroundColor.Equals(value))
                {
                    _span.BackgroundColor = value;
                    ApplyTextAndStyle();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>The font family.</value>
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
                }
            }
        }

        /// <summary>
        /// Gets or sets the font attributes.
        /// </summary>
        /// <value>The font attributes.</value>
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
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
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
                }
            }
        }

        /// <summary>
        /// Implementation of the IMeasurable.Measure() method.
        /// </summary>
        public virtual Size Measure(double availableWidth, double availableHeight)
        {
            Resize(availableWidth.ToScaledPixel(), Geometry.Height);
            var formattedSize = this.GetTextBlockFormattedSize();
            Resize(Geometry.Width, Geometry.Height);
            return new Size
            {
                Width = MinimumWidth + formattedSize.Width,
                Height = Math.Max(MinimumHeight, formattedSize.Height)
            };
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
            bool isVisible = true;
            if (string.IsNullOrEmpty(formattedText))
            {
                base.Text = null;
                this.SetTextBlockStyle(null);
                this.SendTextVisibleSignal(false);
            }
            else
            {
                base.Text = formattedText;
                this.SetTextBlockStyle(textStyle);
                this.SendTextVisibleSignal(isVisible);
            }
        }
    }
}