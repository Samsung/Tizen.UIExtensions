using System;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    public interface IStepper
    {
        public double Minimum { get; }
        public double Maximum { get; }

        public double Increment { get; }

        public double Value { get; set; }
    }

    /// <summary>
    /// A View control that inputs a discrete value, constrained to a range.
    /// </summary>
    public class Stepper : GraphicsView<StepperDrawable>, IStepper
    {
        /// <summary>
        /// Initializes a new instance of the Stepper class.
        /// </summary>
        public Stepper()
        {
            Value = 0;
            Maximum = 10;
            Minimum = 0;
            Increment = 1;
            Drawable = new StepperDrawable(this);
        }

        /// <summary>
        /// Raised when the Value property changes.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public double Value
        {
            get => GetProperty<double>(nameof(Value));
            set
            {
                SetProperty(nameof(Value), value.Clamp(Minimum, Maximum));
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the increment by which Value is increased or decreased. 
        /// </summary>
        public double Increment
        {
            get => GetProperty<double>(nameof(Increment));
            set => SetProperty(nameof(Increment), value);
        }

        /// <summary>
        /// Gets or sets the minimum selectabel value.
        /// </summary>
        public double Minimum
        {
            get => GetProperty<double>(nameof(Minimum));
            set => SetProperty(nameof(Minimum), value);
        }

        /// <summary>
        /// Gets or sets the maximum selectable value.
        /// </summary>
        public double Maximum
        {
            get => GetProperty<double>(nameof(Maximum));
            set => SetProperty(nameof(Maximum), value);
        }
    }
}
