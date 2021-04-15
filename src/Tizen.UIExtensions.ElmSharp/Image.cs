using ElmSharp;
using EImage = ElmSharp.Image;
using ESize = ElmSharp.Size;
using Tizen.UIExtensions.Common;
using CSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// Extends the ElmSharp.Image class with functionality useful to renderer.
    /// </summary>
    public class Image : EImage, IMeasurable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="parent">The parent EvasObject.</param>
        public Image(EvasObject parent) : base(parent)
        {
        }

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
        public CSize Measure(double availableWidth, double availableHeight)
        {
            var imageSize = ObjectSize;
            var size = new CSize()
            {
                Width = imageSize.Width,
                Height = imageSize.Height,
            };

            if (0 != availableWidth && 0 != availableHeight
                && (imageSize.Width > availableWidth || imageSize.Height > availableHeight))
            {
                // when available size is limited and insufficient for the image ...
                double imageRatio = imageSize.Width / imageSize.Height;
                double availableRatio = availableWidth / availableHeight;
                // depending on the relation between availableRatio and imageRatio, copy the availableWidth or availableHeight
                // and calculate the size which preserves the image ratio, but does not exceed the available size
                size.Width = availableRatio > imageRatio ? imageSize.Width * availableHeight / imageSize.Height : availableWidth;
                size.Height = availableRatio > imageRatio ? availableHeight : imageSize.Height * availableWidth / imageSize.Width;
            }

            return new CSize();
        }
    }
}