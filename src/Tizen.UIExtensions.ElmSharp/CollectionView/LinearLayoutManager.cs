using System;
using System.Collections.Generic;
using System.Linq;
using ElmSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// A LinearLayoutManager implementation which provides linear layout.
    /// </summary>
	public class LinearLayoutManager : ICollectionViewLayoutManager
	{
		Size _allocatedSize;
		bool _isLayouting;
		Rect _last;
		Dictionary<int, RealizedItem> _realizedItem = new Dictionary<int, RealizedItem>();
		List<int> _itemSizes;
		List<bool> _cached;
		List<int> _accumulatedItemSizes;

		bool _hasUnevenRows;
		int _baseItemSize;

		Size _headerSize;
		EvasObject? _header;
		Size _footerSize;
		EvasObject? _footer;

        /// <summary>
        /// Initializes a new instance of the LinearLayoutManager class.
        /// </summary>
        /// <param name="isHorizontal">Layout orientation</param>
		public LinearLayoutManager(bool isHorizontal) : this(isHorizontal, ItemSizingStrategy.MeasureFirstItem) { }

        /// <summary>
        /// Initializes a new instance of the LinearLayoutManager class.
        /// </summary>
        /// <param name="isHorizontal">Layout orientation</param>
        /// <param name="sizingStrategy">Item size measuring strategy</param>
        public LinearLayoutManager(bool isHorizontal, ItemSizingStrategy sizingStrategy) : this(isHorizontal, sizingStrategy, 0) { }

        /// <summary>
        /// Initializes a new instance of the LinearLayoutManager class.
        /// </summary>
        /// <param name="isHorizontal">Layout orientation</param>
        /// <param name="sizingStrategy">Item size measuring strategy</param>
        /// <param name="itemSpacing">A space size between items</param>
#pragma warning disable CS8618
        public LinearLayoutManager(bool isHorizontal, ItemSizingStrategy sizingStrategy, int itemSpacing)
#pragma warning restore CS8618
        {
			IsHorizontal = isHorizontal;
			_hasUnevenRows = sizingStrategy == ItemSizingStrategy.MeasureAllItems;
			ItemSpacing = itemSpacing;
		}

        /// <summary>
        /// Whether the item is a layout horizontally.
        /// </summary>
        public bool IsHorizontal { get; }

        /// <summary>
        /// A space size between items.
        /// </summary>
        public int ItemSpacing { get; }

        /// <summary>
        /// CollectionView that interact with layout manager.
        /// </summary>
		public ICollectionViewController CollectionView { get; set; }

		public void SizeAllocated(Size size)
		{
			_scrollCanvasSize = new Size(0, 0);
			_allocatedSize = size;
			InitializeMeasureCache();
		}

		Size _scrollCanvasSize;

		public Size GetScrollCanvasSize()
		{
			if (CollectionView.Count == 0 || _allocatedSize.Width <= 0 || _allocatedSize.Height <= 0)
			{
				return _allocatedSize;
			}

			if (_scrollCanvasSize.Width > 0 && _scrollCanvasSize.Height > 0)
				return _scrollCanvasSize;

			int totalItemSize = 0;

			if (_hasUnevenRows)
			{
				totalItemSize = _accumulatedItemSizes[_accumulatedItemSizes.Count - 1] + FooterSizeWithSpacing;
			}
			else
			{
				totalItemSize = (BaseItemSize + ItemSpacing) * CollectionView.Count - ItemSpacing + ItemStartPoint + FooterSizeWithSpacing;
			}

			if (IsHorizontal)
			{
				_scrollCanvasSize = new Size(totalItemSize, _allocatedSize.Height);
			}
			else
			{
				_scrollCanvasSize = new Size(_allocatedSize.Width, totalItemSize);
			}

			return _scrollCanvasSize;
		}

		int BaseItemSize
		{
			get
			{
				if (_baseItemSize == 0)
				{
					if (_allocatedSize.Width <= 0 || _allocatedSize.Height <= 0)
						return 0;

					var itemBound = CollectionView.GetItemSize(ItemWidthConstraint, ItemHeightConstraint);
					_baseItemSize = IsHorizontal ? itemBound.Width : itemBound.Height;
				}
				return _baseItemSize;
			}
		}

		int ItemWidthConstraint => IsHorizontal ? int.MaxValue : _allocatedSize.Width;
		int ItemHeightConstraint => IsHorizontal ? _allocatedSize.Height : int.MaxValue;

		int FooterSize => IsHorizontal ? _footerSize.Width : _footerSize.Height;
		int HeaderSize => IsHorizontal ? _headerSize.Width : _headerSize.Height;
		int ItemStartPoint
		{
			get
			{
				var startPoint = HeaderSize;
				if (startPoint > 0)
				{
					startPoint += ItemSpacing;
				}
				return startPoint;
			}
		}

		int FooterSizeWithSpacing
		{
			get
			{
				var size = FooterSize;
				if (size > 0)
				{
					size += ItemSpacing;
				}
				return size;
			}
		}

		bool ShouldRearrange(Rect viewport)
		{
			if (_isLayouting)
				return false;
			if (_last.Size != viewport.Size)
				return true;

			var diff = IsHorizontal ? Math.Abs(_last.X - viewport.X) : Math.Abs(_last.Y - viewport.Y);
			if (diff > BaseItemSize)
				return true;

			return false;
		}

		public void LayoutItems(Rect bound, bool force)
		{
			if (_allocatedSize.Width <= 0 || _allocatedSize.Height <= 0)
				return;

			// TODO : need to optimization. it was frequently called with similar bound value.
			if (!ShouldRearrange(bound) && !force)
			{
				return;
			}

			_isLayouting = true;
			_last = bound;

			int startIndex = Math.Max(GetStartIndex(bound) - 1, 0);
			int endIndex = Math.Min(GetEndIndex(bound) + 1, CollectionView.Count - 1);

			foreach (var index in _realizedItem.Keys.ToList())
			{
				if (index < startIndex || index > endIndex)
				{
					CollectionView.UnrealizeView(_realizedItem[index].View);
					_realizedItem.Remove(index);
				}
			}

			var parent = CollectionView.ParentPosition;
			for (int i = startIndex; i <= endIndex; i++)
			{
				EvasObject itemView;
				if (!_realizedItem.ContainsKey(i))
				{
					var view = CollectionView.RealizeView(i);
                    _realizedItem[i] = new RealizedItem(view, i);
					itemView = view;
				}
				else
				{
					itemView = _realizedItem[i].View;
				}
				var itemBound = GetItemBound(i);
				itemBound.X += parent.X;
				itemBound.Y += parent.Y;
				itemView.Geometry = itemBound;
			}
			_isLayouting = false;
		}

		public void ItemInserted(int inserted)
		{
			var items = _realizedItem.Keys.OrderByDescending(key => key);
			foreach (var index in items)
			{
				if (index >= inserted)
				{
					_realizedItem[index + 1] = _realizedItem[index];
				}
			}
			if (_realizedItem.ContainsKey(inserted))
			{
				_realizedItem.Remove(inserted);
			}
			else
			{
				var last = items.LastOrDefault();
				if (last >= inserted)
				{
					_realizedItem.Remove(last);
				}
			}

			UpdateInsertedSize(inserted);

			_scrollCanvasSize = new Size(0, 0);
		}

		public void ItemRemoved(int removed)
		{
			if (_realizedItem.ContainsKey(removed))
			{
				CollectionView.UnrealizeView(_realizedItem[removed].View);
				_realizedItem.Remove(removed);
			}

			var items = _realizedItem.Keys.OrderBy(key => key);
			foreach (var index in items)
			{
				if (index > removed)
				{
					_realizedItem[index - 1] = _realizedItem[index];
				}
			}

			var last = items.LastOrDefault();
			if (last > removed)
			{
				_realizedItem.Remove(last);
			}

			UpdateRemovedSize(removed);

			_scrollCanvasSize = new Size(0, 0);
		}

		public void ItemUpdated(int index)
		{
			if (_realizedItem.ContainsKey(index))
			{
				var bound = _realizedItem[index].View.Geometry;
				CollectionView.UnrealizeView(_realizedItem[index].View);
				var view = CollectionView.RealizeView(index);
				_realizedItem[index].View = view;
				view.Geometry = bound;
			}
		}

		public Rect GetItemBound(int index)
		{
			int itemSize = 0;
			int startPoint = 0;

			if (!_hasUnevenRows)
			{
				itemSize = BaseItemSize;
				startPoint = ItemStartPoint + (itemSize + ItemSpacing) * index;
			}
			else if (index >= _itemSizes.Count)
			{
				return new Rect(0, 0, 0, 0);
			}
			else if (_cached[index])
			{
				itemSize = _itemSizes[index];
				startPoint = _accumulatedItemSizes[index] - itemSize;
			}
			else
			{
				var measured = CollectionView.GetItemSize(index, ItemWidthConstraint, ItemHeightConstraint);
				itemSize = IsHorizontal ? measured.Width : measured.Height;

				if (itemSize != _itemSizes[index])
				{
					UpdateAccumulatedItemSize(index, itemSize - _itemSizes[index]);
					_itemSizes[index] = itemSize;

					CollectionView.ContentSizeUpdated();
				}
				startPoint = _accumulatedItemSizes[index] - itemSize;
				_cached[index] = true;
			}

			return IsHorizontal ?
				new Rect(startPoint, 0, itemSize, _allocatedSize.Height) :
				new Rect(0, startPoint, _allocatedSize.Width, itemSize);
		}

		public void Reset()
		{
			foreach (var realizedItem in _realizedItem.Values.ToList())
			{
				CollectionView.UnrealizeView(realizedItem.View);
			}
			_realizedItem.Clear();
			_scrollCanvasSize = new Size(0, 0);
		}

		public void ItemSourceUpdated()
		{
			InitializeMeasureCache();
		}

		public void ItemMeasureInvalidated(int index)
		{
			if (_hasUnevenRows)
			{
				if (_cached.Count > index)
					_cached[index] = false;

				if (_realizedItem.ContainsKey(index))
				{
					CollectionView.RequestLayoutItems();
				}
			}
			else if (index == 0)
			{
				// Reset item size to measure updated size
				InitializeMeasureCache();
				CollectionView.RequestLayoutItems();
			}
		}

		public int GetVisibleItemIndex(int x, int y)
		{
			int coordinate = IsHorizontal ? x : y;
			int canvasSize = IsHorizontal ? _scrollCanvasSize.Width : _scrollCanvasSize.Height;

			if (coordinate < 0)
				return 0;
			if (canvasSize < coordinate)
				return CollectionView.Count - 1;

			if (!_hasUnevenRows)
			{
				return Math.Min(Math.Max(0, (coordinate - ItemStartPoint) / (BaseItemSize + ItemSpacing)), CollectionView.Count - 1);
			}
			else
			{
				var index = _accumulatedItemSizes.FindIndex(current => coordinate <= current);
				if (index == -1)
					index = CollectionView.Count - 1;
				return index;
			}
		}

		public int GetScrollBlockSize()
		{
			return BaseItemSize + ItemSpacing;
		}

		public void SetHeader(EvasObject? header, Size size)
		{
			bool contentSizeChanged = false;
			if (IsHorizontal)
			{
				if (_headerSize.Width != size.Width)
					contentSizeChanged = true;
			}
			else
			{
				if (_headerSize.Height != size.Height)
					contentSizeChanged = true;
			}

			_header = header;
			_headerSize = size;

			if (contentSizeChanged)
			{
				InitializeMeasureCache();
				CollectionView.ContentSizeUpdated();
			}

			var position = CollectionView.ParentPosition;
			if (_header != null)
			{
				var bound = new Rect(position.X, position.Y, _headerSize.Width, _headerSize.Height);
				if (IsHorizontal)
				{
					bound.Height = _allocatedSize.Height;
				}
				else
				{
					bound.Width = _allocatedSize.Width;
				}
				_header.Geometry = bound;
			}
		}

		public void SetFooter(EvasObject? footer, Size size)
		{
			bool contentSizeChanged = false;
			if (IsHorizontal)
			{
				if (_footerSize.Width != size.Width)
					contentSizeChanged = true;
			}
			else
			{
				if (_footerSize.Height != size.Height)
					contentSizeChanged = true;
			}

			_footer = footer;
			_footerSize = size;

			if (contentSizeChanged)
			{
				InitializeMeasureCache();
				CollectionView.ContentSizeUpdated();
			}

			UpdateFooterPosition();
		}

		void UpdateFooterPosition()
		{
			if (_footer == null)
				return;

			var position = CollectionView.ParentPosition;
			if (IsHorizontal)
			{
				position.X += (GetScrollCanvasSize().Width - _footerSize.Width);
			}
			else
			{
				position.Y += (GetScrollCanvasSize().Height - _footerSize.Height);
			}

			var bound = new Rect(position.X, position.Y, _footerSize.Width, _footerSize.Height);
			if (IsHorizontal)
			{
				bound.Height = _allocatedSize.Height;
			}
			else
			{
				bound.Width = _allocatedSize.Width;
			}
			_footer.Geometry = bound;
		}

		void InitializeMeasureCache()
		{
			_baseItemSize = 0;
			_scrollCanvasSize = new Size(0, 0);

			if (!_hasUnevenRows)
				return;

			if (_allocatedSize.Width <= 0 || _allocatedSize.Height <= 0)
				return;

			int n = CollectionView.Count;
			_itemSizes = new List<int>();
			_cached = new List<bool>();
			_accumulatedItemSizes = new List<int>();

			for (int i = 0; i < n; i++)
			{
				_cached.Add(false);
				_itemSizes.Add(BaseItemSize);
				_accumulatedItemSizes.Add((i > 0 ? (_accumulatedItemSizes[i - 1] + ItemSpacing) : ItemStartPoint) + _itemSizes[i]);
			}
		}

		int GetStartIndex(Rect bound, int itemSize)
		{
			return (ViewPortStartPoint(bound) - ItemStartPoint) / itemSize;
		}

		int GetStartIndex(Rect bound)
		{
			if (!_hasUnevenRows)
			{
				return GetStartIndex(bound, BaseItemSize + ItemSpacing);
			}

			return FindFirstGreaterOrEqualTo(_accumulatedItemSizes, ViewPortStartPoint(bound));
		}

		int GetEndIndex(Rect bound, int itemSize)
		{
			return (int)Math.Ceiling(ViewPortEndPoint(bound) / (double)itemSize) - 1;
		}

		int GetEndIndex(Rect bound)
		{
			if (!_hasUnevenRows)
			{
				return GetEndIndex(bound, BaseItemSize + ItemSpacing);
			}

			return FindFirstGreaterOrEqualTo(_accumulatedItemSizes, ViewPortEndPoint(bound));
		}

		int ViewPortStartPoint(Rect viewPort)
		{
			return IsHorizontal ? viewPort.X : viewPort.Y;
		}

		int ViewPortEndPoint(Rect viewPort)
		{
			return ViewPortStartPoint(viewPort) + ViewPortSize(viewPort);
		}

		int ViewPortSize(Rect viewPort)
		{
			return IsHorizontal ? viewPort.Width : viewPort.Height;
		}

		void UpdateAccumulatedItemSize(int index, int diff)
		{
			for (int i = index; i < _accumulatedItemSizes.Count; i++)
			{
				_accumulatedItemSizes[i] += diff;
			}

			if (_scrollCanvasSize.Width > 0 && _scrollCanvasSize.Height > 0)
			{

				if (IsHorizontal)
				{
					_scrollCanvasSize.Width += diff;
				}
				else
				{
					_scrollCanvasSize.Height += diff;
				}
			}
			UpdateFooterPosition();
		}

		void UpdateRemovedSize(int removed)
		{
			if (!_hasUnevenRows)
				return;
			var removedSize = _itemSizes[removed];
			_itemSizes.RemoveAt(removed);
			UpdateAccumulatedItemSize(removed, -removedSize);
			_accumulatedItemSizes.RemoveAt(removed);
			_cached.RemoveAt(removed);
		}

		void UpdateInsertedSize(int inserted)
		{
			if (!_hasUnevenRows)
				return;

			_cached.Insert(inserted, false);
			_itemSizes.Insert(inserted, BaseItemSize);
			_accumulatedItemSizes.Insert(inserted, 0);
			_accumulatedItemSizes[inserted] = inserted > 0 ? _accumulatedItemSizes[inserted - 1] : ItemStartPoint;
			UpdateAccumulatedItemSize(inserted, BaseItemSize);
		}

		static int FindFirstGreaterOrEqualTo(IList<int> data, int value)
		{
			if (data.Count == 0)
				return 0;

			int start = 0;
			int end = data.Count - 1;
			while (start < end)
			{
				int mid = (start + end) / 2;
				if (data[mid] < value)
				{
					start = mid + 1;
				}
				else
				{
					end = mid - 1;
				}
			}
			if (data[start] < value)
			{
				start++;
			}
			return start;
		}

		class RealizedItem
		{
            public RealizedItem(ViewHolder view, int index)
            {
                View = view;
                Index = index;
            }

            public ViewHolder View { get; set; }
			public int Index { get; set; }
		}
	}
}
