using System.Linq;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI
{
    public static class FormattedStringExtensions
    {
        public static string ToMarkupText(this FormattedString formatted)
        {
            return string.Concat(from span in formatted.Spans select span.ToMarkupText());
        }
    }
}
