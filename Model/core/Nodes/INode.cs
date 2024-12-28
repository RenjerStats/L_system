using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    public interface INode
    {
        public InputOfNode[] Inputs { get; set; }
        public OutputOfNode[] Outputs { get; set; }
    }

    public class Node : INode
    {
        public InputOfNode[] Inputs { get; set; }
        public OutputOfNode[] Outputs { get; set; }

        public bool TryConnect(int inputIndex, INode prefNode, int outputIndex)
        {
            if (inputIndex < 0 || inputIndex >= Inputs.Length ||
            outputIndex < 0 || outputIndex >= prefNode.Outputs.Length)
                return false;

            var inputType = Inputs[inputIndex].GetValue().GetType();
            var outputType = prefNode.Outputs[outputIndex].GetValue().GetType();

            if (inputType.IsAssignableFrom(outputType))
            {
                Inputs[inputIndex].SetConnection(prefNode.Outputs[outputIndex]);
                return true;
            }

            return false;
        }
    }
}
