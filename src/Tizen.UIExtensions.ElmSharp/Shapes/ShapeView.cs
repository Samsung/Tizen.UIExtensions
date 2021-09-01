﻿using System;
using ElmSharp;
using SkiaSharp;
using SkiaSharp.Views.Tizen;
using Tizen.UIExtensions.Common;

namespace Tizen.UIExtensions.ElmSharp
{
    public class ShapeView : Canvas, IMeasurable
    {
        SKCanvasView _skCanvasView;
        SKPath? _skPath;
        SKPaint _skPaint;

        SKRect _drawableBounds;
        SKRect _pathFillBounds;
        SKRect _pathStrokeBounds;
        SKMatrix _transform;

        ShapeBrush? _stroke;
        ShapeBrush? _fill;
        Stretch _stretch;

        float _strokeWidth;
        float[]? _strokeDash;
        float _strokeDashOffset;
        float _strokeMiterLimit;

        public ShapeView(EvasObject parent) : base(parent)
        {
            _skPaint = new SKPaint
            {
                IsAntialias = true
            };
            _skCanvasView = new SKCanvasView(parent);
            _skCanvasView.PaintSurface += OnPaintSurface;
            _skCanvasView.Show();
            Children.Add(_skCanvasView);
            LayoutUpdated += OnLayoutUpdated;

            _pathFillBounds = new SKRect();
            _pathStrokeBounds = new SKRect();

            _skPaint.StrokeCap = SKStrokeCap.Butt;
            _skPaint.StrokeJoin = SKStrokeJoin.Miter;
            _strokeWidth = 1f;
            _strokeDashOffset = 0f;
            _strokeMiterLimit = 10f;
            _stretch = Stretch.None;
        }

        public SKCanvasView SKCanvasView => _skCanvasView;

        public ShapeBrush? Stroke
        {
            get
            {
                return _stroke;
            }
            set
            {
                _stroke = value;
                _skCanvasView.Invalidate();
            }
        }

        public ShapeBrush? Fill
        {
            get
            {
                return _fill;
            }
            set
            {
                _fill = value;
                _skCanvasView.Invalidate();
            }
        }

        public double StrokeThickness
        {
            get
            {
                return _strokeWidth;
            }
            set
            {
                _strokeWidth = value.ToScaledPixel();
                _skPaint.StrokeWidth = _strokeWidth;
                UpdateStrokeDash();
            }
        }

        public float[]? StrokeDashArray
        {
            get
            {
                return _strokeDash;
            }
            set
            {
                _strokeDash = value;
                UpdateStrokeDashArray(_strokeDash);
            }
        }

        public float StrokeDashOffset
        {
            get
            {
                return _strokeDashOffset;
            }
            set
            {
                _strokeDashOffset = value;
                UpdateStrokeDash();
            }
        }

        public PenLineCap StrokeLineCap
        {
            get
            {
                return _skPaint.StrokeCap.ToCommon();
            }
            set
            {
                _skPaint.StrokeCap = value.ToSkia();
                UpdatePathStrokeBounds();
            }
        }

        public PenLineJoin StrokeLineJoin
        {
            get
            {
                return _skPaint.StrokeJoin.ToCommon();
            }
            set
            {
                _skPaint.StrokeJoin = value.ToSkia();
                _skCanvasView.Invalidate();
            }
        }

        public float StrokeMiterLimit
        {
            get
            {
                return _strokeMiterLimit;
            }
            set
            {
                _strokeMiterLimit = value;
                _skPaint.StrokeMiter = _strokeMiterLimit * 2;
                UpdatePathStrokeBounds();
            }
        }

        public Stretch Aspect
        {
            get
            {
                return _stretch;
            }
            set
            {
                _stretch = value;
                _skCanvasView.Invalidate();
            }
        }

        void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            _drawableBounds = e.Info.Rect;
            canvas.Clear();

            if (_skPath == null)
                return;

            SKMatrix transformMatrix = CreateMatrix();
            SKPath transformedSkPath = new SKPath();
            _skPath.Transform(transformMatrix, transformedSkPath);
            SKRect fillBounds = transformMatrix.MapRect(_pathFillBounds);
            SKRect strokeBounds;
            using (SKPath strokePath = new SKPath())
            {
                _skPaint.GetFillPath(transformedSkPath, strokePath);
                strokeBounds = strokePath.Bounds;
            }

            if (_fill != null)
            {
                _skPaint.Style = SKPaintStyle.Fill;

                if (_fill is GradientBrush fillGradientBrush)
                {
                    _skPaint.Shader = fillGradientBrush.CreateShader(fillBounds);
                }
                else if (_fill is SolidColorBrush fillSolidColorBrush)
                {
                    _skPaint.Color = fillSolidColorBrush.ToSolidColor();
                }
                canvas.DrawPath(transformedSkPath, _skPaint);
                _skPaint.Shader = null;
            }

            if (_stroke != null)
            {
                _skPaint.Style = SKPaintStyle.Stroke;

                if (_stroke is GradientBrush strokeGradientBrush)
                {
                    UpdatePathStrokeBounds();
                    _skPaint.Shader = strokeGradientBrush.CreateShader(strokeBounds);
                }
                else if (_stroke is SolidColorBrush strokeSolidColorBrush)
                {
                    _skPaint.Color = strokeSolidColorBrush.ToSolidColor();
                }
                canvas.DrawPath(transformedSkPath, _skPaint);
                _skPaint.Shader = null;
            }
        }

