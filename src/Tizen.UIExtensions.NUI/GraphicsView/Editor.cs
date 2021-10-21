using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;
using TEditor = Tizen.UIExtensions.NUI.Editor;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    /// <summary>
    /// A control that can edit multiple lines of text.
    /// </summary>
    public class Editor : View, IEditor, IMeasurable
    {
        GrapchisEditor _editor;

        /// <summary>
        /// Initializes a new instance of the Editor class.
        /// </summary>
        public Editor()
        {
            Layout = new Tizen.NUI.AbsoluteLayout();
            EmbedEditor = new TEditor
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Margin = new Tizen.NUI.Extents((ushort)(12 * DeviceInfo.ScalingFactor), (ushort)(12 * DeviceInfo.ScalingFactor), (ushort)(20 * DeviceInfo.ScalingFactor), (ushort)(12 * DeviceInfo.ScalingFactor))
            };
            _editor = new GrapchisEditor(EmbedEditor)
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            Add(_editor);

            // NUI AbsoluteLayout not support Margin, so add a internal view as LinearLayout
            var marginView = new View
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Layout = new Tizen.NUI.LinearLayout(),
            };
            Add(marginView);
            marginView.Add(EmbedEditor);
        }

        public TEditor EmbedEditor { get; }

        /// <summary>
        /// Gets or sets the text of the edtior
        /// </summary>
        public string Text
        {
            get => _editor.Text;
            set => _editor.Text = value;
        }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public Color TextColor
        {
            get => _editor.TextColor;
            set => _editor.TextColor = value;
        }

        /// <summary>
        /// Gets or sets the text that is displayed when the control is empty.
        /// </summary>
        public string Placeholder
        {
            get => _editor.Placeholder;
            set => _editor.Placeholder = value;
        }

        /// <summary>
        /// Gets or sets the color of the placeholder text.
        /// </summary>
        public Color PlaceholderColor
        {
            get => _editor.PlaceholderColor;
            set => _editor.PlaceholderColor = value;
        }

        public bool IsFocused
        {
            get => _editor.IsFocused;
        }

        /// <summary>
        /// Gets or sets the color which will fill the background
        /// </summary>
        public new Color BackgroundColor
        {
            get => _editor.BackgroundColor;
            set => _editor.BackgroundColor = value;
        }
        Color IEditor.BackgroundColor => BackgroundColor;


        public Size Measure(double availableWidth, double availableHeight)
        {
            return _editor.Measure(availableWidth, availableHeight);
        }

        class GrapchisEditor : GraphicsView<EditorDrawable>, IEditor
        {
            public GrapchisEditor(TEditor editor)
            {
                EmbedEditor = editor;
                EmbedEditor.FocusGained += OnFocused;
                EmbedEditor.FocusLost += OnUnfocused;
                EmbedEditor.TextChanged += OnTextChanged;

                Drawable = new EditorDrawable(this);
            }

            public TEditor EmbedEditor { get; }

            public string Text
            {
                get => EmbedEditor.Text;
                set => EmbedEditor.Text = value;
            }

            public Color TextColor
            {
                get => EmbedEditor.TextColor;
                set => EmbedEditor.TextColor = value;
            }

            public string Placeholder
            {
                get => GetProperty<string>(nameof(Placeholder));
                set => SetProperty(nameof(Placeholder), value);
            }

            public Color PlaceholderColor
            {
                get => GetProperty<Color>(nameof(PlaceholderColor));
                set => SetProperty(nameof(PlaceholderColor), value);
            }

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
    
}
