using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Rect = Tizen.UIExtensions.Common.Rect;
using Size = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A View that contain a templated list of items.
    /// </summary>
    public class CollectionView : View, ICollectionViewController
    {
        RecyclerPool _pool = new RecyclerPool();
        Dictionary<ViewHolder, int> _viewHolderIndexTable = new Dictionary<ViewHolder, int>();

        SynchronizationContext _mainloopContext;
        ICollectionViewLayoutManager _layoutManager;
        ItemAdaptor _adaptor;

        bool _requestLayoutItems = false;

        double _previousHorizontalOffset;
        double _previousVerticalOffset;
        Size _itemSize = new Size(-1, -1);

        View _headerView;
        View _footerView;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionView"/> class.
        /// </summary>
        public CollectionView()
        {
            _mainloopContext = SynchronizationContext.Current;
            InitializationComponent();
        }

        /// <summary>
        /// Event that is raised after a scroll completes.
        /// </summary>
        public event EventHandler<CollectionViewScrolledEventArgs> Scrolled;

        /// <summary>
        /// Gets a ScrollView instance that used in CollectionView
        /// </summary>
        public ScrollableBase ScrollView { get; private set; }

        /// <summary>
        /// The number of items on CollectionView
        /// </summary>
        public int Count => Adaptor?.Count ?? 0;

        /// <summary>
        /// Gets or sets LayoutManager to organize position of item
        /// </summary>
        public ICollectionViewLayoutManager LayoutManager
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
        public ItemAdaptor Adaptor
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
        /// A size of scrolling area
        /// </summary>
        public Size ScrollCanvasSize => LayoutManager?.GetScrollCanvasSize() ?? new Size(0, 0);

        /// <summary>
        /// A size of allocated by Layout, it become viewport size on scrolling
        /// </summary>
        protected Size AllocatedSize { get; set; }

        Rect ViewPort => ScrollView.GetScrollBound();


        /// <summary>
        /// Create a ViewHolder, override it to customzie a decoration of view
        /// </summary>
        /// <returns>A ViewHolder instance</returns>
        protected virtual ViewHolder CreateViewHolder()
        {
            return new ViewHolder();
        }

        /// <summary>
        /// Create a ScrollView to use in CollectionView
        /// </summary>
        /// <returns>A ScrollView instance</returns>
        protected virtual ScrollableBase CreateScrollView()
        {
            return new ScrollableBase();
        }

        /// <summary>
        /// Initialize internal components, such as ScrollView
        /// </summary>
        protected virtual void InitializationComponent()
        {
            Layout = new AbsoluteLayout();
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            ScrollView = CreateScrollView();
            ScrollView.WidthSpecification = LayoutParamPolicies.MatchParent;
            ScrollView.HeightSpecification = LayoutParamPolicies.MatchParent;
            ScrollView.ScrollingEventThreshold = 10;
            ScrollView.Scrolling += OnScrolling;
            ScrollView.ScrollAnimationEnded += OnScrollAnimationEnded;
            ScrollView.Relayout += OnLayout;

            Add(ScrollView);
        }

        void ICollectionViewController.ItemMeasureInvalidated(int index)
        {
            // If a first item size was updated, need to reset _itemSize
            if (index == 0)
            {
                _itemSize = new Size(-1, -1);
            }
            LayoutManager?.ItemMeasureInvalidated(index);
        }

        void ICollectionViewController.RequestLayoutItems() => RequestLayoutItems();
        void RequestLayoutItems()
        {
            if (AllocatedSize.Width <= 0 || AllocatedSize.Height <= 0)
                return;

            if (!_requestLayoutItems)
            {
                _requestLayoutItems = true;

                _mainloopContext.Post((s) =>
                {
                    _requestLayoutItems = false;
                    if (_adaptor != null && _layoutManager != null)
                    {
                        ContentSizeUpdated();
                        _layoutManager?.LayoutItems(ViewPort, true);
                    }
                }, null);
            }
        }

        void ICollectionViewController.ContentSizeUpdated() => ContentSizeUpdated();
        void ContentSizeUpdated()
        {
            ScrollView.ContentContainer.UpdateSize(LayoutManager.GetScrollCanvasSize());
        }

        Size ICollectionViewController.GetItemSize()
        {
            var widthConstraint = LayoutManager.IsHorizontal ? AllocatedSize.Width * 100 : AllocatedSize.Width;
            var heightConstraint = LayoutManager.IsHorizontal ? AllocatedSize.Height : AllocatedSize.Height * 100;
            return GetItemSize(widthConstraint,heightConstraint);
        }

        Size ICollectionViewController.GetItemSize(double widthConstraint, double heightConstraint) => GetItemSize(widthConstraint, heightConstraint);
        Size GetItemSize(double widthConstraint, double heightConstraint)
        {
            if (Adaptor == null)
            {
                return new Size(0, 0);
            }

            if (_itemSize.Width > 0 && _itemSize.Height > 0)
            {
                return _itemSize;
            }

            _itemSize = Adaptor.MeasureItem(widthConstraint, heightConstraint);
            _itemSize.Width = Math.Max(_itemSize.Width, 10);
            _itemSize.Height = Math.Max(_itemSize.Height, 10);
            return _itemSize;
        }

        Size ICollectionViewController.GetItemSize(int index, double widthConstraint, double heightConstraint) => GetItemSize(index, widthConstraint, heightConstraint);
        Size GetItemSize(int index, double widthConstraint, double heightConstraint)
        {
            if (Adaptor == null)
            {
                return new Size(0, 0);
            }
            return Adaptor.MeasureItem(index, widthConstraint, heightConstraint);
        }

        ViewHolder ICollectionViewController.RealizeView(int index)
        {
            if (Adaptor == null)
                return null;

            var holder = _pool.GetRecyclerView(Adaptor.GetViewCategory(index));
            if (holder != null)
            {
                holder.Show();
            }
            else
            {
                var content = Adaptor.CreateNativeView(index);
                holder = CreateViewHolder();
                holder.Content = content;
                holder.ViewCategory = Adaptor.GetViewCategory(index);
                ScrollView.ContentContainer.Add(holder);
            }

            Adaptor.SetBinding(holder.Content, index);
            _viewHolderIndexTable[holder] = index;

            return holder;
        }

        void ICollectionViewController.UnrealizeView(ViewHolder view)
        {
            _viewHolderIndexTable.Remove(view);
            Adaptor.UnBinding(view.Content);
            view.ResetState();
            view.Hide();

            if (_pool.Count < _viewHolderIndexTable.Count)
            {
                _pool.AddRecyclerView(view);
            }
            else
            {
                Adaptor.RemoveNativeView(view.Content);
                view.Content.Unparent();
                view.Content.Dispose();
                view.Unparent();
                view.Dispose();
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

            Console.WriteLine($"LayoutManagerChanged - AllocatedSize : {AllocatedSize}");

            _itemSize = new Size(-1, -1);
            _layoutManager.CollectionView = this;
            _layoutManager.SizeAllocated(AllocatedSize);

            UpdateHeaderFooter();
            RequestLayoutItems();
        }

        void OnAdaptorChanging()
        {
            // reset header view
            _headerView?.Unparent();
            _headerView?.Dispose();
            _headerView = null;

            // reset footer view
            _footerView?.Unparent();
            _footerView?.Dispose();
            _footerView = null;

            _layoutManager?.Reset();
            if (Adaptor != null)
            {
                _pool.Clear(Adaptor);
                (Adaptor as INotifyCollectionChanged).CollectionChanged -= OnCollectionChanged;
                Adaptor.CollectionView = null;
            }
        }

        void OnAdaptorChanged()
        {
            if (_adaptor == null)
                return;

            _itemSize = new Size(-1, -1);
            Adaptor.CollectionView = this;
            (Adaptor as INotifyCollectionChanged).CollectionChanged += OnCollectionChanged;

            LayoutManager?.ItemSourceUpdated();
            RequestLayoutItems();

            _headerView = Adaptor.GetHeaderView();
            if (_headerView != null)
            {
                ScrollView.ContentContainer.Add(_headerView);
            }
            _footerView = Adaptor.GetFooterView();
            if (_footerView != null)
            {
                ScrollView.ContentContainer.Add(_footerView);
            }

            UpdateHeaderFooter();
        }


        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
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
                    LayoutManager.ItemInserted(idx++);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                int idx = e.OldStartingIndex;

                // Can't tracking remove if there is no data of old index
                if (idx == -1)
                {
                    LayoutManager.ItemSourceUpdated();
                }
                else
                {
                    foreach (var item in e.OldItems)
                    {
                        LayoutManager.ItemRemoved(idx);
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
                LayoutManager.ItemRemoved(e.OldStartingIndex);
                LayoutManager.ItemInserted(e.NewStartingIndex);
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                // Can't tracking if there is no information old data
                if (e.OldItems.Count > 1 || e.NewStartingIndex == -1)
                {
                    LayoutManager.ItemSourceUpdated();
                }
                else
                {
                    LayoutManager.ItemUpdated(e.NewStartingIndex);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                LayoutManager.Reset();
                LayoutManager.ItemSourceUpdated();
            }
            RequestLayoutItems();
        }


        void OnScrollAnimationEnded(object sender, ScrollEventArgs e)
        {
            SendScrolledEvent();
        }

        void OnLayout(object sender, EventArgs e)
        {
            //called when resized
            AllocatedSize = ScrollView.Size.ToCommon();
            _itemSize = new Size(-1, -1);

            if (_adaptor != null && _layoutManager != null)
            {
                ScrollView.ScrollingDirection = LayoutManager.IsHorizontal ? ScrollableBase.Direction.Horizontal : ScrollableBase.Direction.Vertical;
                _layoutManager?.SizeAllocated(AllocatedSize);
                UpdateHeaderFooter();
                ContentSizeUpdated();
                _layoutManager?.LayoutItems(ViewPort);
            }
        }

        void OnScrolling(object sender, ScrollEventArgs e)
        {
            var viewportFromEvent = new Rect(-e.Position.X, -e.Position.Y, ScrollView.Size.Width, ScrollView.Size.Height);
            _layoutManager?.LayoutItems(viewportFromEvent);
        }

        void SendScrolledEvent()
        {
            var args = new CollectionViewScrolledEventArgs();
            args.FirstVisibleItemIndex = _layoutManager.GetVisibleItemIndex(ViewPort.X, ViewPort.Y);
            args.CenterItemIndex = _layoutManager.GetVisibleItemIndex(ViewPort.X + (ViewPort.Width / 2), ViewPort.Y + (ViewPort.Height / 2));
            args.LastVisibleItemIndex = _layoutManager.GetVisibleItemIndex(ViewPort.X + ViewPort.Width, ViewPort.Y + ViewPort.Height);
            args.HorizontalOffset = ViewPort.X;
            args.HorizontalDelta = ViewPort.X - _previousHorizontalOffset;
            args.VerticalOffset = ViewPort.Y;
            args.VerticalDelta = ViewPort.Y - _previousVerticalOffset;
            Scrolled?.Invoke(this, args);

            _previousHorizontalOffset = ViewPort.X;
            _previousVerticalOffset = ViewPort.Y;
        }

        void UpdateHeaderFooter()
        {
            LayoutManager?.SetHeader(_headerView,
                _headerView != null ? Adaptor.MeasureHeader(AllocatedSize.Width, AllocatedSize.Height) : new Size(0, 0));

            LayoutManager?.SetFooter(_footerView,
                _footerView != null ? Adaptor.MeasureFooter(AllocatedSize.Width, AllocatedSize.Height) : new Size(0, 0));
        }

    }
}
