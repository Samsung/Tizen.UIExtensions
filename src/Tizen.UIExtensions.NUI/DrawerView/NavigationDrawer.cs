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
    public class NavigationDrawer : DrawerView, IAnimatable
    {
        ViewGroup _backdropViewGroup;

        View? _backdrop;
        View? _gestureArea;

        bool _isGestureEnabled;
        double _defaultGestureAreaWidth = 50;

        PanGestureDetector? _panGestureDetector;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationDrawer"/> class
        /// </summary>
        public NavigationDrawer() : base(true)
        {
            _backdropViewGroup = new ViewGroup();
            Children.Add(_backdropViewGroup);

            _backdropViewGroup.TouchEvent += OnBackDropTouched;
        }

        /// <summary>
        /// Gets or sets a view that blocks interaction when the drawer is opened.
        /// </summary>
        public override View? Backdrop
        {
            get => _backdrop;
            set
            {
                if (_backdrop == value)
                    return;

                _backdrop = value;
                UpdateBackDrop();
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates if gesture is enabled or not.
        /// </summary>
        public override bool IsGestureEnabled
        {
            get => _isGestureEnabled;
            set
            {
                if (_isGestureEnabled == value)
                    return;

                _isGestureEnabled = value;
                UpdateGestureEnabling(value);
            }
        }

        /// <summary>
        /// Opens the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is opened with animation.</param>
        public override async Task OpenAsync(bool animate = false)
        {
            if (this.IsDrawerOpened())
                return;

            await base.OpenAsync(animate);

            _backdropViewGroup.Show();

            if (_isGestureEnabled)
                EnableGesture();

            SendToggled();
        }

        /// <summary>
        /// Closes the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is closed with animation.</param>
        public override async Task CloseAsync(bool animate = false)
        {
            if (!this.IsDrawerOpened())
                return;

            await base.CloseAsync(animate);

            _backdropViewGroup.Hide();

            if (_isGestureEnabled)
                EnableGesture();

            SendToggled();
        }

        protected bool OnBackDropTouched(object sender, TouchEventArgs args)
        {
            if (args.Touch.GetState(0) == PointStateType.Finished)
                _ = CloseAsync(true);

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
            if (args.Key.IsDeclineKeyEvent())
            {
                _ = CloseAsync(true);
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

        void InitGesture()
        {
            if (_gestureArea == null)
            {
                _gestureArea = new View()
                {
                    GrabTouchAfterLeave = true,
                };

                Children.Add(_gestureArea);
                _gestureArea.RaiseToTop();
            }

            if (_panGestureDetector == null)
            {
                _panGestureDetector = new PanGestureDetector();
                _panGestureDetector.Detected += OnGestureDetected;
            }
        }

        async void OnGestureDetected(object sender, PanGestureDetector.DetectedEventArgs args)
        {
            if (args.PanGesture.State == Gesture.StateType.Started)
            {
                if (!this.IsDrawerOpened())
                {
                    DrawerViewGroup.Show();
                    DrawerViewGroup.UpdatePosition(new Point(0 - DrawerWidth + _defaultGestureAreaWidth / 2, 0));
                    IsOpened = true;
                }
            }
            else if (args.PanGesture.State == Gesture.StateType.Finished || args.PanGesture.State == Gesture.StateType.Cancelled)
            {
                if (DrawerViewGroup.Position.X > (DrawerWidth / 2) * -1)
                {
                    IsOpened = false;
                    await OpenAsync(true);
                }
                else
                {
                    await CloseAsync(true);
                }
            }
            else
            {
                var x = 0 - DrawerWidth + args.PanGesture.ScreenPosition.X;
                DrawerViewGroup.UpdatePosition(new Point(((x < 0) ? x : 0), 0));
            }
        }

        void UpdateGestureEnabling(bool enabled)
        {
            if (enabled && DrawerBehavior == DrawerBehavior.Drawer)
                EnableGesture();
            else
                DisableGesture();
        }

        void UpdateBackDrop()
        {
            _backdropViewGroup.Children.Clear();

            if (_backdrop != null)
            {
                _backdropViewGroup.Children.Add(_backdrop);
            }
        }

        void EnableGesture()
        {
            InitGesture();

            _gestureArea?.Show();
            _panGestureDetector?.Attach(IsOpened ? DrawerViewGroup : _gestureArea);
        }

        void DisableGesture()
        {
            _gestureArea?.Hide();
            _panGestureDetector?.Detach(IsOpened ? DrawerViewGroup : _gestureArea);
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
