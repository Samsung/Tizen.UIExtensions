using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Size = Tizen.UIExtensions.Common.Size;
using View = Tizen.NUI.BaseComponents.View;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// Base class for an Adapter
    /// Adapters provide a binding from an app-specific data set to views that are displayed within a CollectionView.
    /// </summary>
    public abstract class ItemAdaptor : INotifyCollectionChanged
    {
        IList _itemsSource;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ItemAdaptor"/> class.
        /// </summary>
        /// <param name="items">Items soruce</param>
        protected ItemAdaptor(IEnumerable items)
        {
            SetItemsSource(items);
        }

        /// <summary>
        /// A CollectionView associated with current Adaptor
        /// </summary>
        public ICollectionViewController CollectionView { get; set; }

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
        public virtual int Count => _itemsSource.Count;

        INotifyCollectionChanged _observableCollection;
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
        /// Handle Selected item
        /// </summary>
        /// <param name="selected"></param>
        public virtual void SendItemSelected(IEnumerable<int> selected)
        {
        }

        /// <summary>
        /// Update View state
        /// </summary>
        /// <param name="view">A view to update</param>
        /// <param name="state">State of view</param>
        public virtual void UpdateViewState(View view, ViewHolderState state)
        {
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
        public abstract View CreateNativeView();

        /// <summary>
        /// Create a new view
        /// </summary>
        /// <param name="index">To used item when create a view</param>
        /// <returns>Created view</returns>
        public abstract View CreateNativeView(int index);

        /// <summary>
        /// Create a header view, if header is not existed, null will be returned
        /// </summary>
        /// <returns>A created view</returns>
        public abstract View GetHeaderView();

        /// <summary>
        /// Create a footer view, if footer is not existed, null will be returned
        /// </summary>
        /// <returns>A created view</returns>
        public abstract View GetFooterView();

        /// <summary>
        /// Remove view, a created view by Adaptor, should be removed by Adaptor
        /// </summary>
        /// <param name="native">A view to remove</param>
        public abstract void RemoveNativeView(View native);

        /// <summary>
        /// Set data binding between view and item
        /// </summary>
        /// <param name="view">A target view</param>
        /// <param name="index">A target item</param>
        public abstract void SetBinding(View view, int index);

        /// <summary>
        /// Unset data binding on view
        /// </summary>
        /// <param name="view">A view to unbinding</param>
        public abstract void UnBinding(View view);

        /// <summary>
        /// Measure item size
        /// </summary>
        /// <param name="widthConstraint">A width size that could be reached as maximum</param>
        /// <param name="heightConstraint">A height  size that could be reached as maximum</param>
        /// <returns>Item size</returns>
        public abstract Size MeasureItem(double widthConstraint, double heightConstraint);

        /// <summary>
        /// Measure item size
        /// </summary>
        /// <param name="index">A item index to measure</param>
        /// <param name="widthConstraint">A width size that could be reached as maximum</param>
        /// <param name="heightConstraint">A height  size that could be reached as maximum</param>
        /// <returns>Item size</returns>
        public abstract Size MeasureItem(int index, double widthConstraint, double heightConstraint);

        /// <summary>
        /// Measure header size
        /// </summary>
        /// <param name="widthConstraint">A width size that could be reached as maximum</param>
        /// <param name="heightConstraint">A height  size that could be reached as maximum</param>
        /// <returns>Header size</returns>
        public abstract Size MeasureHeader(double widthConstraint, double heightConstraint);

        /// <summary>
        /// Measure Footer size
        /// </summary>
        /// <param name="widthConstraint">A width size that could be reached as maximum</param>
        /// <param name="heightConstraint">A height  size that could be reached as maximum</param>
        /// <returns>Footer size</returns>
        public abstract Size MeasureFooter(double widthConstraint, double heightConstraint);
    }
}
