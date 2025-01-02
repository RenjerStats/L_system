using L_system.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Globalization;
using L_system.Model.core.Nodes;
using System.Windows.Media;

namespace L_system.View
{
    public class ConnectionV
    {
        public ConnectionVM connectionCore;
        public Line face;

        public ConnectionV(ConnectionVM connectionCore, Ellipse outputPoint, Ellipse inputPoint, Canvas canvas)
        {
            this.connectionCore = connectionCore;

            face = new Line();
            face.Stroke = Brushes.White;
            face.Fill = Brushes.White;
            face.StrokeThickness = 10;

            Point relativeLocation = outputPoint.TranslatePoint(new Point(0, 0), canvas);

            Point relativeLocation2 = inputPoint.TranslatePoint(new Point(0, 0), canvas);

            canvas.Children.Add(face);
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
