using ElmSharp;
using ElmSharp.Wearable;
using DeviceInfo = Tizen.UIExtensions.Common.DeviceInfo;
using EColor = ElmSharp.Color;
using EEntry = ElmSharp.Entry;
using ELabel = ElmSharp.Label;
using ELayout = ElmSharp.Layout;
using EProgressBar = ElmSharp.ProgressBar;
using ESize = ElmSharp.Size;
using ESlider = ElmSharp.Slider;
using EToolbarItem = ElmSharp.ToolbarItem;
using TLog = Tizen.UIExtensions.Common.Log;

namespace Tizen.UIExtensions.ElmSharp
{
    public static class ThemeManager
    {
        #region Layout
        public static EdjeTextPartObject GetContentPartEdjeObject(this ELayout layout)
        {
            return layout.EdjeObject[ThemeConstants.Layout.Parts.Content];
        }

        public static EdjeTextPartObject? GetTextPartEdjeObject(this ELayout layout)
        {
            return layout.EdjeObject[ThemeConstants.Layout.Parts.Text];
        }

        public static bool SetTextPart(this ELayout layout, string text)
        {
            return layout.SetPartText(ThemeConstants.Layout.Parts.Text, text);
        }

        public static bool SetContentPart(this ELayout layout, EvasObject? content, bool preserveOldContent = false)
        {
            var ret = layout.SetPartContent(ThemeConstants.Layout.Parts.Content, content, preserveOldContent);
            if (!ret)
            {
                // Restore theme to default if given layout is not available
                layout.SetTheme("layout", "application", "default");
                ret = layout.SetPartContent(ThemeConstants.Layout.Parts.Content, content, preserveOldContent);
            }
            return ret;
        }

        public static bool SetBackgroundPart(this ELayout layout, EvasObject? content, bool preserveOldContent = false)
        {
            return layout.SetPartContent(ThemeConstants.Layout.Parts.Background, content, preserveOldContent);
        }

        public static bool SetOverlayPart(this ELayout layout, EvasObject content, bool preserveOldContent = false)
        {
            return layout.SetPartContent(ThemeConstants.Layout.Parts.Overlay, content, preserveOldContent);
        }
        #endregion

        #region Entry
        public static bool SetPlaceHolderTextPart(this EEntry entry, string text)
        {
            return entry.SetPartText(ThemeConstants.Entry.Parts.PlaceHolderText, text);
        }

        public static void SetVerticalTextAlignment(this EEntry entry, double valign)
        {
            entry.SetVerticalTextAlignment(ThemeConstants.Common.Parts.Text, valign);
        }

        public static void SetVerticalPlaceHolderTextAlignment(this EEntry entry, double valign)
        {
            entry.SetVerticalTextAlignment(ThemeConstants.Entry.Parts.PlaceHolderText, valign);
        }

        public static ESize GetTextBlockFormattedSize(this EEntry entry)
        {
            var textPart = entry.EdjeObject[ThemeConstants.Common.Parts.Text];
            if (textPart == null)
            {
                TLog.Error("There is no elm.text part");
                return new ESize(0, 0);
            }
            return textPart.TextBlockFormattedSize;
        }

        public static ESize GetTextBlockNativeSize(this EEntry entry)
        {
            var textPart = entry.EdjeObject[ThemeConstants.Common.Parts.Text];
            if (textPart == null)
            {
                TLog.Error("There is no elm.text part");
                return new ESize(0, 0);
            }
            return textPart.TextBlockNativeSize;
        }

        public static ESize GetPlaceHolderTextBlockFormattedSize(this EEntry entry)
        {
            var textPart = entry.EdjeObject[ThemeConstants.Entry.Parts.PlaceHolderText];
            if (textPart == null)
            {
                TLog.Error("There is no elm.guide part");
                return new ESize(0, 0);
            }
            return textPart.TextBlockFormattedSize;
        }

        public static ESize GetPlaceHolderTextBlockNativeSize(this EEntry entry)
        {
            var textPart = entry.EdjeObject[ThemeConstants.Entry.Parts.PlaceHolderText];
            if (textPart == null)
            {
                TLog.Error("There is no elm.guide part");
                return new ESize(0, 0);
            }
            return textPart.TextBlockNativeSize;
        }
        #endregion

