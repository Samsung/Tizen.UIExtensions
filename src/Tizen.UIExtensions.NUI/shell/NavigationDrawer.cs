using System;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.Internal;
using Animation = Tizen.UIExtensions.Common.Internal.Animation;
using Size = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A View that contains a drawer for navigation of an app.
    /// </summary>
    public class NavigationDrawer : DrawerView, IAnimatable, INavigationDrawer
    {
        ViewGroup _backdropViewGroup;

        View? _backdrop;
        View? _gestureArea;

        bool _isGestureEnabled;
        double _defaultGestureAreaWidth = 50;

        Lazy<PanGestureDetector> _panGestureDetector = new Lazy<PanGestureDetector>(()=> new PanGestureDetector());
        Lazy<LongPressGestureDetector> _longPressGestureDetector = new Lazy<LongPressGestureDetector>(() => new LongPressGestureDetector());

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationDrawer"/> class
        /// </summary>
        public NavigationDrawer() : base()
        {
            _backdropViewGroup = new ViewGroup();
            _backdropViewGroup.TouchEvent += OnBackDropTouched;
            Children.Add(_backdropViewGroup);

            IsPopover = true;
        }

        /// <summary>
        /// Gets or sets a view that blocks interaction when the drawer is opened.
        /// </summary>
        public View? BackDrop
        {
            get => _backdrop;
            set
            {
                if (_backdrop == value)
                    return;

                _backdrop = value;
                SetBackDropView();
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates if gesture is enabled or not.
        /// </summary>
        public bool IsGestureEnabled
        {
            get => _isGestureEnabled;
            set
            {
                if (_isGestureEnabled == value)
                    return;

                _isGestureEnabled = value;
                EnableGesture(value);
            }
        }

        /// <summary>
        /// Event that is raised when the drawer is toggled.
        /// </summary>
        public event EventHandler? Toggled;

        /// <summary>
        /// Opens the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is opened with animation.</param>
        public override void OpenDrawer(bool animate = false)
        {
            if (IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return;

            base.OpenDrawer(animate);

            _backdropViewGroup.Show();
            _gestureArea?.Hide();

            Toggled?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Closes the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is closed with animation.</param>
        public override void CloseDrawer(bool animate = false)
        {
            if (!IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return;

            base.CloseDrawer(animate);

            _backdropViewGroup.Hide();

            if (_isGestureEnabled)
            {
                _gestureArea?.RaiseToTop();
                _gestureArea?.Show();
            }

            Toggled?.Invoke(this, EventArgs.Empty);
        }

        protected bool OnBackDropTouched(object sender, TouchEventArgs args)
        {
            if (args.Touch.GetState(0) == PointStateType.Finished)
                CloseDrawer(true);

            return true;
        }

        protected override void ConfigureLayout()
        {
            base.ConfigureLayout();

            if (DrawerBehavior == DrawerBehavior.Drawer)
            {
                _backdropViewGroup?.UpdateBounds(new Rect(0, 0, Size.Width, Size.Height));

                if (IsGestureEnabled)
                {
                    _gestureArea?.UpdateBounds(new Rect(0, 0, _defaultGestureAreaWidth, Size.Height));
                    _gestureArea?.Show();
                }

                if (!IsOpened)
                {
                    _backdropViewGroup?.Hide();
                }
            }
            else if (DrawerBehavior == DrawerBehavior.Locked)
            {
                _backdropViewGroup?.Hide();
                _gestureArea?.Hide();
            }
            else
            {
                _backdropViewGroup?.Hide();
                _gestureArea?.Hide();
            }
        }

        protected override bool OnDrawerKeyEventTriggered(object sender, KeyEventArgs args)
        {
            if (args.Key.KeyPressedName == "XF86Back" && args.Key.State == Key.StateType.Up)
            {
                CloseDrawer(true);
                return true;
            }

            return false;
        }

        protected override Task RunAnimationAsync(bool isOpen)
        {
            var start = DrawerViewGroup.Position.X;
            var end = (isOpen) ? 0 : (0 - DrawerWidth);

            return MoveDrawerAsync(start, end);
        }

        Task MoveDrawerAsync(double start, double end)
        {
            var tcs = new TaskCompletionSource<bool>();

            var animation = new Animation(v =>
            {
                var r = start + ((end - start) * v);
                DrawerViewGroup.UpdatePosition(new Point(r, 0));
            }, easing: Easing.Linear);

            animation.Commit(this, "MoveDrawer", length: 200, finished: (l, c) =>
            {
                DrawerViewGroup.UpdatePosition(new Point(end, 0));
                tcs.SetResult(true);
            });

            return tcs.Task;
        }

        void ShowDrawer()
        {
            if (IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return;

            DrawerViewGroup.Show();
            DrawerViewGroup.RaiseToTop();
            DrawerViewGroup.UpdateBounds(new Rect(0 - DrawerWidth + _defaultGestureAreaWidth / 2, 0, DrawerWidth, Size.Height));
        }

        void InitGestureArea()
        {
            _gestureArea = new View();
            _gestureArea.GrabTouchAfterLeave = true;

            Children.Add(_gestureArea);
            _gestureArea.UpdateBounds(new Rect(0, 0, _defaultGestureAreaWidth, Size.Height));

            _panGestureDetector.Value.Detected += (s, e) =>
            {
                var x = 0 - DrawerWidth + e.PanGesture.ScreenPosition.X;
                DrawerViewGroup.UpdatePosition(new Point(((x < 0) ? x : 0), 0));
            };

            _longPressGestureDetector.Value.Detected += (s, e) =>
            {
                if (e.LongPressGesture.State == Gesture.StateType.Started)
                {
                    ShowDrawer();
                    IsOpened = true;
                    _panGestureDetector.Value.Attach(_gestureArea);
                }
                else if (e.LongPressGesture.State == Gesture.StateType.Finished || e.LongPressGesture.State == Gesture.StateType.Cancelled)
                {
                    if (DrawerViewGroup.Position.X > (DrawerWidth / 2) * -1)
                    {
                        IsOpened = false;
                        OpenDrawer(true);
                    }
                    else
                    {
                        CloseDrawer(true);
                    }
                    _panGestureDetector.Value.Detach(_gestureArea);
                }

            };

            _longPressGestureDetector.Value.Attach(_gestureArea);
        }

        void ShowGestureArea()
        {
            if (_gestureArea == null)
                InitGestureArea();

            if (!IsOpened)
            {
                _gestureArea?.Show();
                _gestureArea?.RaiseToTop();
            }
        }

        void HideGestureArea()
        {
            _gestureArea?.Hide();
        }

        void EnableGesture(bool enabled)
        {
            if (enabled)
                ShowGestureArea();
            else
                HideGestureArea();
        }

        void SetBackDropView()
        {
            _backdropViewGroup.Children.Clear();

            if (_backdrop != null)
            {
                _backdropViewGroup.Children.Add(_backdrop);
                _backdropViewGroup.LowerBelow(DrawerViewGroup);
            }
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
