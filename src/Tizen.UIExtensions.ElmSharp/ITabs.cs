using System;
using ElmSharp;
using EColor = ElmSharp.Color;

namespace Tizen.UIExtensions.ElmSharp
{
    public interface ITabs
    {
        TabsType Scrollable { get; set; }

        EColor BackgroundColor { get; set; }

        ToolbarItem SelectedItem { get; }

        event EventHandler<ToolbarItemEventArgs> Selected;

        ToolbarItem Append(string label, string icon);

        ToolbarItem Append(string label);

        ToolbarItem InsertBefore(ToolbarItem before, string label, string icon);
    }

    public enum TabsType
    {
        Fixed,
        Scrollable
    }
}
