namespace L_system.Model.core.Nodes
{
    public class ChangeTO : INode
    {
        public IOutputNode<Object>[] Outputs { get;  set; }

        private InputNode<Command[]> axiom;
        private InputNode<Command> change;
        private InputNode<Command[]> to;

        public ChangeTO(InputNode<Command[]> axiom, InputNode<Command> change, InputNode<Command[]> to)
        {
            this.axiom = axiom;
            this.change = change;
            this.to = to;

            Outputs = [new OutputNode<Command[]>(GetResult)];
        }

        public Command[] GetResult()
        {
            L_system_engine engine = new L_system_engine(axiom.GetValue(), EngineTools.ToDictionary([change.GetValue()], [to.GetValue()]));

            engine.Iterate();

            return engine.GetCommands();
        }
    }
}
