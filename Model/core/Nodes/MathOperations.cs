using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    internal class NodePlus : Node
    {
        public NodePlus()
        {
            Inputs = [new InputOfNode(1.0D), new InputOfNode(2.0D)];
            Outputs = [new OutputOfNode(() => (double)Inputs[0].GetValue() + (double)Inputs[1].GetValue())];
        }
    }
}
