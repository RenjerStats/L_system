using System.Collections.ObjectModel;

namespace L_system.Model.core.Nodes
{
    public class NodeConstant : Node
    {
        public NodeConstant()
        {
            defaultInputs = [1D];
            Inputs = new ObservableCollection<InputOfNode>();
            Outputs = [new OutputOfNode(() => defaultInputs[0])];
            NameOfOutputs = ["Значение"];
            NameOfNode = "Константа";

            FinalNodeConstructor();
        }
    }
}
