using System;
using System.Globalization;

namespace Tizen.UIExtensions.Common
{
    /// <summary>
    /// Struct defining a 2-D point as a pair of doubles.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Location along the horizontal axis.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Location along the vertical axis.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// The Point at {0,0}.
        /// </summary>
        public static Point Zero = new Point();

        public override string ToString()
        {
            return string.Format("{{X={0} Y={1}}}", X.ToString(CultureInfo.InvariantCulture), Y.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Creates a new Point object that represents the point (x,y).
        /// </summary>
        /// <param name="x">The horizontal coordinate.</param>
        /// <param name="y">The vertical coordinate.</param>
        public Point(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new Point object that has coordinates that are specified by the width and height of sz, in that order.
        /// </summary>
        /// <param name="sz">Size that specifies a Point that has the coordinates (Width, Height).</param>
        public Point(Size sz) : this()
        {
            X = sz.Width;
            Y = sz.Height;
        }

        /// <summary>
        /// Returns true if the X and Y values of this are exactly equal to those in the argument.
        /// </summary>
        /// <param name="o">Another Point.</param>
        /// <returns>true if the X and Y values are equal to those in o. Returns false if o is not a Point.</returns>
        public override bool Equals(object? o)
        {
            if (!(o is Point))
                return false;

            return this == (Point)o;
        }

        /// <summary>
        /// Returns a hash value for the Point.
        /// </summary>
        /// <returns>A value intended for efficient insertion and lookup in hashtable-based data structures.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ (Y.GetHashCode() * 397);
        }

        /// <summary>
        /// Returns a new Point that translates the current Point by dx and dy.
        /// </summary>
        /// <param name="dx">The amount to add along the X axis.</param>
        /// <param name="dy">The amount to add along the Y axis.</param>
        /// <returns>A new Point at [this.X + dx, this.Y + dy].</returns>
        public Point Offset(double dx, double dy)
        {
            Point p = this;
            p.X += dx;
            p.Y += dy;
            return p;
        }

        /// <summary>
        /// Returns a new Point whose X and Y have been rounded to the nearest integral value.
        /// </summary>
        /// <returns>A new Point whose X and Y have been rounded to the nearest integral value, per the behavior of Math.Round(Double).</returns>
        public Point Round()
        {
            return new Point(Math.Round(X), Math.Round(Y));
        }

        /// <summary>
        /// Whether both X and Y are 0.
        /// </summary>
        /// <value>true if both X and Y are 0.0.</value>
        public bool IsEmpty
        {
            get { return (X == 0) && (Y == 0); }
        }

        /// <summary>
        /// Returns a new Size whose Width and Height and equivalent to the pt's X and Y properties.
        /// </summary>
        /// <param name="pt">The Point to be translated as a Size.</param>
        /// <returns>A new Size based on the pt.</returns>
        public static explicit operator Size(Point pt)
        {
            return new Size(pt.X, pt.Y);
        }

        /// <summary>
        /// Returns a new Point by adding a Size to a Point.
        /// </summary>
        /// <param name="pt">The Point to which sz is being added.</param>
        /// <param name="sz">The values to add to pt.</param>
        /// <returns>A new Point at [pt.X + sz.Width, pt.Y + sz.Height].</returns>
        public static Point operator +(Point pt, Size sz)
        {
            return new Point(pt.X + sz.Width, pt.Y + sz.Height);
        }

        /// <summary>
        /// Returns a new Point by subtracting a Size from a Point.
        /// </summary>
        /// <param name="pt">The Point from which sz is to be subtracted.</param>
        /// <param name="sz">The Size whose Width and Height will be subtracted from pt's X and Y.</param>
        /// <returns>A new Point at [pt.X - sz.Width, pt.Y - sz.Height].</returns>
        public static Point operator -(Point pt, Size sz)
        {
            return new Point(pt.X - sz.Width, pt.Y - sz.Height);
        }

        /// <summary>
        /// Whether the two Points are equal.
        /// </summary>
        /// <param name="ptA">The first point to compare.</param>
        /// <param name="ptB">The second point to compare.</param>
        /// <returns></returns>
        public static bool operator ==(Point ptA, Point ptB)
        {
            return (ptA.X == ptB.X) && (ptA.Y == ptB.Y);
        }

        public static bool operator !=(Point ptA, Point ptB)
        {
            return (ptA.X != ptB.X) || (ptA.Y != ptB.Y);
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="other">The Point to which the distance is calculated.</param>
        /// <returns>The distance between this and the other.</returns>
        public double Distance(Point other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        public void Deconstruct(out double x, out double y)
        {
            x = X;
            y = Y;
        }
    }
}