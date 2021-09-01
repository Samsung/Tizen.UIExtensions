using System;
using ElmSharp;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The native widget which provides Xamarin.FlyoutPage features.
    /// </summary>
    public class FlyoutPage : DrawerBox
    {
        /// <summary>
        /// The default flyout layout behavior (a.k.a mode).
        /// </summary>
        static readonly FlyoutLayoutBehavior s_defaultFlyoutLayoutBehavior = (DeviceInfo.IsMobile || DeviceInfo.IsWatch) ? FlyoutLayoutBehavior.Popover : FlyoutLayoutBehavior.SplitOnLandscape;

        /// <summary>
        /// The <see cref="FlyoutLayoutBehavior"/> property value.
        /// </summary>
        FlyoutLayoutBehavior _flyoutLayoutBehavior = s_defaultFlyoutLayoutBehavior;

        /// <summary>
        /// The <see cref="Orientation"/> property value.
        /// </summary>
        DeviceOrientation _orientation = DeviceOrientation.Portrait;

        public FlyoutPage(EvasObject parent) : base(parent)
        {
            Toggled += (object? sender, EventArgs e) =>
            {
                IsPresentedChanged?.Invoke(this, new IsPresentedChangedEventArgs(IsPresented));
            };
        }

        /// <summary>
        /// Occurs when the Flyout is shown or hidden.
        /// </summary>
        public event EventHandler<IsPresentedChangedEventArgs>? IsPresentedChanged;

        /// <summary>
        /// Occurs when the IsPresentChangeable was changed.
        /// </summary>
        public event EventHandler<UpdateIsPresentChangeableEventArgs>? UpdateIsPresentChangeable;

        /// <summary>
        /// Gets or sets a value indicating whether the Flyout is shown.
        /// </summary>
        /// <value><c>true</c> if the Flyout is presented.</value>
        public bool IsPresented
        {
            get
            {
                return IsOpen;
            }
            set
            {
                IsOpen = value;
            }
        }

        /// <summary>
        /// Gets or sets the content of the DetailPage.
        /// </summary>
        /// <value>The DetailPage.</value>
        public EvasObject? Detail
        {
            get
            {
                return Content;
            }
            set
            {
                Content = value;
            }
        }

        /// <summary>
        /// Gets or sets the content of the Flyout.
        /// </summary>
        /// <value>The Flyout.</value>
        public EvasObject? Flyout
        {
            get
            {
                return Drawer;
            }
            set
            {
                Drawer = value;
            }
        }

        /// <summary>
        /// Gets or sets the orientation of the screen.
        /// </summary>
        /// <value><c>true</c> if the Flyout is presented.</value>
        public DeviceOrientation Orientation
        {
            get
            {
                return _orientation;
            }

            set
            {
                if (_orientation != value)
                {
                    _orientation = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the FlyoutPage behavior.
        /// </summary>
        /// <value>The behavior of the <c>FlyoutPage</c> requested by the user.</value>
        public FlyoutLayoutBehavior FlyoutLayoutBehavior
        {
            get
            {
                return _flyoutLayoutBehavior;
            }

            set
            {
                _flyoutLayoutBehavior = value;
                UpdateFlyoutLayoutBehavior();
            }
        }

        void UpdateFlyoutLayoutBehavior()
        {
            var behavior = (_flyoutLayoutBehavior == FlyoutLayoutBehavior.Default) ? s_defaultFlyoutLayoutBehavior : _flyoutLayoutBehavior;

            // Screen orientation affects those 2 behaviors
            if (behavior == FlyoutLayoutBehavior.SplitOnLandscape || behavior == FlyoutLayoutBehavior.SplitOnPortrait)
            {
                if ((_orientation == DeviceOrientation.Landscape && behavior == FlyoutLayoutBehavior.SplitOnLandscape) || (_orientation == DeviceOrientation.Portrait && behavior == FlyoutLayoutBehavior.SplitOnPortrait))
                {
                    behavior = FlyoutLayoutBehavior.Split;
                }
                else
                {
                    behavior = FlyoutLayoutBehavior.Popover;
                }
            }

            IsSplit = (behavior == FlyoutLayoutBehavior.Split);
            UpdateIsPresentChangeable?.Invoke(this, new UpdateIsPresentChangeableEventArgs(!IsSplit));
        }
    }

    public class IsPresentedChangedEventArgs : EventArgs
    {
        public IsPresentedChangedEventArgs(bool isPresent)
        {
            IsPresent = isPresent;
        }

        /// <summary>
        /// Value of IsPresent
        /// </summary>
        public bool IsPresent { get; private set; }
    }

    public class UpdateIsPresentChangeableEventArgs : EventArgs
    {
        public UpdateIsPresentChangeableEventArgs(bool canChange)
        {
            CanChange = canChange;
        }

        /// <summary>
        /// Value of changeable
        /// </summary>
        public bool CanChange { get; private set; }
    }
}
