using System;
using ElmSharp;
using Tizen.UIExtensions.Common;
using EButton = ElmSharp.Button;
using ESize = ElmSharp.Size;
using EColor = ElmSharp.Color;

namespace Tizen.UIExtensions.ElmSharp
{
	/// <summary>
	/// Extends the EButton control, providing basic formatting features,
	/// i.e. font color, size, additional image.
	/// </summary>
	public class Button : EButton, IMeasurable, IBatchable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Forms.Platform.Tizen.Native.Button"/> class.
		/// </summary>
		/// <param name="parent">Parent evas object.</param>
		public Button(EvasObject parent) : base(parent)
		{
		}

		/// <summary>
		/// Holds the formatted text of the button.
		/// </summary>
		readonly Span _span = new Span();

		/// <summary>
		/// Optional image, if set will be drawn on the button.
		/// </summary>
		Image _image;

		/// <summary>
		/// Gets or sets the button's text.
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
		public EColor TextColor
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
		public EColor TextBackgroundColor
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
		/// Gets or sets the image to be displayed next to the button's text.
		/// </summary>
		/// <value>The image displayed on the button.</value>
		public Image Image
		{
			get => _image;
			set
			{
				if (value != _image)
				{
					ApplyImage(value);
				}
			}
		}

		/// <summary>
		/// Implementation of the IMeasurable.Measure() method.
		/// </summary>
		public virtual ESize Measure(int availableWidth, int availableHeight)
		{
			if (DeviceInfo.IsWatch)
			{
				if (Style == ThemeConstants.Button.Styles.Default)
				{
					//Should gurantee the finger size (40)
					MinimumWidth = MinimumWidth < 40 ? 40 : MinimumWidth;
					if (Image != null)
						MinimumWidth += Image.Geometry.Width;
					var rawSize = this.GetTextBlockNativeSize();
					return new ESize(rawSize.Width + MinimumWidth, Math.Max(MinimumHeight, rawSize.Height));
				}
				else
				{
					return new ESize(MinimumWidth, MinimumHeight);
				}
			}
			else
			{
				if (Style == ThemeConstants.Button.Styles.Circle)
				{
					return new ESize(MinimumWidth, MinimumHeight);
				}
				else
				{
					if (Image != null)
						MinimumWidth += Image.Geometry.Width;

					var rawSize = this.GetTextBlockNativeSize();
					return new ESize(rawSize.Width + MinimumWidth, Math.Max(MinimumHeight, rawSize.Height));
				}
			}
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

		void ApplyImage(Image image)
		{
			_image = image;

			SetInternalImage();
		}

		void SetInternalImage()
		{
#pragma warning disable CS8604 // Possible null reference argument.
			this.SetIconPart(_image);
#pragma warning restore CS8604 // Possible null reference argument.
		}

		public void UpdateStyle(string style)
		{
			if (Style != style)
			{
				Style = style;
				if (Style == ThemeConstants.Button.Styles.Default)
					_span.HorizontalTextAlignment = TextAlignment.Auto;
				else
					_span.HorizontalTextAlignment = TextAlignment.Center;
				ApplyTextAndStyle();
			}
		}
	}
}