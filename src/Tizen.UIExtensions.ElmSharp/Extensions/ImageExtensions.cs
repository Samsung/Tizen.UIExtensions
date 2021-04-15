using Aspect = Tizen.UIExtensions.Common.Aspect;
using EImage = ElmSharp.Image;

namespace Tizen.UIExtensions.ElmSharp
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Sets the scaling mode for the image.
        /// </summary>
        /// <param name="view">Target view</param>
        /// <param name="aspect">A Aspect representing the scaling mode of the image.</param>
        public static void SetAspect(this EImage view, Aspect aspect)
        {
            switch (aspect)
            {
                case Aspect.AspectFit:
                    view.IsFixedAspect = true;
                    view.CanFillOutside = false;
                    break;
                case Aspect.AspectFill:
                    view.IsFixedAspect = true;
                    view.CanFillOutside = true;
                    break;
                case Aspect.Fill:
                    view.IsFixedAspect = false;
                    view.CanFillOutside = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets the scaling mode for the image.
        /// </summary>
        /// <param name="view">Target view</param>
        /// <returns>A Aspect representing the scaling mode of the image.</returns>
        public static Aspect GetAspect(this EImage view)
        {
            if (view.IsFixedAspect && !view.CanFillOutside)
                return Aspect.AspectFit;
            if (view.IsFixedAspect && view.CanFillOutside)
                return Aspect.AspectFill;
            if (!view.IsFixedAspect)
                return Aspect.Fill;
            return Aspect.AspectFit;
        }

        /// <summary>
        /// Set animation playing state
        /// </summary>
        /// <param name="view">Image view</param>
        /// <param name="value">playing state</param>
        /// <remarks>Should be called after image loading</remarks>
        public static void SetIsAnimationPlaying(this EImage view, bool value)
        {
            view.IsAnimated = value;
            view.IsAnimationPlaying = value;
        }

    }
}
