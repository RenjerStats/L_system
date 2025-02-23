using DrawTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace L_system.Systems.ForDraw
{
    internal class NavigationForDrawing
    {
        private Point lastMousePosition;
        private bool isDragging;
        private readonly ScaleTransform scaleTransform = new ScaleTransform();
        private readonly TranslateTransform translateTransform = new TranslateTransform();
        private readonly TransformGroup transformGroup = new TransformGroup();
        private readonly DrawingCanvas canvas;

        public NavigationForDrawing(DrawingCanvas MainCanvas)
        {
            canvas = MainCanvas;

            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);

            canvas.MouseWheel += Canvas_MouseWheel;
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;
        }

        public TransformGroup GetTransform()
        {
            return transformGroup;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                lastMousePosition = e.GetPosition(canvas);
                canvas.Cursor = Cursors.Hand;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;

            var currentPosition = e.GetPosition(canvas);
            var delta = currentPosition - lastMousePosition;

            // Учитываем масштаб при перемещении
            translateTransform.X += delta.X / scaleTransform.ScaleX;
            translateTransform.Y += delta.Y / scaleTransform.ScaleY;

            lastMousePosition = currentPosition;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            canvas.Cursor = Cursors.Arrow;
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mousePosition = e.GetPosition(canvas);
            var scale = e.Delta > 0 ? 1.1 : 1 / 1.1;

            if (scaleTransform.ScaleX * scale < 0.1 || scaleTransform.ScaleX * scale > 10)
                return;

            // Корректировка позиции для сохранения точки под курсором
            translateTransform.X = mousePosition.X - (mousePosition.X - translateTransform.X) * scale;
            translateTransform.Y = mousePosition.Y - (mousePosition.Y - translateTransform.Y) * scale;

            scaleTransform.ScaleX *= scale;
            scaleTransform.ScaleY *= scale;
        }
    }
}
