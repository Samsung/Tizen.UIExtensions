using ElmSharp;
using System;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.Internal;
using EBox = ElmSharp.Box;
using EColor = ElmSharp.Color;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The native widget to be used in Xamarin.Forms Shell for TV.
    /// </summary>
    public class TVNavigationDrawer : EBox, INavigationDrawer, IAnimatable
    {
        EBox _drawerBox;
        EBox _mainBox;
        EvasObject? _main;
        EvasObject? _drawer;
        Button _focusControlArea;

        DrawerBehavior _behavior;
        bool _isOpen;
        bool _isSplit;
        double _drawerRatio;

        double _OpenRatio = -1;
        double _closeRatio = -1;


        /// <summary>
        /// Initializes a new instance of the <see cref="Tizen.UIExtensions.ElmSharp.TVNavigationDrawer"/> class.
        /// </summary>
        /// <param name="parent"></param>
        public TVNavigationDrawer(EvasObject parent) : base(parent)
        {
            SetLayoutCallback(OnLayout);

            _drawerBox = new EBox(parent);
            _drawerBox.Show();
            PackEnd(_drawerBox);

            _mainBox = new EBox(parent);
            _mainBox.SetLayoutCallback(OnMainBoxLayout);
            _mainBox.Show();
            PackEnd(_mainBox);

            _focusControlArea = new Button(parent)
            {
                Color = EColor.Transparent,
                BackgroundColor = EColor.Transparent
            };
            _focusControlArea.SetEffectColor(EColor.Transparent);
            _focusControlArea.Show();
            _mainBox.PackEnd(_focusControlArea);

            _behavior = Common.DrawerBehavior.Drawer;

            _drawerBox.KeyUp += (s, e) =>
            {
                if (e.KeyName == "Return" || e.KeyName == "Right")
                {
                    IsOpen = false;
                }
            };

            _mainBox.KeyUp += (s, e) =>
            {
                if (e.KeyName == "Left")
                {
                    if (_focusControlArea.IsFocused)
                        IsOpen = true;
                }
                else
                {
                    // Workaround to prevent unexpected movement of the focus to drawer during page pushing.
                    if (_behavior == DrawerBehavior.Locked)
                        _drawerBox.AllowTreeFocus = true;
                }
            };

            _mainBox.KeyDown += (s, e) =>
            {
                if (e.KeyName != "Left")
                {
                    // Workaround to prevent unexpected movement of the focus to drawer during page pushing.
                    if (_behavior == DrawerBehavior.Locked)
                        _drawerBox.AllowTreeFocus = false;
                }
            };

            UpdateFocusPolicy();
        }

        /// <summary>
        /// Occurs when the drawer is shown or hidden.
        /// </summary>
        public event EventHandler? Toggled;

        /// <summary>
        /// Gets the target view of TVNavigationDrawer.
        /// </summary>
        public EvasObject TargetView => this;

        /// <summary>
        /// Gets or sets the navigation view to be shown on the drawer.
        /// </summary>
        public EvasObject? NavigationView
        {
            get => _drawer;
            set => UpdateNavigationView(value);
        }

        /// <summary>
        /// Gets or set the main content of TVNavigationDrawer.
        /// </summary>
        public EvasObject? Main
        {
            get => _main;
            set => UpdateMain(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the drawer is shown.
        /// </summary>
        /// <value><c>true</c> if the Drawer is opened.</value>
        public bool IsOpen
        {
            get => _isOpen;
            set => UpdateOpenState(value);
        }

        /// <summary>
        /// Gets or sets the TVNavigationDrawer is splited.
        /// </summary>
        public bool IsSplit 
        {
            get => _isSplit;
            set => UpdateBehavior(value ? DrawerBehavior.Locked : _behavior);
        }

        /// <summary>
        /// Gets or sets the behavior of the drawer.
        /// </summary>
        public DrawerBehavior DrawerBehavior 
        { 
            get => _behavior;
            set => UpdateBehavior(value);
        }

        /// <summary>
        /// Gets or Sets the portion of the screen then the drawer is opened.
        /// </summary>
        public double OpenRatio
        {
            get => _OpenRatio;
            set
            {
                if (_OpenRatio != value)
                {
                    _OpenRatio = value;
                    OnLayout();
                }
            }
        }

        /// <summary>
        /// Gets or Sets the portion of the screen then the drawer is closed.
        /// </summary>
        public double CloseRatio
        {
            get => _closeRatio;
            set
            {
                if (_closeRatio != value)
                {
                    _closeRatio = value;
                    OnLayout();
                }
            }
        }

        void UpdateBehavior(DrawerBehavior behavior)
        {
            _behavior = behavior;
            _isSplit = (behavior == DrawerBehavior.Locked) ? true : false;
            _focusControlArea.IsEnabled = _behavior == Common.DrawerBehavior.Drawer;

            var open = false;

            if (_behavior == DrawerBehavior.Locked)
                open = true;
            else if (_behavior == DrawerBehavior.Disabled)
                open = false;
            else
                open = _drawerBox.IsFocused;

            UpdateOpenState(open);
        }

        void UpdateNavigationView(EvasObject? navigationView)
        {
            if (_drawer != null)
            {
                _drawerBox.UnPack(_drawer);
                _drawer.Hide();
            }

            _drawer = navigationView;

            if (_drawer != null)
            {
                _drawer.SetAlignment(-1, -1);
                _drawer.SetWeight(1, 1);
                _drawer.Show();
                _drawerBox.PackEnd(_drawer);
            }
        }

        void UpdateMain(EvasObject? main)
        {
            if (_main != null)
            {
                _mainBox.UnPack(_main);
                _main.Hide();
            }
            _main = main;

            if (_main != null)
            {
                _main.SetAlignment(-1, -1);
                _main.SetWeight(1, 1);
                _main.Show();
                _mainBox.PackStart(_main);
            }
        }

        void OnMainBoxLayout()
        {
            if (_main != null)
            {
                _main.Geometry = _mainBox.Geometry;
            }

            var focusedButtonGeometry = _mainBox.Geometry;
            focusedButtonGeometry.X = focusedButtonGeometry.X - 100;
            focusedButtonGeometry.Width = 0;
            focusedButtonGeometry.Height = this.GetTvFocusedButtonHeight();
            _focusControlArea.Geometry = focusedButtonGeometry;
        }

        void OnLayout()
        {
            if (Geometry.Width == 0 || Geometry.Height == 0)
                return;

            var bound = Geometry;

            var openRatio = (_OpenRatio < 0) ? this.GetTvDrawerRatio(Geometry.Width, Geometry.Height) : _OpenRatio;
            var closeRatio = (_behavior == DrawerBehavior.Disabled) ? 0 : ((_closeRatio < 0) ? this.GetTvDrawerCloseRatio() : _closeRatio);
            var drawerWidthMax = (int)(bound.Width * openRatio);
            var drawerWidthMin = (int)(bound.Width * closeRatio);

            var drawerWidthOutBound = (int)((drawerWidthMax - drawerWidthMin) * (1 - _drawerRatio));
            var drawerWidthInBound = drawerWidthMax - drawerWidthOutBound;

            var drawerGeometry = bound;
            drawerGeometry.Width = drawerWidthInBound;
            _drawerBox.Geometry = drawerGeometry;

            var containerGeometry = bound;
            containerGeometry.X = drawerWidthInBound;
            containerGeometry.Width = (_behavior == DrawerBehavior.Locked) ? (bound.Width - drawerWidthInBound) : (bound.Width - drawerWidthMin);
            _mainBox.Geometry = containerGeometry;
        }

        void UpdateOpenState(bool isOpen)
        {
            if (_behavior == DrawerBehavior.Locked && !isOpen)
                return;

            double endState = ((_behavior != DrawerBehavior.Disabled) && isOpen) ? 1 : 0;
            new Animation((r) =>
            {
                _drawerRatio = r;
                OnLayout();
            }, _drawerRatio, endState, Easing.SinOut).Commit(this, "DrawerMove", finished: (f, aborted) =>
            {
                if (!aborted)
                {
                    if (_isOpen != isOpen)
                    {
                        _isOpen = isOpen;
                        UpdateFocusPolicy();
                        Toggled?.Invoke(this, EventArgs.Empty);
                    }
                }
            });
        }

        void UpdateFocusPolicy()
        {
            if (_isOpen)
            {
                if (_behavior == DrawerBehavior.Locked)
                {
                    _drawerBox.AllowTreeFocus = true;
                    _mainBox.AllowTreeFocus = true;
                }
                else
                {
                    _mainBox.AllowTreeFocus = false;
                    _drawerBox.AllowTreeFocus = true;
                    _drawerBox.SetFocus(true);
                }
            }
            else
            {
                _mainBox.AllowTreeFocus = true;
                _drawerBox.AllowTreeFocus = false;
                _mainBox.SetFocus(true);
            }
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
