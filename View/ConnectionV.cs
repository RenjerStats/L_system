using L_system.Model.core.Nodes;
using L_system.Systems;
using L_system.ViewModel;
using System.ComponentModel;
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

        public ConnectionV(ConnectionVM connectionCore, Ellipse outputPoint, Ellipse inputPoint, Canvas canvas)
        {
            this.connectionCore = connectionCore;
            this.outputPoint = outputPoint;
            this.inputPoint = inputPoint;
            this.canvas = canvas;
            face = GetLine();
            canvas.Children.Add(face);

            // Подписываемся на события изменения позиций
            RegisterPositionChanged(outputPoint);
            RegisterPositionChanged(inputPoint);
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

            double offsetX = 20 + Math.Clamp(Math.Abs(difference.Y) / 5, 0, 100) + Math.Clamp(-difference.X / 3, 0, 100);
            double offsetY = Math.Clamp(-difference.X/2, 0, 300) * Math.Sign(difference.Y) * Math.Sign(difference.X);

            Point controlPoint1 = new Point(startPoint.X + offsetX, startPoint.Y - offsetY);
            Point controlPoint2 = new Point(endPoint.X - offsetX, endPoint.Y + offsetY);

            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure { StartPoint = startPoint };
            BezierSegment bezierSegment = new BezierSegment(controlPoint1, controlPoint2, endPoint, true);
            figure.Segments.Add(bezierSegment);
            geometry.Figures.Add(figure);

            Path path = new Path
            {
                Stroke = Brushes.White,
                StrokeThickness = 4,
                Data = geometry
            };
            path.MouseMove += MouseCut;

            return path;
        }
    }
}
