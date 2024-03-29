﻿using System;

namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// Holds information about size of the area which can be used for layout.
    /// </summary>
    public class LayoutEventArgs : EventArgs
    {
        /// <summary>
        /// Geometry of the layout area, absolute coordinate
        /// </summary>
        public Rect Geometry
        {
            get;
            set;
        }
    }
}
