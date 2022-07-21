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
        ViewGroup _backdropViewGroup;
        View? _drawer;
        View? _content;
        View? _backdrop;

        DrawerBehavior _behavior;
        bool _isPopover;

        double _drawerWidth = -1;
        double _drawerWidthCollapsed = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawerView"/> class
        /// </summary>
        /// <param name="isPopover">A value that indicates wherher the drawer overlaps the content or not. </param>
        public DrawerView(bool isPopover)
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

            _backdropViewGroup = new ViewGroup();

            _behavior = DrawerBehavior.Drawer;
            _isPopover = isPopover;

            Children.Add(_contentViewGroup);
            Children.Add(_backdropViewGroup);
            Children.Add(_drawerViewGroup);

            LayoutUpdated += OnLayoutUpdated;

            _drawerViewGroup.FocusGained += OnDrawerFocusGained;
            _drawerViewGroup.KeyEvent += OnDrawerKeyEventTriggered;

            _backdropViewGroup.TouchEvent += OnBackDropTouched;

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

        /// <summary>
        /// Gets or sets a view that blocks interaction when the drawer is opened.
        /// </summary>
        public virtual View? Backdrop
        {
            get => _backdrop;
            set
            {
                if (_backdrop == value)
                    return;

                _backdrop = value;
                ResetView(_backdropViewGroup, _backdrop);
            }
        }

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

        protected ViewGroup BackdropViewGroup => _backdropViewGroup;

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

            _drawerViewGroup.Show();
            _drawerViewGroup.RaiseToTop();

            _backdropViewGroup.Show();

            if (animate)
                await RunAnimationAsync(true);

            _drawerViewGroup.UpdateBounds(new Rect(0, 0, DrawerWidth, Size.Height));

            if (!_isPopover)
                _contentViewGroup.UpdatePosition(new Point(DrawerWidth, 0));

            IsOpened = true;
        }

        /// <summary>
        /// Closes the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is closed with animation.</param>
        public virtual async Task CloseAsync(bool animate = false)
        {
            if (!this.IsDrawerOpened())
                return;

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

            _backdropViewGroup.Hide();

            IsOpened = false;
        }

        protected abstract Task RunAnimationAsync(bool isOpen);

        protected virtual void OnLayoutUpdated(object? sender, LayoutEventArgs? args)
        {
            ConfigureLayout();
        }

        protected virtual void OnDrawerFocusGained(object? sender, EventArgs args)
        {
        }

        protected virtual void OnContentFocusGained(object? sender, EventArgs args)
        {
        }

        protected virtual bool OnDrawerKeyEventTriggered(object sender, KeyEventArgs args)
        {
            if (args.Key.IsDeclineKeyEvent())
            {
                _ = OpenAsync(true);
                return true;
            }

            return false;
        }

        protected virtual bool OnContentKeyEventTriggered(object sender, KeyEventArgs args)
        {
            return false;
        }

        protected virtual bool OnBackDropTouched(object sender, TouchEventArgs args)
        {
            if (args.Touch.GetState(0) == PointStateType.Finished)
                _ = CloseAsync(true);

            return true;
        }

        protected virtual void ConfigureLayout()
        {
            if (DrawerBehavior == DrawerBehavior.Drawer)
            {
                if (_isPopover)
                {
                    var positionX = IsOpened ? 0 : -DrawerWidth;
                    _drawerViewGroup.UpdateBounds(new Rect(positionX, 0, DrawerWidth, Size.Height));
                    _backdropViewGroup.UpdateBounds(new Rect(0, 0, Size.Width, Size.Height));
                    _contentViewGroup.UpdateBounds(new Rect(0, 0, Size.Width, Size.Height));

                    if (IsOpened)
                    {
                        _drawerViewGroup.Show();
                        _backdropViewGroup.Show();

                    }
                    else
                    {
                        _drawerViewGroup.Hide();
                        _backdropViewGroup.Hide();
                    }
                }
                else
                {
                    var width = IsOpened ? DrawerWidth : DrawerWidthCollapsed;
                    _drawerViewGroup.UpdateBounds(new Rect(0, 0, width, Size.Height));
                    _contentViewGroup.UpdateBounds(new Rect(width, 0, Size.Width, Size.Height));
                    _backdropViewGroup.UpdateBounds(new Rect(width, 0, Size.Width, Size.Height));
                    _drawerViewGroup.Show();
                    _backdropViewGroup.Show();
                }

            }
            else if (DrawerBehavior == DrawerBehavior.Locked)
            {
                _drawerViewGroup.UpdateBounds(new Rect(0, 0, DrawerWidth, Size.Height));
                _contentViewGroup.UpdateBounds(new Rect(DrawerWidth, 0, Size.Width - DrawerWidth, Size.Height));
                _drawerViewGroup.Show();
                _backdropViewGroup.Hide();
                IsOpened = true;
            }
            else
            {
                _contentViewGroup.UpdateBounds(new Rect(0, 0, Size.Width, Size.Height));
                _drawerViewGroup.Hide();
                _backdropViewGroup.Hide();
                IsOpened = false;
            }
        }

        protected void SendToggled()
        {
            Toggled?.Invoke(this, EventArgs.Empty);
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
