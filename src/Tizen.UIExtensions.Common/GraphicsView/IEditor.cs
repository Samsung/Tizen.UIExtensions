namespace Tizen.UIExtensions.Common.GraphicsView
{
    public interface IEditor
    {
        string Text { get; set; }

        Color TextColor { get; }

        string Placeholder { get; }

        Color PlaceholderColor { get; }

        Color BackgroundColor { get; }
        bool IsFocused { get; }
    }
}
