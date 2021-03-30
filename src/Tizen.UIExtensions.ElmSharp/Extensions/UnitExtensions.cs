using Tizen.UIExtensions.Common;
using ESize = ElmSharp.Size;
using ERect = ElmSharp.Rect;
using ScrollBlock = ElmSharp.ScrollBlock;
using ScrollBarVisiblePolicy = ElmSharp.ScrollBarVisiblePolicy;

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

        public static ERect ToNative(this Rect rect)
        {
            return new ERect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public static ScrollOrientation ToCommon(this ScrollBlock scrollBlock)
        {
            if (scrollBlock == ScrollBlock.Horizontal)
            {
                return ScrollOrientation.Vertical;
            }
            else if (scrollBlock == ScrollBlock.Vertical)
            {
                return ScrollOrientation.Horizontal;
            }
            else if (scrollBlock == ScrollBlock.None)
            {
                return ScrollOrientation.Both;
            }
            else
            {
                return ScrollOrientation.Neither;
            }
        }

        public static ScrollBlock ToNative(this ScrollOrientation scrollOrientation)
        {
            switch (scrollOrientation)
            {
                case ScrollOrientation.Vertical:
                    return ScrollBlock.Horizontal;
                case ScrollOrientation.Horizontal:
                    return ScrollBlock.Vertical;
                case ScrollOrientation.Neither:
                    return ScrollBlock.Vertical | ScrollBlock.Horizontal;
                case ScrollOrientation.Both:
                default:
                    return ScrollBlock.None;
            }
        }

        public static ScrollBarVisibility ToCommon(this ScrollBarVisiblePolicy policy)
        {
            switch (policy)
            {
                case ScrollBarVisiblePolicy.Auto:
                    return ScrollBarVisibility.Default;
                case ScrollBarVisiblePolicy.Visible:
                    return ScrollBarVisibility.Always;
                case ScrollBarVisiblePolicy.Invisible:
                    return ScrollBarVisibility.Never;
                default:
                    return ScrollBarVisibility.Default;
            }
        }

        public static ScrollBarVisiblePolicy ToNative(this ScrollBarVisibility policy)
        {
            switch (policy)
            {
                case ScrollBarVisibility.Default:
                    return ScrollBarVisiblePolicy.Auto;
                case ScrollBarVisibility.Always:
                    return ScrollBarVisiblePolicy.Visible;
                case ScrollBarVisibility.Never:
                    return ScrollBarVisiblePolicy.Invisible;
                default:
                    return ScrollBarVisiblePolicy.Auto;
            }
        }
    }
}
