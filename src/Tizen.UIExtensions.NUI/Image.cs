using System.IO;
using Tizen.NUI;
using Tizen.UIExtensions.Common;
using CSize = Tizen.UIExtensions.Common.Size;
using ImageView = Tizen.NUI.BaseComponents.ImageView;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// View that holds an image.
    /// </summary>
    public class Image : ImageView, IMeasurable
    {
        ImageUrl? _cachedImageUrl;
        /// <summary>
        /// Gets or sets the scaling mode for the image.
        /// </summary>
        public Aspect Aspect
        {
            get => this.GetAspect();
            set => this.SetAspect(value);
        }

        /// <summary>
        /// Load Image from stream
        /// </summary>
        /// <param name="stream">The stream containing images</param>
        public void Load(Stream stream)
        {
            using var imageBuffer = new EncodedImageBuffer(stream);
            _cachedImageUrl?.Dispose();
            _cachedImageUrl = imageBuffer.GenerateUrl();
            ResourceUrl = _cachedImageUrl.ToString();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cachedImageUrl?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
