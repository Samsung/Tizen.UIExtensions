using System;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    /// <summary>
    /// A button View that reacts to touch events.
    /// </summary>
    public class MaterialIconButton : GraphicsView<MaterialIconDrawable>
    {
        /// <summary>
        /// Initializes a new instance of the Button class.
        /// </summary>
        public MaterialIconButton()
        {
            Focusable = true;
            Drawable = new MaterialIconDrawable();
            var measured = Drawable.Measure(double.PositiveInfinity, double.PositiveInfinity);
            SizeWidth = (float)measured.Width;
            SizeHeight = (float)measured.Height;
            KeyEvent += OnKeyEvent;
        }

        /// <summary>
        /// Occurs when the Button is clicked.
        /// </summary>
        public event EventHandler? Clicked;

        /// <summary>
        /// Occurs when the Button is pressed.
        /// </summary>
        public event EventHandler? Pressed;

        /// <summary>
        /// Occurs when the Button is released.
        /// </summary>
        public event EventHandler? Released;

        public bool IsPressed
        {
            get => GetProperty<bool>(nameof(IsPressed));
            set => SetProperty(nameof(IsPressed), value);
        }

        /// <summary>
        /// Icon type
        /// </summary>
        public MaterialIcons Icon
        {
            get => Drawable?.Icon ?? MaterialIcons.Add;
            set
            {
                if (Drawable != null)
                {
                    Drawable.Icon = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Color of Icons
        /// </summary>
        public new Color Color
        {
            get => Drawable?.Color ?? Color.Default;
            set
            {
                if (Drawable != null)
                {
                    Drawable.Color = value;
                    Invalidate();
                }
            }
        }

        protected override bool OnTouch(object source, TouchEventArgs e)
        {
            if (!IsEnabled)
                return false;

            var consume = base.OnTouch(source, e);
            var state = e.Touch.GetState(0);

            if (state == Tizen.NUI.PointStateType.Down)
            {
                IsPressed = true;
                Pressed?.Invoke(this, EventArgs.Empty);
            }
            else if (state == Tizen.NUI.PointStateType.Up)
            {
                IsPressed = false;
                Released?.Invoke(this, EventArgs.Empty);
                if (this.IsInside(e.Touch.GetLocalPosition(0)))
                {
                    Clicked?.Invoke(this, EventArgs.Empty);
                }
            }
            return consume;
        }

        bool OnKeyEvent(object source, KeyEventArgs e)
        {
            if (e.Key.IsAcceptKeyEvent())
            {
                Clicked?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}