        #region Label
        public static void SetVerticalTextAlignment(this ELabel label, double valign)
        {
            label.SetVerticalTextAlignment(ThemeConstants.Common.Parts.Text, valign);
        }

        public static double GetVerticalTextAlignment(this ELabel label)
        {
            return label.GetVerticalTextAlignment(ThemeConstants.Common.Parts.Text);
        }

        public static ESize GetTextBlockFormattedSize(this ELabel label)
        {
            var textPart = label.EdjeObject[ThemeConstants.Common.Parts.Text];
            if (textPart == null)
            {
                TLog.Error("There is no elm.text part");
                return new ESize(0, 0);
            }
            return textPart.TextBlockFormattedSize;
        }
        #endregion

        #region Button
        public static ESize GetTextBlockNativeSize(this Button button)
        {
            var textPart = button.EdjeObject[ThemeConstants.Common.Parts.Text];
            if (textPart == null)
            {
                TLog.Error("There is no elm.text part");
                return new ESize(0, 0);
            }
            return textPart.TextBlockNativeSize;
        }

        public static void SetTextBlockStyle(this Button button, string? style)
        {
            var textBlock = button.EdjeObject[ThemeConstants.Common.Parts.Text];
            if (textBlock != null)
            {
                textBlock.TextStyle = style;
            }
        }

        public static void SendTextVisibleSignal(this Button button, bool isVisible)
        {
            button.SignalEmit(isVisible ? ThemeConstants.Button.Signals.TextVisibleState : ThemeConstants.Button.Signals.TextHiddenState, ThemeConstants.Button.Signals.ElementaryCode);
        }

        public static Button SetDefaultStyle(this Button button)
        {
            button.Style = ThemeConstants.Button.Styles.Default;
            return button;
        }

        public static Button SetBottomStyle(this Button button)
        {
            button.Style = ThemeConstants.Button.Styles.Bottom;
            return button;
        }

        public static Button SetPopupStyle(this Button button)
        {
            button.Style = ThemeConstants.Button.Styles.Popup;
            return button;
        }

        public static Button SetNavigationTitleRightStyle(this Button button)
        {
            button.Style = ThemeConstants.Button.Styles.NavigationTitleRight;
            return button;
        }

        public static Button SetNavigationTitleLeftStyle(this Button button)
        {
            button.Style = ThemeConstants.Button.Styles.NavigationTitleLeft;
            return button;
        }

        public static Button SetNavigationBackStyle(this Button button)
        {
            button.Style = ThemeConstants.Button.Styles.NavigationBack;
            return button;
        }

        public static Button SetNavigationDrawerStyle(this Button button)
        {
            button.Style = ThemeConstants.Button.Styles.NavigationDrawers;
            return button;
        }

        public static Button SetTransparentStyle(this Button button)
        {
            button.Style = ThemeConstants.Button.Styles.Transparent;
            return button;
        }

        public static Button SetWatchPopupRightStyle(this Button button)
        {
            if (!DeviceInfo.IsWatch)
            {
                TLog.Error($"ToWatchPopupRightStyleButton is only supported on wearable profile : {0}", DeviceInfo.DeviceType);
                return button;
            }
            button.Style = ThemeConstants.Button.Styles.Watch.PopupRight;
            return button;
        }

        public static Button SetWatchPopupLeftStyle(this Button button)
        {
            if (!DeviceInfo.IsWatch)
            {
                TLog.Error($"ToWatchPopupRightStyleButton is only supported on wearable profile : {0}", DeviceInfo.DeviceType);
                return button;
            }
            button.Style = ThemeConstants.Button.Styles.Watch.PopupLeft;
            return button;
        }

        public static Button SetWatchTextStyle(this Button button)
        {
            if (!DeviceInfo.IsWatch)
            {
                TLog.Error($"ToWatchPopupRightStyleButton is only supported on wearable profile : {0}", DeviceInfo.DeviceType);
                return button;
            }
            button.Style = ThemeConstants.Button.Styles.Watch.Text;
            return button;
        }

        public static bool SetIconPart(this Button button, EvasObject? content, bool preserveOldContent = false)
        {
            return button.SetPartContent(ThemeConstants.Button.Parts.Icon, content, preserveOldContent);
        }

