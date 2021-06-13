using ElmSharp;
using System;
using System.Collections.Generic;
using EBox = Tizen.UIExtensions.ElmSharp.Box;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The native box that provides CarouselPage features.
    /// </summary>
    public class CarouselPage : EBox
    {
        const int ItemMaxCount = 20;

        Index _index;
        List<IndexItem> _items = new List<IndexItem>();

        int _childCount = 0;

        EBox _innerContainer;
        Scroller _scroller;

        Size _layoutBound;

        List<EvasObject> _childrenList = new List<EvasObject>();

        /// <summary>
        /// Initializes a new instance of the CarouselPage class.
        /// </summary>
        public CarouselPage(EvasObject parent) : base(parent)
        {
            AlignmentX = -1;
            AlignmentY = -1;
            WeightX = 1;
            WeightY = 1;
            Show();

            _scroller = new Scroller(parent)
            {
                HorizontalScrollBarVisiblePolicy = ScrollBarVisiblePolicy.Invisible,
                VerticalScrollBarVisiblePolicy = ScrollBarVisiblePolicy.Invisible,
                HorizontalPageScrollLimit = 1,
                HorizontalRelativePageSize = 1.0,
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            _scroller.PageScrolled += OnPageScrolled;
            _scroller.Show();

            _innerContainer = new EBox(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            _innerContainer.SetLayoutCallback(OnInnerLayoutUpdate);
            _innerContainer.Show();
            _scroller.SetContent(_innerContainer);

            _index = new Index(parent)
            {
                IsHorizontal = true,
                AutoHide = false,
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            _index.Changed += OnIndexChanged;
            _index.Show();

            SetLayoutCallback(OnOutterLayoutUpdate);
            PackEnd(_scroller);
            PackEnd(_index);
        }

        /// <summary>
        /// Raised when the page has been scrolled.
        /// </summary>
        public event EventHandler<PageScrolledEventArgs>? PageScrolled;

        /// <summary>
        /// The current index of the displayed page.
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// Add a child box to the CarouselPage.
        /// </summary>
        public void AddChildrenList(EvasObject box)
        {
            _childrenList.Add(box);
            _innerContainer.PackEnd(box);
        }

        /// <summary>
        /// Resets the child box list.
        /// </summary>
        public void ResetChildrenList()
        {
            _innerContainer.UnPackAll();
            _childrenList.Clear();
            _layoutBound = new Size(0, 0);
        }

        /// <summary>
        /// Scroll to pages.
        /// </summary>
        /// <param name="index">The index of horizontal page to scroll.</param>
        /// <param name="animated">Whether or not to animate the scroll.</param>
        public void ScrollTo(int index, bool animated)
        {
            _scroller.ScrollTo(index, 0, animated);
        }

        /// <summary>
        /// Updates the index item.
        /// </summary>
        public void UpdateIndexItem()
        {
            _index.SetStyledIndex();
            _index.Clear();
            _items.Clear();

            var indexCount = _childrenList.Count;
            if (indexCount > ItemMaxCount)
                indexCount = ItemMaxCount;
            for (int i = 0; i < indexCount; i++)
            {
                var item = _index.Append(i.ToString());
                _items.Add(item);
            }
            _index.Update(0);
            OnSelect(CurrentPageIndex);
        }

        void OnSelect(int selectIndex)
        {
            if (selectIndex >= ItemMaxCount)
                selectIndex = ItemMaxCount - 1;
            if (selectIndex > -1)
                _items[selectIndex].Select(true);
        }

        void OnIndexChanged(object sender, EventArgs e)
        {
            var changedIndex = _items.IndexOf(_index.SelectedItem);
            if (changedIndex != CurrentPageIndex)
                ScrollTo(changedIndex, true);
        }

        void OnInnerLayoutUpdate()
        {
            if (_layoutBound == _innerContainer.Geometry.Size && _childCount == _childrenList.Count)
                return;

            _layoutBound = _innerContainer.Geometry.Size;
            _childCount = _childrenList.Count;

            int baseX = _innerContainer.Geometry.X;
            Rect bound = _scroller.Geometry;
            int index = 0;

            foreach (var nativeView in _childrenList)
            {
                bound.X = baseX + index * bound.Width;
                nativeView.Geometry = bound;
                index++;
            }

            var widthRequest = _childCount * bound.Width;
            _innerContainer.MinimumWidth = widthRequest;
            if (_innerContainer.Geometry.Width == widthRequest && _scroller.HorizontalPageIndex != CurrentPageIndex)
                ScrollTo(CurrentPageIndex, true);
        }

        void OnOutterLayoutUpdate()
        {
            _scroller.Geometry = Geometry;
            var newGeometry = Geometry;
            newGeometry.Height = (int)(Geometry.Height * 0.1);
            newGeometry.Y += (int)(Geometry.Height * 0.9);
            _index.Geometry = newGeometry;
        }

        void OnPageScrolled(object sender, EventArgs e)
        {
            int previousPageIndex = CurrentPageIndex;
            CurrentPageIndex = _scroller.HorizontalPageIndex;
            if (previousPageIndex != CurrentPageIndex)
            {
                PageScrolled?.Invoke(this, new PageScrolledEventArgs(previousPageIndex, CurrentPageIndex));
                OnSelect(CurrentPageIndex);
            }
        }
    }
}