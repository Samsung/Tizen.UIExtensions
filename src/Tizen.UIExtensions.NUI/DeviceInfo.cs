using System;
using System.Diagnostics;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using TSystemInfo = Tizen.System.Information;

namespace Tizen.UIExtensions.Common
{
    public static partial class DeviceInfo
    {
        static Lazy<int> s_dpi = new Lazy<int>(() =>
        {
            Debug.Assert(Window.Instance != null);
            return (int)Window.Instance.Dpi.Y;
        });

        static Lazy<string> s_profile = new Lazy<string>(() =>
        {
            if (!TSystemInfo.TryGetValue("http://tizen.org/feature/profile", out string profile))
                return "mobile";
            return profile;
        });

        static Lazy<DeviceType> s_deviceType = new Lazy<DeviceType>(() =>
        {
            var type = DeviceType.Unknown;
            if (TSystemInfo.TryGetValue("http://tizen.org/system/device_type", out string deviceType))
            {
                if (deviceType.StartsWith("Mobile"))
                {
                    type = DeviceType.Mobile;
                }
                else if (deviceType.StartsWith("TV"))
                {
                    type = DeviceType.TV;
                }
                else if (deviceType.StartsWith("Wearable"))
                {
                    type = DeviceType.Watch;
                }
                else if (deviceType.StartsWith("Refrigerator"))
                {
                    type = DeviceType.Refrigerator;
                }
                else if (deviceType.StartsWith("TizenIOT"))
                {
                    type = DeviceType.IoT;
                }
            }
            return type;
        });


        public static int DPI => s_dpi.Value;
        public static string Profile => s_profile.Value;
        public static DeviceType DeviceType => s_deviceType.Value;


        public static int ScreenWidth => Window.Instance.WindowSize.Width;
        public static int ScreenHeight => Window.Instance.WindowSize.Height;

        public static double PhysicalScale => DPI / 160.0;

        public static Size PixelScreenSize => Window.Instance.WindowSize.ToCommon();

        public static Size DPScreenSize => new Size(ScreenWidth / PhysicalScale, ScreenHeight / PhysicalScale);

        public static Size ScaledDPScreenSize => new Size(ScreenWidth / ScalingFactor, ScreenHeight / ScalingFactor);

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
                scalingFactor = ScreenWidth / ViewPortWidth;
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
    }

    public enum DeviceType
    {
        Mobile,
        TV,
        Watch,
        Refrigerator,
        IoT,
        Unknown
    }

    public enum DisplayResolutionUnit
    {
        Pixel,
        DeviceScaledPixel,
        DP,
        DeviceScaledDP,
        VP
    }
}
