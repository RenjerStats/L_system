using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    public class NodeEnd : Node
    {
        public NodeEnd()
        {
            defaultInputs = [new Command[] { new(CommandType.nothingDoing1) }];
            Inputs = new InputOfNode[1];
        }

        public Command[] GetResult()
        {
            return (Command[])Inputs[0].GetValue();
        }
    }
}
