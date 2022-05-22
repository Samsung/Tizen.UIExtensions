using System;

namespace Tizen.UIExtensions.Common
{
    public static partial class DeviceInfo
    {
        public static string Profile => s_profile.Value;

        public static DeviceType DeviceType => s_deviceType.Value;

        public static int ScreenWidth => s_screenWidth.Value;

        public static int ScreenHeight => s_screenHeight.Value;

        public static int DPI => s_dpi.Value;

        public static double PhysicalScale => DPI / 160.0;

        public static Size PixelScreenSize => new Size(s_screenWidth.Value, s_screenHeight.Value);

        public static Size DPScreenSize => new Size((int)(s_screenWidth.Value / PhysicalScale), (int)(s_screenHeight.Value / PhysicalScale));

        public static Size ScaledDPScreenSize => new Size((int)(s_screenWidth.Value / ScalingFactor), (int)(s_screenHeight.Value / ScalingFactor));

        public static bool IsMobile => DeviceType == DeviceType.Mobile;

        public static bool IsTV => DeviceType == DeviceType.TV;

        public static bool IsWatch => DeviceType == DeviceType.Watch;

        public static bool IsRefrigerator => DeviceType == DeviceType.Refrigerator;

        public static bool IsIoT => DeviceType == DeviceType.IoT;

        [Obsolete("Use Tizen.UIExtensions.Commonm.DeviceInfo.Profie instead")]
        public static string GetProfile() => Profile;

        [Obsolete("Use Tizen.UIExtensions.Commonm.DeviceInfo.DeviceType instead")]
        public static DeviceType GetDeviceType() => DeviceType;

        static double s_scalingFactor;
        public static double ScalingFactor
        {
            get
            {
                if (s_scalingFactor == 0)
                {
                    UpdateScalingFactor();
                }
                return s_scalingFactor;
            }
        }

        static DisplayResolutionUnit s_displayResolutionUnit = DisplayResolutionUnit.DP;
        public static DisplayResolutionUnit DisplayResolutionUnit
        {
            get => s_displayResolutionUnit;

            set
            {
                if (s_displayResolutionUnit != value)
                {
                    s_displayResolutionUnit = value;
                    if (s_scalingFactor != 0)
                        UpdateScalingFactor();
                }
            }
        }

        static double s_viewPortWidth;
        public static double ViewPortWidth
        {
            get => s_viewPortWidth;
            set
            {
                if (s_viewPortWidth != value)
                {
                    s_viewPortWidth = value;
                    if (DisplayResolutionUnit == DisplayResolutionUnit.VP)
                    {
                        UpdateScalingFactor();
                    }
                }
            }
        }

        static void UpdateScalingFactor()
        {
            var scalingFactor = 1.0;  // scaling is disabled, we're using pixels as Xamarin's geometry units
            if (DisplayResolutionUnit == DisplayResolutionUnit.VP && ViewPortWidth > 0)
            {
                scalingFactor = s_screenWidth.Value / ViewPortWidth;
            }
            else
            {
                if (DisplayResolutionUnit == DisplayResolutionUnit.DP || DisplayResolutionUnit == DisplayResolutionUnit.DeviceScaledDP)
                {
                    scalingFactor = DPI / 160.0;
                }

                if (DisplayResolutionUnit == DisplayResolutionUnit.DeviceScaledPixel || DisplayResolutionUnit == DisplayResolutionUnit.DeviceScaledDP)
                {
                    var portraitSize = Math.Min(DPScreenSize.Width, DPScreenSize.Height);
                    if (portraitSize > 2000)
                    {
                        scalingFactor *= 4;
                    }
                    else if (portraitSize > 1000)
                    {
                        scalingFactor *= 2.5;
                    }
                }
            }
            s_scalingFactor = scalingFactor;
            SkiaSharp.Views.Tizen.ScalingInfo.SetScalingFactor(scalingFactor);
        }

        static DeviceType ToDeviceType(this string deviceType)
        {
            switch (deviceType)
            {
                case "Mobile":
                    return DeviceType.Mobile;
                case "TV":
                    return DeviceType.TV;
                case "Wearable":
                    return DeviceType.Watch;
                case "Refrigerator":
                    return DeviceType.Refrigerator;
                case "TizenIOT":
                    return DeviceType.IoT;
                default:
                    return DeviceType.Unknown;
            }
        }
    }
}