        public static Button SetEditFieldClearStyle(this Button button)
        {
            button.Style = ThemeConstants.Button.Styles.EditFieldClear;
            return button;
        }

        public static EColor GetIconColor(this Button button)
        {
            var ret = EColor.Default;
            if (button == null)
                return ret;

            ret = button.GetPartColor(ThemeConstants.Button.ColorClass.Icon);
            return ret;
        }

        public static void SetIconColor(this Button button, EColor color)
        {
            if (button == null)
                return;

            button.SetPartColor(ThemeConstants.Button.ColorClass.Icon, color);
            button.SetPartColor(ThemeConstants.Button.ColorClass.IconPressed, color);
        }

        public static void SetEffectColor(this Button button, EColor color)
        {
            if (button == null)
                return;

            button.SetPartColor(ThemeConstants.Button.ColorClass.Effect, color);
            button.SetPartColor(ThemeConstants.Button.ColorClass.EffectPressed, color);
        }

        #endregion

        #region Popup
        public static Popup SetWatchCircleStyle(this Popup popup)
        {
            if (!DeviceInfo.IsWatch)
            {
                TLog.Error($"ToWatchPopupRightStyleButton is only supported on wearable profile : {0}", DeviceInfo.DeviceType);
                return popup;
            }
            popup.Style = ThemeConstants.Popup.Styles.Watch.Circle;
            return popup;
        }

        public static void SetTitleColor(this Popup popup, EColor color)
        {
            popup.SetPartColor(DeviceInfo.IsTV ? ThemeConstants.Popup.ColorClass.TV.Title : ThemeConstants.Popup.ColorClass.Title, color);
        }

        public static void SetTitleBackgroundColor(this Popup popup, EColor color)
        {
            popup.SetPartColor(ThemeConstants.Popup.ColorClass.TitleBackground, color);
        }

        public static void SetContentBackgroundColor(this Popup popup, EColor color)
        {
            popup.SetPartColor(ThemeConstants.Popup.ColorClass.ContentBackground, color);
        }

        public static bool SetTitleTextPart(this Popup popup, string? title)
        {
            return popup.SetPartText(ThemeConstants.Popup.Parts.Title, title);
        }

        public static bool SetButton1Part(this Popup popup, EvasObject? content, bool preserveOldContent = false)
        {
            return popup.SetPartContent(ThemeConstants.Popup.Parts.Button1, content, preserveOldContent);
        }

        public static bool SetButton2Part(this Popup popup, EvasObject? content, bool preserveOldContent = false)
        {
            return popup.SetPartContent(ThemeConstants.Popup.Parts.Button2, content, preserveOldContent);
        }

        public static bool SetButton3Part(this Popup popup, EvasObject? content, bool preserveOldContent = false)
        {
            return popup.SetPartContent(ThemeConstants.Popup.Parts.Button3, content, preserveOldContent);
        }
        #endregion

        #region ProgressBar
        public static EProgressBar SetSmallStyle(this EProgressBar progressBar)
        {
            progressBar.Style = ThemeConstants.ProgressBar.Styles.Small;
            return progressBar;
        }

        public static EProgressBar SetLargeStyle(this EProgressBar progressBar)
        {
            progressBar.Style = ThemeConstants.ProgressBar.Styles.Large;
            return progressBar;
        }
        #endregion

        #region Check

        public static void SetOnColors(this Check check, EColor color)
        {
            foreach (string s in check.GetColorParts())
            {
                check.SetPartColor(s, color);
            }
        }

        public static void DeleteOnColors(this Check check)
        {
            foreach (string s in check.GetColorEdjeParts())
            {
                check.EdjeObject.DeleteColorClass(s);
            }
        }

