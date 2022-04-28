namespace Tizen.UIExtensions.Common
{

    /// <summary>
    /// A collection of well-known key names
    /// </summary>
    public static class InputKeyNames
    {
        public static readonly string Return = "Return";
        public static readonly string Enter = "Enter";
        public static readonly string BackButton = "XF86Back";
        public static readonly string Escape = "Escape";

        /// <summary>
        /// Check if the key name is related to enter
        /// </summary>
        /// <param name="keyName">A key name to check</param>
        /// <returns>true, if related to enter</returns>
        public static bool IsEnterKey(this string keyName)
        {
            return keyName == Return || keyName == Enter;
        }

        /// <summary>
        /// Check if the key name is related to back
        /// </summary>
        /// <param name="keyName">A key name to check</param>
        /// <returns>true, if related to back</returns>
        public static bool IsBackKey(this string keyName)
        {
            return keyName == BackButton || keyName == Escape;
        }
    }
}
