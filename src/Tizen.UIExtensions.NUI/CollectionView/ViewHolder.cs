using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using View = Tizen.NUI.BaseComponents.View;

namespace Tizen.UIExtensions.NUI
{
    public enum ViewHolderState
    {
        Normal,
        Selected,
        Focused,
    }

    public class ViewHolder : ViewGroup
    {
        ViewHolderState _state;
        bool _isSelected;
        bool _isFocused;
        bool _isPressed;

        View? _content;

        public ViewHolder()
        {
            Initialize();
        }

        public object? ViewCategory { get; set; }

        public View? Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (_content != null)
                {
                    _content.FocusGained -= OnContentFocused;
                    _content.FocusLost -= OnContentUnfocused;
                    Children.Remove(_content);
                }

                _content = value;

                if (_content != null)
                {
                    _content.WidthSpecification = LayoutParamPolicies.MatchParent;
                    _content.HeightSpecification = LayoutParamPolicies.MatchParent;
                    _content.WidthResizePolicy = ResizePolicyType.FillToParent;
                    _content.HeightResizePolicy = ResizePolicyType.FillToParent;

                    _content.FocusGained += OnContentFocused;
                    _content.FocusLost += OnContentUnfocused;

                    Children.Add(_content);
                }
            }
        }

        public new ViewHolderState State
        {
            get { return _state; }
            set
            {
                if (value == ViewHolderState.Normal)
                    _isSelected = false;
                else if (value == ViewHolderState.Selected)
                    _isSelected = true;

                _state = _isFocused ? ViewHolderState.Focused : (_isSelected ? ViewHolderState.Selected : ViewHolderState.Normal);

                UpdateState();
            }
        }

        public event EventHandler? RequestSelected;

        public event EventHandler? StateUpdated;

        public void UpdateSelected()
        {
            State = ViewHolderState.Selected;
        }

        public void ResetState()
        {
            State = ViewHolderState.Normal;
        }

        protected void Initialize()
        {
            if (Common.DeviceInfo.DeviceType == Common.DeviceType.TV)
            {
                Focusable = true;
            }

            TouchEvent += OnTouchEvent;
            KeyEvent += OnKeyEvent;
            FocusGained += OnFocused;
            FocusLost += OnUnfocused;
            LayoutUpdated += OnLayout;
        }

        void OnLayout(object? sender, Common.LayoutEventArgs e)
        {
            var bounds = this.GetBounds();
            bounds.X = 0;
            bounds.Y = 0;
            foreach (var child in Children)
            {
                child.UpdateBounds(bounds);
            }
        }

        void OnUnfocused(object? sender, EventArgs e)
        {
            _isFocused = false;
            State = _isSelected ? ViewHolderState.Selected : ViewHolderState.Normal;
        }

        void OnFocused(object? sender, EventArgs e)
        {
            _isFocused = true;
            State = ViewHolderState.Focused;
        }

        void OnContentUnfocused(object? sender, EventArgs e)
        {
            OnUnfocused(this, e);
        }

        void OnContentFocused(object? sender, EventArgs e)
        {
            OnFocused(this, e);
        }

        bool OnKeyEvent(object? source, KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "Return" || e.Key.KeyPressedName == "Enter"))
            {
                RequestSelected?.Invoke(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        bool OnTouchEvent(object? source, TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Started)
            {
                _isPressed = true;
                return true;
            }
            else if (e.Touch.GetState(0) == PointStateType.Finished && _isPressed)
            {
                _isPressed = false;
                RequestSelected?.Invoke(this, EventArgs.Empty);
                return true;
            }
            _isPressed = false;
            return false;
        }

        protected virtual void UpdateState()
        {
            if (State == ViewHolderState.Selected)
                _isSelected = true;
            else if (State == ViewHolderState.Normal)
                _isSelected = false;
            else if (State == ViewHolderState.Focused)
                RaiseToTop();

            StateUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
