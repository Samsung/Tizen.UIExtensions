using ElmSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    public class Tabs : Toolbar, ITabs
    {
        TabsType _type;

        public Tabs(EvasObject parent) :base(parent)
        {
            Style = ThemeConstants.Toolbar.Styles.Material;
            SelectionMode = ToolbarSelectionMode.Always;
        }

        public TabsType Scrollable
        {
            get => _type;
            set
            {
                switch (value)
                {
                    case TabsType.Fixed:
                        this.ShrinkMode = ToolbarShrinkMode.Expand;
                        break;
                    case TabsType.Scrollable:
                        this.ShrinkMode = ToolbarShrinkMode.Scroll;
                        break;
                }
                _type = value;
            }
        }
    }
}
