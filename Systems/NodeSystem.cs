using L_system.Model.core;
using L_system.Model.core.Nodes;
using L_system.View;
using L_system.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace L_system.Systems
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

        #region ActiveNode
        private static NodeV activeNode;
        public static NodeV ActiveNode
        {
            get { return activeNode; }
            set
            {
                if (activeNode != value) ActiveNodeChange();      
                activeNode = value;
            }
        }

        public static void OnActiveNodeChange(Action action)
        {
            actionsOnInputsChanged.Add(action);
        }
        private static List<Action> actionsOnInputsChanged = new List<Action>();
        private static void ActiveNodeChange()
        {
            foreach (var action in actionsOnInputsChanged.ToList())
            {
                action();
            }
        }

        public static void DeleteActiveNode()
        {
            if (activeNode.nodeCore.GetOutputsCount() == 0) nodesEnd.Remove(activeNode);
            nodes.Remove(activeNode);
            activeNode.Dispose();
            activeNode = null;
        }
        #endregion

        #region ActionWithKeyPress
        public static void KeyPressed(object sender, KeyEventArgs e)
        {
            if (activeNode == null) return;
            Canvas canvas = activeNode.canvas;
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.D)
                {
                    Duplicate(canvas);
                }
            }
            else if (Keyboard.Modifiers == ModifierKeys.None && e.Key == Key.M)
            {
                activeNode.Flip();
            }
        }

        private static void Duplicate(Canvas canvas)
        {
            Point position = Mouse.GetPosition(canvas);
            position -= new Vector(75, 75); // Центрирование ноды
            Node core = activeNode.nodeCore.GetCopyCore();
            NodeV dupNode = new NodeV(position, new NodeVM(core), canvas);
            dupNode.UpdateZIndex();

            nodes.Add(dupNode);
            if (core is NodeEnd) nodesEnd.Add(dupNode, core as NodeEnd);
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
