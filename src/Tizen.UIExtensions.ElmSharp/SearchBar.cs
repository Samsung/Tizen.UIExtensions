using ElmSharp;
using EColor = ElmSharp.Color;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// SearchBar
    /// </summary>
	public class SearchBar : EditfieldEntry
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBar"/> class.
        /// </summary>
        /// <param name="parent">Parent evas object.</param>
		public SearchBar(EvasObject parent) : base(parent)
		{
			EnableClearButton = true;
		}

        /// <summary>
        /// Set the color of clear button
        /// </summary>
        /// <param name="color">The color to be set.</param>
		public void SetClearButtonColor(EColor color)
		{
			ClearButtonColor = color;
		}
	}
}