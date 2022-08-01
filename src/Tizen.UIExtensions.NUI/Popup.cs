using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// Base class for custom popup
    /// </summary>
    /// <typeparam name="T">A type to return on Popup</typeparam>
    public abstract class Popup<T> : Popup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Popup"/> class.
        /// </summary>
        protected Popup()
        {
            Closed += OnClosed;
        }

        /// <summary>
        /// A TaskCompletionSource for result of popup
        /// </summary>
        protected TaskCompletionSource<T>? ResponseTcs { get; set; }

        /// <summary>
        /// Open popup
        /// </summary>
        /// <returns>The return value on Popup</returns>
        public new async Task<T> Open()
        {
            if (ResponseTcs != null && !ResponseTcs.Task.IsCompleted)
                return await ResponseTcs.Task;

            ResponseTcs = new TaskCompletionSource<T>();

            if (Content == null)
                Content = CreateContent();

            base.Open();

            try
            {
                return await ResponseTcs.Task;
            }
            finally
            {
                ResponseTcs = null;
                Close();
            }
        }

        /// <summary>
        /// Create content of popup
        /// </summary>
        /// <returns>Content View</returns>
        protected abstract View CreateContent();

        /// <summary>
        /// Submit value to return
        /// </summary>
        /// <param name="value">The value to submit</param>
        protected void SendSubmit(T value)
        {
            ResponseTcs?.TrySetResult(value);
        }

        /// <summary>
        /// Cancel popup
        /// </summary>
        protected void SendCancel()
        {
            ResponseTcs?.TrySetCanceled();
        }

        void OnClosed(object? sender, EventArgs e)
        {
            SendCancel();
        }
    }

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
            var focusable = FindFocusableChild(this);
            if (focusable != null)
                FocusManager.Instance.SetCurrentFocusView(focusable);
            else
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

        /// <summary>
        /// Actions when back button was pressed
        /// A default behavior is closing popup
        /// </summary>
        /// <returns>if you consume back button pressed event returing true, otherwise false</returns>
        protected virtual bool OnBackButtonPressed()
        {
            Close();
            return true;
        }

        public static void CloseAll()
        {
            foreach (var popup in s_openedPopup.ToList())
            {
                popup.Close();
            }
        }

        public static bool CloseLast()
        {
            return s_openedPopup.LastOrDefault()?.BackButtonPressed() ?? false;
        }

        bool BackButtonPressed()
        {
            return OnBackButtonPressed();
        }

        bool OnKeyEvent(object source, KeyEventArgs e)
        {
            if (IsOpen && e.Key.IsDeclineKeyEvent())
            {
                return OnBackButtonPressed();
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

        View? FindFocusableChild(View view)
        {
            if (view.Focusable && !(view is Popup))
                return view;

            foreach (var child in view.Children)
            {
                var focusable = FindFocusableChild(child);
                if (focusable != null)
                    return focusable;
            }
            return null;
        }
    }
}
