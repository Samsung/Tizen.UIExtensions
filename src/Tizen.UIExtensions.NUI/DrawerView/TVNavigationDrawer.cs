using System;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.Internal;
using Animation = Tizen.UIExtensions.Common.Internal.Animation;
using Size = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A View that contains a drawer for navigation of an app on TV.
    /// </summary>
    public class TVNavigationDrawer : DrawerView, IAnimatable
    {
        bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationDrawer"/> class
        /// </summary>
        public TVNavigationDrawer() : base(false)
        {
            FocusManager.Instance.FocusChanged += OnFocusChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;
            if (disposing)
            {
                // Remove event handler
                FocusManager.Instance.FocusChanged -= OnFocusChanged;
            }

            base.Dispose(disposing);
        }

        protected override Task RunAnimationAsync(bool isOpen)
        {
            var start = DrawerViewGroup.Size.Width;
            var end = (isOpen) ? DrawerWidth : DrawerWidthCollapsed;

            return ResizeDrawerAsync(start, end);
        }

        Task ResizeDrawerAsync(double start, double end)
        {
            var tcs = new TaskCompletionSource<bool>();

            var animation = new Animation(v =>
            {
                var r = start + ((end - start) * v);
                ContentViewGroup.UpdatePosition(new Point(r, 0));
                DrawerViewGroup.UpdateSize(new Size(r, Size.Height));
            }, easing: Easing.Linear);

            animation.Commit(this, "ResizeDrawer", length: 200, finished: (l, c) =>
            {
                ContentViewGroup.UpdatePosition(new Point(end, 0));
                DrawerViewGroup.UpdateSize(new Size(end, Size.Height));

                tcs.SetResult(true);
            });

            return tcs.Task;
        }

        async void OnFocusChanged(object? sender, FocusManager.FocusChangedEventArgs e)
        {
            if (e.Current == null)
                return;

            if (DrawerViewGroup.FindDescendantByID(e.Current.ID) != null)
                await OpenAsync(true);
            else if (ContentViewGroup.FindDescendantByID(e.Current.ID) != null)
                await CloseAsync(true);
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
