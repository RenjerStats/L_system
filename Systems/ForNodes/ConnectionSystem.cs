using L_system.View;
using L_system.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace L_system.Systems.ForNodes
{
    public static class ConnectionSystem
    {
        private static NodeV? OutputNode;
        private static NodeV? InputNode;

        private static Ellipse OutputPoint;
        private static Ellipse InputPoint;

        private static int OutputIndex;
        private static int InputIndex;

        public static List<ConnectionV> connections = new List<ConnectionV>();


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


        public static void EndNewConnection()
        {
            if (OutputNode != null && InputNode != null && InputNode != OutputNode)
            {
                if (ConnectionVM.CanCreateConnection(OutputNode.nodeCore, OutputIndex, InputNode.nodeCore, InputIndex))
                {
                    if (!InputNode.nodeCore.IsInputFree(InputIndex))
                    {
                        Func<ConnectionV, bool> isConnectionWithSameInput = (a) => a.connectionCore.inputIndex == InputIndex && a.connectionCore.inputNode == InputNode.nodeCore;
                        ConnectionV connectionWithSameInput = connections.Where(isConnectionWithSameInput).ToArray()[0];
                        connectionWithSameInput.Dispose();
                    }
                    ConnectionVM core = new ConnectionVM(OutputNode.nodeCore, OutputIndex, InputNode.nodeCore, InputIndex);

                    Canvas canvas = (Canvas)InputNode.face.Parent;
                    ConnectionV connection = new ConnectionV(core, canvas);
                    connection.SetConnectionPoints(InputPoint, OutputPoint);
                    connection.EndCreate();

                    connections.Add(connection);
                }
            }
            ClearAllReference();
        }
        public static void ClearAllReference()
        {
            OutputNode = null;
            InputNode = null;
            InputPoint = null;
            OutputPoint = null;
        }
    }
}
