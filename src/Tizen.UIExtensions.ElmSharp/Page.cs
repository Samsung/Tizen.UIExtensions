using System;
using System.Collections.Generic;
using ElmSharp;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// A basic page which can hold a single view.
    /// </summary>
    public class Page : Background, IContainable<EvasObject>
    {

        /// <summary>
        /// Exposes the Children property, mapping it to the _canvas' Children property.
        /// </summary>
        public new IList<EvasObject> Children => _canvas.Children;

        /// <summary>
        /// The canvas, used as a container for other objects.
        /// </summary>
        /// <remarks>
        /// The canvas holds all the Views that the ContentPage is composed of.
        /// </remarks>
        internal Canvas _canvas;

        /// <summary>
        /// Initializes a new instance of the ContentPage class.
        /// </summary>
        public Page(EvasObject parent) : base(parent)
        {
            _canvas = new Canvas(this);
            this.SetOverlayPart(_canvas);
        }

        /// <summary>
        /// Allows custom handling of events emitted when the layout has been updated.
        /// </summary>
        public event EventHandler<LayoutEventArgs> LayoutUpdated
        {
            add
            {
                _canvas.LayoutUpdated += value;
            }
            remove
            {
                _canvas.LayoutUpdated -= value;
            }
        }

        /// <summary>
        /// Handles the disposing of a ContentPage
        /// </summary>
        /// <remarks>
        /// Takes the proper care of discarding the canvas, then calls the base method.
        /// </remarks>
        protected override void OnUnrealize()
        {
            _canvas.Unrealize();
            base.OnUnrealize();
        }
    }
}
