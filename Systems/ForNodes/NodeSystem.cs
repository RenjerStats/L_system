using L_system.Model.core;
using L_system.Model.core.Nodes;
using L_system.View;
using L_system.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace L_system.Systems.ForNodes
{
    public static class NodeSystem
    {
        public static List<NodeV> nodes = new List<NodeV>();
        public static Dictionary<NodeV, NodeEnd> nodesEnd = new Dictionary<NodeV, NodeEnd>();

        #region CreateNodes
        private static Dictionary<string, Func<Node>> namesOfNodes = new Dictionary<string, Func<Node>>(){
            { "Нода умножения", () => new NodeMult() },    // 0
            { "Нода деления", () => new NodeDiv() },       // 1
            { "Нода сложения", () => new NodePlus() },     // 2
            { "Нода вычитания", () => new NodeSub() },     // 3
            { "Нода синуса", () => new NodeSin() },        // 4
            { "Нода времени", () => new NodeTime() },      // 5
            { "Нода замены", () => new NodeChangeTO() },   // 6
            { "Нода отрисовки", () => new NodeEnd() },     // 7
            { "Числовая нода", () => new NodeConstant() },  // 8
            { "Нарисовать линию", () => new NodeMove() },    // 9
            { "Повернуть перо", () => new NodeRotate() },     // 10
            { "Переместить перо", () => new NodeJump() },      // 11
            { "Сохранить позицию пера", () => new NodeSave() }, // 12
            { "Загрузить позицию пера", () => new NodeLoad() }, // 13
            { "Пустое действие1", () => new NodeNothing1() },  // 14
            { "Пустое действие2", () => new NodeNothing2() }, // 15
            { "Пустое действие3", () => new NodeNothing3() }, // 16
        };

        public static Dictionary<string, string[]> namesOfGroups = new Dictionary<string, string[]>(){
            {"Базовая математика", [GetNameFormID(0), GetNameFormID(1), GetNameFormID(2), GetNameFormID(3), GetNameFormID(8)]},
            {"Для анимации", [GetNameFormID(4), GetNameFormID(5)]},
            {"Для отрисовки", [GetNameFormID(6), GetNameFormID(7)]},
            {"Действия", [GetNameFormID(9), GetNameFormID(10), GetNameFormID(11), GetNameFormID(12),
                         GetNameFormID(13), GetNameFormID(14), GetNameFormID(15), GetNameFormID(16)]}
        };

        private static string GetNameFormID(int id)
        {
            return namesOfNodes.Keys.ToArray()[id];
        }

        public static void CreateNode(Point position, Canvas canvas, string name)
        {
            Node core = namesOfNodes[name].Invoke();
            NodeV newNode = new NodeV(position, new NodeVM(core), canvas);
            newNode.UpdateZIndex();
            if (core is NodeEnd) nodesEnd.Add(newNode, core as NodeEnd);
            nodes.Add(newNode);
        }

        #endregion

        #region ActionWithKeyPress
        public static void NodeMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                ActiveNodesSystem.GetActiveNodes().Length > 0)
            {
                ActiveNodesSystem.UpdateDrag(e.GetPosition((Canvas)sender));
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

        private static bool canAction = true;
        public static void KeyPressed(object sender, KeyEventArgs e)
        {
            if (!canAction) return;
            if (ActiveNodesSystem.IsActiveNodesEmpty()) return;

            if (e.Key == Key.M && Keyboard.Modifiers == ModifierKeys.None)
            {
                foreach (NodeV node in ActiveNodesSystem.GetActiveNodes())
                {
                    node.Flip();
                }
                canAction = false;
            }
            if (e.Key == Key.D && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.D)
                {
                    foreach (NodeV node in ActiveNodesSystem.GetActiveNodes())
                    {
                        CreateDuplicate(node, ActiveNodesSystem.GetVectorBetweenNodeAndCenter(node));
                    }
                    canAction = false;
                }
            }
            if (e.Key == Key.Delete && Keyboard.Modifiers == ModifierKeys.None)
            {
                DeleteNodes();
                canAction = false;
            }
        }
        public static void ResetAction(object sender, KeyEventArgs e) => canAction = true;


        private static void CreateDuplicate(NodeV node, Vector offsetRelativeToCenterOfSystemNode)
        {
            Canvas canvas = node.canvas;
            Point position = Mouse.GetPosition(canvas);
            position -= new Vector(75, 75); // Центрирование ноды
            position += offsetRelativeToCenterOfSystemNode; //

            Node core = node.nodeCore.GetCopyCore();
            NodeV dupNode = new NodeV(position, new NodeVM(core), canvas);
            if (node.nodeCore.Flipped != dupNode.nodeCore.Flipped) dupNode.Flip();
            if (node.MiniSize != dupNode.MiniSize) dupNode.ChangeForm();
            dupNode.UpdateZIndex();

            nodes.Add(dupNode);
            if (core is NodeEnd) nodesEnd.Add(dupNode, core as NodeEnd);
        }

        private static void DeleteNodes()
        {
            foreach (var activeNode in ActiveNodesSystem.GetActiveNodes())
            {
                nodesEnd.Remove(activeNode);
                nodes.Remove(activeNode);
                ActiveNodesSystem.RemoveActiveNode(activeNode);
                activeNode.Dispose();
            }
        }


        #endregion


        static public IEnumerable<Command[]> GetCommandsFromAllNodesEnd()
        {
            foreach (var item in nodesEnd.Values)
            {
                yield return item.GetCommands();
            }
        }
    }
}
