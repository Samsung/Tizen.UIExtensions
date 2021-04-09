using System;
using Tizen.UIExtensions.Common;
using ImageView = Tizen.NUI.BaseComponents.ImageView;
using CSize = Tizen.UIExtensions.Common.Size;

namespace Tizen.UIExtensions.NUI
{
    public static class ImageExtensions
    {

        /// <summary>
        /// Sets the scaling mode for the image.
        /// </summary>
        /// <param name="view">Target view</param>
        /// <param name="apect">A Aspect representing the scaling mode of the image.</param>
        public static void SetAspect(this ImageView view, Aspect apect)
        {
            view.FittingMode = apect switch
            {
                Aspect.AspectFill => Tizen.NUI.FittingModeType.ScaleToFill,
                Aspect.AspectFit => Tizen.NUI.FittingModeType.ShrinkToFit,
                _ => Tizen.NUI.FittingModeType.Fill,
            };
        }

        /// <summary>
        /// Gets the scaling mode for the image.
        /// </summary>
        /// <param name="view">Target view</param>
        /// <returns>A Aspect representing the scaling mode of the image.</returns>
        public static Aspect GetAspect(this ImageView view)
        {
            return view.FittingMode switch
            {
                Tizen.NUI.FittingModeType.ScaleToFill => Aspect.AspectFill,
                Tizen.NUI.FittingModeType.ShrinkToFit => Aspect.AspectFit,
                _ => Aspect.Fill,
            };
        }

        /// <summary>
        /// Set animation playing state
        /// </summary>
        /// <param name="view">Image view</param>
        /// <param name="value">playing state</param>
        /// <remarks>Should be called after image loading</remarks>
        public static void SetIsAnimationPlaying(this ImageView view, bool value)
        {
            if (value)
            {
                view.Play();
            }
            else
            {
                view.Stop();
            }
        }

        /// <summary>
        /// Measures the size of the control in order to fit it into the available area.
        /// </summary>
        /// <param name="availableWidth">Available width.</param>
        /// <param name="availableHeight">Available height.</param>
        /// <returns>Size of the control that fits the available area.</returns>
        public static CSize Measure(this ImageView view, double availableWidth, double availableHeight)
        {
            var imageSize = new CSize { Width = view.NaturalSize.Width, Height = view.NaturalSize.Height };
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
