using ElmSharp;
using SkiaSharp.Views.Tizen;
using System;
using System.Runtime.InteropServices;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// A clipper area drawing view, it used for clipping
    /// </summary>
    public class SKClipperView : SKCanvasView
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SKClipperView"/> class.
        /// </summary>
        /// <param name="parent">Parent of this instance.</param>
		public SKClipperView(EvasObject parent) : base(parent) { }

		public bool ClippingRequired { get; set; }

        /// <summary>
        /// Invalidate clipping area
        /// </summary>
		public new void Invalidate()
		{
			ClippingRequired = true;
			OnDrawFrame();
			ClippingRequired = false;
		}
	}

	public static class ClipperExtension
	{
        /// <summary>
        /// Set Clipper canvas
        /// </summary>
        /// <param name="target">A target view to clip</param>
        /// <param name="clipper">A clip area</param>
		public static void SetClipperCanvas(this EvasObject target, SKClipperView clipper)
		{
			if (target != null && clipper.ClippingRequired)
			{
				var realHandle = elm_object_part_content_get(clipper, "elm.swallow.content");

				target.SetClip(null); // To restore original image
				evas_object_clip_set(target, realHandle);
			}
		}

		[DllImport("libevas.so.1")]
		internal static extern void evas_object_clip_set(IntPtr obj, IntPtr clip);

		[DllImport("libelementary.so.1")]
		internal static extern IntPtr elm_object_part_content_get(IntPtr obj, string part);
	}
}
