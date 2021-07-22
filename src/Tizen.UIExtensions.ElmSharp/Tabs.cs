using ElmSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    public class Tabs : Toolbar, ITabs
    {
        TabsScrollType _type;

        public Tabs(EvasObject parent) :base(parent)
        {
            Style = ThemeConstants.Toolbar.Styles.Material;
            SelectionMode = ToolbarSelectionMode.Always;
        }

        public TabsScrollType ScrollType
        {
            get => _type;
            set
            {
                switch (value)
                {
                    case TabsScrollType.Fixed:
                        this.ShrinkMode = ToolbarShrinkMode.Expand;
                        break;
                    case TabsScrollType.Scrollable:
                        this.ShrinkMode = ToolbarShrinkMode.Scroll;
                        break;
                }
                _type = value;
            }
        }
    }
}
