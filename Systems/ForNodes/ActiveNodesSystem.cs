using L_system.Model.core.Nodes;
using L_system.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace L_system.Systems.ForNodes
{
    public static class ActiveNodesSystem
    {
        private static HashSet<NodeV> activeNodes = new HashSet<NodeV>();

        #region OperationsWithSet
        public static NodeV[] GetActiveNodes()
        {
            return activeNodes.ToArray();
        }

        public static bool IsActiveNodesEmpty()
        {
            return activeNodes.Count == 0;
        }

        private static void AddActiveNode(NodeV node)
        {
            node.IsActive = true;
            activeNodes.Add(node);
            RecalculateCenterOfSystem();
        }

        public static void RemoveActiveNode(NodeV node)
        {
            node.IsActive = false;
            activeNodes.Remove(node);
            RecalculateCenterOfSystem();
        }

        private static void ClearActiveNodes()
        {
            foreach (var node in activeNodes)
            {
                node.IsActive = false;
            }
            activeNodes.Clear();
            centerOfSystemOfNode = new Vector();
        }
        #endregion

        #region PositionRelativeToCenterOfSystem
        private static Vector centerOfSystemOfNode;

        private static void RecalculateCenterOfSystem()
        {
            centerOfSystemOfNode = new Vector();
            foreach (var node in activeNodes)
            {
                double x = Canvas.GetLeft(node.face);
                double y = Canvas.GetTop(node.face);
                centerOfSystemOfNode += new Vector(x, y);
            }
            centerOfSystemOfNode /= activeNodes.Count;
        }

        public static Vector GetVectorBetweenNodeAndCenter(NodeV node)
        {
            double x = Canvas.GetLeft(node.face);
            double y = Canvas.GetTop(node.face);
            Vector nodePosition = new Vector(x, y);
            return nodePosition - centerOfSystemOfNode;
        }
        #endregion

        #region DragActiveNode
        private static Point dragStartPoint;
        private static bool isDragging = false;
        private static NodeV clickedActiveNode = null;
        private static Dictionary<NodeV, Point> originalPositions = new Dictionary<NodeV, Point>();

        public static void ActionHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is not Canvas)
            {
                Border border = FindBorder((DependencyObject)e.OriginalSource);
                if (border?.DataContext is not NodeV node) return;
                Canvas canvas = node.canvas;

                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    // Если зажат Ctrl – переключаем состояние ноды
                    if (activeNodes.Contains(node))
                        RemoveActiveNode(node);
                    else
                        AddActiveNode(node);
                }
                else
                {
                    // Если нода ещё не выделена – очищаем остальные и выделяем её
                    if (!activeNodes.Contains(node))
                    {
                        ClearActiveNodes();
                        AddActiveNode(node);
                    }
                    else
                    {
                        // Если нода уже выделена – сохраняем её как потенциально "единственную" для клика без перетаскивания
                        clickedActiveNode = node;
                    }
                }

                // Если клик не по элементу, отвечающему за Connection – запускаем процесс перетаскивания
                if (e.OriginalSource is not Ellipse)
                {
                    StartDrag(e.GetPosition(canvas));
                }
            }
            else
            {
                // Клик по фону Canvas – если Ctrl не зажат, очищаем выделение
                if (Keyboard.Modifiers != ModifierKeys.Control)
                {
                    ClearActiveNodes();
                }
            }
        }

        public static Border FindBorder(DependencyObject child)
        {
            if (child is Border border) return border;
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null && parent is not Border)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as Border;
        }

        public static void StartDrag(Point startPoint)
        {
            dragStartPoint = startPoint;
            foreach (var node in activeNodes)
            {
                originalPositions[node] = node.Position;
            }
        }

        public static void UpdateDrag(Point currentPoint)
        {
            if (dragStartPoint == default) return;

            Vector offset = currentPoint - dragStartPoint;
            // Если смещение превышает порог 5 pixels – считаем, что происходит перетаскивание
            if (offset.Length > 5)
            {
                isDragging = true;
            }
            foreach (var node in activeNodes)
            {
                if (originalPositions.TryGetValue(node, out Point originalPos))
                {
                    node.Position = originalPos + offset;
                }
            }
        }

        public static void EndDrag()
        {
            // Если перетаскивание не произошло, значит это был просто клик
            if (!isDragging && clickedActiveNode != null)
            {
                // Оставляем выделенной только нажатую ноду, снимая выделение с остальных
                ClearActiveNodes();
                AddActiveNode(clickedActiveNode);
            }

            // Сбрасываем флаги и данные перетаскивания
            originalPositions.Clear();
            dragStartPoint = default;
            isDragging = false;
            clickedActiveNode = null;
        }
        #endregion

        #region Rectangle
        private static Point startPointRectangle;
        public static void StartCreateRectangle(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != sender) return; // если клик не на Canvas, то return

            Canvas canvas = (Canvas)sender;
            startPointRectangle = e.GetPosition(canvas);
            StartDrawRectangle(sender, e);
        }
        public static void EndCreateRectangle(object sender, MouseButtonEventArgs e)
        {
            if (startPointRectangle == default) return;
            Canvas canvas = (Canvas)sender;
            Point secondPoint = e.GetPosition(canvas);

            if ((startPointRectangle - secondPoint).Length < 10)
            {
                startPointRectangle = default;
                DeleteRectangle();
                return;
            }

            if (Keyboard.Modifiers != ModifierKeys.Control) ClearActiveNodes();
            foreach (NodeV node in GetNodesInRectangle(canvas, startPointRectangle, secondPoint))
            {
                AddActiveNode(node);
            }

            startPointRectangle = default;
            DeleteRectangle();
        }

        private static NodeV[] GetNodesInRectangle(Canvas canvas, Point topLeft, Point bottomRight)
        {
            List<NodeV> borders = new List<NodeV>();

            Rect selectionRect = new Rect(topLeft, bottomRight);
            Rectangle rectangle = new Rectangle();
            RectangleGeometry geometry = new RectangleGeometry(selectionRect);

            VisualTreeHelper.HitTest(
                canvas,
                null,
                result =>
                {
                    if (result.VisualHit is Border border)
                    {
                        if (border.DataContext is NodeV node) borders.Add(node);
                    }
                    return HitTestResultBehavior.Continue;
                },
                new GeometryHitTestParameters(geometry)
            );

            return borders.ToArray();
        }

        #region DrawRectangle
        private static Rectangle rect;

        private static void StartDrawRectangle(object sender, MouseButtonEventArgs e)
        {
            rect = new Rectangle
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                StrokeDashArray = new DoubleCollection { 4, 4 },
                Fill = new SolidColorBrush(Color.FromArgb(85, 0, 0, 255)),
            };

            DoubleAnimation dashOffsetAnimation = new DoubleAnimation
            {
                From = 0,
                To = 8,
                Duration = TimeSpan.FromSeconds(1),
                RepeatBehavior = RepeatBehavior.Forever
            };
            rect.BeginAnimation(Shape.StrokeDashOffsetProperty, dashOffsetAnimation);

            Canvas.SetZIndex(rect, int.MaxValue);
            Canvas.SetLeft(rect, startPointRectangle.X);
            Canvas.SetTop(rect, startPointRectangle.Y);
            ((Canvas)sender).Children.Add(rect);
        }

        public static void DrawRectangle(object sender, MouseEventArgs e)
        {
            if (rect == null) return;

            Point currentPoint = e.GetPosition(((Canvas)sender));
            double width = currentPoint.X - startPointRectangle.X;
            double height = currentPoint.Y - startPointRectangle.Y;

            rect.Width = Math.Abs(width);
            rect.Height = Math.Abs(height);

            Canvas.SetLeft(rect, width >= 0 ? startPointRectangle.X : currentPoint.X);
            Canvas.SetTop(rect, height >= 0 ? startPointRectangle.Y : currentPoint.Y);
        }

        public static void DeleteRectangle()
        {
            if (rect == null) return;
            Canvas canvas = rect.Parent as Canvas;
            canvas.Children.Remove(rect);
            rect = null;
        }
        #endregion
        #endregion
    }
}
