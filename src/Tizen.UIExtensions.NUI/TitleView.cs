using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using TColor = Tizen.UIExtensions.Common.Color;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A View to present the title area
    /// </summary>
    public class TitleView : View
    {
        Label _label;
        View _contentContainer;
        View _actionsContainer;
        View? _icon;
        View? _content;
        ObservableCollection<View> _actions = new ObservableCollection<View>();

        /// <summary>
        /// Initializes a new instance of the TitleView class.
        /// </summary>
#pragma warning disable CS8618
        public TitleView()
#pragma warning restore CS8618
        {
            Initialize();
        }

        /// <summary>
        /// Title text
        /// </summary>
        public string Title
        {
            get => _label.Text;
            set => _label.Text = value;
        }

        /// <summary>
        /// Gets a Label for title
        /// </summary>
        public Label Label => _label;

        /// <summary>
        /// Views for placing at right side of title
        /// Ownership of view is moved to TitleView
        /// Only support Add/Remove/Reset method
        /// </summary>
        public IList<View> Actions => _actions;

        /// <summary>
        /// A view placed at left size of title
        /// Ownership of view is moved to TitleView
        /// </summary>
        public View? Icon
        {
            get => _icon;
            set
            {
                if (_icon != null)
                {
                    Remove(Icon);
                    _icon.Dispose();
                }
                _icon = value;

                if (_icon != null)
                {
                    Add(_icon);
                    if (!(_icon.Layout is LayoutGroup))
                    {
                        _icon.Layout = new AbsoluteLayout();
                    }
                    (_icon.Layout as LayoutGroup)?.ChangeLayoutSiblingOrder(0);
                    _icon.Margin = new Extents(0, 20, 0, 0);
                }
            }
        }

        /// <summary>
        /// A View placed at beside of title
        /// Ownership of view is moved to TitleView
        /// </summary>
        public View? Content
        {
            get => _content;
            set
            {
                if (_content != null)
                {
                    _contentContainer.Remove(_content);
                    _content.Dispose();
                }
                _content = value;

                if (_content != null)
                {
                    _contentContainer.Add(_content);
                }
            }
        }

        void Initialize()
        {
            Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Begin,
            };

            SizeHeight = (float)(DeviceInfo.ScalingFactor * 50);
            WidthSpecification = LayoutParamPolicies.MatchParent;

            Padding = new Extents(20, 20, 10, 10);
            BackgroundColor = TColor.FromHex("#2196f3").ToNative();
            BoxShadow = new Shadow(5, TColor.FromHex("#bbbbbb").ToNative(), new Vector2(0, 5));

            _contentContainer = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Begin,
                },
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            Add(_contentContainer);

            _label = new Label
            {
                FontSize = 9,
                TextColor = TColor.White,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Center,
            };
            _contentContainer.Add(_label);

            _actionsContainer = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.End,
                },
                HeightSpecification = LayoutParamPolicies.MatchParent,
                WidthSpecification = LayoutParamPolicies.WrapContent,
            };
            Add(_actionsContainer);

            _actions.CollectionChanged += OnActionCollectionChanged;
        }

        void OnActionCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is View view)
                    {
                        view.Margin = new Extents(10, 10, 0, 0);
                        _actionsContainer.Add(view);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is View view)
                    {
                        _actionsContainer.Remove(view);
                        view.Dispose();
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                var toBeRemoved = _actionsContainer.Children.ToList();
                foreach (var child in toBeRemoved)
                {
                    _actionsContainer.Remove(child);
                    child.Dispose();
                }

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_icon != null)
                {
                    _icon.Unparent();
                    _icon.Dispose();
                    _icon = null;
                }
                if (_content != null)
                {
                    _content.Unparent();
                    _content.Dispose();
                    _content = null;
                }
                if (_actions.Count > 0)
                {
                    _actions.Clear();
                }
            }
            base.Dispose(disposing);
        }
    }
}
