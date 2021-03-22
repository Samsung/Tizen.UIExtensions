using System.Collections.Generic;

namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// Interface defining methods for managing elements of the container.
    /// </summary>
    /// <typeparam name="T">The type of element that can be added to the container.</typeparam>
    public interface IContainable<T>
    {
        /// <summary>
        /// The children collection of an element.
        /// </summary>
        IList<T> Children { get; }
    }
}
