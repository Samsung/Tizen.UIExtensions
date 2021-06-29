namespace Tizen.UIExtensions.Common.GraphicsView
{
    public interface IButton
    {
        bool IsPressed { get; }

        string Text { get; }

        Color TextColor { get; }

        Color BackgroundColor { get; }

        double CornerRadius { get; }
    }
}
