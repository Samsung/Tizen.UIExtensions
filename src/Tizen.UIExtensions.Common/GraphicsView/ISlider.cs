namespace Tizen.UIExtensions.Common.GraphicsView
{
    public interface ISlider
    {
        double Minimum { get; }

        double Maximum { get; }

        double Value { get; set; }

        Color MinimumTrackColor { get; }

        Color MaximumTrackColor { get; }

        Color ThumbColor { get; }
    }
}
