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
        public View? BackDrop { get; set; }

        /// <summary>
        /// Event that is raised when the drawer is toggled.
        /// </summary>
        public event EventHandler? Toggled;

        /// <summary>
        /// Opens the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is opened with animation.</param>
        public override void OpenDrawer(bool animate = false)
        {
            if (IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return;

            base.OpenDrawer(animate);
            Toggled?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Closes the drawer.
        /// </summary>
        /// <param name="animate">Whether or not the drawer is closed with animation.</param>
        public override void CloseDrawer(bool animate = false)
        {
            if (!IsOpened || (DrawerBehavior != DrawerBehavior.Drawer))
                return;

            base.CloseDrawer(animate);
            Toggled?.Invoke(this, EventArgs.Empty);
        }

        protected override Task RunAnimationAsync(bool isOpen)
        {
            if (isOpen)
                return ResizeDrawerAsync(DrawerWidthCollapsed, DrawerWidth);
            else
                return ResizeDrawerAsync(DrawerWidth, DrawerWidthCollapsed);
        }

        protected override void OnDrawerFocusGained(object? sender, EventArgs args)
        {
            if (!IsOpened)
                OpenDrawer(true);

            DrawerViewGroup.FocusableChildren = true;
            base.OnDrawerFocusGained(sender, args);
        }

        protected override void OnContentFocusGained(object? sender, EventArgs args)
        {
            if (IsOpened)
                CloseDrawer(true);

            DrawerViewGroup.FocusableChildren = false;
            base.OnContentFocusGained(sender, args);
        }

        protected override bool OnDrawerKeyEventTriggered(object sender, KeyEventArgs args)
        {
            if (args.Key.KeyPressedName == "XF86Back")
            {
                CloseDrawer(true);
                return true;
            }

            if (args.Key.KeyPressedName == "Right")
            {
                if (args.Key.State == Key.StateType.Up)
                {
                    CloseDrawer(true);
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
