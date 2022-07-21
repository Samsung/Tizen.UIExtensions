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
        View? _gestureArea;
        View? _gestureAttatedView;

        bool _isGestureEnabled;
        double _defaultGestureAreaWidth = 50;

        PanGestureDetector? _panGestureDetector;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationDrawer"/> class
        /// </summary>
        public NavigationDrawer() : base(true)
        {
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

            BackdropViewGroup.Opacity = 1f;

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

            BackdropViewGroup.Opacity = 0f;

            if (_isGestureEnabled)
                EnableGesture();

            SendToggled();
        }

        protected override void ConfigureLayout()
        {
            base.ConfigureLayout();

            if (DrawerBehavior == DrawerBehavior.Drawer)
            {
                if (IsGestureEnabled)
                {
                    _gestureArea?.UpdateBounds(new Rect(0, 0, _defaultGestureAreaWidth, Size.Height));
                    _gestureArea?.Show();
                }
            }
            else if (DrawerBehavior == DrawerBehavior.Locked)
            {
                _gestureArea?.Hide();
            }
            else
            {
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
                BackdropViewGroup.Opacity = CalcBackdropOpacityWithPosition();

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
                    BackdropViewGroup.Show();
                    DrawerViewGroup.UpdatePosition(new Point(_defaultGestureAreaWidth - DrawerWidth / 2, 0));
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
                var x = args.PanGesture.ScreenPosition.X - DrawerWidth;
                DrawerViewGroup.UpdatePosition(new Point(((x < 0) ? x : 0), 0));
                BackdropViewGroup.Opacity = CalcBackdropOpacityWithPosition();
            }
        }

        void UpdateGestureEnabling(bool enabled)
        {
            if (enabled && DrawerBehavior == DrawerBehavior.Drawer)
                EnableGesture();
            else
                DisableGesture();
        }

        void EnableGesture()
        {
            InitGesture();

            _gestureAttatedView = IsOpened ? DrawerViewGroup : _gestureArea;
            _gestureArea?.Show();
            _panGestureDetector?.Attach(_gestureAttatedView);
        }

        void DisableGesture()
        {
            _gestureArea?.Hide();
            _panGestureDetector?.Detach(_gestureAttatedView);
        }

        float CalcBackdropOpacityWithPosition()
        {
            return (float)((DrawerViewGroup.Position.X < 0) ? ((DrawerViewGroup.Position.X / DrawerWidth)) + 1 : 1);
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
