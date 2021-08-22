using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ElmSharp;
using EBox = ElmSharp.Box;
using EPoint = ElmSharp.Point;
using ERect = ElmSharp.Rect;
using EScroller = ElmSharp.Scroller;
using ESize = ElmSharp.Size;
using ScrollToPosition = Tizen.UIExtensions.Common.ScrollToPosition;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The CollectionView is a flexible and performant view for presenting lists of data using different layout specifications.
    /// </summary>
    public class CollectionView : EBox, ICollectionViewController
    {
        RecyclerPool _pool = new RecyclerPool();
        ICollectionViewLayoutManager? _layoutManager;
        ItemAdaptor? _adaptor;
        EBox _innerLayout;
        EvasObject? _emptyView;

        Dictionary<ViewHolder, int> _viewHolderIndexTable = new Dictionary<ViewHolder, int>();
        ViewHolder? _lastSelectedViewHolder;
        int _selectedItemIndex = -1;
        CollectionViewSelectionMode _selectionMode = CollectionViewSelectionMode.None;

        bool _requestLayoutItems = false;
        SnapPointsType _snapPoints;
        ESize _itemSize = new ESize(-1, -1);

        EvasObject? _headerView;
        EvasObject? _footerView;
        SmartEvent _scrollAnimationStop;
        SmartEvent _scrollAnimationStart;
        bool _isScrollAnimationStarted;
        bool _allowFocusOnItem;

        /// <summary>
        /// Event that is raised after a scroll completes.
        /// </summary>
        public event EventHandler<ItemsViewScrolledEventArgs> Scrolled;

        /// <summary>
        /// Initializes a new instance of the CarouselView class.
        /// </summary>
#pragma warning disable CS8618
        public CollectionView(EvasObject parent) : base(parent)
#pragma warning restore CS8618
        {
            AllowFocus(true);
            SetLayoutCallback(OnLayout);
            Scroller = new EScroller(parent);
            Scroller.Show();
            PackEnd(Scroller);
            Scroller.Scrolled += OnScrolled;

            _scrollAnimationStart = new SmartEvent(Scroller, ThemeConstants.Scroller.Signals.StartScrollAnimation);
            _scrollAnimationStart.On += OnScrollStarted;

            _scrollAnimationStop = new SmartEvent(Scroller, ThemeConstants.Scroller.Signals.StopScrollAnimation);
            _scrollAnimationStop.On += OnScrollStopped;

            Scroller.DragStart += OnDragStart;
            Scroller.KeyDown += OnKeyDown;

            _innerLayout = new EBox(parent);
            _innerLayout.SetLayoutCallback(OnInnerLayout);
            _innerLayout.Show();
            Scroller.SetContent(_innerLayout);
        }

        /// <summary>
        /// Gets or sets a value that controls whether and how many items can be selected.
        /// </summary>
        public CollectionViewSelectionMode SelectionMode
        {
            get => _selectionMode;
            set
            {
                _selectionMode = value;
                UpdateSelectionMode();
            }
        }

        /// <summary>
        /// Gets or sets the selected item index.
        /// </summary>
        public int SelectedItemIndex
        {
            get => _selectedItemIndex;
            set
            {
                if (_selectedItemIndex != value)
                {
                    _selectedItemIndex = value;
                    UpdateSelectedItemIndex();
                }
            }
        }

        /// <summary>
        /// Gets or sets the SnapPointsType value.
        /// </summary>
        public SnapPointsType SnapPointsType
        {
            get => _snapPoints;
            set
            {
                _snapPoints = value;
                UpdateSnapPointsType(_snapPoints);
            }
        }

        protected EScroller Scroller { get; }

        /// <summary>
        /// Gets or sets LayoutManager to organize position of item
        /// </summary>
        public ICollectionViewLayoutManager? LayoutManager
        {
            get => _layoutManager;
            set
            {
                OnLayoutManagerChanging();
                _layoutManager = value;
                OnLayoutManagerChanged();
            }
        }

        /// <summary>
        /// Gets or sets ItemAdaptor to adapt items source
        /// </summary>
        public ItemAdaptor? Adaptor
        {
            get => _adaptor;
            set
            {
                OnAdaptorChanging();
                _adaptor = value;
                OnAdaptorChanged();
            }
        }

        /// <summary>
        /// Gets or sets the vertical scrollbar visible policy.
        /// </summary>
        public ScrollBarVisiblePolicy VerticalScrollBarVisiblePolicy
        {
            get => Scroller.VerticalScrollBarVisiblePolicy;
            set => Scroller.VerticalScrollBarVisiblePolicy = value;
        }

        /// <summary>
        /// Gets or sets the horizontal scrollbar visible policy.
        /// </summary>
        public ScrollBarVisiblePolicy HorizontalScrollBarVisiblePolicy
        {
            get => Scroller.HorizontalScrollBarVisiblePolicy;
            set => Scroller.HorizontalScrollBarVisiblePolicy = value;
        }

        /// <summary>
        /// Gets or sets the focused item scroll position.
        /// </summary>
        public ScrollToPosition FocusedItemScrollPosition { get; set; }

        int ICollectionViewController.Count
        {
            get
            {
                if (Adaptor == null || Adaptor is IEmptyAdaptor)
                    return 0;
                return Adaptor.Count;
            }
        }

        EPoint ICollectionViewController.ParentPosition => new EPoint
        {
            X = Scroller.Geometry.X - Scroller.CurrentRegion.X,
            Y = Scroller.Geometry.Y - Scroller.CurrentRegion.Y
        };

        protected ESize AllocatedSize { get; set; }

        ERect ViewPort => Scroller.CurrentRegion;

        /// <summary>
        /// Scrolls the CollectionView to the index
        /// </summary>
        /// <param name="index">Index of item</param>
        /// <param name="position">How the item should be positioned on screen.</param>
        /// <param name="animate">Whether or not the scroll should be animated.</param>
        public void ScrollTo(int index, ScrollToPosition position = ScrollToPosition.MakeVisible, bool animate = true)
        {
            if (LayoutManager == null)
                throw new InvalidOperationException("No Layout manager");

            var itemBound = LayoutManager.GetItemBound(index);
            int itemStart;
            int itemEnd;
            int scrollStart;
            int scrollEnd;
            int itemPadding = 0;
            int itemSize;
            int viewportSize;

            if (LayoutManager.IsHorizontal)
            {
                itemStart = itemBound.Left;
                itemEnd = itemBound.Right;
                itemSize = itemBound.Width;
                scrollStart = Scroller.CurrentRegion.Left;
                scrollEnd = Scroller.CurrentRegion.Right;
                viewportSize = AllocatedSize.Width;
            }
            else
            {
                itemStart = itemBound.Top;
                itemEnd = itemBound.Bottom;
                itemSize = itemBound.Height;
                scrollStart = Scroller.CurrentRegion.Top;
                scrollEnd = Scroller.CurrentRegion.Bottom;
                viewportSize = AllocatedSize.Height;
            }

            if (position == ScrollToPosition.MakeVisible)
            {
                if (itemStart < scrollStart)
                {
                    position = ScrollToPosition.Start;
                }
                else if (itemEnd > scrollEnd)
                {
                    position = ScrollToPosition.End;
                }
                else
                {
                    // already visible
                    return;
                }
            }

            if (itemSize < viewportSize)
            {
                switch (position)
                {
                    case ScrollToPosition.Center:
                        itemPadding = (viewportSize - itemSize) / 2;
                        break;
                    case ScrollToPosition.End:
                        itemPadding = (viewportSize - itemSize);
                        break;
                }
                itemSize = viewportSize;
            }

            if (LayoutManager.IsHorizontal)
            {
                itemBound.X -= itemPadding;
                itemBound.Width = itemSize;
            }
            else
            {
                itemBound.Y -= itemPadding;
                itemBound.Height = itemSize;
            }

            Scroller.ScrollTo(itemBound, animate);
        }

        /// <summary>
        /// Scrolls the CollectionView to the item
        /// </summary>
        /// <param name="item">Item to scroll</param>
        /// <param name="position">How the item should be positioned on screen.</param>
        /// <param name="animate">Whether or not the scroll should be animated.</param>
        public void ScrollTo(object item, ScrollToPosition position = ScrollToPosition.MakeVisible, bool animate = true)
        {
            if (Adaptor == null)
                throw new InvalidOperationException("No Adaptor");

            ScrollTo(Adaptor.GetItemIndex(item), position, animate);
        }

        /// <summary>
        /// Notify that item measure result is changed
        /// </summary>
        /// <param name="index"></param>
        public void ItemMeasureInvalidated(int index)
        {
            // If a first item size was updated, need to reset _itemSize
            if (index == 0)
            {
                _itemSize = new ESize(-1, -1);
            }
            LayoutManager?.ItemMeasureInvalidated(index);
        }

        void ICollectionViewController.RequestLayoutItems() => RequestLayoutItems();

        ESize ICollectionViewController.GetItemSize()
        {
            return (this as ICollectionViewController).GetItemSize(LayoutManager!.IsHorizontal ? AllocatedSize.Width * 100 : AllocatedSize.Width, LayoutManager!.IsHorizontal ? AllocatedSize.Height : AllocatedSize.Height * 100);
        }

        ESize ICollectionViewController.GetItemSize(int widthConstraint, int heightConstraint)
        {
            if (Adaptor == null)
            {
                return new ESize(0, 0);
            }

            if (_itemSize.Width > 0 && _itemSize.Height > 0)
            {
                return _itemSize;
            }

            _itemSize = Adaptor.MeasureItem(widthConstraint, heightConstraint);
            _itemSize.Width = Math.Max(_itemSize.Width, 10);
            _itemSize.Height = Math.Max(_itemSize.Height, 10);

            if (_snapPoints != SnapPointsType.None)
            {
                Scroller.SetPageSize(_itemSize.Width, _itemSize.Height);
            }
            return _itemSize;
        }

        ESize ICollectionViewController.GetItemSize(int index, int widthConstraint, int heightConstraint)
        {
            if (Adaptor == null)
            {
                return new ESize(0, 0);
            }
            return Adaptor.MeasureItem(index, widthConstraint, heightConstraint);
        }

        protected virtual ViewHolder CreateViewHolder()
        {
            return new ViewHolder(this);
        }

        ViewHolder ICollectionViewController.RealizeView(int index)
        {
            if (Adaptor == null)
                throw new InvalidOperationException("No Adaptor");

            var holder = _pool.GetRecyclerView(Adaptor.GetViewCategory(index));
            if (holder != null)
            {
                holder.Show();
            }
            else
            {
                var content = Adaptor.CreateNativeView(index, this);
                holder = CreateViewHolder();
                holder.RequestSelected += OnRequestItemSelection;
                holder.StateUpdated += OnItemStateChanged;
                holder.Content = content;
                holder.ViewCategory = Adaptor.GetViewCategory(index);
                _innerLayout.PackEnd(holder);
            }

            holder.AllowItemFocus = _allowFocusOnItem;

            Adaptor.SetBinding(holder.Content!, index);
            _viewHolderIndexTable[holder] = index;
            if (index == SelectedItemIndex)
            {
                OnRequestItemSelection(holder, EventArgs.Empty);
            }
            return holder;
        }

        void OnItemStateChanged(object sender, EventArgs e)
        {
            ViewHolder holder = (ViewHolder)sender;
            if (holder.Content != null)
            {
                Adaptor?.UpdateViewState(holder.Content, holder.State);
            }

            if (holder.State == ViewHolderState.Focused && FocusedItemScrollPosition != ScrollToPosition.MakeVisible)
            {
                //TODO: Adds BeginInvokeOnMainThread later
                EcoreMainloop.AddTimer(TimeSpan.FromTicks(1).TotalSeconds, () =>
                {
                    if (holder.State == ViewHolderState.Focused && _viewHolderIndexTable.TryGetValue(holder, out int itemIndex))
                    {
                        ScrollTo(itemIndex, FocusedItemScrollPosition, true);
                    }
                    return false;
                });
            }
        }

        void OnRequestItemSelection(object sender, EventArgs e)
        {
            if (SelectionMode == CollectionViewSelectionMode.None)
                return;

            if (_lastSelectedViewHolder != null)
            {
                _lastSelectedViewHolder.ResetState();
            }

            _lastSelectedViewHolder = sender as ViewHolder;
            if (_lastSelectedViewHolder != null)
            {
                _lastSelectedViewHolder.State = ViewHolderState.Selected;
                if (_viewHolderIndexTable.TryGetValue(_lastSelectedViewHolder, out int index))
                {
                    _selectedItemIndex = index;
                    Adaptor?.SendItemSelected(index);
                }
            }
        }

        void ICollectionViewController.UnrealizeView(ViewHolder view)
        {
            if (Adaptor == null)
                throw new InvalidOperationException("No Adaptor");

            _viewHolderIndexTable.Remove(view);
            Adaptor.UnBinding(view.Content!);
            view.ResetState();
            view.Hide();

            _pool.AddRecyclerView(view);
            if (_lastSelectedViewHolder == view)
            {
                _lastSelectedViewHolder = null;
            }
        }

        void ICollectionViewController.ContentSizeUpdated()
        {
            OnInnerLayout();
        }

        protected virtual EScroller CreateScroller(EvasObject parent)
        {
            return new EScroller(parent);
        }

        void UpdateSelectedItemIndex()
        {
            if (SelectionMode == CollectionViewSelectionMode.None)
                return;

            ViewHolder? holder = null;
            foreach (var item in _viewHolderIndexTable)
            {
                if (item.Value == SelectedItemIndex)
                {
                    holder = item.Key;
                    break;
                }
            }
            if (holder != null)
            {
                OnRequestItemSelection(holder, EventArgs.Empty);
            }
        }

        void UpdateSelectionMode()
        {
            if (SelectionMode == CollectionViewSelectionMode.None)
            {
                if (_lastSelectedViewHolder != null)
                {
                    _lastSelectedViewHolder.ResetState();
                    _lastSelectedViewHolder = null;
                }
                _selectedItemIndex = -1;
            }
        }

        void OnLayoutManagerChanging()
        {
            _layoutManager?.Reset();
        }

        void OnLayoutManagerChanged()
        {
            if (_layoutManager == null)
                return;

            _itemSize = new ESize(-1, -1);
            _layoutManager.CollectionView = this;
            _layoutManager.SizeAllocated(AllocatedSize);
            UpdateSnapPointsType(SnapPointsType);
            if (Adaptor != null)
            {
                LayoutManager?.SetHeader(_headerView, Adaptor.MeasureHeader(AllocatedSize.Width, AllocatedSize.Height));
                LayoutManager?.SetFooter(_footerView, Adaptor.MeasureFooter(AllocatedSize.Width, AllocatedSize.Height));
            }
            RequestLayoutItems();
        }

        void OnAdaptorChanging()
        {
            if (Adaptor is IEmptyAdaptor)
            {
                RemoveEmptyView();
            }
            else
            {
                if (_headerView != null)
                {
                    _innerLayout.UnPack(_headerView);
                    _headerView.Unrealize();
                }
                if (_footerView != null)
                {
                    _innerLayout.UnPack(_footerView);
                    _footerView.Unrealize();
                }
            }
            _headerView = null;
            _footerView = null;

            _layoutManager?.Reset();
            if (Adaptor != null)
            {
                _pool.Clear(Adaptor);
                (Adaptor as INotifyCollectionChanged).CollectionChanged -= OnCollectionChanged;
                Adaptor.CollectionView = null;
            }
            _innerLayout.UnPackAll();
        }

        void OnAdaptorChanged()
        {
            if (Adaptor == null)
                return;

            _itemSize = new ESize(-1, -1);
            Adaptor.CollectionView = this;
            (Adaptor as INotifyCollectionChanged).CollectionChanged += OnCollectionChanged;

            UpdateSnapPointsType(SnapPointsType);
            LayoutManager?.ItemSourceUpdated();
            RequestLayoutItems();

            if (Adaptor is IEmptyAdaptor)
            {
                CreateEmptyView();
            }
            else
            {
                _headerView = Adaptor.GetHeaderView(this);
                if (_headerView != null)
                {
                    _innerLayout.PackEnd(_headerView);
                }
                _footerView = Adaptor.GetFooterView(this);
                if (_footerView != null)
                {
                    _innerLayout.PackEnd(_footerView);
                }
                LayoutManager?.SetHeader(_headerView, Adaptor.MeasureHeader(AllocatedSize.Width, AllocatedSize.Height));
                LayoutManager?.SetFooter(_footerView, Adaptor.MeasureFooter(AllocatedSize.Width, AllocatedSize.Height));
            }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != Adaptor)
            {
                return;
            }
            // CollectionChanged could be called when Apaptor was changed on CollectionChanged event
            if (Adaptor is IEmptyAdaptor)
            {
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                int idx = e.NewStartingIndex;
                if (idx == -1)
                {
                    idx = Adaptor.Count - e.NewItems.Count;
                }
                foreach (var item in e.NewItems)
                {
                    foreach (var viewHolder in _viewHolderIndexTable.Keys.ToList())
                    {
                        if (_viewHolderIndexTable[viewHolder] >= idx)
                        {
                            _viewHolderIndexTable[viewHolder]++;
                        }
                    }
                    LayoutManager?.ItemInserted(idx++);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                int idx = e.OldStartingIndex;

                // Can't tracking remove if there is no data of old index
                if (idx == -1)
                {
                    LayoutManager?.ItemSourceUpdated();
                }
                else
                {
                    foreach (var item in e.OldItems)
                    {
                        LayoutManager?.ItemRemoved(idx);
                        foreach (var viewHolder in _viewHolderIndexTable.Keys.ToList())
                        {
                            if (_viewHolderIndexTable[viewHolder] > idx)
                            {
                                _viewHolderIndexTable[viewHolder]--;
                            }
                        }
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                LayoutManager?.ItemRemoved(e.OldStartingIndex);
                LayoutManager?.ItemInserted(e.NewStartingIndex);
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                // Can't tracking if there is no information old data
                if (e.OldItems.Count > 1 || e.NewStartingIndex == -1)
                {
                    LayoutManager?.ItemSourceUpdated();
                }
                else
                {
                    LayoutManager?.ItemUpdated(e.NewStartingIndex);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                LayoutManager?.Reset();
                LayoutManager?.ItemSourceUpdated();
            }
            RequestLayoutItems();
        }

        ERect _lastGeometry;
        void OnLayout()
        {
            if (_lastGeometry == Geometry)
            {
                return;
            }

            _lastGeometry = Geometry;
            Scroller.Geometry = Geometry;
            Scroller.ScrollBlock = ScrollBlock.None;
            AllocatedSize = Geometry.Size;
            _itemSize = new ESize(-1, -1);

            if (Adaptor != null && LayoutManager != null)
            {
                LayoutManager.SizeAllocated(Geometry.Size);

                LayoutManager.LayoutItems(ViewPort);
                LayoutManager.SetHeader(_headerView, Adaptor.MeasureHeader(AllocatedSize.Width, AllocatedSize.Height));
                LayoutManager.SetFooter(_footerView, Adaptor.MeasureFooter(AllocatedSize.Width, AllocatedSize.Height));
                Scroller.ScrollBlock = LayoutManager.IsHorizontal ? ScrollBlock.Vertical : ScrollBlock.Horizontal;
                Scroller.HorizontalStepSize = LayoutManager.GetScrollBlockSize();
                Scroller.VerticalStepSize = LayoutManager.GetScrollBlockSize();
                UpdateSnapPointsType(SnapPointsType);
                if (Geometry.Width > 0 && Geometry.Height > 0)
                {
                    EcoreMainloop.Post(() =>
                    {
                        SendScrolledEvent();
                    });
                }
            }
        }

        void RequestLayoutItems()
        {
            if (AllocatedSize.Width <= 0 || AllocatedSize.Height <= 0)
                return;

            if (!_requestLayoutItems)
            {
                _requestLayoutItems = true;

                //TODO: Adds BeginInvokeOnMainThread later
                EcoreMainloop.AddTimer(TimeSpan.FromTicks(1).TotalSeconds, () =>
                {
                    _requestLayoutItems = false;
                    if (_adaptor != null && _layoutManager != null)
                    {
                        OnInnerLayout();
                        _layoutManager?.LayoutItems(ViewPort, true);
                    }
                    return false;
                });
            }
        }

        void OnInnerLayout()
        {
            if (LayoutManager == null)
                return;

            // OnInnerLayout was called when child item was added
            // so, need to check scroll canvas size
            var size = LayoutManager.GetScrollCanvasSize();
            _innerLayout.MinimumWidth = size.Width;
            _innerLayout.MinimumHeight = size.Height;
        }

        int _previousHorizontalOffset = 0;
        int _previousVerticalOffset = 0;

        void OnScrollStarted(object sender, EventArgs e)
        {
            _isScrollAnimationStarted = true;
        }

        void OnScrollStopped(object sender, EventArgs e)
        {
            SendScrolledEvent();
            _isScrollAnimationStarted = false;
        }

        void OnScrolled(object sender, EventArgs e)
        {
            LayoutManager?.LayoutItems(ViewPort);
            if (!_isScrollAnimationStarted)
            {
                SendScrolledEvent();
            }
        }

        void OnKeyDown(object sender, EvasKeyEventArgs e)
        {
            _allowFocusOnItem = true;
            UpdateAllowFocusOnItem(_allowFocusOnItem);
        }

        void OnDragStart(object sender, EventArgs e)
        {
            _allowFocusOnItem = false;
            UpdateAllowFocusOnItem(_allowFocusOnItem);
        }

        void SendScrolledEvent()
        {
            var args = new ItemsViewScrolledExtendedEventArgs();
            args.FirstVisibleItemIndex = LayoutManager!.GetVisibleItemIndex(ViewPort.X, ViewPort.Y);
            args.CenterItemIndex = LayoutManager!.GetVisibleItemIndex(ViewPort.X + (ViewPort.Width / 2), ViewPort.Y + (ViewPort.Height / 2));
            args.LastVisibleItemIndex = LayoutManager!.GetVisibleItemIndex(ViewPort.X + ViewPort.Width, ViewPort.Y + ViewPort.Height);
            args.HorizontalOffset = ViewPort.X;
            args.HorizontalDelta = ViewPort.X - _previousHorizontalOffset;
            args.VerticalOffset = ViewPort.Y;
            args.VerticalDelta = ViewPort.Y - _previousVerticalOffset;
            args.CanvasSize = LayoutManager!.GetScrollCanvasSize();

            Scrolled?.Invoke(this, args);

            _previousHorizontalOffset = ViewPort.X;
            _previousVerticalOffset = ViewPort.Y;
        }

        void UpdateSnapPointsType(SnapPointsType snapPoints)
        {
            if (LayoutManager == null)
                return;

            int itemSize = 0;
            switch (snapPoints)
            {
                case SnapPointsType.None:
                    Scroller.HorizontalPageScrollLimit = 0;
                    Scroller.VerticalPageScrollLimit = 0;
                    itemSize = 0;
                    break;
                case SnapPointsType.MandatorySingle:
                    Scroller.HorizontalPageScrollLimit = 1;
                    Scroller.VerticalPageScrollLimit = 1;
                    itemSize = LayoutManager.GetScrollBlockSize();
                    break;
                case SnapPointsType.Mandatory:
                    Scroller.HorizontalPageScrollLimit = 0;
                    Scroller.VerticalPageScrollLimit = 0;
                    itemSize = LayoutManager.GetScrollBlockSize();
                    break;
            }

            if (LayoutManager.IsHorizontal)
            {
                Scroller.SetPageSize(itemSize, 0);
            }
            else
            {
                Scroller.SetPageSize(0, itemSize);
            }
        }

        void CreateEmptyView()
        {
            _emptyView = Adaptor!.CreateNativeView(this);
            _emptyView?.Show();
            if (_emptyView != null)
            {
                Adaptor!.SetBinding(_emptyView, 0);

                _emptyView.Geometry = Geometry;
                _emptyView.MinimumHeight = Geometry.Height;
                _emptyView.MinimumWidth = Geometry.Width;
                Scroller.SetContent(_emptyView, true);
            }
            _innerLayout.Hide();
        }

        void RemoveEmptyView()
        {
            _innerLayout.Show();
            Scroller.SetContent(_innerLayout, true);
            Adaptor!.RemoveNativeView(_emptyView!);
            _emptyView = null;
        }

        void UpdateAllowFocusOnItem(bool allowFocus)
        {
            foreach (var holer in _viewHolderIndexTable)
            {
                holer.Key.AllowItemFocus = allowFocus;
            }
        }
    }
}
