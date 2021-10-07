using System;
using ElmSharp;
using Tizen.UIExtensions.Common.Internal;

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

        int _maximumDistance = ThemeConstants.RefreshLayout.Resources.RefreshDistance;
        int _minimumSize = ThemeConstants.RefreshLayout.Resources.MinimumLayoutSize;
        float _iconSize = ThemeConstants.RefreshLayout.Resources.IconSize;
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

            this.PackEnd(_refreshIcon);
            this.PackEnd(_contentLayout);

            SetLayoutCallback(OnLayoutUpdate);
        }

        /// <summary>
        /// Invokes when refreshing starts by pulling down the content.
        /// </summary>
        public event EventHandler? Refreshing;

        public RefreshIcon RefreshIcon
        {
            get
            {
                return _refreshIcon;
            }
            set
            {
                _refreshIcon = value;
            }
        }

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
        public Common.Color RefreshIconColor
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
            _maximumDistance = _maximumDistance + Geometry.Y;

            var iconGeometryX = Geometry.X + (Geometry.Width - _refreshIcon.Geometry.Width) / 2;
            var iconBottomPadding = (int)_iconSize / 2;
            _initialIconGeometryY = Geometry.Y - (_refreshIcon.Geometry.Height+ iconBottomPadding);
            var iconHeight = _refreshIcon.Geometry.Height + iconBottomPadding;
            _refreshIcon.Geometry = new Rect(iconGeometryX, _initialIconGeometryY, _refreshIcon.Geometry.Width, iconHeight);
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
                if (IsEdgeScrolling())
                {
                    _refreshState = RefreshState.Drag;
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

        bool IsEdgeScrolling()
        {
            if (_content is ScrollView scrollView)
            {
                if (scrollView.ScrollBound.Y != 0)
                {
                    return false;
                }
            }
            return true;
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
        }

        void BeginRefreshing(bool isPulledRefresh)
        {
            var movedDistance = GetMovedDistance();
            var refreshDistance = _maximumDistance - Geometry.Y + _refreshIcon.Geometry.Height;
            var contentDistanceDiff = refreshDistance - movedDistance;
            var _refreshStartAnimation = new Animation(v => _contentLayout.Move(Geometry.X, Geometry.Y + movedDistance + (int)v), 0, contentDistanceDiff, Easing.Linear);
            _refreshStartAnimation.Commit(this, "RefreshBegin", length: _animationLength);

            if (!isPulledRefresh)
            {
                var currentIconGeometryY = Geometry.Y + _refreshIcon.Geometry.Y;
                var iconDistanceDiff = _maximumDistance - currentIconGeometryY;
                var _refreshIconBeginAnimation = new Animation(v => _refreshIcon.Move(_refreshIcon.Geometry.X, currentIconGeometryY + (int)v), 0, iconDistanceDiff, Easing.Linear);
                _refreshIconBeginAnimation.Commit(this, "RefreshIconBegin", length: _animationLength);
            }

            _refreshState = RefreshState.Loading;
            _isRefreshing = true;
            _refreshIcon.IsRunning = true;
            Refreshing?.Invoke(this, EventArgs.Empty);
        }

        void ResetRefreshing()
        {
            var movedDistance = GetMovedDistance();
            var _refreshResetAnimation = new Animation(v => _contentLayout.Move(Geometry.X, Geometry.Y + movedDistance - (int)v), 0, movedDistance, Easing.Linear);
            _refreshResetAnimation.Commit(this, "RefreshReset", length: _animationLength);

            var currentIconGeometryY = _refreshIcon.Geometry.Y;
            var _refreshIconResetAnimation = new Animation(v => _refreshIcon.Move(_refreshIcon.Geometry.X, currentIconGeometryY - (int)v), 0, movedDistance, Easing.Linear);
            _refreshIconResetAnimation.Commit(this, "RefreshIconReset", length: _animationLength);

            _refreshState = RefreshState.Idle;
            _refreshIcon.IsRunning = false;
        }

        int GetMovedDistance()
        {
            return _contentLayout.Geometry.Y - Geometry.Y;
        }

        void MoveLayout(int distance)
        {
            var iconDistance = _initialIconGeometryY + distance;
            if (iconDistance > _maximumDistance)
            {
                iconDistance = _maximumDistance;
                _shouldRefresh = true;
            }
            else
            {
                _shouldRefresh = false;
            }
            _refreshIcon.Move(_refreshIcon.Geometry.X, iconDistance);
            _contentLayout.Move(_contentLayout.Geometry.X, Geometry.Y + distance);
        }

        void IAnimatable.BatchBegin() {}

        void IAnimatable.BatchCommit() {}
    }
}
