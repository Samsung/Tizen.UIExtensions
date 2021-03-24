using Tizen.UIExtensions.Common;
using ESize = ElmSharp.Size;
using ERect = ElmSharp.Rect;

namespace Tizen.UIExtensions.ElmSharp
{
    public static class UnitExtensions
    {
        public static Size ToCommon(this ESize size)
        {
            return new Size(size.Width, size.Height);
        }

        public static Rect ToCommon(this ERect rect)
        {
            return new Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
