namespace Tizen.UIExtensions.Common.GraphicsView
{
    public interface ISwitch
    {
        bool IsToggled { get; set; }

        Color OnColor { get; }

        Color ThumbColor { get; }

        Color BackgroundColor { get; }
    }
}
