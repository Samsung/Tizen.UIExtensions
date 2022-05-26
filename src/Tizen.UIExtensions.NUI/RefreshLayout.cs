using System;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;
using Tizen.NUI.Components;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.NUI.GraphicsView;
using Color = Tizen.UIExtensions.Common.Color;

namespace Tizen.UIExtensions.NUI
{

    /// <summary>
    /// RefreshLayout is a container that holds a single content and allows you to pull down to refresh content with showing refresh animation.
    /// </summary>
    public class RefreshLayout : ViewGroup
    {
        // pulling distance to refresh in DP unit
        static float s_thresholdDistanceInDP = 70;

        View? _content;
        View _overlayArea;
        RefreshIcon _refreshIcon;

        PanGestureDetector _panGestureDetector;
        RefreshState _refreshState;
        float _iconDistance = 0;

        /// <summary>
        /// Initializes a new instance of the RefreshLayout class.
        /// </summary>
        public RefreshLayout()
        {
            _overlayArea = new View
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            Add(_overlayArea);

            _refreshIcon = new RefreshIcon
            {
                Opacity = 0
            };
            _overlayArea.Add(_refreshIcon);

            _panGestureDetector = new PanGestureDetector();
            _panGestureDetector.Attach(this);
            _panGestureDetector.Detected += OnDetected;

            LayoutUpdated += OnLayout;

            _refreshState = RefreshState.Idle;
        }

        /// <summary>
        /// Invokes when refreshing starts by pulling down the content.
        /// </summary>
        public event EventHandler? Refreshing;


        /// <summary>
        /// Gets or sets the refreshing status of RefreshLayout.
        /// Refresh animation starts when it is set to true.
        /// </summary>
        public bool IsRefreshing
        {
            get => _refreshState == RefreshState.Refresh;
            set
            {
                if (value)
                {
                    RequestRefresh();
                }
                else
                {
                    CompleteRefresh();
                }
            }
        }


        /// <summary>
        /// Gets or sets the background color of the refresh icon.
        /// </summary>
        public Color IconBackgroundColor
        {
            get => _refreshIcon.BackgroundColor;
            set => _refreshIcon.BackgroundColor = value;
        }

        /// <summary>
        /// Gets or sets the color of the refresh icon.
        /// </summary>
        public Color IconColor
        {
            get => _refreshIcon.Color;
            set => _refreshIcon.Color = value;
        }

        /// <summary>
        /// Gets or sets the content of the RefreshLayout.
        /// </summary>
        public View? Content
        {
            get => _content;

            set
            {
                if (_content == value)
                    return;

                var old = _content;
                _content = value;
                ResetContent(old);
                SetupContent(_content);
            }
        }

        ScrollableBase? ScrollContent { get; set; }

        float ThresholdDistance => (float)(s_thresholdDistanceInDP * DeviceInfo.ScalingFactor);

        float IconDistance
        {
            get => _iconDistance;
            set
            {
                _iconDistance = value;
                _refreshIcon.PositionY = _iconDistance;
                _refreshIcon.PullDistance = Math.Min(_iconDistance / ThresholdDistance, 1);
            }
        }

        protected override void OnEnabled(bool enabled)
        {
            base.OnEnabled(enabled);
            if (!IsEnabled)
            {
                CompleteRefresh();
            }
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            if (child is View view)
            {
                if (view != _overlayArea && Content == null)
                    Content = view;
            }
        }

        protected override void OnChildRemoved(Element child)
        {
            base.OnChildRemoved(child);
            if (child is View view)
            {
                if (view == Content)
                {
                    Content = null;
                }
            }
        }

        void RequestRefresh()
        {
            if (_refreshState == RefreshState.Idle)
            {
                IconDistance = ThresholdDistance;
                _refreshIcon.AnimationTo(nameof(_refreshIcon.Opacity), 1, 100);
                StartRefresh();
            }
            else if (_refreshState == RefreshState.Pulling)
            {
                StartRefresh();
            }
        }
        void StartRefresh()
        {
            _refreshState = RefreshState.Refresh;
            _refreshIcon.IsRunning = true;
            _refreshIcon.IsPulling = false;
            _refreshIcon.AnimationTo(nameof(_refreshIcon.PositionY), ThresholdDistance, 100);

            Refreshing?.Invoke(this, EventArgs.Empty);
        }

