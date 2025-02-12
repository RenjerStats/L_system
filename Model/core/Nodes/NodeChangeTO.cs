using System.Collections.ObjectModel;

namespace L_system.Model.core.Nodes
{
    public class NodeChangeTO : Node
    {
        public NodeChangeTO()
        {
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[3]);
            defaultInputs = [Array.Empty<Command>(), Array.Empty<Command>(), Array.Empty<Command>()];
            Outputs = [new OutputOfNode(GetResult)];
            NameOfNode = "Замена";
            NameOfInputs = ["Команды", "Заменить", "На"];
            NameOfOutputs = ["Результат"];

            FinalNodeConstructor();
        }

        public Command[] GetResult()
        {
            Command[] axiom = Inputs[0] == null? (Command[])defaultInputs[0] : (Command[])Inputs[0].GetValue();
            Command[] change = Inputs[1] == null? (Command[])defaultInputs[1] : (Command[])Inputs[1].GetValue();
            Command[] to = Inputs[2] == null? (Command[])defaultInputs[2] : (Command[])Inputs[2].GetValue();
            L_system_engine engine = new L_system_engine(axiom, change[^1], to);

            engine.Iterate();

            return engine.GetCommands();
        }
    }
}
