namespace L_system.Model.core.Nodes
{
    public class NodeChangeTO : Node
    {
        public NodeChangeTO()
        {
            InputOfNode axiom = new InputOfNode(new Command[] { new Command(CommandType.nothingDoing1) });
            InputOfNode change = new InputOfNode(new Command(CommandType.nothingDoing1) );
            InputOfNode to = new InputOfNode(new Command[] { new Command(CommandType.nothingDoing1) });
            Inputs = [axiom, change, to];
            Outputs = [new OutputOfNode(GetResult)];
            NameOfNode = "Замена";
            NameOfInputs = ["Команды", "Заменяемое", "Заменитель"];
            NameOfOutputs = ["Результат"];
        }

        public Command[] GetResult()
        {
            L_system_engine engine = new L_system_engine((Command[])Inputs[0].GetValue(),
                                         EngineTools.ToDictionary([(Command)Inputs[1].GetValue()], [(Command[])Inputs[2].GetValue()]));

            engine.Iterate();

            return engine.GetCommands();
        }
    }
}
