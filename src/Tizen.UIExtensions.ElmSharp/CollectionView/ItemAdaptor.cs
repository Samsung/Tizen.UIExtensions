using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using ElmSharp;
using ESize = ElmSharp.Size;

namespace Tizen.UIExtensions.ElmSharp
{
	public interface IEmptyAdaptor { }

    /// <summary>
    /// Base class for an Adapter
    /// Adapters provide a binding from an app-specific data set to views that are displayed within a CollectionView.
    /// </summary>
	public abstract class ItemAdaptor : INotifyCollectionChanged
	{
		IList _itemsSource;

        /// <summary>
        /// A CollectionView associated with current Adaptor
        /// </summary>
		public CollectionView? CollectionView { get; set; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="ItemAdaptor"/> class.
        /// </summary>
        /// <param name="items">Items soruce</param>
#pragma warning disable CS8618
        protected ItemAdaptor(IEnumerable items)
#pragma warning restore CS8618
        {
			SetItemsSource(items);
		}

		public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        /// <summary>
        /// Handle Selected item
        /// </summary>
        /// <param name="selected"></param>
		public virtual void SendItemSelected(int index)
		{
			ItemSelected?.Invoke(this, new SelectedItemChangedEventArgs(this[index], index));
		}

        /// <summary>
        /// Update View state
        /// </summary>
        /// <param name="view">A view to update</param>
        /// <param name="state">State of view</param>
		public virtual void UpdateViewState(EvasObject view, ViewHolderState state)
		{
		}

		public void RequestItemSelected(object item)
		{
			if (CollectionView != null)
			{
				CollectionView.SelectedItemIndex = _itemsSource.IndexOf(item);
			}
		}

        /// <summary>
        /// Sets ItemsSource
        /// </summary>
        /// <param name="items">Items source</param>
		protected void SetItemsSource(IEnumerable items)
		{
			switch (items)
			{
				case IList list:
					_itemsSource = list;
					_observableCollection = list as INotifyCollectionChanged;
					break;
				case IEnumerable<object> generic:
					_itemsSource = new List<object>(generic);
					break;
				case IEnumerable _:
					_itemsSource = new List<object>();
					foreach (var item in items)
					{
						_itemsSource.Add(item);
					}
					break;
			}
		}

		public object this[int index]
		{
			get
			{
				return _itemsSource[index];
			}
		}

        /// <summary>
        /// the number of items
        /// </summary>
		public int Count => _itemsSource.Count;

		INotifyCollectionChanged? _observableCollection;
		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
		{
			add
			{
				if (_observableCollection != null)
				{
					_observableCollection.CollectionChanged += value;
				}
			}
			remove
			{
				if (_observableCollection != null)
				{
					_observableCollection.CollectionChanged -= value;
				}
			}
		}

        /// <summary>
        /// Find item index by item
        /// </summary>
        /// <param name="item">item to find</param>
        /// <returns>index of item</returns>
		public int GetItemIndex(object item)
		{
			return _itemsSource.IndexOf(item);
		}

        /// <summary>
        /// A view category that represent a item, it use to distinguish kinds of view
        /// </summary>
        /// <param name="index">item index</param>
        /// <returns>An identifier of category</returns>
		public virtual object GetViewCategory(int index)
		{
			return this;
		}

        /// <summary>
        /// Create a new view
        /// </summary>
        /// <returns>Created view</returns>
		public abstract EvasObject CreateNativeView(EvasObject parent);

        /// <summary>
        /// Create a new view
        /// </summary>
        /// <param name="index">To used item when create a view</param>
        /// <returns>Created view</returns>
		public abstract EvasObject CreateNativeView(int index, EvasObject parent);

        /// <summary>
        /// Create a header view, if header is not existed, null will be returned
        /// </summary>
        /// <returns>A created view</returns>
		public abstract EvasObject GetHeaderView(EvasObject parent);

        /// <summary>
        /// Create a footer view, if footer is not existed, null will be returned
        /// </summary>
        /// <returns>A created view</returns>
		public abstract EvasObject GetFooterView(EvasObject parent);

        /// <summary>
        /// Remove view, a created view by Adaptor, should be removed by Adaptor
        /// </summary>
        /// <param name="native">A view to remove</param>
        public abstract void RemoveNativeView(EvasObject native);

        /// <summary>
        /// Set data binding between view and item
        /// </summary>
        /// <param name="view">A target view</param>
        /// <param name="index">A target item</param>
        public abstract void SetBinding(EvasObject view, int index);

        /// <summary>
        /// Unset data binding on view
        /// </summary>
        /// <param name="view">A view to unbinding</param>
        public abstract void UnBinding(EvasObject view);

        /// <summary>
        /// Measure item size
        /// </summary>
        /// <param name="widthConstraint">A width size that could be reached as maximum</param>
        /// <param name="heightConstraint">A height  size that could be reached as maximum</param>
        /// <returns>Item size</returns>
        public abstract ESize MeasureItem(int widthConstraint, int heightConstraint);

        /// <summary>
        /// Measure item size
        /// </summary>
        /// <param name="index">A item index to measure</param>
        /// <param name="widthConstraint">A width size that could be reached as maximum</param>
        /// <param name="heightConstraint">A height  size that could be reached as maximum</param>
        /// <returns>Item size</returns>
        public abstract ESize MeasureItem(int index, int widthConstraint, int heightConstraint);

        /// <summary>
        /// Measure header size
        /// </summary>
        /// <param name="widthConstraint">A width size that could be reached as maximum</param>
        /// <param name="heightConstraint">A height  size that could be reached as maximum</param>
        /// <returns>Header size</returns>
        public abstract ESize MeasureHeader(int widthConstraint, int heightConstraint);

        /// <summary>
        /// Measure Footer size
        /// </summary>
        /// <param name="widthConstraint">A width size that could be reached as maximum</param>
        /// <param name="heightConstraint">A height  size that could be reached as maximum</param>
        /// <returns>Footer size</returns>
        public abstract ESize MeasureFooter(int widthConstraint, int heightConstraint);
	}
}
