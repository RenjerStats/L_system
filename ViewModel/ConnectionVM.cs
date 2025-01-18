using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.ViewModel
{
    public class ConnectionVM : IDisposable
    {
        public NodeVM outputNode { get; private set; }
        public NodeVM inputNode { get; private set; }

        public int outputIndex { get; private set; }
        public int inputIndex { get; private set; }

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

            inputNode.CreateConnection(inputIndex, outputNode, outputIndex);
        }
        public void Dispose()
        {
            inputNode.Disconnect(inputIndex, outputIndex);
        }
    }
}
