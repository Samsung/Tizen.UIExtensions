using System;
using ElmSharp;
using Tizen.UIExtensions.Common.Internal;
using Tizen.UIExtensions.ElmSharp.GraphicsView;

namespace Tizen.UIExtensions.ElmSharp
{
    enum RefreshState
    {
        Idle,
        Drag,
        Loading,
    }

    /// <summary>
    /// RefreshLayout is a container that holds a single content and allows you to pull down to refresh content with showing refresh animation.
    /// </summary>
    public class RefreshLayout : Box, IAnimatable
    {
        RefreshIcon _refreshIcon;
        Box _contentLayout;
        EvasObject? _content;
        GestureLayer _gestureLayer;
        RefreshState _refreshState;
        bool _shouldRefresh;
        bool _isRefreshing;
        bool _isRefreshEnabled;
        int _initialIconGeometryY;
        int _maximumDistance;
        float _iconSize;

        int _minimumSize = ThemeConstants.RefreshLayout.Resources.MinimumLayoutSize;
        uint _animationLength = ThemeConstants.RefreshLayout.Resources.RefreshAnimationLength;

        /// <summary>
        /// Initializes a new instance of the RefreshLayout class.
        /// </summary>
        /// <param name="parent"></param>
        public RefreshLayout(EvasObject parent) : base(parent)
        {
            MinimumHeight = _minimumSize;
            _refreshState = RefreshState.Idle;
            _refreshIcon = new RefreshIcon(parent);
            _contentLayout = new Box(parent);
            _refreshIcon.Show();
            _contentLayout.Show();

            _gestureLayer = new GestureLayer(parent);
            _gestureLayer.Attach(_contentLayout);
            _gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Move, OnMoved);
            _gestureLayer.SetMomentumCallback(GestureLayer.GestureState.End, OnEnded);
            _gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Abort, OnEnded);

            this.PackEnd(_contentLayout);
            this.PackEnd(_refreshIcon);

            SetLayoutCallback(OnLayoutUpdate);
        }

        /// <summary>
        /// Invokes when refreshing starts by pulling down the content.
        /// </summary>
        public event EventHandler? Refreshing;

        /// <summary>
        /// Gets or sets the delegate to decide if the RefreshLayout is at the edge of scrolling when it has scrolling contents.
        /// </summary>
        public Func<bool>? IsEdgeScrolling { get; set; }

        /// <summary>
        /// Gets or sets if the RefreshLayout is in its refresh state.
        /// </summary>
        public bool IsRefreshEnabled
        {
            get
            {
                return _isRefreshEnabled;
            }
            set
            {
                _isRefreshEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the refreshing status of RefreshLayout.
        /// Refresh animation starts when it is set to true.
        /// </summary>
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    if (_isRefreshing)
                    {
                        BeginRefreshing(false);
                    }
                    else
                    {
                        ResetRefreshing();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the refresh icon.
        /// </summary>
        public Common.Color IconColor
        {
            get
            {
                return _refreshIcon.Color;
            }
            set
            {
                _refreshIcon.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color of the refresh icon.
        /// </summary>
        public Common.Color IconBackgroundColor
        {
            get
            {
                return _refreshIcon.BackgroundColor;
            }
            set
            {
                _refreshIcon.BackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the content of the RefreshLayout.
        /// </summary>
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
                    _contentLayout.UnPackAll();
                }
                _content = value;
                _contentLayout.PackEnd(_content);
            }
        }

        void OnLayoutUpdate()
        {
            _iconSize = _refreshIcon.Geometry.Height;
            _maximumDistance = (int)_iconSize + Geometry.Y;

            var iconGeometryX = Geometry.X + (Geometry.Width - _refreshIcon.Geometry.Width) / 2;
            _initialIconGeometryY = Geometry.Y - (int)_iconSize;
            _refreshIcon.Geometry = new Rect(iconGeometryX, _initialIconGeometryY, _refreshIcon.Geometry.Width, _refreshIcon.Geometry.Height);
            _contentLayout.Geometry = Geometry;
            if (_content != null)
            {
                _content.Geometry = Geometry;
            }
        }

        void OnMoved(GestureLayer.MomentumData moment)
        {

            if (_refreshState == RefreshState.Idle && _isRefreshEnabled)
            {
                var isEdge = IsEdgeScrolling?.Invoke() ?? true;
                if (isEdge)
                {
                    _refreshState = RefreshState.Drag;
                    _refreshIcon.IsPulling = true;
                }
            }

            if (_refreshState == RefreshState.Drag)
            {
                var dy = moment.Y2 - moment.Y1;
                if (dy < 0)
                    return;
                MoveLayout(dy);
            }
        }

        void OnEnded(GestureLayer.MomentumData moment)
        {
            if (_refreshState != RefreshState.Drag)
            {
                return;
            }

            if (_shouldRefresh)
            {
                BeginRefreshing(true);
                _shouldRefresh = false;
            }
            else
            {
                ResetRefreshing();
            }
            _refreshIcon.IsPulling = false;
        }

        void BeginRefreshing(bool isPulledRefresh)
        {
            var currentIconGeometryY = _refreshIcon.Geometry.Y;
            var contentDistanceDiff = _maximumDistance - currentIconGeometryY;
            var _refreshIconBeginAnimation = new Animation(v => _refreshIcon.Move(_refreshIcon.Geometry.X, currentIconGeometryY + (int)v), 0, contentDistanceDiff, Easing.Linear);
            _refreshIconBeginAnimation.Commit(this, "RefreshIconBegin", length: _animationLength);

            _refreshState = RefreshState.Loading;
            _isRefreshing = true;
            _refreshIcon.IsRunning = true;
            Refreshing?.Invoke(this, EventArgs.Empty);
        }

        void ResetRefreshing()
        {
            var movedDistance = _refreshIcon.Geometry.Y - _initialIconGeometryY;
            var currentIconGeometryY = _refreshIcon.Geometry.Y;
            var _refreshIconResetAnimation = new Animation(v => _refreshIcon.Move(_refreshIcon.Geometry.X, currentIconGeometryY - (int)v), 0, movedDistance, Easing.Linear);
            _refreshIconResetAnimation.Commit(this, "RefreshIconReset", length: _animationLength);

            _refreshState = RefreshState.Idle;
            _refreshIcon.IsRunning = false;
        }

        void MoveLayout(int distance)
        {
            var iconDistance = _initialIconGeometryY + distance;
            if (iconDistance > _maximumDistance)
            {
                _shouldRefresh = true;
            }
            else
            {
                _shouldRefresh = false;
            }
            _refreshIcon.Move(_refreshIcon.Geometry.X, iconDistance);
            var totalDistance = Math.Abs(_initialIconGeometryY) + _maximumDistance - Geometry.Y;
            var pullDistance = (float)distance / totalDistance;
            _refreshIcon.PullDistance = pullDistance >= 1f ? 1f : pullDistance;
        }

        void IAnimatable.BatchBegin() {}

        void IAnimatable.BatchCommit() {}
    }
}
