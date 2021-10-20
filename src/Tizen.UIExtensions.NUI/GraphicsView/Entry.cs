using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;
using TEntry = Tizen.UIExtensions.NUI.Entry;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    /// <summary>
    /// A control that can edit a single line of text.
    /// </summary>
    public class Entry : View, IEntry, IMeasurable
    {
        GraphcisEntry _entry;

        /// <summary>
        /// Initializes a new instance of the Entry class.
        /// </summary>
        public Entry()
        {
            Layout = new Tizen.NUI.AbsoluteLayout();
            EmbedEntry = new TEntry
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Tizen.NUI.Extents((ushort)(12 * DeviceInfo.ScalingFactor), (ushort)(40 * DeviceInfo.ScalingFactor), (ushort)(12 * DeviceInfo.ScalingFactor), 0)
            };
            _entry = new GraphcisEntry(EmbedEntry)
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            Add(_entry);

            // NUI AbsoluteLayout not support Margin, so add a internal view as LinearLayout
            var marginView = new View
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Layout = new Tizen.NUI.LinearLayout(),
            };
            Add(marginView);
            marginView.Add(EmbedEntry);
        }

        public TEntry EmbedEntry { get; }

        /// <summary>
        /// Gets or sets the text of the entry
        /// </summary>
        public string Text 
        {
            get => _entry.Text;
            set => _entry.Text = value;
        }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public Color TextColor
        {
            get => _entry.TextColor;
            set => _entry.TextColor = value;
        }

        /// <summary>
        /// Gets or sets the text that is displayed when the control is empty.
        /// </summary>
        public string Placeholder
        {
            get => _entry.Placeholder;
            set => _entry.Placeholder = value;
        }

        /// <summary>
        /// Gets or sets the color of the placeholder text.
        /// </summary>
        public Color PlaceholderColor
        {
            get => _entry.PlaceholderColor;
            set => _entry.PlaceholderColor = value;
        }

        public bool IsFocused => _entry.IsFocused;

        /// <summary>
        /// Gets or sets the color which will fill the background
        /// </summary>
        public new Color BackgroundColor
        {
            get => _entry.BackgroundColor;
            set => _entry.BackgroundColor = value;
        }
        Color IEntry.BackgroundColor => _entry.BackgroundColor;

        public Size Measure(double availableWidth, double availableHeight)
        {
            return _entry.Measure(availableWidth, availableHeight);
        }

        class GraphcisEntry : GraphicsView<EntryDrawable>, IEntry
        {
            public GraphcisEntry(TEntry entry)
            {
                EmbedEntry = entry;
                EmbedEntry.FocusGained += OnFocused;
                EmbedEntry.FocusLost += OnUnfocused;
                EmbedEntry.TextChanged += OnTextChanged;

                Drawable = new EntryDrawable(this);
            }

            public TEntry EmbedEntry { get; }

            public string Text
            {
                get => EmbedEntry.Text;
                set => EmbedEntry.Text = value;
            }

            public Color TextColor
            {
                get => EmbedEntry.TextColor;
                set => EmbedEntry.TextColor = value;
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
            Color IEntry.BackgroundColor => BackgroundColor;

            public bool IsFocused => EmbedEntry.HasFocus();

            void OnTextChanged(object? sender, TextField.TextChangedEventArgs e)
            {
                Invalidate();
            }
        }
    }

}
