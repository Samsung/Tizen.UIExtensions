using System.Threading.Tasks;
using ElmSharp;
using Tizen.UIExtensions.Common;
using EScroller = ElmSharp.Scroller;
using CRect = Tizen.UIExtensions.Common.Rect;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The ScrollView is a container that holds and clips a single object and allows you to scroll across it.
    /// </summary>
    public class ScrollView : EScroller
    {
        TaskCompletionSource<bool>? _animationTaskComplateSource;
        bool _isAnimation = false;
        EvasObject? _scrollCanvas;

        /// <summary>
        /// Creates and initializes a new instance of the ScrollView class.
        /// </summary>
        /// <param name="parent">The <see cref="EvasObject"/> to which the new Scroller will be attached as a child.</param>
        public ScrollView(EvasObject parent) : base(parent)
        {
        }


        /// <summary>
        /// Creates and initializes a new instance of the ScrollView class.
        /// </summary>
        protected ScrollView()
        {
        }

        /// <summary>
        /// Gets or sets the scrolling direction of the ScrollView.
        /// </summary>
        public ScrollOrientation ScrollOrientation
        {
            get => ScrollBlock.ToCommon();
            set => ScrollBlock = value.ToNative();
        }

        /// <summary>
        /// Gets or sets a value that controls when the vertical scroll bar is visible.
        /// </summary>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get => VerticalScrollBarVisiblePolicy.ToCommon();
            set => VerticalScrollBarVisiblePolicy = value.ToNative();
        }

        /// <summary>
        /// Gets or sets a value that controls when the horizontal scroll bar is visible.
        /// </summary>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get => HorizontalScrollBarVisiblePolicy.ToCommon();
            set => HorizontalScrollBarVisiblePolicy = value.ToNative();
        }

        /// <summary>
        /// Gets the current scroll bound.
        /// </summary>
        public CRect ScrollBound
        {
            get => CurrentRegion.ToCommon();
        }

        /// <summary>
        /// Set a content to display on scroll view
        /// </summary>
        /// <param name="content"></param>
        public void SetScrollCanvas(EvasObject content)
        {
            _scrollCanvas = content;
            SetContent(content);
        }


        /// <summary>
        /// Sets the size of the Content.
        /// </summary>
        /// <param name="size">A Size that represents the size of the content.</param>
        public void SetContentSize(int width, int height)
        {
            if (_scrollCanvas != null)
            {
                _scrollCanvas.MinimumWidth = width;
                _scrollCanvas.MinimumHeight = height;
            }
        }

        /// <summary>
        /// Scroll to page index
        /// </summary>
        /// <param name="horizontalPageIndex">The index of horizontal page to scroll</param>
        /// <param name="verticalPageIndex">The index of vertical page to scroll</param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public Task ScrollToAsync(int horizontalPageIndex, int verticalPageIndex, bool animated)
        {
            CheckTaskCompletionSource();
            ScrollTo(horizontalPageIndex, verticalPageIndex, animated);
            return animated && _isAnimation && _animationTaskComplateSource != null ? _animationTaskComplateSource.Task : Task.CompletedTask;
        }

        /// <summary>
        /// Scroll to position
        /// </summary>
        /// <param name="rect">The area of the finished scroll</param>
        /// <param name="animated">Whether or not to animate the scroll.</param>
        /// <returns>Returns a task that scrolls the scroll view to a position asynchronously.</returns>
        public Task ScrollToAsync(CRect rect, bool animated)
        {
            CheckTaskCompletionSource();
            ScrollTo(rect.ToNative(), animated);
            return animated && _isAnimation && _animationTaskComplateSource != null ? _animationTaskComplateSource.Task : Task.CompletedTask;
        }

        protected override void OnRealized()
        {
            base.OnRealized();
            new SmartEvent(this, RealHandle, ThemeConstants.Scroller.Signals.StartScrollAnimation).On += (s, e) => _isAnimation = true;
            new SmartEvent(this, RealHandle, ThemeConstants.Scroller.Signals.StopScrollAnimation).On += (s, e) =>
            {
                _animationTaskComplateSource?.TrySetResult(true);
                _isAnimation = false;
            };
        }

        void CheckTaskCompletionSource()
        {
            if (_animationTaskComplateSource != null)
            {
                if (_animationTaskComplateSource.Task.Status == TaskStatus.Running)
                {
                    _animationTaskComplateSource.TrySetCanceled();
                }
            }
            _animationTaskComplateSource = new TaskCompletionSource<bool>();
        }
    }
}