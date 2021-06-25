using System;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    /// <summary>
    /// A visual control used to indicate that something is checked.
    /// </summary>
    public class CheckBox : GraphicsView<CheckBoxDrawable>, ICheckBox
    {
        /// <summary>
        /// Initializes a new instance of the CheckBox class.
        /// </summary>
        public CheckBox()
        {
            Text = string.Empty;
            Drawable = new CheckBoxDrawable(this);
        }

        /// <summary>
        /// Occurs when IsChecked is changed
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets a value indicating whether control is checked currently.
        /// </summary>
        public bool IsChecked
        {
            get => GetProperty<bool>(nameof(IsChecked));
            set
            {
                SetProperty(nameof(IsChecked), value);
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the Color of the CheckBox.
        /// </summary>
        public new Color Color
        {
            get => GetProperty<Color>(nameof(Color));
            set => SetProperty(nameof(Color), value);
        }

        /// <summary>
        /// Gets or sets the Color for the text of the CheckBox
        /// </summary>
        public Color TextColor
        {
            get => GetProperty<Color>(nameof(TextColor));
            set => SetProperty(nameof(TextColor), value);
        }

        /// <summary>
        /// Gets or sets the Text displayed as the content of the CheckBox.
        /// </summary>
        public string Text
        {
            get => GetProperty<string>(nameof(Text));
            set => SetProperty(nameof(Text), value);
        }
    }
}
