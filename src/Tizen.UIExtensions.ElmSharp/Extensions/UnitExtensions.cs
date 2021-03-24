using Tizen.UIExtensions.Common;
using ElmSharp;
using ESize = ElmSharp.Size;

namespace Tizen.UIExtensions.ElmSharp
{
    public static class UnitExtensions
    {
        public static Common.Size ToCommon(this ESize size)
        {
            return new Common.Size(size.Width, size.Height);
        }
    }
}
