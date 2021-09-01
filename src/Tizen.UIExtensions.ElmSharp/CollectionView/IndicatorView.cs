using System;
using System.Collections.Generic;
using ElmSharp;
using Index = ElmSharp.Index;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The IndicatorView is a control that displays indicators that represent the number of items, and current position, in a CarouselView.
    /// </summary>
	public class IndicatorView : Index
	{
		List<IndexItem> _list = new List<IndexItem>();

		public IndicatorView(EvasObject parent) : base(parent)
		{
			AutoHide = false;
			IsHorizontal = true;
            this.SetStyledIndex();
		}

        /// <summary>
        /// Occurs when the IndicatorView.Position selected.
        /// </summary>
		public event EventHandler<SelectedPositionChangedEventArgs>? SelectedPosition;

        /// <summary>
        /// Update the selection status of index.
        /// </summary>
        /// <param name="index"></param>
		public void UpdateSelectedIndex(int index)
		{
			if (index > -1 && index < _list.Count)
			{
				_list[index].Select(true);
			}
		}

        /// <summary>
        /// Update the number of index.
        /// </summary>
		public void AppendIndex(int count = 1)
		{
			for (int i = 0; i < count; i++)
			{
				var item = Append(null);
				item.Selected += OnSelected;
				_list.Add(item);
			}
		}

        /// <summary>
        /// Clear the index.
        /// </summary>
		public void ClearIndex()
		{
			foreach (var item in _list)
			{
				item.Selected -= OnSelected;
			}
			_list.Clear();
            Clear();
        }

		void OnSelected(object? sender, EventArgs e)
		{
			if (sender == null)
				return;
			int index = _list.IndexOf((IndexItem)sender);
			SelectedPosition?.Invoke(this, new SelectedPositionChangedEventArgs(index));
			UpdateSelectedIndex(index);
		}
	}
}
