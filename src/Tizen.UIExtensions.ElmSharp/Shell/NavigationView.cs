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
    public class NavigationView : Background
    {
        static EColor s_defaultBackgroundColor = ThemeConstants.Shell.ColorClass.DefaultNavigationViewBackgroundColor;

        Box _mainLayout;
        Box _headerBox;

        GenList _menu;
        GenItemClass _templateClass;
        GenItemClass _headerClass;

        EvasObject _header;
        EvasObject _backgroundImage;
        EColor _backgroundColor;

        DrawerHeaderBehavior _headerBehavior = DrawerHeaderBehavior.Fixed;

        bool HeaderOnMenu => HeaderBehavior == DrawerHeaderBehavior.Scroll ||
                     HeaderBehavior == DrawerHeaderBehavior.CollapseOnScroll;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tizen.UIExtensions.ElmSharp.NavigationView"/> class.
        /// </summary>
        /// <param name="parent">Parent evas object.</param>
        public NavigationView(EvasObject parent) : base(parent)
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
        public EvasObject BackgroundImage
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
        public EvasObject Header
        {
            get => _header;
            set => UpdateHeader(value);
        }

        /// <summary>
        /// Occurs when an item is selected in the NavigationView.
        /// </summary>
        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        /// <summary>
        /// Create the list of items to be displayed on the NavigationView.
        /// </summary>
        /// <param name="items"></param>
        public void BuildMenu(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                var genItem = _menu.Append(_templateClass, item);
                genItem.SetBackgroundColor(EColor.Transparent);
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
                ItemSelected?.Invoke(this, new SelectedItemChangedEventArgs(e.Item.Data, e.Item.Index));
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
            if(_headerBox == null)
            {
                _headerBox = new Box(this)
                {
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                };
                _headerBox.SetLayoutCallback(OnHeaderBoxLayout);
                _headerBox.MinimumHeight = _header.MinimumHeight;
            }

            var header = (EvasObject)data;
            _headerBox.PackEnd(header);

            return _headerBox;
        }

        void OnHeaderBoxLayout()
        {
            _header.Geometry = _headerBox.Geometry;
        }

        EvasObject GetTemplatedContent(object data, string part)
        {
            if (data is EvasObject evas)
                return evas;

            return new Label(this)
            {
                Text = data.ToString()
            };
        }

        void OnLayout()
        {
            if (Geometry.Width == 0 || Geometry.Height == 0)
                return;

            var bound = Geometry;
            int headerHeight = 0;

            if (!HeaderOnMenu)
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

        void UpdateHeader(EvasObject header)
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
            _header.Show();
        }

        void UpdateHeaderBehavior()
        {
            if (_header == null)
                return;

            if (HeaderOnMenu)
            {
                if (_header != null)
                {
                    _mainLayout.UnPack(_header);
                }
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

        void UpdateHeaderOnMenu(EvasObject header)
        {
            if (_menu.FirstItem != null && _menu.FirstItem.Data == header)
                return;

            GenListItem item = null;
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

        void ResetHeaderOnMenu()
        {
            if (_menu.FirstItem != null && _headerBox != null)
            {
                _headerBox.UnPackAll();
                _menu.FirstItem.Delete();
                _headerBox = null;
            }
        }
    }
}