        async void CompleteRefresh()
        {
            if (_refreshState != RefreshState.Refresh)
                return;

            _refreshIcon.PullDistance = 0;
            _refreshIcon.IsRunning = false;
            await _refreshIcon.AnimationTo(nameof(_refreshIcon.Opacity), 0, 100);
            _refreshState = RefreshState.Idle;
            IconDistance = 0;
        }

        void HandleMoveIcon(float displacementY)
        {
            float maximumDistance = ThresholdDistance * 1.5f;
            IconDistance = Math.Max(0, Math.Min(maximumDistance, IconDistance + displacementY));
        }

        void HandlePullingFinish()
        {
            if (IconDistance >= ThresholdDistance)
            {
                StartRefresh();
            }
            else
            {
                ResetPulling();
            }
        }

        void HandlePullingStart()
        {
            _refreshState = RefreshState.Pulling;
            _refreshIcon.IsPulling = true;
            IconDistance = 0;
            _ = _refreshIcon.AnimationTo(nameof(_refreshIcon.Opacity), 1, 100);
        }

        async void ResetPulling()
        {
            _refreshState = RefreshState.Idle;
            _refreshIcon.IsPulling = false;
            await _refreshIcon.AnimationTo(nameof(_refreshIcon.PositionY), 0, 100);
            await _refreshIcon.AnimationTo(nameof(_refreshIcon.Opacity), 0, 100);
        }

        bool IsTopEdge()
        {
            if (ScrollContent == null)
                return false;

            return ScrollContent.ContentContainer.PositionY == 0;
        }

        void SetupContent(View? view)
        {
            if (view == null)
                return;

            if (!Children.Contains(view))
            {
                Children.Add(view);
            }

            view.LowerBelow(_overlayArea);
            ScrollContent = FindScrollContent(view);
        }

        void ResetContent(View? view)
        {
            if (view == null)
                return;

            if (Children.Contains(view))
            {
                Children.Remove(view);
            }

            ScrollContent = null;
        }

        void OnLayout(object? sender, LayoutEventArgs e)
        {
            var bounds = new Rect(0, 0, SizeWidth, SizeHeight);

            _overlayArea.UpdateBounds(bounds);
            _content?.UpdateBounds(bounds);
            var iconMeasured = _refreshIcon.Measure(bounds.Width, bounds.Height);
            var iconBounds = new Rect(bounds.Width / 2f - iconMeasured.Width / 2f, _iconDistance, iconMeasured.Width, iconMeasured.Height);
            _refreshIcon.UpdateBounds(iconBounds);
        }

        void OnDetected(object source, PanGestureDetector.DetectedEventArgs e)
        {
            e.Handled = false;

            if (!IsEnabled)
                return;

            if (_refreshState == RefreshState.Idle && e.PanGesture.State == Gesture.StateType.Started && IsTopEdge())
            {
                HandlePullingStart();
            }
            else if (_refreshState == RefreshState.Pulling && e.PanGesture.State == Gesture.StateType.Continuing)
            {
                HandleMoveIcon(e.PanGesture.Displacement.Y);
            }
            else if (_refreshState == RefreshState.Pulling && e.PanGesture.State == Gesture.StateType.Finished)
            {
                HandlePullingFinish();
            }
        }

        static ScrollableBase? FindScrollContent(View view)
        {
            if (view is ScrollableBase)
                return (ScrollableBase)view;

            return FindScrollContentBFS(view.Children);
        }

        static ScrollableBase? FindScrollContentBFS(List<View> queue)
        {
            if (queue.Count == 0)
                return null;

            List<View> children = new List<View>();
            foreach (var child in queue)
            {
                if (child is ScrollableBase scrollview)
                {
                    return scrollview;
                }
                children.AddRange(child.Children);
            }
            return FindScrollContentBFS(children);
        }

        enum RefreshState
        {
            Idle,
            Pulling,
            Refresh,
        }
    }
}
