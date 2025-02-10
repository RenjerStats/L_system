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

namespace L_system.Systems
{
    public static class NodeSystem
    {
        public static List<NodeV> nodes = new List<NodeV>();

        #region CreateNodes
        private static Dictionary<string, Func<NodeVM>> namesOfNodes = new Dictionary<string, Func<NodeVM>>(){
            { "Нода умножения", () => new NodeVM(new NodeMult()) },    // 0
            { "Нода деления", () => new NodeVM(new NodeDiv()) },       // 1
            { "Нода сложения", () => new NodeVM(new NodePlus()) },     // 2
            { "Нода вычитания", () => new NodeVM(new NodeSub()) },     // 3
            { "Нода синуса", () => new NodeVM(new NodeSin()) },        // 4
            { "Нода времени", () => new NodeVM(new NodeTime()) },      // 5
            { "Нода замены", () => new NodeVM(new NodeChangeTO()) },   // 6
            { "Нода отрисовки", () => new NodeVM(new NodeEnd()) },     // 7
            { "Числовая нода", () => new NodeVM(new NodeConstant()) },  // 8
            { "Нарисовать линию", () => new NodeVM(new NodeMove()) },    // 9
            { "Повернуть перо", () => new NodeVM(new NodeRotate()) },     // 10
            { "Переместить перо", () => new NodeVM(new NodeJump()) },      // 11
            { "Сохранить позицию пера", () => new NodeVM(new NodeSave()) }, // 12
            { "Загрузить позицию пера", () => new NodeVM(new NodeLoad()) }, // 13
            { "Пустое действие1", () => new NodeVM(new  NodeNothing1()) }, // 14
            { "Пустое действие2", () => new NodeVM(new NodeNothing2()) }, // 15
            { "Пустое действие3", () => new NodeVM(new NodeNothing3()) }, // 16
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
            nodes.Add(new NodeV(position, namesOfNodes[name].Invoke(), canvas));
        }

        #endregion

        #region ActiveNode
        private static NodeV activeNode;
        public static NodeV ActiveNode
        {
            get { return activeNode; }
            set
            {
                if (activeNode != value)
                {
                    ActiveNodeChange();
                }
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
        #endregion
    }
}
