using System;
using System.Collections.Generic;
using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A Popup provides a class which can be display topmost area
    /// </summary>
    public class Popup : View
    {
        static Layer? s_popupLayer;
        View? _content;

        static Layer PopupLayer
        {
            get
            {
                if (s_popupLayer == null)
                {
                    s_popupLayer = new Layer();
                    Window.Instance.AddLayer(s_popupLayer);
                    s_popupLayer.RaiseToTop();
                }
                return s_popupLayer;
            }
        }
        static List<Popup> s_openedPopup = new List<Popup>();

        public static bool HasOpenedPopup => s_openedPopup.Count > 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Popup"/> class.
        /// </summary>
        public Popup()
        {
            Focusable = true;
            Layout = new AbsoluteLayout();
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;
            TouchEvent += OnTouched;
            KeyEvent += OnKeyEvent;
        }

        /// <summary>
        /// OutsideClicked will be triggered when users taps on the outside of Popup.
        /// </summary>
        public event EventHandler? OutsideClicked;

        /// <summary>
        /// Closed will be triggered when the popup has been closed.
        /// </summary>
        public event EventHandler? Closed;

        /// <summary>
        /// Content of popup
        /// </summary>
        public View? Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    if (_content != null)
                    {
                        _content.Unparent();
                        _content.TouchEvent -= OnContentTouch;
                    }
                    
                    _content = value;
                    if (_content != null)
                    {
                        Add(_content);
                        _content.TouchEvent += OnContentTouch;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates open state
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Open popup
        /// </summary>
        public void Open()
        {
            if (IsOpen)
            {
                return;
            }

            PopupLayer.Add(this);
            IsOpen = true;
            s_openedPopup.Add(this);
            FocusManager.Instance.SetCurrentFocusView(this);
        }

        /// <summary>
        /// Close popup
        /// </summary>
        public void Close()
        {
            if (!IsOpen)
            {
                return;
            }

            PopupLayer.Remove(this);
            IsOpen = false;
            s_openedPopup.Remove(this);
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public static void CloseAll()
        {
            foreach (var popup in s_openedPopup.ToList())
            {
                popup.Close();
            }
        }

        public static void CloseLast()
        {
            s_openedPopup.LastOrDefault()?.Close();
        }

        bool OnKeyEvent(object source, KeyEventArgs e)
        {
            if (IsOpen && e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape"))
            {
                Console.WriteLine($"Popup - OnKeyEvent - {e.Key.KeyPressedName}");
                Close();
                return true;
            }
            return false;
        }

        bool OnTouched(object source, TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                OutsideClicked?.Invoke(this, EventArgs.Empty);
            }
            return true;
        }

        bool OnContentTouch(object source, TouchEventArgs e)
        {
            return true;
        }
    }
}
