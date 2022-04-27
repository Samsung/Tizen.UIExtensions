using System;
using Tizen.NUI;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.NUI
{
    public static class KeyEventExtensions
    {
        /// <summary>
        /// Check if the key event means acceptance
        /// </summary>
        /// <param name="key">A key event to check</param>
        /// <returns>true, if the key event means acceptance</returns>
        public static bool IsAcceptKeyEvent(this Key key)
        {
            return Key.StateType.Up == key.State && key.KeyPressedName.IsEnterKey();
        }

        /// <summary>
        /// Check if the key event means decline
        /// </summary>
        /// <param name="key">A key event to check</param>
        /// <returns>true, if the key event means declien</returns>
        public static bool IsDeclineKeyEvent(this Key key)
        {
            return Key.StateType.Up == key.State && key.KeyPressedName.IsBackKey();
        }
    }
}
