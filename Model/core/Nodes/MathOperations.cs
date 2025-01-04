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
            Inputs = new InputOfNode[2];
            defaultInputs = [1.0D, 2.0D];
            Outputs = [new OutputOfNode(GetOutput)]; // самозацикливание
            NameOfNode = "Сложение";
            NameOfInputs = ["Слагаемое", "Слагаемое"];
            NameOfOutputs = ["Сумма"];
        }
        private object GetOutput()
        {
            double firstValue = Inputs[0] ==  null ? (double)defaultInputs[0] : (double)Inputs[0].GetValue();
            double secondValue = Inputs[1] == null ? (double)defaultInputs[1] : (double)Inputs[1].GetValue();
            return firstValue + secondValue;
        } 
    }
    internal class NodeSub : Node
    {
        public NodeSub()
        {
            defaultInputs = [1.0D, 2.0D];
            Outputs = [new OutputOfNode(GetOutput)];
            NameOfNode = "Вычитание";
            NameOfInputs = ["Уменьшаемое", "Вычитаемое"];
            NameOfOutputs = ["Разность"];
        }
        private object GetOutput()
        {
            double firstValue = Inputs[0] == null ? (double)defaultInputs[0] : (double)Inputs[0].GetValue();
            double secondValue = Inputs[1] == null ? (double)defaultInputs[1] : (double)Inputs[1].GetValue();
            return firstValue - secondValue;
        }
    }
    internal class NodeMult : Node
    {
        public NodeMult()
        {
            defaultInputs = [1.0D, 2.0D];
            Outputs = [new OutputOfNode(GetOutput)];
            NameOfNode = "Умножение";
            NameOfInputs = ["Множитель", "Множитель"];
            NameOfOutputs = ["Произведение"];
        }
        private object GetOutput()
        {
            double firstValue = Inputs[0] == null ? (double)defaultInputs[0] : (double)Inputs[0].GetValue();
            double secondValue = Inputs[1] == null ? (double)defaultInputs[1] : (double)Inputs[1].GetValue();
            return firstValue * secondValue;
        }
    }
    internal class NodeDiv : Node
    {
        public NodeDiv()
        {
            defaultInputs = [1.0D, 2.0D];
            Outputs = [new OutputOfNode(GetOutput)];
            NameOfNode = "Деление";
            NameOfInputs = ["Делимое", "Делитель"];
            NameOfOutputs = ["Частное"];
        }
        private object GetOutput()
        {
            double firstValue = Inputs[0] == null ? (double)defaultInputs[0] : (double)Inputs[0].GetValue();
            double secondValue = Inputs[1] == null ? (double)defaultInputs[1] : (double)Inputs[1].GetValue();
            return firstValue / secondValue;
        }
    }


}
