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
    public abstract class DrawerView : ViewGroup, INavigationDrawer
    {
        ViewGroup _drawerViewGroup;
        ViewGroup _contentViewGroup;
        View? _drawer;
        View? _content;

        DrawerBehavior _behavior;
        bool _isPopover;

        double _drawerWidth = -1;
        double _drawerWidthCollapsed = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawerView"/> class
        /// </summary>
        public DrawerView(bool isPopover)
        {
            _drawerViewGroup = new ViewGroup()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Focusable = true,
            };
            _contentViewGroup = new ViewGroup()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Focusable = true,
            };

            _behavior = DrawerBehavior.Drawer;
            _isPopover = isPopover;

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
        public virtual bool IsOpened { get; protected set; } = false;

        /// <summary>
        /// Gets or sets a value that controls the shadow of the drawer.
        /// </summary>
        public virtual Shadow? DrawerShadow
        {
            get => _drawerViewGroup.BoxShadow;
            set => _drawerViewGroup.BoxShadow = value;
        }

        /// <summary>
        /// Gets or sets a view for drawer.
        /// </summary>
        public virtual View? Drawer
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
        public virtual View? Content
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

        public virtual View? Backdrop { get; set; }

        public virtual bool IsGestureEnabled { get; set; }

        /// <summary>
        /// Gets or sets an enumeration value that controls how the drawer appears.
        /// </summary>
        public virtual DrawerBehavior DrawerBehavior
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
        public virtual bool IsPopover
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
        public virtual double DrawerWidth
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

        ///// <summary>
        ///// Event that is raised when the drawer is toggled.
        ///// </summary>
        public event EventHandler? Toggled;

        /// <summary>
        /// Opens the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is opened with animation.</param>
        public virtual async Task OpenAsync(bool animate = false)
        {
            if (this.IsDrawerOpened())
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
                _contentViewGroup.UpdatePosition(new Point(DrawerWidth, 0));

            IsOpened = true;
            _drawerViewGroup.Sensitive = true;
            _contentViewGroup.Sensitive = true;

            FocusManager.Instance.SetCurrentFocusView(_drawerViewGroup);
        }

        /// <summary>
        /// Closes the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is closed with animation.</param>
        public virtual async Task CloseAsync(bool animate = false)
        {
            if (!this.IsDrawerOpened())
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

            IsOpened = false;
            _drawerViewGroup.Sensitive = true;
            _contentViewGroup.Sensitive = true;

            FocusManager.Instance.SetCurrentFocusView(_contentViewGroup);
        }

        protected void SendToggled()
        {
            Toggled?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLayoutUpdated(object? sender, LayoutEventArgs? args)
        {
            ConfigureLayout();
        }

        protected virtual void OnDrawerFocusGained(object? sender, EventArgs args)
        {
            SetCurrentFocusView(DrawerViewGroup);
        }

        protected virtual void OnContentFocusGained(object? sender, EventArgs args)
        {
            SetCurrentFocusView(ContentViewGroup);
        }

        void SetCurrentFocusView(ViewGroup viewGroup)
        {
            bool isDrawerView = viewGroup == DrawerViewGroup;
            DrawerViewGroup.Focusable = !isDrawerView;
            ContentViewGroup.Focusable = isDrawerView;

            var focusable = FindFocusableChild(viewGroup) ?? viewGroup;
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
            foreach (var child in view.Children)
            {
                if (child.Focusable)
                    return child;

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
                    var positionX = IsOpened ? 0 : 0 - DrawerWidth;
                    _drawerViewGroup.UpdateBounds(new Rect(positionX, 0, DrawerWidth, Size.Height));
                    _contentViewGroup.UpdateBounds(new Rect(0, 0, Size.Width, Size.Height));

                    if (IsOpened)
                        _drawerViewGroup.Show();
                    else
                        _drawerViewGroup.Hide();
                }
                else
                {
                    var width = IsOpened ? DrawerWidth : DrawerWidthCollapsed;
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
                IsOpened = true;
            }
            else
            {
                _contentViewGroup.UpdateBounds(new Rect(0, 0, Size.Width, Size.Height));
                _drawerViewGroup.Hide();
                IsOpened = false;
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
