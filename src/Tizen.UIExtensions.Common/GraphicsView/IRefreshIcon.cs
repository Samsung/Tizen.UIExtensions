namespace Tizen.UIExtensions.Common.GraphicsView
{
    public interface IRefreshIcon
    {
        bool IsRunning { get; }

        bool IsPulling { get; }

        Color Color { get; }

        float PullDistance { get; }
    }
}
