using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    public class NodeEnd : Node
    {
        public NodeEnd()
        {
            defaultInputs = [Array.Empty<Command>()];
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[1]);
            Outputs = [];
            NameOfNode = "Отрисовка";
            NameOfInputs = ["На отрисовку"];

            FinalNodeConstructor();
        }

        public Command[] GetCommands()
        {
            Command[] commands = Inputs[0] == null ? (Command[])defaultInputs[0] : (Command[])Inputs[0].GetValue();
            return commands;
        }
    }
}
