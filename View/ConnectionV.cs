using L_system.Model.core.Nodes;
using L_system.Systems;
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
        private NodeV inputNode;
        private NodeV outputNode;
        private Ellipse outputPoint;
        private Ellipse inputPoint;
        private Canvas canvas;
        public Path face;

        public ConnectionV(ConnectionVM connectionCore, Canvas canvas)
        {
            this.connectionCore = connectionCore;
            this.canvas = canvas;
        }
        public void SetInputNode(NodeV node, Ellipse point)
        {
            inputPoint = point;
            inputNode = node;

            RegisterPositionChanged(inputPoint);
        }
        public void SetOutputNode(NodeV node, Ellipse point)
        {
            outputPoint = point;
            outputNode = node;
            RegisterPositionChanged(outputPoint);
        }
        public void EndCreate()
        {
            face = GetLine();
            canvas.Children.Add(face);
        }
        public void Dispose()
        {
            connectionCore.Dispose();

            DeleteRegisterPositionChanged(outputPoint);
            DeleteRegisterPositionChanged(inputPoint);

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
            face = GetLine();
            canvas.Children.Add(face);
        }

        #endregion

        private Path GetLine()
        {
            Point startPoint = outputPoint.TranslatePoint(new Point(outputPoint.Width / 2, outputPoint.Height / 2), canvas);
            Point endPoint = inputPoint.TranslatePoint(new Point(inputPoint.Width / 2, inputPoint.Height / 2), canvas);
            Vector difference = endPoint - startPoint;

            double offsetX = 0; double offsetY = 0;
            bool XorFlip = inputNode.Flipped ^ outputNode.Flipped;
            bool NodeFrontByFront = difference.X > 0 && !inputNode.Flipped && !outputNode.Flipped
                                 || difference.X < 0 && inputNode.Flipped && outputNode.Flipped;


            offsetX = 60 * Math.Clamp(Math.Abs(difference.Y), 0, 200) / 200 + 50 * Math.Clamp(Math.Abs(difference.X), 0, 200) / 200;
            offsetY = 50 * Math.Clamp(-difference.Y, -200, 200) / 200;

            if (!NodeFrontByFront) offsetY -= 100 * Math.Clamp(Math.Abs(difference.X), 0, 200) / 200 * Math.Sign(difference.Y);

            double cp1X = outputNode.Flipped ? -offsetX : offsetX;
            double cp1Y = -offsetY;
            Point controlPointNearStart = new Point(startPoint.X + cp1X, startPoint.Y + cp1Y);

            double cp2X = inputNode.Flipped ? offsetX : -offsetX;
            double cp2Y = offsetY;
            Point controlPointNearEnd = new Point(endPoint.X + cp2X, endPoint.Y + cp2Y);

            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure { StartPoint = startPoint };
            BezierSegment bezierSegment = new BezierSegment(controlPointNearStart, controlPointNearEnd, endPoint, true);
            figure.Segments.Add(bezierSegment);
            geometry.Figures.Add(figure);

            Path path = new Path
            {
                Stroke = GetGradientForLine(startPoint, endPoint),
                StrokeThickness = 4,
                Data = geometry
            };
            path.MouseMove += MouseCut;

            return path;
        }


        //private static double Normalize()
        //{
        //    return 60 * Math.Clamp(Math.Abs(difference.Y), 0, 200) / 200;
        //}

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
    }
}
