using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    public class NodeConstant : Node
    {

        public NodeConstant(object value)
        {
            defaultInputs = [value];
            Outputs = [new OutputOfNode(() => defaultInputs[0])];
            NameOfOutputs = ["Значение"];
            NameOfNode = "Константа";
            Inputs = [];
        }
    }
}
