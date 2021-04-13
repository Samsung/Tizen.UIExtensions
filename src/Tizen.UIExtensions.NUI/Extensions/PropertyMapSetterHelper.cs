using Tizen.NUI;

namespace Tizen.UIExtensions.NUI
{

    internal static class PropertyMapSetterHelper
    {
        internal static PropertyMap Add<T>(this PropertyMap propertyMap, int key, T value)
        {
            using (var pv = PropertyValue.CreateFromObject(value))
            {
                propertyMap.Add(key, pv);
            }
            return propertyMap;
        }

        internal static PropertyMap Add<T>(this PropertyMap propertyMap, string key, T value)
        {
            using (var pv = PropertyValue.CreateFromObject(value))
            {
                propertyMap.Add(key, pv);
            }
            return propertyMap;
        }
    }
}
