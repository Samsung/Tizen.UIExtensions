namespace Tizen.UIExtensions.Common.GraphicsView
{
    public interface ICheckBox
    {
        /// <summary>
        /// Gets a value that indicates whether the CheckBox is checked.
        /// </summary>
        bool IsChecked { get; set; }

        /// <summary>
        /// Gets a Color value that defines the display color.
        /// </summary>
        Color Color { get; }

        Color TextColor { get; }

        string Text { get; }
    }
}
