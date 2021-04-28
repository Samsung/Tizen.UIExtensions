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

    public class ViewHolder : View
    {
        ViewHolderState _state;
        bool _isSelected;
        bool _isFocused;

        View _content;

        public ViewHolder()
        {
            Initialize();
        }

        public object ViewCategory { get; set; }

        public View Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content?.Unparent();
                _content = value;
                if (_content != null)
                {
                    _content.WidthSpecification = LayoutParamPolicies.MatchParent;
                    _content.HeightSpecification = LayoutParamPolicies.MatchParent;
                    Add(_content);
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

        public event EventHandler RequestSelected;

        public event EventHandler StateUpdated;

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
            Layout = new AbsoluteLayout();
            Relayout += OnLayout;
            TouchEvent += OnTouchEvent;
            KeyEvent += OnKeyEvent;
            FocusGained += OnFocused;
            FocusLost += OnUnfocused;
        }

        void OnLayout(object sender, EventArgs e)
        {
            if (Content != null)
            {
                Content.Size = new Size(Size);
            }
        }

        void OnUnfocused(object sender, EventArgs e)
        {
            _isFocused = false;
            State = _isSelected ? ViewHolderState.Selected : ViewHolderState.Normal;
        }

        void OnFocused(object sender, EventArgs e)
        {
            _isFocused = true;
            State = ViewHolderState.Focused;
        }

        bool OnKeyEvent(object source, KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && e.Key.KeyPressedName == "Enter")
            {
                RequestSelected?.Invoke(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        bool OnTouchEvent(object source, TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Finished)
            {
                RequestSelected?.Invoke(this, EventArgs.Empty);
                return true;
            }
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