        public static string[] GetColorParts(this Check check)
        {
            if (DeviceInfo.IsWatch)
            {
                if (check.Style == ThemeConstants.Check.Styles.Toggle)
                {
                    return new string[] { ThemeConstants.Check.ColorClass.Watch.OuterBackgroundOn };
                }
                else
                {
                    return new string[] {
                        ThemeConstants.Check.ColorClass.Watch.OuterBackgroundOn,
                        ThemeConstants.Check.ColorClass.Watch.OuterBackgroundOnPressed,
                        ThemeConstants.Check.ColorClass.Watch.CheckOn,
                        ThemeConstants.Check.ColorClass.Watch.CheckOnPressed
                    };
                }
            }
            else if (DeviceInfo.IsTV)
            {
                if (check.Style == ThemeConstants.Check.Styles.Toggle)
                {
                    return new string[] { ThemeConstants.Check.ColorClass.TV.SliderOn, ThemeConstants.Check.ColorClass.TV.SliderFocusedOn };
                }
                else
                {
                    return new string[] {
                        ThemeConstants.Check.ColorClass.TV.SliderOn,
                        ThemeConstants.Check.ColorClass.TV.SliderFocusedOn,
                    };
                }
            }
            else
            {
                if (check.Style == ThemeConstants.Check.Styles.Toggle)
                {
                    return new string[] { ThemeConstants.Check.ColorClass.BackgroundOn };
                }
                else
                {
                    return new string[] { ThemeConstants.Check.ColorClass.BackgroundOn, ThemeConstants.Check.ColorClass.Stroke };
                }
            }
        }

        public static string[] GetColorEdjeParts(this Check check)
        {
            string[] ret = check.GetColorParts();

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = check.ClassName.ToLower().Replace("elm_", "") + "/" + ret[i];
            }
            return ret;
        }
        #endregion

        #region NaviItem
        public static void SetTitle(this NaviItem item, string text)
        {
            item.SetPartText(ThemeConstants.NaviItem.Parts.Title, text);
        }

        public static void SetBackButton(this NaviItem item, EvasObject content, bool preserveOldContent = false)
        {
            item.SetPartContent(ThemeConstants.NaviItem.Parts.BackButton, content, preserveOldContent);
        }

        public static void SetLeftToolbarButton(this NaviItem item, EvasObject content, bool preserveOldContent = false)
        {
            item.SetPartContent(ThemeConstants.NaviItem.Parts.LeftToolbarButton, content, preserveOldContent);
        }

        public static void SetRightToolbarButton(this NaviItem item, EvasObject content, bool preserveOldContent = false)
        {
            item.SetPartContent(ThemeConstants.NaviItem.Parts.RightToolbarButton, content, preserveOldContent);
        }

        public static void SetNavigationBar(this NaviItem item, EvasObject content, bool preserveOldContent = false)
        {
            item.SetPartContent(ThemeConstants.NaviItem.Parts.NavigationBar, content, preserveOldContent);
        }

        public static NaviItem SetNavigationBarStyle(this NaviItem item)
        {
            item.Style = ThemeConstants.NaviItem.Styles.NavigationBar;
            return item;
        }

        public static NaviItem SetTabBarStyle(this NaviItem item)
        {
            if (DeviceInfo.IsTV)
            {
                //According to TV UX Guideline, item style should be set to "tabbar" in case of TabbedPage only for TV profile.
                item.Style = ThemeConstants.NaviItem.Styles.TV.TabBar;
            }
            else
            {
                item.Style = ThemeConstants.NaviItem.Styles.Default;
            }
            return item;
        }
        #endregion

        #region Toolbar
        public static Toolbar SetNavigationBarStyle(this Toolbar toolbar)
        {
            toolbar.Style = ThemeConstants.Toolbar.Styles.NavigationBar;
            return toolbar;
        }

        public static Toolbar SetTVTabBarWithTitleStyle(this Toolbar toolbar)
        {
            if (!DeviceInfo.IsTV)
            {
                TLog.Error($"ToWatchPopupRightStyleButton is only supported on TV profile : {0}", DeviceInfo.DeviceType);
                return toolbar;
            }
            toolbar.Style = ThemeConstants.Toolbar.Styles.TV.TabbarWithTitle;
            return toolbar;
        }
        #endregion

        #region ToolbarItem
        public static void SetIconPart(this EToolbarItem item, EvasObject content, bool preserveOldContent = false)
        {
            item.SetPartContent(ThemeConstants.ToolbarItem.Parts.Icon, content, preserveOldContent);
        }

