using System;
using System.Diagnostics;
using Tizen.NUI;
using TSystemInfo = Tizen.System.Information;

namespace Tizen.UIExtensions.Common
{
    public static partial class DeviceInfo
    {
        static Lazy<string> s_profile = new Lazy<string>(() =>
        {
            if (!TSystemInfo.TryGetValue("http://tizen.org/feature/profile", out string profile))
                return "mobile";
            return profile;
        });

        static Lazy<int> s_dpi = new Lazy<int>(() =>
        {
            if (IsTV || IsIoT)
            {
                return 213;
            }
            Debug.Assert(Window.Instance != null);
            return (int)Window.Instance.Dpi.Y;
        });

        static Lazy<int> s_screenWidth = new Lazy<int>(() =>
        {
            return Window.Instance.WindowSize.Width;
        });

        static Lazy<int> s_screenHeight = new Lazy<int>(() =>
        {
            return Window.Instance.WindowSize.Height;
        });

        static Lazy<DeviceType> s_deviceType = new Lazy<DeviceType>(() =>
        {
            TSystemInfo.TryGetValue("http://tizen.org/system/device_type", out string deviceType);
            return deviceType.ToDeviceType();
        });

        internal static Lazy<float> FontScale = new Lazy<float>(() =>
        {
            return (float)(ScalingFactor * 160 / Window.Instance.Dpi.Y);
        });
    }
}
