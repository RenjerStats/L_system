using L_system.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Systems
{
    public static class NodeSystem
    {
        private static NodeV activeNode;

        public static NodeV ActiveNode
        {
            get { return activeNode; }
            set {
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
    }
}
