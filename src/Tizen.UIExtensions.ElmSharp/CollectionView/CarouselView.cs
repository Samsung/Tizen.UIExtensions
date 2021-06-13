using ElmSharp;

namespace Tizen.UIExtensions.ElmSharp
{
    /// <summary>
    /// The CarouselView is a view for presenting data in a scrollable layout, where users can swipe to move through a collection of items.
    /// </summary>
	public class CarouselView : CollectionView, ICollectionViewController
	{
        /// <summary>
        /// Initializes a new instance of the CarouselView class.
        /// </summary>
		public CarouselView(EvasObject parent) : base(parent)
		{
			SelectionMode = CollectionViewSelectionMode.Single;
            Scroll.ScrollBlock = ScrollBlock.None;
			SnapPointsType = SnapPointsType.MandatorySingle;
		}

        /// <summary>
        /// Access the scroller.
        /// </summary>
		public Scroller Scroll => base.Scroller;

		Size ICollectionViewController.GetItemSize(int widthConstraint, int heightConstraint)
		{
			return AllocatedSize;
		}
	}
}
