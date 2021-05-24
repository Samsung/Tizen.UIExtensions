using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using NColor = Tizen.NUI.Color;
using NPanelLayoutType = Tizen.NUI.InputMethod.PanelLayoutType;
using Size = Tizen.UIExtensions.Common.Size;
using TColor = Tizen.UIExtensions.Common.Color;
using AutoCapitalType = Tizen.NUI.InputMethod.AutoCapitalType;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A control which provides a single line editable text field.
    /// </summary>
    public class Entry : TextField, IMeasurable
    {
        float _defaultFontSize;
        NColor _defaultTextColor;
        NColor _defaultPlaceholderColor;

        FontAttributes _fontAttributes;
        Keyboard _keyboard = Keyboard.Normal;
        ReturnType _returnType = ReturnType.Default;
        AutoCapitalType _autoCapital = AutoCapitalType.None;
        bool _isPassword;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class.
        /// </summary>
        public Entry()
        {
            _defaultFontSize = PointSize;
            _defaultTextColor = base.TextColor;
            _defaultPlaceholderColor = PlaceholderTextColor;
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

        /// <summary>
        /// Gets or sets the vertical text alignment.
        /// </summary>
        /// <value>The vertical text alignment.</value>
        public TextAlignment VerticalTextAlignment
        {
            get => VerticalAlignment.ToCommon();
            set => VerticalAlignment = value.ToVertical();
        }

        /// <summary>
        /// Gets or sets the color of the formatted text.
        /// </summary>
        /// <value>The color of the text.</value>
        public new TColor TextColor
        {
            get => base.TextColor.ToCommon();
            set => base.TextColor = value.IsDefault ? _defaultTextColor : value.ToNative();
        }

        /// <summary>
        /// Gets or sets a value that indicates if the entry should visually obscure typed text.
        /// </summary>
        public bool IsPassword
        {
            get => _isPassword;
            set
            {
                if (_isPassword != value)
                {
                    _isPassword = value;
                    ApplyIsPassword();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text that is displayed when the control is empty.
        /// </summary>
        public new string Placeholder
        {
            get => PlaceholderText;
            set => PlaceholderText = value;
        }

        /// <summary>
        /// Gets or sets the color of the placeholder text.
        /// </summary>
        public TColor PlaceholderColor
        {
            get
            {
                return PlaceholderTextColor?.ToColor() ?? TColor.Default;
            }
            set
            {
                PlaceholderTextColor = value.IsDefault ? _defaultPlaceholderColor : value.ToNative();
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
                    _keyboard = value;
                    ApplyInputMethodSetting();
                }
            }
        }

        /// <summary>
        /// Gets or sets an enumeration value that controls the appearance of the return button.
        /// </summary>
        public ReturnType ReturnType
        {
            get => _returnType;
            set
            {
                if (_returnType != value)
                {
                    _returnType = value;
                    ApplyInputMethodSetting();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that controls whether text prediction and automatic text correction is on or off.
        /// </summary>
        public bool IsTextPredictionEnabled
        {
            get => GetInputMethodContext().TextPrediction;
            set => GetInputMethodContext().TextPrediction = value;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether user should be prevented from modifying the text. Default is false.
        /// </summary>
        public bool IsReadOnly
        {
            get => !EnableEditing;
            set => EnableEditing = !value;
        }

        /// <summary>
        /// Sets or Gets the autocapitalization type on the im module.
        /// </summary>
        public AutoCapitalType AutoCapital
        {
            get => _autoCapital;
            set
            {
                if (_autoCapital != value)
                {
                    _autoCapital = value;
                    ApplyInputMethodSetting();
                }
            }
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
            if (!string.IsNullOrEmpty(Text))
            {
                if (availableWidth < NaturalSize.Width)
                {
                    return new Size(availableWidth, GetHeightForWidth((float)availableWidth));
                }
                else if (NaturalSize.Width > 0)
                {
                    return new Size(NaturalSize.Width, GetHeightForWidth(NaturalSize.Width));
                }
                else
                {
                    // even though text but natural size is zero. it is abnormal state
                    return new Size(Text.Length * PixelSize + 10, PixelSize + 10);
                }
            }
            else
            {
                return new Size(PixelSize, PixelSize);
            }
#pragma warning restore CS0618
        }

        void ApplyIsPassword()
        {
            HiddenInputSettings = new PropertyMap()
                .Add(HiddenInputProperty.Mode, IsPassword ? (int)HiddenInputModeType.ShowLastCharacter : (int)HiddenInputModeType.HideNone)
                .Add(HiddenInputProperty.ShowLastCharacterDuration, 5000);
        }

        void ApplyInputMethodSetting()
        {
            InputMethodSettings = CreateInputMethodSettings(Keyboard, ReturnType, AutoCapital);
        }

        PropertyMap CreateInputMethodSettings(Keyboard keyboard, ReturnType returnType, AutoCapitalType autoCapital)
        {
            var inputMethod = new InputMethod
            {
                PanelLayout = keyboard.ToPanelLayoutType(),
                ActionButton = returnType.ToActionButtonType(),
                AutoCapital = autoCapital,
            };
            if (inputMethod.PanelLayout == NPanelLayoutType.NumberOnly)
            {
                if (keyboard == Keyboard.Numeric)
                {
                    inputMethod.NumberOnlyVariation = InputMethod.NumberOnlyLayoutType.WithSignedAndDecimal;
                }
                else
                {
                    inputMethod.NumberOnlyVariation = InputMethod.NumberOnlyLayoutType.Normal;
                }
            }
            return inputMethod.OutputMap;
        }
    }
}
