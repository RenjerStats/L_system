using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.ViewModel
{
    public class ConnectionVM : IDisposable
    {
        private NodeVM outputNode;
        private NodeVM inputNode;

        private int outputIndex;
        private int inputIndex;

        public static bool CanCreateConnection(NodeVM outputNode, int outputIndex, NodeVM inputNode, int inputIndex)
        {
            return inputNode.CanCreateConnection(inputIndex, outputNode, outputIndex);
        }

        public ConnectionVM(NodeVM outputNode, int outputIndex, NodeVM inputNode, int inputIndex)
        {
            this.outputNode = outputNode;
            this.inputNode = inputNode;
            this.inputIndex = inputIndex;
            this.outputIndex = outputIndex;

            inputNode.CreateConnection(inputIndex, outputNode, outputIndex, this);
        }
        public void Dispose()
        {
            inputNode.Disconnect(inputIndex, outputIndex);
        }
    }

    public static class ConnectionSystem
    {
        private static NodeVM? OutputNode;
        private static NodeVM? InputNode;

        private static List<ConnectionVM> connections = new List<ConnectionVM>();

        private static int OutputIndex;
        private static int InputIndex;

        public static void SetInput(NodeVM inputNode, int inputIndex)
        {
            InputNode = inputNode;
            InputIndex = inputIndex;
        }
        public static void SetOutput(NodeVM outputNode, int outputIndex)
        {
            OutputNode = outputNode;
            OutputIndex = outputIndex;
        }

        public static void StartNewConnection()
        {
            OutputNode = null;
            InputNode = null;
        }

        public static void EndNewConnection()
        {
            if (OutputNode != null && InputNode != null && InputNode != OutputNode)
            {
                if (ConnectionVM.CanCreateConnection(OutputNode, OutputIndex, InputNode, InputIndex))
                {
                    connections.Add(new ConnectionVM(OutputNode, OutputIndex, InputNode, InputIndex));

                }
            }
        }
    }
}
