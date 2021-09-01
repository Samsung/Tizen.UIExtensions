using System;
using ElmSharp;
using EButton = ElmSharp.Button;
using Color = Tizen.UIExtensions.Common.Color;

namespace Tizen.UIExtensions.ElmSharp
{

    /// <summary>
    /// Enumerates the three valid positions of a dialog button.
    /// </summary>
    public enum ButtonPosition
    {
        Positive,
        Neutral,
        Negative
    }

    /// <summary>
    /// Base class for Dialogs.
    /// A dialog is a small window that prompts the user to make a decision or enter additional information.
    /// </summary>
    public class Dialog : Popup
    {
        EButton? _positiveButton;
        EButton? _neutralButton;
        EButton? _negativeButton;
        EvasObject? _content;
        string? _title;
        string? _message;
        Color _titleColor = Common.Color.Default;

        /// <summary>
        ///  Creates a dialog window.
        /// </summary>
        public static Dialog CreateDialog(EvasObject parent, bool hasAcceptButton = false)
        {
            return new Dialog(parent);
        }

        /// <summary>
        /// Creates a dialog window that uses the default dialog theme.
        /// </summary>
        public Dialog(EvasObject parent) : base(parent)
        {
            Initialize();
        }

        /// <summary>
        /// Occurs whenever the dialog is first displayed.
        /// </summary>
#pragma warning disable CS0109
        public new event EventHandler? Shown;
#pragma warning restore CS0109

        /// <summary>
        /// Gets or sets the title of the dialog
        /// </summary>
        public string? Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    ApplyTitle(value);
                }
            }
        }

        public Color TitleColor
        {
            get
            {
                return _titleColor;
            }
            set
            {
                if (_titleColor != value)
                {
                    _titleColor = value;
                    ApplyTitleColor(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the message to display in the dialog
        /// </summary>
        public string? Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    ApplyMessage(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the content to display in that dialog.
        /// </summary>
        public EvasObject? Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    ApplyContent(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the positive button used in the dialog
        /// </summary>
        public EButton? PositiveButton
        {
            get
            {
                return _positiveButton;
            }
            set
            {
                if (_positiveButton != value)
                {
                    _positiveButton = value;
                    ApplyButton(ButtonPosition.Positive, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the neutral button used in the dialog
        /// </summary>
        public EButton? NeutralButton
        {
            get
            {
                return _neutralButton;
            }
            set
            {
                if (_neutralButton != value)
                {
                    _neutralButton = value;
                    ApplyButton(ButtonPosition.Neutral, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the negative button used in the dialog
        /// </summary>
        public EButton? NegativeButton
        {
            get
            {
                return _negativeButton;
            }
            set
            {
                if (_negativeButton != value)
                {
                    _negativeButton = value;
                    ApplyButton(ButtonPosition.Negative, value);
                }
            }
        }

        /// <summary>
        /// Starts the dialog and displays it on screen.
        /// </summary>
        public new void Show()
        {
            base.Show();
            Shown?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the disposing of a dialog widget.
        /// </summary>
        protected override void OnUnrealize()
        {
            _content?.Unrealize();
            _positiveButton?.Unrealize();
            _neutralButton?.Unrealize();
            _negativeButton?.Unrealize();
            ApplyButton(ButtonPosition.Positive, null);
            ApplyButton(ButtonPosition.Neutral, null);
            ApplyButton(ButtonPosition.Negative, null);
            ApplyContent(null);

            base.OnUnrealize();
        }

        /// <summary>
        /// Called when the dialog is shown.
        /// </summary>
        /// <remarks>When shown, the dialog will register itself for the back key press event handling.</remarks>
        protected virtual void OnShown()
        {
        }

        /// <summary>
        /// Called when the dialog is dismissed.
        /// </summary>
        /// <remarks>When dismissed, the dialog will unregister itself from the back key press event handling.</remarks>
        protected virtual void OnDismissed()
        {
        }

        /// <summary>
        /// Changes the dialog title.
        /// </summary>
        /// <param name="title">New dialog title.</param>
        protected virtual void ApplyTitle(string? title)
        {
            this.SetTitleTextPart(title);
        }

        protected virtual void ApplyTitleColor(Color color)
        {
            this.SetTitleColor(color.ToNative());
        }

        /// <summary>
        /// Puts the button in one of the three available slots.
        /// </summary>
        /// <param name="position">The slot to be occupied by the button expressed as a <see cref="ButtonPosition"/></param>
        /// <param name="button">The new button.</param>
        protected virtual void ApplyButton(ButtonPosition position, EButton? button)
        {
            if (button is Button tButton)
            {
                tButton.SetPopupStyle();
            }

            switch (position)
            {
                case ButtonPosition.Positive:
                    this.SetButton3Part(button, true);
                    break;

                case ButtonPosition.Neutral:
                    this.SetButton2Part(button, true);
                    break;

                default:
                    this.SetButton1Part(button, true);
                    break;
            }
        }

        /// <summary>
        /// Updates the content of the dialog.
        /// </summary>
        /// <param name="content">New dialog content.</param>
        protected virtual void ApplyContent(EvasObject? content)
        {
            this.SetContentPart(content, true);
        }

        protected virtual void ApplyMessage(string? message)
        {
            base.Text = message;
        }

        /// <summary>
        /// Handles the initialization process.
        /// </summary>
        /// <remarks>Creates handlers for vital events</remarks>
        void Initialize()
        {
            // Adds a handler for the Dismissed event.
            // In effect, unregisters this instance from being affected by the hardware back key presses.
            Dismissed += (s, e) =>
            {
                OnDismissed();
            };

            // Adds a handler for the Shown event.
            // In effect, registers this instance to be affected by the hardware back key presses.
            Shown += (s, e) =>
            {
                OnShown();
            };
        }
    }
}
