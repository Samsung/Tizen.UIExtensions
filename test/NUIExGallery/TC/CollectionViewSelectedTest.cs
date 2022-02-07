using System;
using System.Collections;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{

    public class CollectionViewSelectedTest : TestCaseBase
    {

        class SelectedItemAdaptor : MyAdaptor
        {

            public event EventHandler UpdateSelected;

            public IEnumerable<int> selected { get; set; }
            public SelectedItemAdaptor(IEnumerable items) : base(items)
            {
            }

            public override void SendItemSelected(IEnumerable<int> selected)
            {
                this.selected = selected;
                UpdateSelected?.Invoke(this, EventArgs.Empty);
            }

            public override void UpdateViewState(View view, ViewHolderState state)
            {
                if (state == ViewHolderState.Focused)
                {
                    view.UpdateBackgroundColor(Color.Green);
                }
                else if (state == ViewHolderState.Selected)
                {
                    view.UpdateBackgroundColor(Color.Red);
                }
                else
                {
                    view.UpdateBackgroundColor(Color.Yellow);
                }
            }

            public override View CreateNativeView(int index)
            {
                var item = base.CreateNativeView(index);

                item.Focusable = true;
                item.FocusableInTouch = true;

                return item;
            }
        }

        public override string TestName => "CollectionView Selected event Test1";

        public override string TestDescription => "CollectionView Scrolled event test1";

        public override View Run()
        {
            var items = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add($"Items {i}");
            }

            var adaptor = new SelectedItemAdaptor(items);

            var collectionView = new CollectionView();
            collectionView.SelectionMode = CollectionViewSelectionMode.Single;
            collectionView.Adaptor = adaptor;
            collectionView.LayoutManager = new LinearLayoutManager(false);

            var layout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Top
                }
            };

            var label = new Label
            {
                FontSize = 10,
                Text = "Selected:"
            };

            adaptor.UpdateSelected += (s, e) =>
            {
                label.Text = "Selected:" + string.Join(",", adaptor.selected);
            };

            layout.Add(label);

            var horizontal = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                }
            };
            layout.Add(horizontal);
            var selectionNone = new Button
            {
                Focusable = true,
                Text = "None",
                SizeWidth = 200,
            };
            horizontal.Add(selectionNone);
            selectionNone.Clicked += (s, e) => collectionView.SelectionMode = CollectionViewSelectionMode.None;

            var selectionSingle = new Button
            {
                Focusable = true,
                Text = "Single",
                SizeWidth = 200,
            };
            horizontal.Add(selectionSingle);
            selectionSingle.Clicked += (s, e) => collectionView.SelectionMode = CollectionViewSelectionMode.Single;

            var selectionMulti = new Button
            {
                Focusable = true,
                Text = "Multiple",
                SizeWidth = 200,
            };
            horizontal.Add(selectionMulti);
            selectionMulti.Clicked += (s, e) => collectionView.SelectionMode = CollectionViewSelectionMode.Multiple;

            layout.Add(collectionView);
            return layout;
        }
    }
}
