namespace Tizen.UIExtensions.Common
{
    public static partial class DeviceInfo
    {
        static Lazy<string> s_profile = new Lazy<string>(() =>
        {
            return "stub";
        });

        static Lazy<int> s_dpi = new Lazy<int>(() =>
        {
            return 0;
        });

        static Lazy<int> s_screenWidth = new Lazy<int>(() =>
        {
            return 0;
        });

        static Lazy<int> s_screenHeight = new Lazy<int>(() =>
        {
            return 0;
        });

        static Lazy<DeviceType> s_deviceType = new Lazy<DeviceType>(() =>
        {
            return DeviceType.Unknown;
        });
    }
}
