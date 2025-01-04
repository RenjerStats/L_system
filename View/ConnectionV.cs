using L_system.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Globalization;
using L_system.Model.core.Nodes;
using System.Windows.Media;
using System.ComponentModel;

namespace L_system.View
{
    public class ConnectionV
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

        private void RegisterPositionChanged(Ellipse ellipse)
        {
            DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, typeof(Border))
                .AddValueChanged((Border)((Grid)((Grid)ellipse.Parent).Parent).Parent, OnPositionChanged);
            DependencyPropertyDescriptor.FromProperty(Canvas.TopProperty, typeof(Border))
                .AddValueChanged((Border)((Grid)((Grid)ellipse.Parent).Parent).Parent, OnPositionChanged);
        }

        public void OnPositionChanged(object? sender, EventArgs e)
        {
            canvas.Children.Remove(face);
            face = GetLine();
            canvas.Children.Add(face);
        }

        private Path GetLine()
        {
            Point startPoint = outputPoint.TranslatePoint(new Point(outputPoint.Width / 2, outputPoint.Height / 2), canvas);
            Point endPoint = inputPoint.TranslatePoint(new Point(inputPoint.Width / 2, inputPoint.Height / 2), canvas);
            Vector difference = endPoint - startPoint;

            double offsetX = 20 + Math.Clamp(Math.Abs(difference.Y) / 5, 0, 100) + Math.Clamp(-difference.X / 3, 0, 100);
            double offsetY = Math.Clamp(-difference.X/2, 0, 300) * -Math.Sign(difference.Y);

            Point controlPoint1 = new Point(startPoint.X + offsetX, startPoint.Y - offsetY);
            Point controlPoint2 = new Point(endPoint.X - offsetX, endPoint.Y + offsetY * -Math.Sign(difference.X));

            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure { StartPoint = startPoint };
            BezierSegment bezierSegment = new BezierSegment(controlPoint1, controlPoint2, endPoint, true);
            figure.Segments.Add(bezierSegment);
            geometry.Figures.Add(figure);

            return new Path
            {
                Stroke = Brushes.White,
                StrokeThickness = 4,
                Data = geometry
            };
        }
    }

    public static class ConnectionSystem
    {
        private static Canvas Canvas;
        private static NodeV? OutputNode;
        private static NodeV? InputNode;

        private static Ellipse OutputPoint;
        private static Ellipse InputPoint;

        private static int OutputIndex;
        private static int InputIndex;

        private static List<ConnectionV> connections = new List<ConnectionV>();


        public static void SetInput(NodeV inputNode, int inputIndex, Ellipse inputPoint)
        {
            InputNode = inputNode;
            InputIndex = inputIndex;
            InputPoint = inputPoint;
        }
        public static void SetOutput(NodeV outputNode, int outputIndex, Ellipse outputPoint)
        {
            OutputNode = outputNode;
            OutputIndex = outputIndex;
            OutputPoint = outputPoint;
        }

        public static void StartNewConnection(Canvas canvas)
        {
            Canvas = canvas;
            OutputNode = null;
            InputNode = null;
            InputPoint = null;
            OutputPoint = null;
        }

        public static void EndNewConnection()
        {
            if (OutputNode != null && InputNode != null && InputNode != OutputNode)
            {
                if (ConnectionVM.CanCreateConnection(OutputNode.nodeCore, OutputIndex, InputNode.nodeCore, InputIndex))
                {
                    ConnectionVM core = new ConnectionVM(OutputNode.nodeCore, OutputIndex, InputNode.nodeCore, InputIndex);
                    ConnectionV connection = new ConnectionV(core, OutputPoint, InputPoint, Canvas);

                    connections.Add(connection);
                }
            }
        }
    }
}
