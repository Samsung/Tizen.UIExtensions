using System;
using System.ComponentModel;
using System.Globalization;

namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// Struct defining height and width as a pair of doubles.
    /// </summary>
    public struct Size
    {
        double _width;
        double _height;

        /// <summary>
        /// The Size whose values for height and width are 0.0.
        /// </summary>
        public static readonly Size Zero;

        /// <summary>
        /// Creates a new Size object with width and height.
        /// </summary>
        /// <param name="width">The width of the new size.</param>
        /// <param name="height">The height of the new size.</param>
        public Size(double width, double height)
        {
            if (double.IsNaN(width))
                throw new ArgumentException("NaN is not a valid value for width");
            if (double.IsNaN(height))
                throw new ArgumentException("NaN is not a valid value for height");
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Whether the Size has Height and Width of 0.0.
        /// </summary>
        public bool IsZero
        {
            get { return (_width == 0) && (_height == 0); }
        }

        /// <summary>
        /// Magnitude along the horizontal axis, in platform-defined units.
        /// </summary>
        [DefaultValue(0d)]
        public double Width
        {
            get { return _width; }
            set
            {
                if (double.IsNaN(value))
                    throw new ArgumentException("NaN is not a valid value for Width");
                _width = value;
            }
        }

        /// <summary>
        /// Magnitude along the vertical axis, in platform-specific units.
        /// </summary>
        [DefaultValue(0d)]
        public double Height
        {
            get { return _height; }
            set
            {
                if (double.IsNaN(value))
                    throw new ArgumentException("NaN is not a valid value for Height");
                _height = value;
            }
        }

        /// <summary>
        /// Returns a new Size whose Height and Width are the sum of the component's height and width.
        /// </summary>
        /// <param name="s1">A Size to be added.</param>
        /// <param name="s2">A Size to be added.</param>
        /// <returns>A Size whose Width is equal to s1.Width + s2.Width and whose Height is equal to sz1.Height + sz2.Height.</returns>
        public static Size operator +(Size s1, Size s2)
        {
            return new Size(s1._width + s2._width, s1._height + s2._height);
        }

        /// <summary>
        /// Returns a new Size whose Height and Width are s1's height and width minus the values in s2.
        /// </summary>
        /// <param name="s1">A Size from whose values a size will be subtracted.</param>
        /// <param name="s2">The Size to subtract from s1.</param>
        /// <returns>A Size whose Width is equal to s1.Width - s2.Width and whose Height is equal to sz1.Height - sz2.Height.</returns>
        public static Size operator -(Size s1, Size s2)
        {
            return new Size(s1._width - s2._width, s1._height - s2._height);
        }

        /// <summary>
        /// Scales both Width and Height.
        /// </summary>
        /// <param name="s1">A Size to be scaled.</param>
        /// <param name="value">A factor by which to multiple s1's Width and Height values.</param>
        /// <returns>A new Size whose Width and Height have been scaled by value.</returns>
        public static Size operator *(Size s1, double value)
        {
            return new Size(s1._width * value, s1._height * value);
        }

        /// <summary>
        /// Whether two Sizes have equal values.
        /// </summary>
        /// <param name="s1">A Size to be compared.</param>
        /// <param name="s2">A Size to be compared.</param>
        /// <returns>true if s1 and s2 have equal values for Height and Width.</returns>
        public static bool operator ==(Size s1, Size s2)
        {
            return (s1._width == s2._width) && (s1._height == s2._height);
        }

        /// <summary>
        /// Whether two Sizes have unequal values.
        /// </summary>
        /// <param name="s1">The first Size to compare.</param>
        /// <param name="s2">The second Size to compare.</param>
        /// <returns>true if s1 and s2 have unequal values for either Height or Width.</returns>
        public static bool operator !=(Size s1, Size s2)
        {
            return (s1._width != s2._width) || (s1._height != s2._height);
        }

        /// <summary>
        /// Returns a new Point based on a Size.
        /// </summary>
        /// <param name="size">The Size to be converted to a Point.</param>
        /// <returns>A Point whose X and Y are equal to size's Width and Height, respectively.</returns>
        public static explicit operator Point(Size size)
        {
            return new Point(size.Width, size.Height);
        }

        /// <summary>
        /// Whether thisSize is equivalent to other.
        /// </summary>
        /// <param name="other">The Size to which this is being compared.</param>
        /// <returns>true if other's values are identical to thisSize's Height and Width.</returns>
        public bool Equals(Size other)
        {
            return _width.Equals(other._width) && _height.Equals(other._height);
        }

        /// <summary>
        /// Whether thisSize is equivalent to obj.
        /// </summary>
        /// <param name="obj">The object to which this is being compared.</param>
        /// <returns>true if obj is a Size whose values are identical to thisSize's Height and Width.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Size && Equals((Size)obj);
        }

        /// <summary>
        /// Returns a hash value for the Size.
        /// </summary>
        /// <returns>A value intended for efficient insertion and lookup in hashtable-based data structures.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_width.GetHashCode() * 397) ^ _height.GetHashCode();
            }
        }

        /// <summary>
        /// Returns a human-readable representation of the Size.
        /// </summary>
        /// <returns>The format has the pattern "{Width={0} Height={1}}".</returns>
        public override string ToString()
        {
            return string.Format("{{Width={0} Height={1}}}", _width.ToString(CultureInfo.InvariantCulture), _height.ToString(CultureInfo.InvariantCulture));
        }

        public void Deconstruct(out double width, out double height)
        {
            width = Width;
            height = Height;
        }
    }
}