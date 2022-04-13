﻿using System;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    /// <summary>
    /// A View control that provides a toggled value.
    /// </summary>
    public class Switch : GraphicsView<SwitchDrawable>, ISwitch
    {
        /// <summary>
        /// Initializes a new instance of the Switch class.
        /// </summary>
        public Switch()
        {
            Focusable = true;
            Drawable = new SwitchDrawable(this);
            KeyEvent += OnKeyEVent;
        }

        /// <summary>
        /// Event that is raised when this Switch is toggled.
        /// </summary>
        public event EventHandler? Toggled;

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether this Switch element is toggled.
        /// </summary>
        public bool IsToggled
        {
            get => GetProperty<bool>(nameof(IsToggled));
            set
            {
                SetProperty(nameof(IsToggled), value);
                Toggled?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the color of the switch when it is in the "On" position.
        /// </summary>
        public Color OnColor
        {
            get => GetProperty<Color>(nameof(OnColor));
            set => SetProperty(nameof(OnColor), value);
        }

        /// <summary>
        /// Gets or sets the color of the thumb
        /// </summary>
        public Color ThumbColor
        {
            get => GetProperty<Color>(nameof(ThumbColor));
            set => SetProperty(nameof(ThumbColor), value);
        }

        /// <summary>
        /// Gets or sets the color which will fill the background.
        /// </summary>
        public new Color BackgroundColor
        {
            get => GetProperty<Color>(nameof(BackgroundColor));
            set => SetProperty(nameof(BackgroundColor), value);
        }

        bool OnKeyEVent(object source, KeyEventArgs e)
        {
            if (e.Key.State == Tizen.NUI.Key.StateType.Up && (e.Key.KeyPressedName == "Return" || e.Key.KeyPressedName == "Enter"))
            {
                IsToggled = !IsToggled;
                return true;
            }
            return false;
        }
    }
}
