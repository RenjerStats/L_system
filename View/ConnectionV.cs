using L_system.Model.core.Nodes;
using L_system.Systems.ForNodes;
using L_system.ViewModel;
using System.ComponentModel;
using System.Net;
using System.Security.Claims;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace L_system.View
{
    public class ConnectionV : IDisposable
    {
        public ConnectionVM connectionCore;
        private Ellipse outputPoint;
        private Ellipse inputPoint;
        private Canvas canvas;
        public Path face;

        public ConnectionV(ConnectionVM connectionCore, Canvas canvas)
        {
            this.connectionCore = connectionCore;
            this.canvas = canvas;
        }
        public void SetConnectionPoints(Ellipse input, Ellipse output)
        {
            inputPoint = input;
            inputPoint.Fill = Brushes.Blue;

            outputPoint = output;
            outputPoint.Fill = Brushes.Red;

            RegisterPositionChanged(inputPoint);
            RegisterPositionChanged(outputPoint);
        }

        public void EndCreate()
        {
            face = GetCurve();
            canvas.Children.Add(face);
        }

        public void Dispose()
        {
            connectionCore.Dispose();

            DeleteRegisterPositionChanged(outputPoint);
            DeleteRegisterPositionChanged(inputPoint);

            inputPoint.Fill = Brushes.White;
            outputPoint.Fill = Brushes.White;

            canvas.Children.Remove(face);
            ConnectionSystem.connections.Remove(this);
        }

        #region ActionsAndEvents

        private void MouseCut(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Dispose();
            }
        }

        private void RegisterPositionChanged(Ellipse ellipse)
        {
            Border faceNode = (Border)((Grid)((Grid)ellipse.Parent).Parent).Parent;
            DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, typeof(Border))
                .AddValueChanged(faceNode, OnPositionChanged);
            DependencyPropertyDescriptor.FromProperty(Canvas.TopProperty, typeof(Border))
                .AddValueChanged(faceNode, OnPositionChanged);
        }
        private void DeleteRegisterPositionChanged(Ellipse ellipse)
        {
            Border faceNode = (Border)((Grid)((Grid)ellipse.Parent).Parent).Parent;
            DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, typeof(Border))
                .RemoveValueChanged(faceNode, OnPositionChanged);
            DependencyPropertyDescriptor.FromProperty(Canvas.TopProperty, typeof(Border))
                .RemoveValueChanged(faceNode, OnPositionChanged);
        }

        public void OnPositionChanged(object? sender, EventArgs e)
        {
            canvas.Children.Remove(face);
            face = GetCurve();
            canvas.Children.Add(face);
        }

        #endregion

        #region CreateBezierCurve
        private Path GetCurve()
        {
            Point startPoint = outputPoint.TranslatePoint(new Point(outputPoint.Width / 2, outputPoint.Height / 2), canvas);
            Point endPoint = inputPoint.TranslatePoint(new Point(inputPoint.Width / 2, inputPoint.Height / 2), canvas);
            Vector difference = endPoint - startPoint;

            double offsetX; double offsetY;
            CreateOffsets(difference, out offsetX, out offsetY);

            Point controlPointNearStart, controlPointNearEnd;
            CreateControlPoints(startPoint, endPoint, offsetX, offsetY, out controlPointNearStart, out controlPointNearEnd);

            PathGeometry geometry = CreateBezierCurve(startPoint, endPoint, controlPointNearStart, controlPointNearEnd);

            Path path = new Path
            {
                Stroke = GetGradientForLine(startPoint, endPoint),
                StrokeThickness = 4,
                Data = geometry
            };
            path.MouseMove += MouseCut;

            return path;
        }
        private void CreateOffsets(Vector difference, out double offsetX, out double offsetY)
        {
            bool inputFlipped = connectionCore.inputNode.Flipped;
            bool outputFlipped = connectionCore.outputNode.Flipped;
            bool XorFlip = inputFlipped ^ outputFlipped;
            bool NodeFrontByFront = difference.X > 0 && !inputFlipped && !outputFlipped
                                 || difference.X < 0 && inputFlipped && outputFlipped;


            offsetX = 60 * NormalizeWithAbs(difference.Y, 200) + 50 * NormalizeWithAbs(difference.X, 200);
            offsetY = 50 * Normalize(-difference.Y, 200);

            if (!NodeFrontByFront) offsetY -= 100 * NormalizeWithAbs(difference.X, 200) * Math.Sign(difference.Y);
        }

        private static double NormalizeWithAbs(double input, double maxInput)
        {
            return Math.Clamp(Math.Abs(input), 0, maxInput) / maxInput;
        }

        private static double Normalize(double input, double inputRange)
        {
            return Math.Clamp(input, -inputRange, inputRange) / inputRange;
        }

        private void CreateControlPoints(Point startPoint, Point endPoint, double offsetX, double offsetY, out Point controlPointNearStart, out Point controlPointNearEnd)
        {
            bool inputFlipped = connectionCore.inputNode.Flipped;
            bool outputFlipped = connectionCore.outputNode.Flipped;

            double cp1X = outputFlipped ? -offsetX : offsetX;
            double cp1Y = -offsetY;
            controlPointNearStart = new Point(startPoint.X + cp1X, startPoint.Y + cp1Y);
            double cp2X = inputFlipped ? offsetX : -offsetX;
            double cp2Y = offsetY;
            controlPointNearEnd = new Point(endPoint.X + cp2X, endPoint.Y + cp2Y);
        }

        private static PathGeometry CreateBezierCurve(Point startPoint, Point endPoint, Point controlPointNearStart, Point controlPointNearEnd)
        {
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure { StartPoint = startPoint };
            BezierSegment bezierSegment = new BezierSegment(controlPointNearStart, controlPointNearEnd, endPoint, true);
            figure.Segments.Add(bezierSegment);
            geometry.Figures.Add(figure);
            return geometry;
        }

        private Brush GetGradientForLine(Point start, Point end)
        {
            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = start; 
            gradientBrush.EndPoint = end;
            gradientBrush.MappingMode = BrushMappingMode.Absolute;
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Blue, 1.0));

            return gradientBrush;
        }

        #endregion
    }
}
