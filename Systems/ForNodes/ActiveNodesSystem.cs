using L_system.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Windows.ApplicationModel.Core;

namespace L_system.Systems.ForNodes
{
    public static class ActiveNodesSystem
    {
        private static HashSet<NodeV> activeNodes = new HashSet<NodeV>();
        #region OperationsWithSet
        private static void AddActiveNode(NodeV node)
        {
            node.IsActive = true;
            activeNodes.Add(node);
        }

        public static void RemoveActiveNode(NodeV node)
        {
            node.IsActive = false;
            activeNodes.Remove(node);
        }

        private static void ClearActiveNodes()
        {
            foreach (var node in activeNodes)
            {
                node.IsActive = false;
            }
            activeNodes.Clear();
        }

        #endregion

        public static void ActionHandler(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control) ClearActiveNodes();
            if (e.OriginalSource is not Canvas)
            {
                Border border = FindParent<Border>((DependencyObject)e.OriginalSource);
                if (border.DataContext is not NodeV) return;
                Canvas canvas = (Canvas)sender;

                NodeV node = (NodeV)border.DataContext;
                if (activeNodes.Contains(node)) RemoveActiveNode(node);
                else AddActiveNode(node);
            }
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            if (child is T item) return item;
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null && parent is not T)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as T;
        }


        #region Rectangle
        private static Point startPoint;
        public static void StartCreateRectangle(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != sender) return;

            Canvas canvas = (Canvas)sender;
            startPoint = e.GetPosition(canvas);
        }
        public static void EndCreateRectangle(object sender, MouseButtonEventArgs e)
        {
            if (startPoint == default) return;
            Canvas canvas = (Canvas)sender;
            Point secondPoint = e.GetPosition(canvas);

            if ((startPoint - secondPoint).Length < 10)
            {
                startPoint = default;
                return;
            }

            if (Keyboard.Modifiers != ModifierKeys.Control) ClearActiveNodes();
            foreach (NodeV node in GetNodesInRectangle(canvas, startPoint, secondPoint))
            {
                AddActiveNode(node);
            }

            startPoint = default;
        }

        public static NodeV[] GetNodesInRectangle(Canvas canvas, Point topLeft, Point bottomRight)
        {
            List<NodeV> borders = new List<NodeV>();

            Rect selectionRect = new Rect(topLeft, bottomRight);
            RectangleGeometry geometry = new RectangleGeometry(selectionRect);

            VisualTreeHelper.HitTest(
                canvas,
                null,
                result =>
                {
                    if (result.VisualHit is Border border)
                    {
                        if (border.DataContext is NodeV node)borders.Add(node);
                    }
                    return HitTestResultBehavior.Continue;
                },
                new GeometryHitTestParameters(geometry)
            );

            return borders.ToArray();
        }
        #endregion
    }
}
