using System;
using Tizen.UIExtensions.Common;
using Color = Tizen.UIExtensions.Common.Color;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    public interface ISlider
    {
        double Minimum { get; }

        double Maximum { get; }

        double Value { get; set; }

        Color MinimumTrackColor { get; }

        Color MaximumTrackColor { get; }

        Color ThumbColor { get; }
    }

    /// <summary>
    /// A View control that inputs a linear value.
    /// </summary>
    public class Slider : GraphicsView<SliderDrawable>, ISlider
    {
        /// <summary>
        /// Initializes a new instance of the Slider class.
        /// </summary>
        public Slider()
        {
            Drawable = new SliderDrawable(this);
            SizeHeight = (float)(DeviceInfo.ScalingFactor * 18);
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
        /// Gets or sets the maximum selectable value.
        /// </summary>
        public double Maximum { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the minimum selectable value.
        /// </summary>
        public double Minimum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the color of the portion of the slider track that contains the minimum value of the slider.
        /// </summary>
        public Color MinimumTrackColor
        {
            get => GetProperty<Color>(nameof(MinimumTrackColor));
            set => SetProperty(nameof(MinimumTrackColor), value);
        }

        /// <summary>
        /// Gets or sets the color of the portion of the slider track that contains the maximum value of the slider.
        /// </summary>
        public Color MaximumTrackColor
        {
            get => GetProperty<Color>(nameof(MaximumTrackColor));
            set => SetProperty(nameof(MaximumTrackColor), value);
        }

        /// <summary>
        /// Gets or sets the color of the slider thumb button.
        /// </summary>
        public Color ThumbColor
        {
            get => GetProperty<Color>(nameof(ThumbColor));
            set => SetProperty(nameof(ThumbColor), value);
        }
    }
}
