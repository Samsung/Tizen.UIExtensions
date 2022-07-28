
using Tizen.NUI;

namespace Tizen.UIExtensions.NUI
{
    public static class ContainerExtensions
    {
        public static bool Contains(this Container view, Container? child)
        {
            if (child == null)
                return false;

            if (child == view || child.Parent == view)
                return true;

            return view.Contains(child.Parent);
        }
    }
}
