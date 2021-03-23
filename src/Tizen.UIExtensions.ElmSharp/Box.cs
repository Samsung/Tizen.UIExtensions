using System;
using ElmSharp;
using EBox = ElmSharp.Box;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// Extends the ElmSharp.Box class with functionality useful to custom layout.
    /// </summary>
    /// <remarks>
    /// This class overrides the layout mechanism. Instead of using the native layout,
    /// <c>LayoutUpdated</c> event is sent.
    /// </remarks>
    public class Box : EBox
    {
        public Box(EvasObject parent) : base(parent)
        {
            SetLayoutCallback(() => { NotifyOnLayout(); });
        }

        /// <summary>
        /// Notifies that the layout has been updated.
        /// </summary>
        public event EventHandler<LayoutEventArgs> LayoutUpdated;

        /// <summary>
        /// Triggers the <c>LayoutUpdated</c> event.
        /// </summary>
        /// <remarks>
        /// This method is called whenever there is a possibility that the size and/or position has been changed.
        /// </remarks>
        void NotifyOnLayout()
        {
            LayoutUpdated?.Invoke(this, new LayoutEventArgs() { Geometry = Geometry });
        }
    }
}
