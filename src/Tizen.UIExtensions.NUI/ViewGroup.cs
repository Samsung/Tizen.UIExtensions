using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
        bool _disposed;

        float _cachedWidth;
        float _cachedHeight;
        bool _markChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewGroup"/> class.
        /// </summary>
        /// <remarks>ViewGroup doesn't support replacing its children, this will be ignored.</remarks>
        public ViewGroup()
        {
            Layout = new ViewGroupLayout
            {
                LayoutRequest = () => SendLayoutUpdated(),
                MeasureRequest = (width, height) => SendMeasureRequested(width, height),
            };
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;
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

        /// <summary>
        /// Notifies that requesting measure
        /// </summary>
        public event EventHandler<MeasureRequestedEventArgs>? MeasureRequested;

        public void MarkChanged() => _markChanged = true;

        public override void Add(View child)
        {
            Children.Add(child);
        }

        public override void Remove(View child)
        {
            Children.Remove(child);
        }

        void AddInternal(View child)
        {
            _markChanged = true;
            base.Add(child);
        }

        void RemoveInternal(View child)
        {
            _markChanged = true;
            base.Remove(child);
        }

        void SendLayoutUpdated()
        {
            if (_disposed)
                return;

            if (this == null)
                return;

            var currentSize = Size2D;
            var needUpdate = _cachedWidth != currentSize.Width || _cachedHeight != currentSize.Height || _markChanged;

            _cachedWidth = currentSize.Width;
            _cachedHeight = currentSize.Height;
            _markChanged = false;

            if (!needUpdate)
                return;

            LayoutUpdated?.Invoke(this, new LayoutEventArgs
            {
                Geometry = new Rect(Position.X, Position.Y, Size.Width, Size.Height)
            });
        }

        void SendMeasureRequested(double widthConstraint, double heightConstraint)
        {
            MeasureRequested?.Invoke(this, new MeasureRequestedEventArgs
            {
                WidthConstraint = widthConstraint,
                HeightConstraint = heightConstraint
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
                        AddInternal(view);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (var v in e.OldItems)
                {
                    if (v is View view)
                    {
                        RemoveInternal(view);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var child in base.Children.ToList())
                {
                    RemoveInternal(child);
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

        class ViewGroupLayout : AbsoluteLayout
        {
            public Action? LayoutRequest { get; set; }
            public Action<double, double>? MeasureRequest { get; set; }

            protected override void OnLayout(bool changed, LayoutLength left, LayoutLength top, LayoutLength right, LayoutLength bottom)
            {
                LayoutRequest?.Invoke();
                base.OnLayout(changed, left, top, right, bottom);
            }

            protected override void OnMeasure(MeasureSpecification widthMeasureSpec, MeasureSpecification heightMeasureSpec)
            {
                double widthConstraint = widthMeasureSpec.GetSize().AsDecimal();
                double heightConstraint = heightMeasureSpec.GetSize().AsDecimal();

                if (widthMeasureSpec.GetMode() == MeasureSpecification.ModeType.Unspecified)
                    widthConstraint = double.PositiveInfinity;
                if (heightMeasureSpec.GetMode() == MeasureSpecification.ModeType.Unspecified)
                    heightConstraint = double.PositiveInfinity;

                MeasureRequest?.Invoke(widthConstraint, heightConstraint);
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
        }

    }
}
