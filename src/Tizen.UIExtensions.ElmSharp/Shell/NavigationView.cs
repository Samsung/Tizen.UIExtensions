using System;
using System.Collections.Generic;
using ElmSharp;
using Tizen.UIExtensions.Common;
using EColor = ElmSharp.Color;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The native widget that is configured with an header and an list of items to be used in NavigationDrawer.
    /// </summary>
    public class NavigationView : Background, INavigationView
    {
        static readonly EColor s_defaultBackgroundColor = ThemeConstants.Shell.ColorClass.DefaultNavigationViewBackgroundColor;

        Box _mainLayout;
        Box? _headerBox;

        GenList _menu;
        GenItemClass _templateClass;
        GenItemClass _headerClass;

        EvasObject? _header;
        EvasObject? _backgroundImage;
        EColor _backgroundColor;

        DrawerHeaderBehavior _headerBehavior = DrawerHeaderBehavior.Fixed;

        bool HeaderOnMenu => HeaderBehavior == DrawerHeaderBehavior.Scroll ||
                     HeaderBehavior == DrawerHeaderBehavior.CollapseOnScroll;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tizen.UIExtensions.ElmSharp.NavigationView"/> class.
        /// </summary>
        /// <param name="parent">Parent evas object.</param>
#pragma warning disable CS8618
        public NavigationView(EvasObject parent) : base(parent)
#pragma warning restore CS8618
        {
            InitializeComponent(parent);
        }

        /// <summary>
        /// Gets or sets the header behavior.
        /// </summary>
        public DrawerHeaderBehavior HeaderBehavior
        {
            get => _headerBehavior;
            set
            {
                if (_headerBehavior == value)
                    return;

                _headerBehavior = value;
                UpdateHeaderBehavior();
            }
        }

        /// <summary>
        /// Gets or sets the background color of the NavigtiaonView.
        /// </summary>
        public override EColor BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                EColor effectiveColor = _backgroundColor.IsDefault ? s_defaultBackgroundColor : _backgroundColor;
                base.BackgroundColor = effectiveColor;
            }
        }

        /// <summary>
        /// Gets or sets the background image of the NavigtiaonView.
        /// </summary>
        public EvasObject? BackgroundImage
        {
            get => _backgroundImage;
            set
            {
                _backgroundImage = value;
                this.SetBackgroundPart(_backgroundImage);
            }
        }

        /// <summary>
        /// Gets or sets the header view of the NavigtiaonView.
        /// </summary>
        public EvasObject? Header
        {
            get => _header;
            set => UpdateHeader(value);
        }

        /// <summary>
        /// Gets or sets the target view of the NavigtiaonView.
        /// </summary>
        public EvasObject TargetView => this;

        /// <summary>
        /// Occurs when an item is selected in the NavigationView.
        /// </summary>
        public event EventHandler<ItemSelectedEventArgs>? ItemSelected;

        /// <summary>
        /// Create the list of items to be displayed on the NavigationView.
        /// </summary>
        /// <param name="items"></param>
        public void BuildMenu(IEnumerable<object> items)
        {
            _menu.Clear();

            foreach(var item in items)
            {
                var genItem = _menu.Append(_templateClass, item);
                genItem.SetBackgroundColor(EColor.Transparent);

                if (item is NavigationViewItem naviItem)
                {
                    if (!naviItem.IsFirst)
                    {
                        genItem.SetBottomlineColor(EColor.Transparent);
                    }
                }

            }
        }

        void InitializeComponent(EvasObject parent)
        {
            base.BackgroundColor = s_defaultBackgroundColor;

            _mainLayout = new Box(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1
            };
            _mainLayout.SetLayoutCallback(OnLayout);
            _mainLayout.Show();
            SetContent(_mainLayout);

            _menu = new GenList(parent)
            {
                Homogeneous = false,
                SelectionMode = GenItemSelectionMode.Always,
                BackgroundColor = EColor.Transparent,
                Style = ThemeConstants.GenList.Styles.Solid,
                ListMode = GenListMode.Scroll,
            };
            _menu.Show();
            _mainLayout.PackEnd(_menu);

            _menu.ItemSelected += (s, e) =>
            {
                ItemSelected?.Invoke(this, new ItemSelectedEventArgs(e.Item.Data, e.Item.Index));
            };


            _templateClass = new GenItemClass(ThemeConstants.GenItemClass.Styles.Full)
            {
                GetContentHandler = GetTemplatedContent,
            };

            _headerClass = new GenItemClass(ThemeConstants.GenItemClass.Styles.Full)
            {
                GetContentHandler = GetHeaderContent
            };
        }

        EvasObject GetHeaderContent(object data, string part)
        {
            if (_headerBox == null)
            {
                _headerBox = new Box(this)
                {
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                };
            }

            var header = (EvasObject)data;
            _headerBox.PackEnd(header);
            _headerBox.SetLayoutCallback(OnHeaderBoxLayout);
            _headerBox.MinimumHeight = header!.MinimumHeight;

            return _headerBox;
        }
        EvasObject GetTemplatedContent(object data, string part)
        {
            if (data is NavigationViewItem item)
            {
                if (item.GetContent != null)
                {
                    return item.GetContent(item.Data);
                }
            }

            return new Label(this)
            {
                Text = data.ToString()
            };
        }

        void OnHeaderBoxLayout()
        {
            if (_header != null && _headerBox != null)
            {
                _header.Geometry = _headerBox.Geometry;
            }
        }

        void OnLayout()
        {
            if (Geometry.Width == 0 || Geometry.Height == 0)
                return;

            var bound = Geometry;
            int headerHeight = 0;

            if (!HeaderOnMenu && _header != null)
            {
                var headerbound = bound;
                headerHeight = _header.MinimumHeight;
                headerbound.Height = headerHeight;
                _header.Geometry = headerbound;
            }

            bound.Y += headerHeight;
            bound.Height -= headerHeight;
            _menu.Geometry = bound;
        }

        void UpdateHeader(EvasObject? header)
        {
            if (_header != null)
            {
                if (HeaderOnMenu)
                {
                    ResetHeaderOnMenu();
                }
                else if (_header != null)
                {
                    _mainLayout.UnPack(_header);
                    _header.Unrealize();
                    _header = null;
                }
            }

            if (header != null)
            {
                if (HeaderOnMenu)
                {
                    UpdateHeaderOnMenu(header);
                }
                else
                {
                    _mainLayout.PackEnd(header);
                }
            }
            _header = header;
            _header?.Show();
        }

        void UpdateHeaderBehavior()
        {
            if (_header == null)
                return;

            if (HeaderOnMenu)
            {
                _mainLayout.UnPack(_header);
                UpdateHeaderOnMenu(_header);
            }
            else
            {
                ResetHeaderOnMenu();
                if (_header != null)
                {
                    _mainLayout.PackEnd(_header);
                }
            }
            OnLayout();
        }

        void ResetHeaderOnMenu()
        {
            if (_menu.FirstItem != null && _headerBox != null)
            {
                _headerBox.UnPackAll();
                _menu.FirstItem.Delete();
                _headerBox = null;
            }
        }

        void UpdateHeaderOnMenu(EvasObject header)
        {
            if (_menu.FirstItem != null && _menu.FirstItem.Data == header)
                return;

            GenListItem item;
            if (_menu.Count > 0)
            {
                item = _menu.InsertBefore(_headerClass, header, _menu.FirstItem);
            }
            else
            {
                item = _menu.Append(_headerClass, header);
            }
            item.SelectionMode = GenItemSelectionMode.None;
        }

        void INavigationView.UpdateHeaderLayout()
        {
            if (_header == null)
                return;

            ResetHeaderOnMenu();
            if (HeaderOnMenu)
            {
                UpdateHeaderOnMenu(_header);
            }
            else
            {
                if (_header != null)
                {
                    _mainLayout.PackEnd(_header);
                }
            }
            OnLayout();
        }
    }

    public class NavigationViewItem
    {
        public NavigationViewItem()
        {
            Data = null;
            GetContent = null;
            IsFirst = false;
        }

        public bool IsFirst { get; set; }

        public object? Data { get; set; }

        public GetContentDelegate? GetContent { get; set; }

        public delegate EvasObject GetContentDelegate(object? data);
    }
}
