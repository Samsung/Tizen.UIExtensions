namespace Tizen.UIExtensions.Common.GraphicsView
{
    public interface IEntry
    {
        string Text { get; set; }

        Color TextColor { get; }

        string Placeholder { get; }

        Color PlaceholderColor { get; }

        Color BackgroundColor { get; }
        bool IsFocused { get; }
    }
}
