namespace Tizen.UIExtensions.Common.GraphicsView
{
    /// <summary>
    /// An interface defining properties for RefreshIcon.
    /// </summary>
    public interface IRefreshIcon
    {
        bool IsRunning { get; }

        bool IsPulling { get; }

        Color Color { get; }

        float PullDistance { get; }
    }
}
