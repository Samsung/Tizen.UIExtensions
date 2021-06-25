using System.Threading.Tasks;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.Internal;
using Tizen.UIExtensions.Common.GraphicsView;

namespace Tizen.UIExtensions.NUI.GraphicsView
{


    /// <summary>
    /// A View control that displays progress.
    /// </summary>
    public class ProgressBar : GraphicsView<ProgressBarDrawable>, IProgressBar, IAnimatable
    {
        /// <summary>
        /// Initializes a new instance of the ProgressBar class
        /// </summary>
        public ProgressBar()
        {
            Drawable = new ProgressBarDrawable(this);
        }

        /// <summary>
        /// Gets or sets the progress value.
        /// </summary>
        public double Progress 
        {
            get => GetProperty<double>(nameof(Progress));
            set => SetProperty(nameof(Progress), value);
        }

        /// <summary>
        /// Get or sets the color of the progress bar.
        /// </summary>
        public Color ProgressColor
        {
            get => GetProperty<Color>(nameof(ProgressColor));
            set => SetProperty(nameof(ProgressColor), value);
        }

        /// <summary>
        ///  Animate the Progress property to value.
        /// </summary>
        public Task<bool> ProgressTo(double value)
        {
            var tcs = new TaskCompletionSource<bool>();
            this.Animate("Progress", d => Progress = d, Progress, value, finished: (d, finished) => tcs.SetResult(finished));
            return tcs.Task;
        }

        void IAnimatable.BatchBegin() { }
        void IAnimatable.BatchCommit() { }
    }
}