        public static void SetBackgroundColor(this EToolbarItem item, EColor color)
        {
            item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.Background, color);
        }

        public static void SetUnderlineColor(this EToolbarItem item, EColor color)
        {
            item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.Underline, color);
        }

        public static void SetTextColor(this EToolbarItem item, EColor color)
        {
            if (string.IsNullOrEmpty(item.Icon))
            {
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.Text, color);
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.TextPressed, color);
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.TextSelected, color);
            }
            else
            {
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIcon, color);
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIconPressed, color);
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIconSelected, color);
            }
            item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.Underline, color);
        }

        public static void SetSelectedTabColor(this EToolbarItem item, EColor color)
        {
            if (string.IsNullOrEmpty(item.Icon))
            {
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.TextSelected, color);
            }
            else
            {
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIconSelected, color);
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.IconSelected, color);
            }
            item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.Underline, color);
        }

        public static void SetUnselectedTabColor(this EToolbarItem item, EColor color)
        {
            if (string.IsNullOrEmpty(item.Icon))
            {
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.Text, color);
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.TextPressed, color);
            }
            else
            {
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIcon, color);
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIconPressed, color);
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.Icon, color);
                item.SetPartColor(ThemeConstants.ToolbarItem.ColorClass.IconPressed, color);
            }
        }

        public static void DeleteBackgroundColor(this EToolbarItem item)
        {
            item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.Background);
        }

        public static void DeleteUnderlineColor(this EToolbarItem item)
        {
            item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.Underline);
        }

        public static void DeleteTextColor(this EToolbarItem item)
        {
            if (string.IsNullOrEmpty(item.Icon))
            {
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.Text);
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.TextPressed);
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.TextSelected);
            }
            else
            {
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIcon);
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIconPressed);
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIconSelected);
            }
            item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.Underline);
        }

        public static void DeleteSelectedTabColor(this EToolbarItem item)
        {
            if (string.IsNullOrEmpty(item.Icon))
            {
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.TextSelected);
            }
            else
            {
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIconSelected);
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.IconSelected);
            }
            item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.Underline);
        }

        public static void DeleteUnselectedTabColor(this EToolbarItem item)
        {
            if (string.IsNullOrEmpty(item.Icon))
            {
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.Text);
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.TextPressed);
            }
            else
            {
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIcon);
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.TextUnderIconPressed);
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.Icon);
                item.DeletePartColor(ThemeConstants.ToolbarItem.ColorClass.IconPressed);
            }
        }

        #endregion

        #region Background
        public static bool SetOverlayPart(this Background bg, EvasObject content, bool preserveOldContent = false)
        {
            return bg.SetPartContent(ThemeConstants.Background.Parts.Overlay, content, preserveOldContent);
        }
        #endregion

        #region Panes
        public static bool SetLeftPart(this Panes panes, EvasObject? content, bool preserveOldContent = false)
        {
            return panes.SetPartContent(ThemeConstants.Panes.Parts.Left, content, preserveOldContent);
        }

        public static bool SetRightPart(this Panes panes, EvasObject? content, bool preserveOldContent = false)
        {
            return panes.SetPartContent(ThemeConstants.Panes.Parts.Right, content, preserveOldContent);
        }
        #endregion

        #region GenItemClass
        //public static string GetMainPart()
        #endregion

        #region GenList
        public static GenList SetSolidStyle(this GenList list)
        {
            list.Style = ThemeConstants.GenList.Styles.Solid;
            return list;
        }
        #endregion

        #region GenListItem
        public static void SetBottomlineColor(this GenListItem item, EColor color)
        {
            item.SetPartColor(ThemeConstants.GenListItem.ColorClass.BottomLine, color);
        }

        public static void SetBackgroundColor(this GenListItem item, EColor color)
        {
            item.SetPartColor(ThemeConstants.GenListItem.ColorClass.Background, color);
        }

        public static void DeleteBottomlineColor(this GenListItem item)
        {
            item.DeletePartColor(ThemeConstants.GenListItem.ColorClass.BottomLine);
        }

        public static void DeleteBackgroundColor(this GenListItem item)
        {
            item.DeletePartColor(ThemeConstants.GenListItem.ColorClass.Background);
        }
        #endregion

        #region Radio
        public static ESize GetTextBlockFormattedSize(this Radio radio)
        {
            return radio.EdjeObject[ThemeConstants.Common.Parts.Text].TextBlockFormattedSize;
        }

        public static void SetTextBlockStyle(this Radio radio, string? style)
        {
            var textBlock = radio.EdjeObject[ThemeConstants.Common.Parts.Text];
            if (textBlock != null)
            {
                textBlock.TextStyle = style;
            }
        }

        public static void SendTextVisibleSignal(this Radio radio, bool isVisible)
        {
            radio.SignalEmit(isVisible ? ThemeConstants.Radio.Signals.TextVisibleState : ThemeConstants.Radio.Signals.TextHiddenState, ThemeConstants.Radio.Signals.ElementaryCode);
        }
        #endregion

        #region Slider
        public static EColor GetBarColor(this ESlider slider)
        {
            return slider.GetPartColor(ThemeConstants.Slider.ColorClass.Bar);
        }

        public static void SetBarColor(this ESlider slider, EColor color)
        {
            slider.SetPartColor(ThemeConstants.Slider.ColorClass.Bar, color);
            slider.SetPartColor(ThemeConstants.Slider.ColorClass.BarPressed, color);
        }

        public static EColor GetBackgroundColor(this ESlider slider)
        {
            return slider.GetPartColor(ThemeConstants.Slider.ColorClass.Background);
        }

        public static void SetBackgroundColor(this ESlider slider, EColor color)
        {
            slider.SetPartColor(ThemeConstants.Slider.ColorClass.Background, color);
        }

        public static EColor GetHandlerColor(this ESlider slider)
        {
            return slider.GetPartColor(ThemeConstants.Slider.ColorClass.Handler);
        }

        public static void SetHandlerColor(this ESlider slider, EColor color)
        {
            slider.SetPartColor(ThemeConstants.Slider.ColorClass.Handler, color);
            slider.SetPartColor(ThemeConstants.Slider.ColorClass.HandlerPressed, color);
        }
        #endregion

        #region Index
        public static Index SetStyledIndex(this Index index)
        {
            index.Style = DeviceInfo.IsWatch ? ThemeConstants.Index.Styles.Circle : ThemeConstants.Index.Styles.PageControl;
            return index;
        }
        #endregion

        #region IndexItem
        public static void SetIndexItemStyle(this IndexItem item, int itemCount, int offset, int evenMiddleItem, int oddMiddleItem)
        {
            string style;
            int position;

            if (itemCount % 2 == 0)  //Item count is even.
            {
                position = evenMiddleItem - itemCount / 2 + offset;
                style = ThemeConstants.IndexItem.Styles.EvenItemPrefix + position;
            }
            else  //Item count is odd.
            {
                position = oddMiddleItem - itemCount / 2 + offset;
                style = ThemeConstants.IndexItem.Styles.OddItemPrefix + position;
            }
            item.Style = style;
        }
        #endregion

        #region CircleSpinner
        public static bool SetTitleTextPart(this CircleSpinner spinner, string title)
        {
            return spinner.SetPartText(ThemeConstants.Common.Parts.Text, title);
        }
        #endregion

        #region DrawerLayoutBox
        public static double GetDrawerRatio(this DrawerBox drawerlayoutBox, int width, int height)
        {
            return (width > height) ? 0.4 : 0.83;
        }

        public static double GetSplitRatio(this DrawerBox drawerlayoutBox)
        {
            return 0.4;
        }
        #endregion


        #region TVNavigationView
        public static EColor GetTvDefaultBackgroundColor(this INavigationView nav)
        {
            return ThemeConstants.Shell.ColorClass.TV.DefaultBackgroundColor;
        }
        #endregion

        #region TVNavigationDrawer
        static int s_defaultDrawerItemHeight = -1;
        public static int GetTvFocusedButtonHeight(this INavigationDrawer drawer)
        {
            if (s_defaultDrawerItemHeight > 0)
                return s_defaultDrawerItemHeight;
            return s_defaultDrawerItemHeight = ThemeConstants.Shell.Resources.DefaultDrawerItemHeight;
        }

        static double s_navigationDrawerRatio = -1;
        public static double GetTvDrawerRatio(this INavigationDrawer drawer, int width, int height)
        {
            return s_navigationDrawerRatio = (width > height) ? ThemeConstants.Shell.Resources.TV.DefaultDrawerRatio : ThemeConstants.Shell.Resources.DefaultDrawerRatio;
        }

        public static double GetTvDrawerCloseRatio(this INavigationDrawer drawer)
        {
            return ThemeConstants.Shell.Resources.TV.DefaultDrawerCloseRatio;
        }
        #endregion
    }
}