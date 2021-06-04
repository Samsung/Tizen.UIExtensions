using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI.GraphicsView
{
    public interface IActivityIndicator
    {
        bool IsRunning { get; }

        Color Color { get; }
    }

    /// <summary>
    /// A visual control used to indicate that something is ongoing.
    /// </summary>
    public class ActivityIndicator : GraphicsView<ActivityIndicatorDrawable>, IActivityIndicator
    {
        /// <summary>
        /// Initializes a new instance of the ActivityIndicator class.
        /// </summary>
        public ActivityIndicator()
        {
            Drawable = new ActivityIndicatorDrawable(this);
        }

        /// <summary>
        /// Gets or sets the value indicating if the ActivityIndicator is running.
        /// </summary>
        public bool IsRunning
        {
            get => GetProperty<bool>(nameof(IsRunning));
            set 
            {
                SetProperty(nameof(IsRunning), value);
                Drawable?.UpdateAnimation(value);
            }
        }

        /// <summary>
        /// Gets or sets the Color of the ActivityIndicator.
        /// </summary>
        public new Color Color
        {
            get => GetProperty<Color>(nameof(Color));
            set => SetProperty(nameof(Color), value);
        }
    }
}
