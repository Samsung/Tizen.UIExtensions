using System;
using ElmSharp;
using static ElmSharp.GestureLayer;
using EBox = ElmSharp.Box;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The native widget which provides the Box with Drawer.
    /// </summary>
    public class DrawerBox : EBox
    {
        /// <summary>
        /// The Drawer native container.
        /// </summary>
        readonly EBox _drawerBox;

        /// <summary>
        /// The Content native container.
        /// </summary>
        readonly EBox _contentBox;

        /// <summary>
        /// The <see cref="Drawer"/> property value.
        /// </summary>
        EvasObject? _drawer;

        /// <summary>
        /// The <see cref="Content"/> property value.
        /// </summary>
        EvasObject? _content;

        /// <summary>
        /// The <see cref="DimArea"/> property value.
        /// </summary>
        readonly EBox _dimArea;

        /// <summary>
        /// The gesture layer for dim area
        /// </summary>
        GestureLayer _gestureOnDimArea;

        /// <summary>
        /// The container for <c>_drawerBox</c> and <c>_contentBox</c> used in split mode.
        /// </summary>
        readonly Panes _splitPane;

        /// <summary>
        /// The container for <c>_drawerBox</c> used in popover mode.
        /// </summary>
        readonly Panel _panel;

        /// <summary>
        /// The main widget - either <see cref="_splitPlane"/> or <see cref="_contentBox"/>, depending on the mode.
        /// </summary>
        EvasObject _mainWidget;

        /// <summary>
        /// The <see cref="SplitRatio"/> property value.
        /// </summary>
        double _splitRatio = 0;

        /// <summary>
        /// The <see cref="PopoverRatio"/> property value.
        /// </summary>
        double _popoverRatio = 0;

        /// <summary>
        /// The <see cref="IsSplit"/> property value.
        /// </summary>
        bool _isSplit = false;

        /// <summary>
        /// The <see cref="IsGestureEnabled"/> property value.
        /// </summary>
        bool _isGestureEnabled = true;

        /// <summary>
        /// Occurs when the Drawer is shown or hidden.
        /// </summary>
        public event EventHandler? Toggled;

        public DrawerBox(EvasObject parent) : base(parent)
        {
            _drawerBox = new EBox(this);
            _drawerBox.SetAlignment(-1.0, -1.0);
            _drawerBox.SetWeight(1.0, 1.0);
            _drawerBox.Show();

            _contentBox = new EBox(this);
            _contentBox.SetAlignment(-1.0, -1.0);
            _contentBox.SetWeight(1.0, 1.0);
            _contentBox.Show();

            _dimArea = new EBox(this)
            {
                BackgroundColor = ThemeConstants.Shell.ColorClass.DefaultDrawerDimBackgroundColor,
                Opacity = ThemeConstants.Shell.Resources.DefaultDrawerDimOpacity
            };

            _gestureOnDimArea = new GestureLayer(_dimArea);
            _gestureOnDimArea.SetTapCallback(GestureType.Tap, GestureLayer.GestureState.Start, OnTapped);
            _gestureOnDimArea.Attach(_dimArea);

            _splitPane = new Panes(this)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
                IsFixed = true,
                IsHorizontal = false,

            };

            _panel = new Panel(this);
            _panel.SetScrollable(_isGestureEnabled);
            _panel.SetScrollableArea(1.0);
            _panel.Direction = PanelDirection.Left;
            _panel.Toggled += (object sender, EventArgs e) =>
            {
                if (_panel.IsOpen)
                    _dimArea.Show();

                Toggled?.Invoke(this, EventArgs.Empty);
            };

            _mainWidget = _contentBox;

            ConfigureLayout();
            SetLayoutCallback(OnLayout);
        }

        /// <summary>
        /// Gets or sets the content of the Drawer.
        /// </summary>
        /// <value>The Drawer.</value>
        public EvasObject? Drawer
        { 
            get
            {
                return _drawer;
            }
            set
            {
                if(_drawer != null)
                {
                    _drawerBox.UnPack(_drawer);
                    _drawer.Hide();
                }

                _drawer = value;

                if(_drawer != null)
                {
                    _drawer.SetAlignment(-1, -1);
                    _drawer.SetWeight(1, 1);
                    _drawer.Show();
                    _drawerBox.PackEnd(_drawer);
                }
            }
        }

        /// <summary>
        /// Gets or sets the content of the Content.
        /// </summary>
        /// <value>The Content.</value>
        public EvasObject? Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (_content != null)
                {
                    _contentBox.UnPack(_content);
                    _content.Hide();
                }

                _content = value;

                if (_content != null) 
                {
                    _content.SetAlignment(-1, -1);
                    _content.SetWeight(1, 1);
                    _content.Show();
                    _contentBox.PackEnd(_content);
                }
            }
        }

        /// <summary>
        /// Gets the box for dim area
        /// </summary>
        /// <value>The Content.</value>
        public EBox DimArea => _dimArea;

        /// <summary>
        /// Gets or sets the DrawerBox is splited
        /// </summary>
        public bool IsSplit 
        {
            get
            {
                return _isSplit;
            }
            set
            {
                if (_isSplit != value)
                {
                    _isSplit = value;
                    ConfigureLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Drawer is shown.
        /// </summary>
        /// <value><c>true</c> if the Drawer is opened.</value>
        public bool IsOpen
        {
            get
            {
                if (IsSplit)
                    return true;

                return _panel.IsOpen;
            }
            set
            {
                if (IsSplit)
                    return;

                if (_panel.IsOpen != value)
                {
                    _panel.IsOpen = value;

                    if (_panel.IsOpen)
                        _dimArea.Show();
                    else
                        _dimArea.Hide();
                }

            }
        }

        /// <summary>
        /// Gets or Sets the portion of the screen that the DrawerBox takes in split mode.
        /// </summary>
        /// <value>The portion.</value>
        public double SplitRatio
        {
            get
            {
                return _splitRatio;
            }
            set
            {
                if (_splitRatio != value)
                {
                    _splitRatio = value;
                    ConfigureLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the portion of the screen that the DrawerBox takes in Popover mode.
        /// </summary>
        /// <value>The portion.</value>
        public double PopoverRatio
        {
            get
            {
                return _popoverRatio;
            }
            set
            {
                if (_popoverRatio != value)
                {
                    _popoverRatio = value;
                    OnLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Drawer can be opend by swipe gesture.
        /// </summary>
        /// <value><c>true</c> if the Drawer can be revealed with a gesture.</value>
        public bool IsGestureEnabled
        {
            get
            {
                return _isGestureEnabled;
            }

            set
            {
                if (_isGestureEnabled != value)
                {
                    
                    _isGestureEnabled = value;
                    // Fixme
                    // Elementary panel was not support to change scrollable property on runtime
                    // Please uncomment when EFL was updated
                    //_panel.SetScrollable(_isGestureEnabled);
                }
            }
        }

        /// <summary>
        /// Configure the layout of DrawerBox to split or popover
        /// </summary>
        void ConfigureLayout()
        {
            _panel.SetContent(null, true);
            _panel.Hide();

            _splitPane.SetLeftPart(null, true);
            _splitPane.SetRightPart(null, true);
            _splitPane.Hide();

            UnPackAll();

            // the structure for split mode and for popover mode looks differently
            if (IsSplit)
            {
                if (_panel != null)

                _splitPane.SetLeftPart(_drawerBox, true);
                _splitPane.SetRightPart(_contentBox, true);
                _splitPane.Proportion = (SplitRatio > 0) ? SplitRatio : this.GetSplitRatio();
                _splitPane.Show();
                PackEnd(_splitPane);

                _mainWidget = _splitPane;

                if (!IsOpen)
                {
                    IsOpen = true;
                    Toggled?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                _panel.SetContent(_drawerBox, true);
                _panel.Show();

                if (_panel.IsOpen)
                    _dimArea.Show();

                PackEnd(_contentBox);
                PackEnd(_dimArea);
                PackEnd(_panel);

                _mainWidget = _contentBox;
            }
        }

        /// <summary>
        /// Force update the layout of DrawerBox
        /// </summary>
        void OnLayout()
        {
            var bound = Geometry;
            if (_mainWidget != null)
            {
                _mainWidget.Geometry = bound;
            }

            _dimArea.Geometry = bound;

            var ratio = (PopoverRatio > 0) ? PopoverRatio : this.GetDrawerRatio(Geometry.Width, Geometry.Height);
            bound.Width = (int)((ratio * bound.Width));
            _panel.Geometry = bound;
        }

        /// <summary>
        /// Event handler for tap gesture on dim area
        /// </summary>
        void OnTapped(GestureLayer.TapData data)
        {
            if (IsOpen)
            {
                IsOpen = false;
            }
        }
    }
}
