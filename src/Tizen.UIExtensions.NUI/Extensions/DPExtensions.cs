using System;
using DeviceInfo = Tizen.UIExtensions.Common.DeviceInfo;

namespace Tizen.UIExtensions.NUI
{
    public static class DPExtensions
    {
        public static int ToPixel(this double dp)
        {
            return (int)Math.Round(dp * DeviceInfo.DPI / 160.0);
        }

        public static int ToScaledPixel(this double dp)
        {
            return (int)Math.Round(dp * DeviceInfo.ScalingFactor);
        }

        public static double ToScaledDP(this int pixel)
        {
            if (pixel == int.MaxValue)
                return double.PositiveInfinity;
            return pixel / DeviceInfo.ScalingFactor;
        }

        public static double ToScaledDP(this double pixel)
        {
            return pixel / DeviceInfo.ScalingFactor;
        }

        public static int ConvertToPixel(double dp)
        {
            return (int)Math.Round(dp * DeviceInfo.DPI / 160.0);
        }

        public static int ConvertToScaledPixel(double dp)
        {
            return (int)Math.Round(dp * DeviceInfo.ScalingFactor);
        }

        public static double ConvertToScaledDP(int pixel)
        {
            return pixel / DeviceInfo.ScalingFactor;
        }

        public static double ConvertToScaledDP(double pixel)
        {
            return pixel / DeviceInfo.ScalingFactor;
        }
    }
}
