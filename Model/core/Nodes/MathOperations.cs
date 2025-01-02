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
            defaultInputs = [new InputOfNode(1.0D), new InputOfNode(2.0D)];
            ResetInputsToDefault();
            Outputs = [new OutputOfNode(() => (double)Inputs[0].GetValue() + (double)Inputs[1].GetValue())];
            NameOfNode = "Сложение";
            NameOfInputs = ["Слагаемое", "Слагаемое"];
            NameOfOutputs = ["Сумма"];
        }
    }
    internal class NodeSub : Node
    {
        public NodeSub()
        {
            defaultInputs = [new InputOfNode(1.0D), new InputOfNode(2.0D)];
            ResetInputsToDefault();
            Outputs = [new OutputOfNode(() => (double)Inputs[0].GetValue() - (double)Inputs[1].GetValue())];
            NameOfNode = "Вычитание";
            NameOfInputs = ["Уменьшаемое", "Вычитаемое"];
            NameOfOutputs = ["Разность"];
        }
    }
    internal class NodeMult : Node
    {
        public NodeMult()
        {
            defaultInputs = [new InputOfNode(1.0D), new InputOfNode(2.0D)];
            ResetInputsToDefault();
            Outputs = [new OutputOfNode(() => (double)Inputs[0].GetValue() * (double)Inputs[1].GetValue())];
            NameOfNode = "Умножение";
            NameOfInputs = ["Множитель", "Множитель"];
            NameOfOutputs = ["Произведение"];
        }
    }
    internal class NodeDiv : Node
    {
        public NodeDiv()
        {
            defaultInputs = [new InputOfNode(1.0D), new InputOfNode(2.0D)];
            ResetInputsToDefault();
            Outputs = [new OutputOfNode(() => (double)Inputs[0].GetValue() / (double)Inputs[1].GetValue())];
            NameOfNode = "Деление";
            NameOfInputs = ["Делимое", "Делитель"];
            NameOfOutputs = ["Частное"];
        }
    }


}
