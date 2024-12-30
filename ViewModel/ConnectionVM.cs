using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.ViewModel
{
    public class ConnectionVM
    {
        private NodeVM outputNode;
        private NodeVM inputNode;

        private int outputIndex;
        private int inputIndex;

        public static bool CanCreateConnection(NodeVM outputNode, int outputIndex, NodeVM inputNode, int inputIndex)
        {
            return inputNode.CanCreateConnection(outputIndex, inputNode, inputIndex);
        }

        public ConnectionVM(NodeVM outputNode, NodeVM inputNode, int outputIndex, int inputIndex)
        {
            this.outputNode = outputNode;
            this.inputNode = inputNode;
            this.inputIndex = inputIndex;
            this.outputIndex = outputIndex;

            inputNode.CreateConnection(outputIndex, inputNode, inputIndex, this);
        }

        ~ConnectionVM()
        {
            inputNode.Disconnect(inputIndex);
        }
    }
}
