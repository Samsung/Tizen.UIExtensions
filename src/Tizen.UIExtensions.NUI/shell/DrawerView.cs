using System;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using Size = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A View that consists of a drawer and a content.
    /// </summary>
    public abstract class DrawerView : ViewGroup
    {
        ViewGroup _drawerViewGroup;
        ViewGroup _contentViewGroup;
        View? _drawer;
        View? _content;

        DrawerBehavior _behavior;
        bool _isOpened;
        bool _isPopover;

        double _drawerWidth = -1;
        double _drawerWidthCollapsed = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawerView"/> class
        /// </summary>
        public DrawerView()
        {
            _drawerViewGroup = new ViewGroup()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            _contentViewGroup = new ViewGroup()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };

            _behavior = DrawerBehavior.Drawer;
            _isOpened = false;
            _isPopover = true;

            _drawerViewGroup.Focusable = true;
            _contentViewGroup.Focusable = true;

            _drawerViewGroup.FocusableInTouch = true;
            _contentViewGroup.FocusableInTouch = true;

            Children.Add(_contentViewGroup);
            Children.Add(_drawerViewGroup);

            LayoutUpdated += OnLayoutUpdated;

            _drawerViewGroup.FocusGained += OnDrawerFocusGained;
            _drawerViewGroup.KeyEvent += OnDrawerKeyEventTriggered;

            _contentViewGroup.FocusGained += OnContentFocusGained;
            _contentViewGroup.KeyEvent += OnContentKeyEventTriggered;
        }

        /// <summary>
        /// Gets a value that indicates if the drawer is opened.
        /// </summary>
        public bool IsOpened
        {
            get => _isOpened;
            protected set => _isOpened = value;
        }

        /// <summary>
        /// Gets or sets a value that controls the shadow of the drawer.
        /// </summary>
        public Shadow? DrawerShadow
        {
            get => _drawerViewGroup.BoxShadow;
            set => _drawerViewGroup.BoxShadow = value;
        }

        /// <summary>
        /// Gets or sets a view for drawer.
        /// </summary>
        public View? Drawer
        {
            get => _drawer;
            set
            {
                if (_drawer == value)
                    return;

                _drawer = value;
                ResetView(_drawerViewGroup, _drawer);
            }
        }

        /// <summary>
        /// Gets or sets a view for content.
        /// </summary>
        public View? Content
        {
            get => _content;
            set
            {
                if (_content == value)
                    return;

                _content = value;
                ResetView(_contentViewGroup, _content);
            }
        }

        /// <summary>
        /// Gets or sets an enumeration value that controls how the drawer appears.
        /// </summary>
        public DrawerBehavior DrawerBehavior
        {
            get => _behavior;
            set
            {
                if (_behavior == value)
                    return;

                _behavior = value;
                ConfigureLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value that controls how the drawer appears on the content.
        /// </summary>
        public bool IsPopover
        {
            get => _isPopover;
            set
            {
                if (_isPopover == value)
                    return;

                _isPopover = value;
                ConfigureLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value that controls the drawer width.
        /// </summary>
        public double DrawerWidth
        {
            get => (_drawerWidth > -1) ? _drawerWidth : DefaultDrawerWidth;
            set
            {
                if (_drawerWidth == value)
                    return;

                _drawerWidth = value;
                ConfigureLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value that controls the drawer width when the drawer is collapsed.
        /// </summary>
        public double DrawerWidthCollapsed
        {
            get => (_drawerWidthCollapsed > -1) ? _drawerWidthCollapsed : DefaultDrawerWidthCollapsed;
            set
            {
                if (_drawerWidthCollapsed == value)
                    return; 

                _drawerWidthCollapsed = value;
                ConfigureLayout();
            }
        }

        protected double DefaultDrawerWidth => (Size.Width > Size.Height) ? Size.Width * 0.3 : Size.Width * 0.83;

        protected double DefaultDrawerWidthCollapsed => Size.Width * 0.05;

        protected ViewGroup DrawerViewGroup => _drawerViewGroup;

        protected ViewGroup ContentViewGroup => _contentViewGroup;

        /// <summary>
        /// Opens the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is opened with animation.</param>
        public async virtual void OpenDrawer(bool animate = false)
        {
            if (IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return;

            _contentViewGroup.FocusableChildren = false;
            _drawerViewGroup.Sensitive = false;
            _contentViewGroup.Sensitive = false;

            _drawerViewGroup.Show();
            _drawerViewGroup.RaiseToTop();

            if (animate)
                await RunAnimationAsync(true);

            _drawerViewGroup.UpdateBounds(new Rect(0, 0, DrawerWidth, Size.Height));

            if (!_isPopover)
                ContentViewGroup.UpdatePosition(new Point(DrawerWidth, 0));

            _isOpened = true;
            _drawerViewGroup.Sensitive = true;
            _contentViewGroup.Sensitive = true;

            FocusManager.Instance.SetCurrentFocusView(_drawerViewGroup);
        }

        /// <summary>
        /// Closes the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is closed with animation.</param>
        public async virtual void CloseDrawer(bool animate = false)
        {
            if (!IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return;

            _contentViewGroup.FocusableChildren = true;
            _drawerViewGroup.Sensitive = false;
            _contentViewGroup.Sensitive = false;

            if (animate)
                await RunAnimationAsync(false);

            if (_isPopover)
            {
                _drawerViewGroup.UpdateBounds(new Rect(0 - DrawerWidth, 0, DrawerWidth, Size.Height));
                _drawerViewGroup.Hide();
            }
            else
            {
                _drawerViewGroup.UpdateBounds(new Rect(0, 0, DrawerWidthCollapsed, Size.Height));
                _contentViewGroup.UpdatePosition(new Point(DrawerWidthCollapsed, 0));
            }

            _isOpened = false;
            _drawerViewGroup.Sensitive = true;
            _contentViewGroup.Sensitive = true;

            FocusManager.Instance.SetCurrentFocusView(_contentViewGroup);
        }

        protected virtual void OnLayoutUpdated(object? sender, LayoutEventArgs? args)
        {
            ConfigureLayout();
        }

        protected virtual void OnDrawerFocusGained(object? sender, EventArgs args)
        {
            DrawerViewGroup.Focusable = false;
            ContentViewGroup.Focusable = true;

            var focusable = FindFocusableChild(DrawerViewGroup) ?? DrawerViewGroup;
            FocusManager.Instance.SetCurrentFocusView(focusable);            
        }

        protected virtual void OnContentFocusGained(object? sender, EventArgs args)
        {
            DrawerViewGroup.Focusable = true;
            ContentViewGroup.Focusable = false;

            var focusable = FindFocusableChild(ContentViewGroup) ?? ContentViewGroup;
            FocusManager.Instance.SetCurrentFocusView(focusable);
        }

        protected virtual bool OnDrawerKeyEventTriggered(object sender, KeyEventArgs args)
        {
            return false;
        }

        protected virtual bool OnContentKeyEventTriggered(object sender, KeyEventArgs args)
        {
            return false;
        }

        protected View? FindFocusableChild(View view)
        {
            if (view.Focusable && !(view is ViewGroup))
                return view;

            foreach (var child in view.Children)
            {
                var focusable = FindFocusableChild(child);
                if (focusable != null)
                    return focusable;
            }

            return null;
        }

        protected virtual void ConfigureLayout()
        {
            if (DrawerBehavior == DrawerBehavior.Drawer)
            {
                if (_isPopover)
                {
                    var positionX = _isOpened ? 0 : 0 - DrawerWidth;
                    _drawerViewGroup.UpdateBounds(new Rect(positionX, 0, DrawerWidth, Size.Height));
                    _contentViewGroup.UpdateBounds(new Rect(0, 0, Size.Width, Size.Height));

                    if (_isOpened)
                        _drawerViewGroup.Show();
                    else
                        _drawerViewGroup.Hide();
                }
                else
                {
                    var width = _isOpened ? DrawerWidth : DrawerWidthCollapsed;
                    _drawerViewGroup.UpdateBounds(new Rect(0, 0, width, Size.Height));
                    _contentViewGroup.UpdateBounds(new Rect(width, 0, Size.Width, Size.Height));
                    _drawerViewGroup.Show();
                }

            }
            else if (DrawerBehavior == DrawerBehavior.Locked)
            {
                _drawerViewGroup.UpdateBounds(new Rect(0, 0, DrawerWidth, Size.Height));
                _contentViewGroup.UpdateBounds(new Rect(DrawerWidth, 0, Size.Width - DrawerWidth, Size.Height));
                _drawerViewGroup.Show();
                _isOpened = true;
            }
            else
            {
                _contentViewGroup.UpdateBounds(new Rect(0, 0, Size.Width, Size.Height));
                _drawerViewGroup.Hide();
                _isOpened = false;
            }
        }

        protected virtual Task RunAnimationAsync(bool isOpen)
        {
            return Task.CompletedTask;
        }

        void ResetView(ViewGroup viewGroup, View? view)
        {
            viewGroup.Children.Clear();

            if (view != null)
            {
                viewGroup.Children.Add(view);
            }
        }
    }
}
