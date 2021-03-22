using ElmSharp;
using TLog = Tizen.UIExtensions.Common.Log;

namespace Tizen.UIExtensions.ElmSharp
{
	public static class ColorExtensions
	{
		/// <summary>
		/// Returns a string representing the provided ElmSharp.Color instance in a hexagonal notation
		/// </summary>
		/// <returns>string value containing the encoded color</returns>
		/// <param name="c">The ElmSharp.Color class instance which will be serialized</param>
		public static string ToHex(this Color c)
		{
			if (c.IsDefault)
			{
				TLog.Warn("Trying to convert the default color to hexagonal notation, it does not works as expected.");
			}
			return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", c.R, c.G, c.B, c.A);
		}
	}
}
