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
        double _defaultGestureAreaWidth = 20;

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
                UpdateGestureEnabling();
            }
        }

        protected override void ConfigureLayout()
        {
            base.ConfigureLayout();

            if (DrawerBehavior == DrawerBehavior.Drawer)
            {
                if (IsGestureEnabled)
                {
                    _gestureArea?.UpdateBounds(new Rect(0, 0, _defaultGestureAreaWidth.ToPixel(), Size.Height));

                    if (IsOpened)
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
                UpdateBackdropOpacityByPosition();

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

                _gestureArea.TouchEvent += (s, e) =>
                {
                    return true;
                };

                Children.Add(_gestureArea);
                _gestureArea.LowerBelow(DrawerViewGroup);
            }

            if (_panGestureDetector == null)
            {
                _panGestureDetector = new PanGestureDetector();
                _panGestureDetector.Detected += OnGestureDetected;
            }
        }

        async void OnGestureDetected(object sender, PanGestureDetector.DetectedEventArgs args)
        {
            if (DrawerBehavior != DrawerBehavior.Drawer)
                return;

            if (args.PanGesture.State == Gesture.StateType.Started)
            {
                if (!IsOpened)
                {
                    DrawerViewGroup.Show();
                    BackdropViewGroup.Show();
                    DrawerViewGroup.UpdatePosition(new Point(_defaultGestureAreaWidth.ToPixel() / 2 - DrawerWidth , 0));
                }
            }
            else if (args.PanGesture.State == Gesture.StateType.Finished || args.PanGesture.State == Gesture.StateType.Cancelled)
            {
                if (DrawerViewGroup.Position.X > (DrawerWidth / 2) * -1)
                    await OpenDrawer(true);
                else
                    await CloseDrawer(true);
            }
            else
            {
                UpdateDrawerPositionByGesture(args.PanGesture);
                UpdateBackdropOpacityByPosition();
            }
        }

        protected override void UpdateGestureEnabling()
        {
            if (_isGestureEnabled && DrawerBehavior == DrawerBehavior.Drawer)
                EnableGesture();
            else
                DisableGesture();
        }

        protected override void UpdateBackdrop()
        {
            if (IsOpened)
                BackdropViewGroup.Opacity = 1f;
            else
                BackdropViewGroup.Opacity = 0f;
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

        void UpdateDrawerPositionByGesture(PanGesture gesture)
        {
            var x = DrawerViewGroup.Position.X + gesture.Displacement.X;
            DrawerViewGroup.UpdatePosition(new Point(((x < 0) ? x : 0), 0));
        }

        void UpdateBackdropOpacityByPosition()
        {
            var opacity = (DrawerViewGroup.Position.X < 0) ? 1 + (DrawerViewGroup.Position.X / DrawerWidth): 1;
            BackdropViewGroup.Opacity = (float)opacity;
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
