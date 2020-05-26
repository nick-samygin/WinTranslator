﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PasswordBoss.Views.InAppAdvertising
{
    public class Arc : Shape
    {
        public double StartPoint
        {
            get { return (double)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(double), typeof(Arc), new UIPropertyMetadata(0.0, new PropertyChangedCallback(UpdateArc)));

        public double EndPoint
        {
            get { return (double)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(double), typeof(Arc), new UIPropertyMetadata(0.0, new PropertyChangedCallback(UpdateArc)));

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new UIPropertyMetadata(0.0, new PropertyChangedCallback(UpdateArc)));

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new UIPropertyMetadata(90.0, new PropertyChangedCallback(UpdateArc)));


        protected static void UpdateArc(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Arc arc = d as Arc;
            arc.InvalidateVisual();
        }

        protected override Geometry DefiningGeometry
        {
            get { return GetArcGeometry(); }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            drawingContext.DrawGeometry(null, new Pen(Stroke, StrokeThickness), GetArcGeometry());

            for (int i = Convert.ToInt32(StartPoint); i < Convert.ToInt32(EndPoint) + 1; i++)
            {
                if (Convert.ToInt32(EndPoint) < 1) return;
                drawingContext.DrawGeometry(null, new Pen(Fill, StrokeThickness), GetArcBarsGeometry(i * 12));
            }
        }

        private Geometry GetArcGeometry()
        {
            Point startPoint = PointAtAngle(Math.Min(StartAngle, EndAngle));
            Point endPoint = PointAtAngle(Math.Max(StartAngle, EndAngle));

            Size arcSize = new Size(Math.Max(0, (RenderSize.Width - StrokeThickness) / 2),
                Math.Max(0, (RenderSize.Height - StrokeThickness) / 2));
            bool isLargeArc = Math.Abs(EndAngle - StartAngle) > 180;

            StreamGeometry geom = new StreamGeometry();
            using (StreamGeometryContext context = geom.Open())
            {
                context.BeginFigure(startPoint, false, false);
                context.ArcTo(endPoint, arcSize, 0, isLargeArc, SweepDirection.Clockwise, true, false);
            }
            geom.Transform = new TranslateTransform(StrokeThickness / 2, StrokeThickness / 2);
            return geom;

        }

        private Geometry GetArcBarsGeometry(double offsetStart)
        {
            var start = StartAngle + offsetStart;
            Point startPoint = PointAtAngle(Math.Min(start, start + 2));
            Point endPoint = PointAtAngle(Math.Max(start, start + 2));

            Size arcSize = new Size(Math.Max(0, (RenderSize.Width - StrokeThickness) / 2),
                Math.Max(0, (RenderSize.Height - StrokeThickness) / 2));
            bool isLargeArc = false;// Math.Abs(EndAngle - StartAngle) > 180;

            StreamGeometry geom = new StreamGeometry();
            using (StreamGeometryContext context = geom.Open())
            {
                context.BeginFigure(startPoint, false, false);
                context.ArcTo(endPoint, arcSize, 0, isLargeArc, SweepDirection.Clockwise, true, false);
            }
            geom.Transform = new TranslateTransform(StrokeThickness / 2, StrokeThickness / 2);
            return geom;
        }

        private Point PointAtAngle(double angle)
        {
            double radAngle = angle * (Math.PI / 180);
            double xr = (RenderSize.Width - StrokeThickness) / 2;
            double yr = (RenderSize.Height - StrokeThickness) / 2;

            double x = xr + xr * Math.Sin(radAngle);
            double y = yr - yr * Math.Cos(radAngle);

            return new Point(x, y);
        }
    }
}
