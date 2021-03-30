namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// Enumerates conditions under which scroll bars will be visible.
    /// </summary>
    public enum ScrollBarVisibility
    {
        /// <summary>
        /// Indicates the default scroll bar behavior for the platform.
        /// </summary>
        Default,
        /// <summary>
        /// Indicates that scroll bars will be visible, even when the content fits on the control.
        /// </summary>
        Always,
        /// <summary>
        /// Indicates that scroll bars are not visible, even if the content does not fit on the control.
        /// </summary>
        Never,
    }
}
