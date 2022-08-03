using System;
using ElmSharp;
using Tizen.UIExtensions.ElmSharp;
using TSystemInfo = Tizen.System.Information;

namespace Tizen.UIExtensions.Common
{
    public static partial class DeviceInfo
    {
        static Lazy<string> s_profile = new Lazy<string>(() =>
        {
            //TODO : Fix me if elm_config_profile_get() unavailable
            return Elementary.GetProfile();
        });

        static Lazy<int> s_dpi = new Lazy<int>(() =>
        {
            int dpi = 0;
            if (IsTV)
            {
                // Use fixed DPI value (72) if TV profile
                return 72;
            }
            TSystemInfo.TryGetValue<int>("http://tizen.org/feature/screen.dpi", out dpi);
            return dpi;
        });

        static Lazy<int> s_screenWidth = new Lazy<int>(() =>
        {
            int width = 0;
            TSystemInfo.TryGetValue("http://tizen.org/feature/screen.width", out width);
            return width;
        });

        static Lazy<int> s_screenHeight = new Lazy<int>(() =>
        {
            int height = 0;
            TSystemInfo.TryGetValue("http://tizen.org/feature/screen.height", out height);
            return height;
        });

        static Lazy<double> s_elmScale = new Lazy<double>(() =>
        {
            return s_deviceScale!.Value / Elementary.GetScale();
        });

        static Lazy<DeviceType> s_deviceType = new Lazy<DeviceType>(() =>
        {
            if (TSystemInfo.TryGetValue("http://tizen.org/system/device_type", out string deviceType))
            {
                return deviceType.ToDeviceType();
            }
            else
            {
                // Since, above key("http://tizen.org/system/device_type") is not available on Tizen 4.0, we uses profile to decide the type of device on 4.0.
                switch (Profile)
                {
                    case "mobile":
                        return DeviceType.Mobile;
                    case "tv":
                        return DeviceType.TV;
                    case "wearable":
                        return DeviceType.Watch;
                    default:
                        return DeviceType.Unknown;
                }
            }
        });

        static Lazy<double> s_deviceScale = new Lazy<double>(() =>
        {
            // This is the base scale value and varies from profile
            return GetBaseScale(s_deviceType.Value);
        });

        public static double ElmScale => s_elmScale.Value;

        public static double GetPhysicalPortraitSizeInDP()
        {
            var screenSize = DPScreenSize;
            return Math.Min(screenSize.Width, screenSize.Height);
        }

        public static double CalculateDoubleScaledSizeInLargeScreen(double size)
        {
            if (DisplayResolutionUnit == DisplayResolutionUnit.VP)
                return size;

            if ((DisplayResolutionUnit == DisplayResolutionUnit.Pixel || DisplayResolutionUnit == DisplayResolutionUnit.DP) && GetPhysicalPortraitSizeInDP() > 1000)
            {
                size *= 2.5;
            }

            if (DisplayResolutionUnit == DisplayResolutionUnit.Pixel || DisplayResolutionUnit == DisplayResolutionUnit.DeviceScaledPixel)
            {
                size = DPExtensions.ConvertToPixel(size);
            }
            return size;
        }

        public static double GetBaseScale(DeviceType deviceType)
        {
            if (deviceType == DeviceType.Mobile)
            {
                return ThemeConstants.Common.Resource.Mobile.BaseScale;
            }
            else if (deviceType == DeviceType.TV)
            {
                return ThemeConstants.Common.Resource.TV.BaseScale;
            }
            else if (deviceType == DeviceType.Watch)
            {
                return ThemeConstants.Common.Resource.Watch.BaseScale;
            }
            else if (deviceType == DeviceType.Refrigerator)
            {
                return ThemeConstants.Common.Resource.Refrigerator.BaseScale;
            }
            else if (deviceType == DeviceType.IoT)
            {
                return ThemeConstants.Common.Resource.Iot.BaseScale;
            }
            return 1.0;
        }
    }
}
