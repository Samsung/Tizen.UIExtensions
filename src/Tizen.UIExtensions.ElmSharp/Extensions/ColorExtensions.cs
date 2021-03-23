using TLog = Tizen.UIExtensions.Common.Log;
using EColor = ElmSharp.Color;
using Color = Tizen.UIExtensions.Common.Color;
namespace Tizen.UIExtensions.ElmSharp
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Returns a string representing the provided ElmSharp.Color instance in a hexagonal notation (#RRGGBBAA)
        /// </summary>
        /// <returns>string value containing the encoded color</returns>
        /// <param name="c">The ElmSharp.Color class instance which will be serialized</param>
        public static string ToHex(this EColor c)
        {
            if (c.IsDefault)
            {
                TLog.Warn("Trying to convert the default color to hexagonal notation, it does not works as expected.");
            }
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", c.R, c.G, c.B, c.A);
        }

        /// <summary>
        /// Creates an instance of ElmSharp.Color class based on provided Tizen.UIExtensions.Common.Color instance
        /// </summary>
        /// <returns>ElmSharp.Color instance representing a color which corresponds to the provided Tizen.UIExtensions.Common.Color</returns>
        /// <param name="c">The Tizen.UIExtensions.Common.Color instance which will be converted to a ElmSharp.Color</param>
        public static EColor ToNative(this Color c)
        {
            return c.IsDefault ? EColor.Default : 
                new EColor((int)(255.0 * c.R), (int)(255.0 * c.G), (int)(255.0 * c.B), (int)(255.0 * c.A));
        }
    }
}
