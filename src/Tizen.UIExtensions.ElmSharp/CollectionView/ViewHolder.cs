using System;
using ElmSharp;
using EColor = ElmSharp.Color;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
	public class ViewHolder : Box
	{
		Button _focusArea;
		EvasObject? _content;
		ViewHolderState _state;
		bool _isSelected;
		bool _isFocused;
		bool _focusable;

#pragma warning disable CS8618
        public ViewHolder(EvasObject parent) : base(parent)
#pragma warning restore CS8618
        {
			Initialize(parent);
		}

		public object ViewCategory { get; set; }

		public EvasObject? Content
		{
			get
			{
				return _content;
			}
			set
			{
				if (_content != null)
				{
					UnPack(_content);
				}
				_content = value;
				if (_content != null)
				{
					PackEnd(_content);
					_content.StackBelow(_focusArea);
				}
			}
		}

		public bool AllowItemFocus
		{
			get => _focusable;
			set
			{
				_focusable = value;
				if (!value && _focusArea.IsFocused)
				{
					_focusArea.SetFocus(false);
				}
				_focusArea.AllowFocus(_focusable);
			}
		}

		public ViewHolderState State
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

		public void ResetState()
		{
			State = ViewHolderState.Normal;
		}

		protected void Initialize(EvasObject parent)
		{
			SetLayoutCallback(OnLayout);

			_focusArea = new Button(parent);
			_focusArea.Color = EColor.Transparent;
			_focusArea.BackgroundColor = EColor.Transparent;
			_focusArea.SetEffectColor(EColor.Transparent);
			_focusArea.Clicked += OnClicked;
			_focusArea.Focused += OnFocused;
			_focusArea.Unfocused += OnUnfocused;
			_focusArea.KeyUp += OnKeyUp;
			_focusArea.RepeatEvents = true;
			_focusArea.Show();
			_focusArea.AllowFocus(_focusable);

			PackEnd(_focusArea);
			Show();
		}

		protected virtual void OnFocused(object? sender, EventArgs e)
		{
			UpdateFocusState();
		}

		protected virtual void OnUnfocused(object? sender, EventArgs e)
		{
			UpdateFocusState();
		}

		protected virtual void OnClicked(object? sender, EventArgs e)
		{
			RequestSelected?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnLayout()
		{
			_focusArea.Geometry = Geometry;
			if (_content != null)
			{
				_content.Geometry = Geometry;
			}
		}

		protected virtual void UpdateState()
		{
			if (State == ViewHolderState.Selected)
				_isSelected = true;
			else if (State == ViewHolderState.Normal)
				_isSelected = false;
			else if (State == ViewHolderState.Focused)
				RaiseTop();

			StateUpdated?.Invoke(this, EventArgs.Empty);
		}

		void UpdateFocusState()
		{
			if (_focusArea.IsFocused)
			{
				_isFocused = true;
				State = ViewHolderState.Focused;
			}
			else
			{
				_isFocused = false;
				State = _isSelected ? ViewHolderState.Selected : ViewHolderState.Normal;
			}
		}

		void OnKeyUp(object? sender, EvasKeyEventArgs e)
		{
			if (e.KeyName.IsEnterKey() && _focusArea.IsFocused)
			{
				RequestSelected?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
