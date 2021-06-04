using System;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using TEntry = Tizen.UIExtensions.NUI.Entry;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    public interface IEntry
    {
        string Text { get; set; }

        Color TextColor { get; }
        
        string Placeholder { get; }

        Color PlaceholderColor { get; }

        Color BackgroundColor { get; }
        bool IsFocused { get; }
    }

    /// <summary>
    /// A control that can edit a single line of text.
    /// </summary>
    public class Entry : GraphicsView<EntryDrawable>, IEntry
    {
        /// <summary>
        /// Initializes a new instance of the Entry class.
        /// </summary>
        public Entry()
        {
            Layout = new Tizen.NUI.LinearLayout();
            EmbedEntry = new TEntry
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Tizen.NUI.Extents((ushort)(12 * DeviceInfo.ScalingFactor), (ushort)(40 * DeviceInfo.ScalingFactor), (ushort)(12 * DeviceInfo.ScalingFactor), 0)
            };
            EmbedEntry.FocusGained += OnFocused;
            EmbedEntry.FocusLost += OnUnfocused;
            EmbedEntry.TextChanged += OnTextChanged;
            Add(EmbedEntry);

            Drawable = new EntryDrawable(this);
        }

        public TEntry EmbedEntry { get; }

        /// <summary>
        /// Gets or sets the text of the entry
        /// </summary>
        public string Text
        {
            get => EmbedEntry.Text;
            set => EmbedEntry.Text = value;
        }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public Color TextColor
        {
            get => EmbedEntry.TextColor;
            set => EmbedEntry.TextColor = value;
        }

        /// <summary>
        /// Gets or sets the text that is displayed when the control is empty.
        /// </summary>
        public string Placeholder
        {
            get => GetProperty<string>(nameof(Placeholder));
            set => SetProperty(nameof(Placeholder), value);
        }

        /// <summary>
        /// Gets or sets the color of the placeholder text.
        /// </summary>
        public Color PlaceholderColor
        {
            get => GetProperty<Color>(nameof(PlaceholderColor));
            set => SetProperty(nameof(PlaceholderColor), value);
        }

        /// <summary>
        /// Gets or sets the color which will fill the background
        /// </summary>
        public new Color BackgroundColor
        {
            get => GetProperty<Color>(nameof(BackgroundColor));
            set => SetProperty(nameof(BackgroundColor), value);
        }
        Color IEntry.BackgroundColor => BackgroundColor;

        public bool IsFocused => EmbedEntry.HasFocus();

        void OnTextChanged(object? sender, TextField.TextChangedEventArgs e)
        {
            Invalidate();
        }
    }
}
