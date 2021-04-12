using System.Collections.ObjectModel;
using ElmSharp;
using SkiaSharp;
using CPoint = Tizen.UIExtensions.Common.Point;

namespace Tizen.UIExtensions.ElmSharp
{
    public class PolygonView : ShapeView
    {
        ObservableCollection<CPoint> _points;
        bool _fillMode;

        public PolygonView(EvasObject parent) : base(parent)
        {
            UpdateShape();
        }

        public ObservableCollection<CPoint> Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
                UpdateShape();
            }
        }

        public bool FillMode
        {
            get
            {
                return _fillMode;
            }
            set
            {
                _fillMode = value;
                UpdateShape();
            }
        }

        public void UpdatePoints(ObservableCollection<CPoint> points)
        {
            _points = points;
            UpdateShape();
        }

        public void UpdateFillMode(bool fillMode)
        {
            _fillMode = fillMode;
            UpdateShape();
        }

        void UpdateShape()
        {
            if (_points != null && _points.Count > 1)
            {
                SKPath path = new SKPath();
                path.FillType = _fillMode ? SKPathFillType.Winding : SKPathFillType.EvenOdd;

                path.MoveTo((float)_points[0].X, (float)_points[0].Y);
                for (int index = 1; index < _points.Count; index++)
                {
                    path.LineTo((float)_points[index].X, (float)_points[index].Y);
                }
                path.Close();

                UpdateShape(path);
            }
        }
    }
}
