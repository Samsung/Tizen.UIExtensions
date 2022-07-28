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
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationDrawer"/> class
        /// </summary>
        public TVNavigationDrawer() : base(false)
        {
            FocusManager.Instance.FocusChanged += OnFocusChanged;
        }

        /// <summary>
        /// Opens the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is opened with animation.</param>
        public override async Task OpenAsync(bool animate = false)
        {
            if (IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return;

            await base.OpenAsync(animate);

            SendToggled();
        }

        /// <summary>
        /// Closes the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is closed with animation.</param>
        public override async Task CloseAsync(bool animate = false)
        {
            if (!IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return;

            await base.CloseAsync(animate);

            SendToggled();
        }

        protected override Task RunAnimationAsync(bool isOpen)
        {
            if (isOpen)
                return ResizeDrawerAsync(DrawerWidthCollapsed, DrawerWidth);
            else
                return ResizeDrawerAsync(DrawerWidth, DrawerWidthCollapsed);
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
            if (DrawerViewGroup.Contains(e.Current))
                await OpenAsync(true);
            else
                await CloseAsync(true);
        }

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
