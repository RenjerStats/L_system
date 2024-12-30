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
        protected InputOfNode[] defaultInputs;
        public InputOfNode[] Inputs { get; set; }
        public OutputOfNode[] Outputs { get; set; }


        public void ResetInputToDefault(int inputIndex)
        {
            Inputs[inputIndex] = defaultInputs[inputIndex];
        }
        public void ResetInputsToDefault()
        {
            Inputs = (InputOfNode[]) defaultInputs.Clone();
        }

        public bool CanConnect(int inputIndex, INode prefNode, int outputIndex)
        {
            if (inputIndex < 0 || inputIndex >= Inputs.Length ||
                    outputIndex < 0 || outputIndex >= prefNode.Outputs.Length)
                return false;

            var inputType = Inputs[inputIndex].GetValue().GetType();
            var outputType = prefNode.Outputs[outputIndex].GetValue().GetType();

            return inputType.IsAssignableFrom(outputType);
        }
        public void Connect(int inputIndex, INode prefNode, int outputIndex)
        {
            if (!CanConnect(inputIndex, prefNode, outputIndex)) throw new ArgumentException();

            Inputs[inputIndex] = new InputOfNode(prefNode.Outputs[outputIndex]);
        }
    }
}
