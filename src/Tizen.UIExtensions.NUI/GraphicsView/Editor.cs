using System;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using TEditor = Tizen.UIExtensions.NUI.Editor;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    public interface IEditor
    {
        string Text { get; set; }

        Color TextColor { get; }
        
        string Placeholder { get; }

        Color PlaceholderColor { get; }

        Color BackgroundColor { get; }
        bool IsFocused { get; }
    }

    /// <summary>
    /// A control that can edit multiple lines of text.
    /// </summary>
    public class Editor : GraphicsView<EditorDrawable>, IEditor
    {
        /// <summary>
        /// Initializes a new instance of the Editor class.
        /// </summary>
        public Editor()
        {
            Layout = new Tizen.NUI.LinearLayout();
            EmbedEditor = new TEditor
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Margin = new Tizen.NUI.Extents((ushort)(12 * DeviceInfo.ScalingFactor), (ushort)(12 * DeviceInfo.ScalingFactor), (ushort)(20 * DeviceInfo.ScalingFactor), (ushort)(12 * DeviceInfo.ScalingFactor))
            };
            EmbedEditor.FocusGained += OnFocused;
            EmbedEditor.FocusLost += OnUnfocused;
            EmbedEditor.TextChanged += OnTextChanged;
            Add(EmbedEditor);

            Drawable = new EditorDrawable(this);
        }

        public TEditor EmbedEditor { get; }

        /// <summary>
        /// Gets or sets the text of the edtior
        /// </summary>
        public string Text
        {
            get => EmbedEditor.Text;
            set => EmbedEditor.Text = value;
        }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public Color TextColor
        {
            get => EmbedEditor.TextColor;
            set => EmbedEditor.TextColor = value;
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
        Color IEditor.BackgroundColor => BackgroundColor;

        public bool IsFocused => EmbedEditor.HasFocus();

        void OnTextChanged(object? sender, TextEditor.TextChangedEventArgs e)
        {
            Invalidate();
        }
    }
}
