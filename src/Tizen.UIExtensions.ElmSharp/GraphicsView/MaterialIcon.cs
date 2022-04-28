using ElmSharp;
using Tizen.UIExtensions.Common.GraphicsView;

namespace Tizen.UIExtensions.ElmSharp.GraphicsView
{
    /// <summary>
    /// A visual control of material icons
    /// </summary>
    public class MaterialIcon : GraphicsView<MaterialIconDrawable>
    {
        /// <summary>
        /// Initializes a new instance of the MaterialIcon
        /// </summary>
        public MaterialIcon(EvasObject parent) : base(parent)
        {
            Drawable = new MaterialIconDrawable();
            var measured = Drawable.Measure(double.PositiveInfinity, double.PositiveInfinity);

            MinimumWidth = (int)measured.Width;
            MinimumHeight = (int)measured.Height;
        }

        /// <summary>
        /// Gets of sets the type of the MaterialIcon
        /// </summary>
        public MaterialIcons IconType
        {
            get => Drawable?.Icon ?? MaterialIcons.Add;
            set
            {
                if(Drawable != null)
                {
                    Drawable.Icon = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets of sets the color of the MaterialIcon
        /// </summary>
        public new Common.Color Color
        {
            get => Drawable?.Color ?? Common.Color.Default;
            set
            {
                if (Drawable != null)
                {
                    Drawable.Color = value;
                    Invalidate();
                }
            }
        }
    }
}
