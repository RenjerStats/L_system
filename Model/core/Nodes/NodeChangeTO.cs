namespace L_system.Model.core.Nodes
{
    public class NodeChangeTO : Node
    {
        public NodeChangeTO()
        {
            object axiom = new Command[] { new Command(CommandType.nothingDoing1) };
            object change = new Command(CommandType.nothingDoing1);
            object to = new Command[] { new Command(CommandType.nothingDoing1) };
            Inputs = new InputOfNode[3];
            defaultInputs = [axiom, change, to];
            Outputs = [new OutputOfNode(GetResult)];
            NameOfNode = "Замена";
            NameOfInputs = ["Команды", "Заменяемое", "Заменитель"];
            NameOfOutputs = ["Результат"];
        }

        public Command[] GetResult()
        {
            object axiom = Inputs[0] == null? defaultInputs[0] : Inputs[0].GetValue();
            object change = Inputs[1] == null? defaultInputs[1] : Inputs[1].GetValue();
            object to = Inputs[2] == null? defaultInputs[2] : Inputs[2].GetValue();
            L_system_engine engine = new L_system_engine((Command[])axiom,
                                         EngineTools.ToDictionary([(Command)change], [(Command[])to ]));

            engine.Iterate();

            return engine.GetCommands();
        }
    }
}
