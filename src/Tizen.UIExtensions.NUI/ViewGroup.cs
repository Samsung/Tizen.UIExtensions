using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI
{

    /// <summary>
    /// A ViewGroup provides a class which can be a container for other controls.
    /// </summary>
    /// <remarks>
    /// This class is used as a container view for Layouts from Xamarin.Forms.Platform.Tizen framework.
    /// It is used for implementing xamarin pages and layouts.
    /// </remarks>
    public class ViewGroup : View, IContainable<View>
    {
        readonly ObservableCollection<View> _children = new ObservableCollection<View>();
        bool _layoutRequested;
        bool _disposed;
        SynchronizationContext _mainloopContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewGroup"/> class.
        /// </summary>
        /// <remarks>ViewGroup doesn't support replacing its children, this will be ignored.</remarks>
        public ViewGroup()
        {
            Debug.Assert(SynchronizationContext.Current != null, "It must be used on main thread");
            _mainloopContext = SynchronizationContext.Current;

            Layout = new AbsoluteLayout();
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;
            Relayout += OnRelayout;
            _children.CollectionChanged += OnCollectionChanged;
        }

        IList<View> IContainable<View>.Children => _children;

        /// <summary>
        /// Gets list of native elements that are placed in the ViewGroup.
        /// </summary>
        public new IList<View> Children => _children;

        /// <summary>
        /// Notifies that the layout has been updated.
        /// </summary>
        public event EventHandler<LayoutEventArgs>? LayoutUpdated;

        public override void Add(View child)
        {
            base.Add(child);
            LayoutRequest();
        }

        public override void Remove(View child)
        {
            base.Remove(child);
            LayoutRequest();
        }

        void OnRelayout(object? sender, EventArgs e)
        {
            SendLayoutUpdated();
        }

        void LayoutRequest()
        {
            if (!_layoutRequested)
            {
                _layoutRequested = true;
                _mainloopContext.Post((s) => SendLayoutUpdated(), null);
            }
        }

        void SendLayoutUpdated()
        {
            if (_disposed)
                return;

            if (this == null)
                return;

            LayoutUpdated?.Invoke(this, new LayoutEventArgs
            {
                Geometry = new Rect(Position.X, Position.Y, Size.Width, Size.Height)
            });
        }

        void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (var v in e.NewItems)
                {
                    if (v is View view)
                    {
                        Add(view);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (var v in e.OldItems)
                {
                    if (v is View view)
                    {
                        Remove(view);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var child in base.Children.ToList())
                {
                    Remove(child);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;
            if (disposing)
            {
                // Remove All children, becuase NUI didn't delete child
                var tobeDisposed = _children.ToList();
                _children.Clear(); // will removed from ViewGroup by OnCollectionChanged
                foreach (var child in tobeDisposed)
                {
                    child.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