        void OnLayoutUpdated(object? sender, LayoutEventArgs e)
        {
            _skCanvasView.Geometry = Geometry;
        }

        public void UpdateShape(SKPath sKPath)
        {
            _skPath = sKPath;
            UpdatePathShape();
        }

        public void UpdateShapeTransform(SKMatrix matrix)
        {
            _transform = matrix;
            _skPath?.Transform(_transform);
            _skCanvasView.Invalidate();
        }

        public void UpdateStrokeDashArray(float[]? dash)
        {
            _strokeDash = dash;
            UpdateStrokeDash();
        }

        public void UpdateStrokeDash()
        {
            if (_strokeDash != null && _strokeDash.Length > 1)
            {
                float[] strokeDash = new float[_strokeDash.Length];

                for (int i = 0; i < _strokeDash.Length; i++)
                    strokeDash[i] = _strokeDash[i] * _strokeWidth;
                _skPaint.PathEffect = SKPathEffect.CreateDash(strokeDash, _strokeDashOffset * _strokeWidth);
            }
            else
            {
                _skPaint.PathEffect = null;
            }
            UpdatePathStrokeBounds();
        }

        protected void UpdatePathShape()
        {
            if (_skPath != null)
            {
                using (SKPath fillPath = new SKPath())
                {
                    _skPaint.StrokeWidth = 0.01f;
                    _skPaint.Style = SKPaintStyle.Stroke;
                    _skPaint.GetFillPath(_skPath, fillPath);
                    _pathFillBounds = fillPath.Bounds;
                    _skPaint.StrokeWidth = _strokeWidth;
                }
            }
            else
            {
                _pathFillBounds = SKRect.Empty;
            }

            UpdatePathStrokeBounds();
        }

        SKMatrix CreateMatrix()
        {
            SKMatrix matrix = SKMatrix.CreateIdentity();

            SKRect drawableBounds = _drawableBounds;
            float halfStrokeWidth = _skPaint.StrokeWidth / 2;

            drawableBounds.Left += halfStrokeWidth;
            drawableBounds.Top += halfStrokeWidth;
            drawableBounds.Right -= halfStrokeWidth;
            drawableBounds.Bottom -= halfStrokeWidth;

            float widthScale = drawableBounds.Width / _pathFillBounds.Width;
            float heightScale = drawableBounds.Height / _pathFillBounds.Height;

            switch (_stretch)
            {
                case Stretch.None:
                    drawableBounds = _drawableBounds;
                    float adjustX = Math.Min(0, _pathStrokeBounds.Left);
                    float adjustY = Math.Min(0, _pathStrokeBounds.Top);
                    if (adjustX < 0 || adjustY < 0)
                    {
                        matrix = SKMatrix.CreateTranslation(-adjustX, -adjustY);
                    }
                    break;
                case Stretch.Fill:
                    matrix = SKMatrix.CreateScale(widthScale, heightScale);
                    matrix = matrix.PostConcat(
                        SKMatrix.CreateTranslation(drawableBounds.Left - widthScale * _pathFillBounds.Left,
                        drawableBounds.Top - heightScale * _pathFillBounds.Top));
                    break;
                case Stretch.Uniform:
                    float minScale = Math.Min(widthScale, heightScale);
                    matrix = SKMatrix.CreateScale(minScale, minScale);
                    matrix = matrix.PostConcat(
                        SKMatrix.CreateTranslation(drawableBounds.Left - (minScale * _pathFillBounds.Left) + (drawableBounds.Width - (minScale * _pathFillBounds.Width)) / 2,
                        drawableBounds.Top - (minScale * _pathFillBounds.Top) + (drawableBounds.Height - (minScale * _pathFillBounds.Height)) / 2));
                    break;
                case Stretch.UniformToFill:
                    float maxScale = Math.Max(widthScale, heightScale);
                    matrix = SKMatrix.CreateScale(maxScale, maxScale);
                    matrix = matrix.PostConcat(
                        SKMatrix.CreateTranslation(drawableBounds.Left - (maxScale * _pathFillBounds.Left),
                        drawableBounds.Top - (maxScale * _pathFillBounds.Top)));
                    break;
            }

            return matrix;
        }

        void UpdatePathStrokeBounds()
        {
            if (_skPath != null)
            {
                using (SKPath strokePath = new SKPath())
                {
                    _skPaint.Style = SKPaintStyle.Stroke;
                    _skPaint.GetFillPath(_skPath, strokePath);
                    _pathStrokeBounds = strokePath.Bounds;
                }
            }
            else
            {
                _pathStrokeBounds = SKRect.Empty;
            }

            _skCanvasView.Invalidate();
        }

        Common.Size IMeasurable.Measure(double availableWidth, double availableHeight)
        {
            if (_skPath != null)
            {
                return new Common.Size((int)Math.Max(_pathStrokeBounds.Right - Math.Min(0, _pathStrokeBounds.Left), _strokeWidth),
                    (int)Math.Max(_pathStrokeBounds.Bottom - Math.Min(0, _pathStrokeBounds.Top), _strokeWidth));
            }
            return new Common.Size(MinimumWidth, MinimumHeight);
        }
    }
}
