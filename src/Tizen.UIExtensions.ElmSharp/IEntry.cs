using System;
using Tizen.UIExtensions.Common;
using EColor = ElmSharp.Color;

namespace Tizen.UIExtensions.ElmSharp
{
	public interface IEntry
	{
		double FontSize { get; set; }

		FontAttributes FontAttributes { get; set; }

		string FontFamily { get; set; }

		EColor TextColor { get; set; }

		TextAlignment HorizontalTextAlignment { get; set; }

		string Placeholder { get; set; }

		EColor PlaceholderColor { get; set; }

		string FontWeight { get; set; }

		Keyboard Keyboard { get; set; }

		event EventHandler TextBlockFocused;

		event EventHandler TextBlockUnfocused;

		event EventHandler EntryLayoutFocused;

		event EventHandler EntryLayoutUnfocused;
	}
}
