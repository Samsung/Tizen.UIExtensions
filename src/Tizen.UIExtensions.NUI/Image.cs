using System;
using ImageView = Tizen.NUI.BaseComponents.ImageView;
using Tizen.UIExtensions.Common;
using CSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// View that holds an image.
    /// </summary>
    public class Image : ImageView, IMeasurable
    {

        /// <summary>
        /// Gets or sets the scaling mode for the image.
        /// </summary>
        public Aspect Aspect
        {
            get => this.GetAspect();
            set => this.SetAspect(value);
        }

        /// <summary>
        /// Measures the size of the control in order to fit it into the available area.
        /// </summary>
        /// <param name="availableWidth">Available width.</param>
        /// <param name="availableHeight">Available height.</param>
        /// <returns>Size of the control that fits the available area.</returns>
        CSize IMeasurable.Measure(double availableWidth, double availableHeight)
        {
            return this.Measure(availableWidth, availableHeight);
        }
    }
}
