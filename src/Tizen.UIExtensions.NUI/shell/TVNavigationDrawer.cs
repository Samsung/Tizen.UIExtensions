using System;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.Internal;
using Animation = Tizen.UIExtensions.Common.Internal.Animation;
using Size = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A View that contains a drawer for navigation of an app on TV.
    /// </summary>
    public class TVNavigationDrawer : DrawerView, IAnimatable, INavigationDrawer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationDrawer"/> class
        /// </summary>
        public TVNavigationDrawer() : base()
        {
            IsPopover = false;
        }

        /// <summary>
        /// Gets or sets a value that indicates if gesture is enabled or not.
        /// </summary>
        public bool IsGestureEnabled { get; set; }

        /// <summary>
        /// Gets or sets a view that blocks interaction when the drawer is opened.
        /// </summary>
        public View? Backdrop { get; set; }

        /// <summary>
        /// Event that is raised when the drawer is toggled.
        /// </summary>
        public event EventHandler? Toggled;

        /// <summary>
        /// Opens the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is opened with animation.</param>
        public override async Task<bool> OpenAsync(bool animate = false)
        {
            if (IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return true;

            await base.OpenAsync(animate);
            Toggled?.Invoke(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Closes the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is closed with animation.</param>
        public override async Task<bool> CloseAsync(bool animate = false)
        {
            if (!IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return true;

            await base.CloseAsync(animate);
            Toggled?.Invoke(this, EventArgs.Empty);
            return true;
        }

        protected override Task RunAnimationAsync(bool isOpen)
        {
            if (isOpen)
                return ResizeDrawerAsync(DrawerWidthCollapsed, DrawerWidth);
            else
                return ResizeDrawerAsync(DrawerWidth, DrawerWidthCollapsed);
        }

        protected override async void OnDrawerFocusGained(object? sender, EventArgs args)
        {
            if (!IsOpened)
                await OpenAsync(true);

            DrawerViewGroup.FocusableChildren = true;
            base.OnDrawerFocusGained(sender, args);
        }

        protected override async void OnContentFocusGained(object? sender, EventArgs args)
        {
            if (IsOpened)
                await CloseAsync(true);

            DrawerViewGroup.FocusableChildren = false;
            base.OnContentFocusGained(sender, args);
        }

        async void OpenCloseDrawer(bool open)
        {
            if (open)
                await OpenAsync();
            else
                await CloseAsync();
        }

        protected override bool OnDrawerKeyEventTriggered(object sender, KeyEventArgs args)
        {
            if (args.Key.IsDeclineKeyEvent())
            {
                //await CloseDrawer(true);
                OpenCloseDrawer(false);
                return true;
            }

            if (args.Key.KeyPressedName == "Right")
            {
                if (args.Key.State == Key.StateType.Up)
                {
                    //await CloseDrawer(true);
                    OpenCloseDrawer(false);
                    return true;
                }
            }

            return false;
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

        void IAnimatable.BatchBegin() { }

        void IAnimatable.BatchCommit() { }
    }
}
