using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Xsl;

namespace L_system.Systems
{
    public interface IDrawerFractals
    {
        DrawingGroup Lines { get; }

        void DrawLine(double length, Pen pen);
        void MovePen(double length);
        void SaveStartPosition();
        void LoadStartPosition();
        void RotatePen(double angle);
    }

    public class Drawer : IDrawerFractals
    {
        private DrawingGroup _lines;
        private Pen defaultPen;
        private Matrix startPointTransform;
        private Stack<Matrix> saveMatrixes;

        public DrawingGroup Lines => _lines;

        public Drawer(double startX, double startY, double startAngle = 180)
        {
            defaultPen = new Pen(Brushes.White, 3);
            _lines = new DrawingGroup();
            saveMatrixes = new Stack<Matrix>();
            startPointTransform = new Matrix();
            startPointTransform.Rotate(startAngle);
            startPointTransform.Translate(startX, startY);
        }

        public void DrawLine(double length, Pen pen)
        {
            LineGeometry line = new LineGeometry(new Point(), new Point(0, length));
            line.Transform = new MatrixTransform(startPointTransform);

            GeometryDrawing drawingLine = new GeometryDrawing();
            drawingLine.Geometry = line;
            drawingLine.Pen = pen;

            MovePen(length);
            _lines.Children.Add(drawingLine);
        }
        public void DrawLine(double length)
        {
            LineGeometry line = new LineGeometry(new Point(), new Point(0, length));
            line.Transform = new MatrixTransform(startPointTransform);

            GeometryDrawing drawingLine = new GeometryDrawing();
            drawingLine.Geometry = line;
            drawingLine.Pen = defaultPen;

            MovePen(length);
            _lines.Children.Add(drawingLine);
        }

        public void MovePen(double length)
        {
            startPointTransform.TranslatePrepend(0, length);
        }

        public void RotatePen(double angle)
        {
            Matrix rotateMatrix = new Matrix();
            rotateMatrix.Rotate(-angle);
            startPointTransform.Prepend(rotateMatrix);
        }

        public void SaveStartPosition()
        {
            saveMatrixes.Push(startPointTransform);
        }

        public void LoadStartPosition()
        {
            startPointTransform = saveMatrixes.Pop();
        }

        internal void SetDefaultPen(Pen defaultPen)
        {
            this.defaultPen = defaultPen;
        }
    }
}
