using System;

namespace Tizen.UIExtensions.NUI
{

    /// <summary>
    /// Holds information about size of the measure constraint
    /// </summary>
    public class MeasureRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Width constraint
        /// </summary>
        public double WidthConstraint { get; set; }

        /// <summary>
        /// Height constraint
        /// </summary>
        public double HeightConstraint { get; set; }
    }
}
