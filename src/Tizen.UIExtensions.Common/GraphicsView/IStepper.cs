namespace Tizen.UIExtensions.Common.GraphicsView
{
    public interface IStepper
    {
        public double Minimum { get; }
        public double Maximum { get; }

        public double Increment { get; }

        public double Value { get; set; }
    }
}
